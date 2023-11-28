using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API_CreditCard;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace API_CreditCard
{
    public class DBConfig
    {

       
       // public IConfiguration Configuration { get; }
        //public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<GeneratePaymentFee>();
            

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateCardPolicy", policy =>
                    policy.RequireAuthenticatedUser());
            });
        }
    }
}

           

            









