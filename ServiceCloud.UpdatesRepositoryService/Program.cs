using Asp.Versioning;
using Microsoft.Extensions.Options;
using ServiceCloud.UpdatesRepository.BusinessLogic.Properties;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using ServiceCloud.UpdatesRepositoryService;
using ServiceCloud.UpdatesRepositoryService.Middleware;
using ServiceCloud.UpdatesRepositoryService.Properties;

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
        document.Info.Title = RepositoryServiceOptions.ServiceName;
        document.Info.Description = RepositoryServiceOptions.ServiceDescription;
        return Task.CompletedTask;
    });
});

builder.Services
    .AddOptions<RepositoryServiceOptions>()
    .BindConfiguration(RepositoryServiceOptions.SectionName)
    .Validate(options => !string.IsNullOrWhiteSpace(options.Path), Resource.Message_IncorrectRepositoryPath)
    .ValidateOnStart();

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
                $"{RepositoryServiceOptions.ServiceName} {description.GroupName}");
        }
    });

    Console.WriteLine(Resource.DebugMessage_PressAnyKeyToContinue);
    Console.ReadLine();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
