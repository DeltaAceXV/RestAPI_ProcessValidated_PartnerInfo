
using Asp.Versioning;
using RestAPI_ProcessValidated_PartnerInfo;

try
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { 
        Args = args,
        ContentRootPath = AppContext.BaseDirectory,
    });

    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", false, true)
        .Build();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

    builder.Logging.ClearProviders();

    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);
    var app = builder.Build();
    startup.Configure(app);
}
catch(Exception ex)
{
    Console.WriteLine($"{ex.StackTrace}");
}

