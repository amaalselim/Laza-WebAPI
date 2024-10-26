
using LazaProject.Application.IRepository;
using LazaProject.Application.IUnitOfWork;
using LazaProject.Core.Mapping;
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
			builder.Services.AddScoped<AuthService>();

			builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
			builder.Services.AddAutoMapper(typeof(MappingProfile));


			builder.Services.AddCors(corsOptions =>
			{
				corsOptions.AddPolicy("MyPolicy", policyBuilder =>
				{
					policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});
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






			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			/*-----------------------------Swagger Part-----------------------------*/
			#region Swagger REgion
			//builder.Services.AddSwaggerGen();

			builder.Services.AddSwaggerGen(swagger =>
			{
				//This�is�to�generate�the�Default�UI�of�Swagger�Documentation����
				swagger.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "ASP.NET�5�Web�API",
					Description = " Laza Project"
				});
				//�To�Enable�authorization�using�Swagger�(JWT)����
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter�'Bearer'�[space]�and�then�your�valid�token�in�the�text�input�below.\r\n\r\nExample:�\"Bearer�eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
					new OpenApiSecurityScheme
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
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			
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
