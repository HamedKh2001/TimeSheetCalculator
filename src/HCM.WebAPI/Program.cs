using HCM.Application;
using HCM.Infrastucture;
using HCM.Infrastucture.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Extensions;
using SharedKernel.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Host.AddMySerilogWithELK("HCM");

builder.Services.AddApiVersioning();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddResponseCaching();

var app = builder.Build();

// app.Services.DbContextInitializer();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseElasticMiddleware();
app.Run();

