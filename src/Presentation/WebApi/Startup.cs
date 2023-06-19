using Application;
using Persistence;
using Persistence.Interseptors;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddApplicationPart(Presentation.AssemblyReference.Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
	option.SupportNonNullableReferenceTypes();
});

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();


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

app.Run();
