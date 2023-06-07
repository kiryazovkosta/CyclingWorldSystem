using Application;
using Persistence;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(Presentation.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddPersistence(builder.Configuration)
	.AddApplication()
	.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>()
	.UseHttpsRedirection()
	.UseAuthorization();

app.MapControllers();

app.Run();
