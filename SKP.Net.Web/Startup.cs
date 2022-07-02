using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SKP.Net.Core;
using SKP.Net.Services.Articles;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Categories;
using SKP.Net.Services.Customers;
using SKP.Net.Services.documents;
using SKP.Net.Services.Images;
using SKP.Net.Services.Messages;
using SKP.Net.Services.Orders;
using SKP.Net.Services.Security;
using SKP.Net.Services.SEO;
using SKP.Net.Storage.Common;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Framework.Mvc;
using SKP.Net.Web.Framework.Mvc.Routing;
using System;
using System.IO;
using System.Net;

namespace SKP.Net.Web
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
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = SkpAuthenticationDefaults.AuthenticationScheme;
                o.DefaultScheme = SkpAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = SkpAuthenticationDefaults.ExternalAuthenticationScheme;
            }).AddCookie(SkpAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.LoginPath = "/SignIn";
                o.Cookie.Name = "skp.test";
                o.Cookie.HttpOnly = true;
                //o.ClaimsIssuer = SkpAuthenticationDefaults.ClaimIssuer;
                o.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(ITableStorage<>), typeof(TableStorage<>));
            services.AddScoped<StorageAccountConnection>(m => new StorageAccountConnection(Configuration.GetValue<string>("TableStorage:Connection")));
            services.AddScoped<IBlobStorage, BlobStorage>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IEmailAccountService, EmailAccountService>();
            services.AddScoped<IMessageSender, MessageSender>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IProductServics, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IWorkContext, WebWorkContext>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerSettingService, CustomerSettingService>();
            services.AddSingleton<IRoutePublisher, RoutePublisher>();
            services.AddScoped<IUrlRecordService, UrlRecordService>();
            services.AddScoped(typeof(SlugRouteTransformer));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; ;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync(
                                                      "File error thrown!<br><br>\r\n");
                        }

                        await context.Response.WriteAsync(
                                                      "<a href=\"/\">Home</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512));
                    });
                });
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "areaRoute", pattern: "{area:exists}/{controller=Customers}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                var result=app.ApplicationServices.GetService<IRoutePublisher>();
                result.RegisterRoutes(endpoints);

            });
        }
    }
}
