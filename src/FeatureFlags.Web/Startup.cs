using FeatureFlags.Web.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System.Security.Claims;

namespace FeatureFlags.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();

            //Set a retry for the service API for 3 times
            services.AddHttpClient<ServiceAPIClient>()
              .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(3));

            //Add DI for the service api client 
            services.AddScoped<IServiceAPIClient, ServiceAPIClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGitHub(options =>
            {
                options.ClientId = Configuration["GitHubOAuth:ClientId"];
                options.ClientSecret = Configuration["GitHubOAuth:ClientSecret"];
                options.Scope.Add("read:user");
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var accessToken = context.AccessToken;
                        var githubUser = /* Call GitHub API to get user info using accessToken */;
                        var identifier = githubUser.Id.ToString();
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, identifier),
                            new Claim(ClaimTypes.Name, githubUser.Name)
                        };
                        var identity = new ClaimsIdentity(claims, context.Scheme.Name);
                        context.Principal = new ClaimsPrincipal(identity);
                    }
                };
            });

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
