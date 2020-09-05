using aspnetcore_azure_blobStorage.Options;
using aspnetcore_azure_blobStorage.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aspnetcore_azure_blobStorage
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
      // Cria um objeto com os valores atribuidos nas configurações
      // e coloca o objeto acessivel globalmente na aplicação
      var blobStorage = new BlobStorageOptions();
      Configuration.Bind(nameof(BlobStorageOptions), blobStorage);
      services.AddSingleton(blobStorage);

      services.AddScoped<IBlobStorageService, BlobStorageService>();
      services.AddAutoMapper(typeof(Startup));
      services.AddControllers();
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
