using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Email
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

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddSingleton<EmailSender, EmailSender>();
            //services.Configure<FormOptions>(o => {
            //    o.ValueLengthLimit = int.MaxValue;
            //    o.MultipartBodyLengthLimit = int.MaxValue;
            //    o.MemoryBufferThreshold = int.MaxValue;
            //});

            services.AddControllers();

            //##################################################

            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
            });

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                //setupAction.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //setupAction.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("User", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "User API",
                    Version = "1",
                    Description = "Through this api, a user will be send mail",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "davidcloudobi@gmail.com",
                        Name = "David",
                        Url = new Uri("https://healthstation.ng/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                //var security = new Dictionary<string, IEnumerable<string>>
                //{
                //    {"Bearer", new string[0] }
                //};

                //setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "Jwt Authorization header using the bearer scheme",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey

                //});

            });
            //##########################################################




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {

                setupAction.SwaggerEndpoint("/swagger/User/swagger.json", "Test User");

                setupAction.RoutePrefix = "";

            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
