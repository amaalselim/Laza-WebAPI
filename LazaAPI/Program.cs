
using LazaProject.Application.IRepository;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.Models;
using LazaProject.persistence.Data;
using LazaProject.persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using LazaProject.persistence.Services;
using LazaProject.Application.IServices;
using LazaProject.persistence.UnitOfWork;
using LazaProject.Core.Enums;
using Autofac.Core;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LazaAPI
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
					.AddEntityFrameworkStores<ApplicationDbContext>()
					.AddDefaultTokenProviders();

			// Register Email Service using Google API credentials
			var smtpSettings = builder.Configuration.GetSection("SMTP");
			builder.Services.AddSingleton<IEmailService>(new EmailService(
				smtpSettings["Server"],
				int.Parse(smtpSettings["Port"]),
				smtpSettings["User"],
				smtpSettings["Password"]
			));

			builder.Services.AddScoped<IRepository<ApplicationUser>, UserRepository>();
			builder.Services.AddScoped<IAuthRepo, AuthRepository>();
			builder.Services.AddScoped<IProductRepository,productRepository>();
			builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
			builder.Services.AddScoped<IProductImageRepository,productImageRepository>();
			builder.Services.AddScoped<IImageService, ImageService>();
			builder.Services.AddScoped<IWishListItemRepository, WishListItemRepository>();
			builder.Services.AddScoped<AuthService>();
			builder.Services.AddScoped<IReviewRepository, ReviewRepository>();	

			builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
			builder.Services.AddAutoMapper(typeof(MappingProfile));


			builder.Services.AddCors(corsOptions =>
			{
				corsOptions.AddPolicy("MyPolicy", policyBuilder =>
				{
					policyBuilder.WithOrigins("http://localhost:5099", "http://laza.runasp.net").AllowAnyHeader().AllowAnyMethod();
				});
			});
			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy(Roles.Admin.ToString(), policy => policy.RequireRole(Roles.Admin.ToString()));
				options.AddPolicy(Roles.User.ToString(), policy => policy.RequireRole(Roles.User.ToString()));
			});



			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options => {
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
				};
			})
			.AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
				googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
			})
			.AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
				facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
			})
			.AddTwitter(twitterOptions =>
			{
				twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"];
				twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"];
			});





			builder.Services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
			});
			// Add services to the container.

			builder.Services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy = null; // Customize as needed
			});

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			/*-----------------------------Swagger Part-----------------------------*/
			#region Swagger REgion

			// إضافة خدمات Swagger
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laza API", Version = "v1" });

				// Add support for bearer token authentication
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter 'Bearer' followed by a space and the token.",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{ new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
			});

			#endregion

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			/*if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laza API V1");
				});
			}*/
			// Enable Swagger in all environments
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laza API V1");

			});





			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseCors("MyPolicy");


			app.UseAuthentication();
			app.UseAuthorization();
			


			app.MapControllers();

			app.Run();
		}
	}
}
