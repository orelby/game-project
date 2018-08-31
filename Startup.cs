using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Hubs.TicTacToe;
using System.IO;


namespace GameProject
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddCors(options => options.AddPolicy("CorsPolicy", builder => {
            //     builder.AllowAnyMethod()
            //         .AllowAnyHeader()
            //         .AllowAnyOrigin()
            //         .AllowCredentials();
            // }));

            services.AddSignalR();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/") &&
                    !context.Request.Path.Value.StartsWith("/signalr/")) {
                        context.Request.Path = "/index.html";
                        await next();
                    }
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<AIHub>("/signalr/games/tic-tac-toe/ai-hub");
                routes.MapHub<PvPHub>("/signalr/games/tic-tac-toe/pvp-hub");
            });

            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            // app.UseCors("CorsPolicy");
        }
    }
}
