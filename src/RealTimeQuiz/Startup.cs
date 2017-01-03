using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace RealTimeQuiz
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder = builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthentication(
                opts => opts.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddSignalR(opts => opts.Hubs.EnableDetailedErrors = true);
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();


            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            app.UseStaticFiles();

            if (env.IsDevelopment() == false)
            {
                //Enforce HTTPS
                //Enable App Insights
                //Enable custom error pages
            }

            app.UseCookieAuthentication();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                ClientId = Configuration["AzureAd:ClientId"],
                Authority = Configuration["AzureAd:Authority"],
                CallbackPath = Configuration["AzureAd:CallbackPath"],
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false
                }
            });

            app.UseMvcWithDefaultRoute();

            //app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
