using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using UI.Controllers;
using Microsoft.AspNetCore.Authentication;
using UI.HttpHandlers;

namespace UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddHttpContextAccessor();
            services.AddHttpClient<ClientesController>();
            services.AddHttpClient<CiudadesController>();

            // Configuración de autorización basada en roles
            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("DebeSerLector", policy =>
                    policy.RequireRole("Lector"));

                authorizationOptions.AddPolicy("DebeSerEscritor", policy =>
                    policy.RequireRole("Escritor"));
            });

            //services.AddHttpContextAccessor();
            //services.AddTransient<BearerTokenHandler>();

            //// crea un HttpClient que se usa para acceder a la API
            //services.AddHttpClient("APIClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:44342/"); // URL para API.Lecturas
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            //}).AddHttpMessageHandler<BearerTokenHandler>();

            //// crea un HttpClient usado para acceder al IDP
            //services.AddHttpClient("IDPClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:44318/");
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            //});

            //// Configuración de autenticación con IdentityServer
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.AccessDeniedPath = "/Authorization/AccessDenied";
            //})
            //.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            //{
            //    options.Authority = "https://localhost:44318/";
            //    options.ClientId = "uiclient"; // Client ID actualizado
            //    options.ResponseType = "code";
            //    options.UsePkce = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("address");
            //    options.Scope.Add("roles");
            //    options.Scope.Add("apilecturasapi");
            //    options.Scope.Add("apiescriturasapi");
            //    options.Scope.Add("pais");
            //    options.Scope.Add("nivelsubscripcion");
            //    options.Scope.Add("offline_access"); //acceso al token de refresco
            //    options.SaveTokens = true;
            //    options.ClientSecret = "secret";

            //    options.ClaimActions.Remove("nbf");
            //    options.ClaimActions.DeleteClaim("sid");
            //    options.ClaimActions.DeleteClaim("idp");
            //    options.ClaimActions.DeleteClaim("s_hash");
            //    options.ClaimActions.DeleteClaim("auth_time");
            //    options.ClaimActions.DeleteClaim("address");

            //    options.ClaimActions.MapUniqueJsonKey("role", "role");
            //    options.ClaimActions.MapUniqueJsonKey("pais", "pais");
            //    options.ClaimActions.MapUniqueJsonKey("nivelsubscripcion", "nivelsubscripcion");

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "given_name",
            //        RoleClaimType = "role"
            //    };
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
