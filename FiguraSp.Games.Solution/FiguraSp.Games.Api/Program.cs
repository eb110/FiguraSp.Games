using FiguraSp.Games.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSharedDbConnection(builder.Configuration);
builder.Services.AddCustomServices();
builder.Services.AddSharedJwtScheme(builder.Configuration);

builder.Services.AddHttpClient("figuraHttp", (serviceProvider, HttpClient) =>
{
    HttpClient.BaseAddress = new Uri("http://localhost:5000");
});


//##############################################################MIDDLEWARE###############

var app = builder.Build();
app.UserSharedGatewayMiddleware();
app.UseAuthorization();
app.MapControllers();
app.Run();
