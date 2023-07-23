using Application;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Serilog;
using System.IO.Compression;
using System.Text;
using WebApi.Extensions;
using WebApi.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression(options =>
{
	//options.MimeTypes = new[] { "application/json" };
	options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Services.RegisterMapsterConfiguration();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
	option.SupportNonNullableReferenceTypes();
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var jwtSettingsSection = builder.Configuration.GetSection("Jwt");

		options.TokenValidationParameters = new TokenValidationParameters()
		{
			ClockSkew = TimeSpan.Zero,
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettingsSection.GetValue<string>("Issuer"),
			ValidAudience = jwtSettingsSection.GetValue<string>("Audience"),
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(jwtSettingsSection.GetValue<string>("SecretKey") ?? string.Empty)
			),
		};
	});

builder.Host.UseSerilog((context, configuration) =>
	configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCloudinary(builder.Configuration);

WebApplication app = builder.Build();

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.Run();
