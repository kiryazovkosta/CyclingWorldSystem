using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Persistence;
using Persistence.Interseptors;
using WebApi.Middlewares;
using WebApi.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(Presentation.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
	option.SupportNonNullableReferenceTypes();
});

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.Run();
