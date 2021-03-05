using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IO;
using System.IO.Compression;
using System.Text;
using wordmeister_api.Dtos;
using wordmeister_api.Entity;
using wordmeister_api.Helpers;
using wordmeister_api.Interfaces;
using wordmeister_api.Services;

namespace wordmeister_api
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
            services.AddCors();
            services.AddControllers();
            services.AddHttpClient<ISlackService, SlackService>();
            services.AddLogging(c => c.AddSerilog());

            services.AddSingleton<Serilog.ILogger>(x =>
            {
                var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

                return new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                //return new LoggerConfiguration().WriteTo.PostgreSQL(Configuration["Serilog:ConnectionString"], Configuration["Serilog:TableName"], autoCreateSqlTable: true).CreateLogger();
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "WordMeister API",
                    Version = "v1",
                    Description = "Sample interface for test service",
                });
                options.CustomSchemaIds(x => x.FullName);
            });

            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            services.AddEntityFrameworkNpgsql().AddDbContext<WordmeisterContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("MyWebApiConection"), npgsqlOptionsAction: sqlOptions =>
                 {
                     sqlOptions.EnableRetryOnFailure();
                 });
            });

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            // configure strongly typed settings object
            services.Configure<Appsettings>(Configuration.GetSection("AppSettings"));

            // JWT Authentication ayarlarý yapýlýyor.
            var key = Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            // configure DI for application services
            services.AddSingleton<ITranslateService, TranslateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWordService, WordService>();
            services.AddScoped<IWordAPIService, WordAPIService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddSingleton<IMapper, Mapper>();

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
            });

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
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.ConfigureCustomExceptionMiddleware();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Query result Service API"));

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                },
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles")),
                RequestPath = "/Files",
            });
        }
    }
}
