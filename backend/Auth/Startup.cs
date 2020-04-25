using Auth;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace WindowsAuthSpike
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // Identity Server config
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
                .AddInMemoryApiResources(IdentityServerConfig.Apis)
                .AddInMemoryClients(IdentityServerConfig.Clients)
#if DEBUG
                .AddDeveloperSigningCredential()
#endif
            ;

            services.AddScoped<IProfileService, ProfileService>();

            // End Identity Server config

            services.Configure<IISOptions>(c =>
            {
                c.AuthenticationDisplayName = "Windows";
                c.AutomaticAuthentication = false;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            // Identity Server config

            app.UseIdentityServer();

            // End Identity Server config

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
