using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NetCoreAPI;
using NetCoreAPI.Application.Controllers;
using NetCoreAPI.Domain.Models;
using NetCoreAPI.Infra.Identidade;
using NetCoreAPI.Infra.Repositories;
using NetCoreAPI.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

AuthorizationSetup.InitializeRolesAndPermissions(builder.Services.BuildServiceProvider());

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();

});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<Server>, Repository<Server>>();
builder.Services.AddScoped<IRepository<Channel>, Repository<Channel>>();
builder.Services.AddScoped<IRepository<Conversation>, Repository<Conversation>>();
builder.Services.AddScoped<IRepository<EmailCode>, Repository<EmailCode>>();
builder.Services.AddScoped<IRepository<Friendship>, Repository<Friendship>>();
builder.Services.AddScoped<IRepository<UserServer>, Repository<UserServer>>();
builder.Services.AddScoped<IRepository<Solicitation>, Repository<Solicitation>>();
builder.Services.AddScoped<IRepository<Message>, Repository<Message>>();
builder.Services.AddScoped<IRepository<MessageChannel>, Repository<MessageChannel>>();
builder.Services.AddScoped<IdentidadeService>();
builder.Services.AddScoped<IMemoryCache, MemoryCache>();
//builder.Services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();
var app = builder.Build();
var inactivityTimeout = TimeSpan.FromMinutes(1);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseCors(options =>
{

    options.WithOrigins("http://localhost:4200")
    .AllowCredentials()
           .AllowAnyMethod()
           .AllowAnyHeader();
         
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<InactivityTimeoutMiddleware>(inactivityTimeout);

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.Run();
