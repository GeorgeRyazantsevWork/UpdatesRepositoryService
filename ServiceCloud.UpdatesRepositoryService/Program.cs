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
        document.Info.Title = RepositoryOptions.ServiceName;
        document.Info.Description = RepositoryOptions.ServiceDescription;
        return Task.CompletedTask;
    });
});

builder.Services
    .AddOptions<RepositoryOptions>()
    .BindConfiguration(RepositoryOptions.SectionName)
    .Validate(options => !string.IsNullOrWhiteSpace(options.Path), Resource.Message_IncorrectRepositoryPath)
    .ValidateOnStart();

builder.Services.AddSingleton<IRepositoryIndex, RepositoryIndex>();
builder.Services.AddSingleton<IUpdatesRepository, UpdatesRepository>();
builder.Services.AddHostedService<RepositoryAutoRefresherHostedService>();

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
                $"{RepositoryOptions.ServiceName} {description.GroupName}");
        }
    });

    Console.WriteLine(Resource.DebugMessage_PressAnyKeyToContinue);
    Console.ReadLine();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
