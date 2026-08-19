using Asp.Versioning;
using ServiceCloud.UpdatesRepositoryService.Middleware;
using ServiceCloud.UpdatesRepositoryService.Properties;
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
.AddOpenApi(options =>
{
    options.Document.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Updates Repository Service";
        document.Info.Description = "Сервис репозитория обновлений конфигураций информационных баз 1С";
        return Task.CompletedTask;
    });
});

var repositoryPath = builder.Configuration["Repository:Path"];
if (string.IsNullOrWhiteSpace(repositoryPath))
{
    throw new InvalidOperationException(Resource.Message_IncorrectRepositoryPath);
}

builder.Services.AddSingleton<IRepositoryIndex, RepositoryIndex>();
builder.Services.AddSingleton<IUpdatesRepository>(sp =>
    new UpdatesRepository(sp.GetRequiredService<IRepositoryIndex>(), repositoryPath));

builder.Services.AddHostedService(sp =>
{
    var refreshInterval = builder.Configuration.GetValue("Repository:RefreshInterval", TimeSpan.Zero);// TODO убрать магические строки, вынести отдельный класс настроек
    return new RepositoryAutoRefresherHostedService(
        sp.GetRequiredService<IUpdatesRepository>(),
        sp.GetRequiredService<ILogger<RepositoryAutoRefresherHostedService>>(),
        refreshInterval);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().WithDocumentPerVersion();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions().Reverse())
        {
            options.SwaggerEndpoint(
                $"/openapi/{description.GroupName}.json",
                $"Updates Repository Service {description.GroupName}");
        }
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

Console.WriteLine("Нажмите Enter, чтобы запустить сервер...");
Console.ReadLine();

app.Run();
