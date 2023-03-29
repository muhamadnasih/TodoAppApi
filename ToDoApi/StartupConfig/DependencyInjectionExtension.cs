using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoLibrary.DataAccess;

namespace ToDoApi.StartupConfig;

public static class DependencyInjectionExtension
{
    public static void AddstandardServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
       
    
    }
    public static void AddCustomServices(this WebApplicationBuilder builder)
    {

        //dependency injection
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ITodoData,TodoData>();

    }

    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
           .AddSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection"));

    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        //add authorization to the pipeline
        builder.Services.AddAuthorization(opts =>
        {

            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });


        //add authentication to the pipeline
        builder.Services.AddAuthentication("Bearer")
           .AddJwtBearer(opts =>
           {
               opts.TokenValidationParameters = new()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                   ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))

               };
           });
       

    }



}
