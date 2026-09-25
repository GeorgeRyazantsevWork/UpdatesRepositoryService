using Asp.Versioning;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using ServiceCloud.UpdatesRepository.BusinessLogic.Properties;
using ServiceCloud.UpdatesRepository.BusinessLogic.Repository;
using RepositoryImpl = ServiceCloud.UpdatesRepository.BusinessLogic.Repository.UpdatesRepository;
using ServiceCloud.UpdatesRepositoryService;
using ServiceCloud.UpdatesRepositoryService.Grpc;
using ServiceCloud.UpdatesRepositoryService.Middleware;
using ServiceCloud.UpdatesRepositoryService.Properties;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
});

builder.Host.UseWindowsService();

builder.Services.AddControllers();
builder.Services.AddGrpc();
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

builder.Services.AddSingleton<IUpdatesRepository>(sp =>
{
    var options = sp.GetRequiredService<IOptions<RepositoryServiceOptions>>();
    return new RepositoryImpl(options.Value.Path);
});

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
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseWhen(
    context => HttpProtocol.IsHttp11(context.Request.Protocol),
    branch => branch.UseHttpsRedirection());
app.MapControllers();
app.MapGrpcService<UpdatesRepositoryGrpcService>();

app.Run();
