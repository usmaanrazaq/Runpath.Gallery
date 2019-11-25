using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Runpath.Gallery.Domain.Models;
using Runpath.Gallery.Service;
using Runpath.Gallery.Service.External;
using Runpath.Gallery.Service.Mapper;

namespace Runpath.Gallery.WebApi
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
            services.AddControllers();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPhotoMapper, PhotoMapper>();
            services.AddScoped<IAlbumMapper, AlbumMapper>();
            services.AddScoped<IValidator, Validator>();
            services.AddHttpClient<IHttpClientCaller<Photo>, HttpClientCaller<Photo>>(client =>
            {
                client.BaseAddress = new System.Uri(Configuration["BaseUrl"]);
            });

            services.AddHttpClient<IHttpClientCaller<Album>, HttpClientCaller<Album>>(client =>
            {
                client.BaseAddress = new System.Uri(Configuration["BaseUrl"]);
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
