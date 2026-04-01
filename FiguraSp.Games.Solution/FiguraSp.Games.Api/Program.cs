using FiguraSp.Teams.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSharedDbConnection(builder.Configuration);
builder.Services.AddCustomServices();
builder.Services.AddSharedJwtScheme(builder.Configuration);


//##############################################################MIDDLEWARE###############

var app = builder.Build();
app.UserSharedGatewayMiddleware();
app.UseAuthorization();
app.MapControllers();
app.Run();
