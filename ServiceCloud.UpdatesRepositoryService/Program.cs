using Asp.Versioning;
using Microsoft.OpenApi;
using ServiceCloud.UpdatesRepositoryService.Middleware;
using ServiceCloud.UpdatesRepositoryService.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Updates Repository Service",
        Version = "v1",
        Description = "Сервис репозитория обновлений"
    });
});

var repositoryPath = builder.Configuration["Repository:Path"];
if (string.IsNullOrWhiteSpace(repositoryPath))
{
    throw new InvalidOperationException("Не задан путь к репозиторию обновлений (Repository:Path).");
}

builder.Services.AddSingleton<IRepositoryIndex, RepositoryIndex>();
builder.Services.AddSingleton<IUpdatesRepository>(sp =>
    new UpdatesRepository(sp.GetRequiredService<IRepositoryIndex>(), repositoryPath));

builder.Services.AddHostedService(sp =>
{
    var refreshInterval = builder.Configuration.GetValue("Repository:RefreshInterval", TimeSpan.Zero);
    return new RepositoryAutoRefresherHostedService(
        sp.GetRequiredService<IUpdatesRepository>(),
        sp.GetRequiredService<ILogger<RepositoryAutoRefresherHostedService>>(),
        refreshInterval);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Updates Repository Service v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

Console.WriteLine("Нажмите Enter, чтобы запустить сервер...");
Console.ReadLine();

app.Run();