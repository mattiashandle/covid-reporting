using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Karolinska.Application.Features.Queries;
using Karolinska.Application.Dtos;
using Karolinska.Application.Wrappers;
using Karolinska.Application.AutoMapper;

namespace Karolinska.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IQueryHandler<GetSupplierByIdQuery, SupplierDto>, GetSupplierByIdQueryHandler>();
          
            services.AddTransient<IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierDto[]>>, GetSuppliersQueryHandler>();

            services.AddTransient<IQueryHandler<GetProvidersQuery, PagedResponse<HealthcareProviderDto[]>>, GetProvidersQueryHandler>();

            var mapperConfig = new MapperConfiguration(cfg =>
                    cfg.AddMaps(new[] { typeof(SupplierProfile) })
                    );

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddDbContextPool<KarolinskaContext>(
            dbContextOptions => dbContextOptions
                .UseInMemoryDatabase("karolinska-in-memory"));

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddOpenApiDocument(options =>
            {
                options.Version = "1.0.0";
                options.Title = "Karolinska API";
            });

            //Log invalid model state
            services.PostConfigure<ApiBehaviorOptions>(options =>
            {
                var builtInFactory = options.InvalidModelStateResponseFactory;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var loggerFactory = context.HttpContext.RequestServices
                        .GetRequiredService<ILoggerFactory>();

                    var logger = loggerFactory.CreateLogger(context.ActionDescriptor.DisplayName ?? nameof(ApiBehaviorOptions));

                    var errors = context.ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);

                    logger?.LogWarning($"ModelState Invalid:'{string.Join("; ", errors)}'");

                    return builtInFactory(context);
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            else 
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseOpenApi();

            app.UseSwaggerUi3(settings =>
            {
                settings.DocumentPath = "/openapi.json";
                settings.Path = "/swagger";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

//builder.Services.AddOpenApiDocument(options =>
//{
//    options.Version = "1.0.0";
//    options.Title = "Access API";
//});


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwaggerUi3(settings =>
//    {
//        //settings.Path = "/swagger";
//        settings.DocumentPath = "/wwwroot/openapi.json";
//    });
//    app.UseOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();