using Asp.Versioning;
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
})
.AddOpenApi();

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
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();