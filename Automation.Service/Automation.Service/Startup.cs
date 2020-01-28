using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.DataProtection;
using AutoMapper;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;

using AutomationService.Data;
using AutomationService.Core.DataAccessAbstractions;
using AutomationService.Core;
using AutomationService.Data.Auth;
using Serilog;

namespace Automation.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            var mapingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mapper.Mapper());
            });

            IMapper mapper = mapingConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(sw =>
            {
                sw.SwaggerDoc("V1", new Info { Title = "Automation API", Version = "V1" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOData();

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            AddServiceDependencies(services);
        }

        private static void AddServiceDependencies(IServiceCollection services)
        {
            services.AddTransient<ITestScriptsRepo, TestScriptsRepo>();
            services.AddTransient<IGenericRepo<TestScripts>, GenericRepo<TestScripts>>();
            services.AddTransient<IGenericRepo<ModuleController>, GenericRepo<ModuleController>>();
            services.AddTransient<IGenericRepo<ModuleController_Map>, GenericRepo<ModuleController_Map>>();
            services.AddTransient<IGenericRepo<TestController>, GenericRepo<TestController>>();
            services.AddTransient<IGenericRepo<TestController_Map>, GenericRepo<TestController_Map>>();
            services.AddTransient<IGenericRepo<BrowserVMExec>, GenericRepo<BrowserVMExec>>();
            services.AddTransient<IGenericRepo<KeywordLibrary>, GenericRepo<KeywordLibrary>>();
            services.AddTransient<IGenericRepo<TestData>, GenericRepo<TestData>>();
            services.AddTransient<IGenericRepo<Repository>, GenericRepo<Repository>>();
            services.AddTransient<IGenericRepo<KeywordLibrary_Map>, GenericRepo<KeywordLibrary_Map>>();
            services.AddTransient<IGenericRepo<TestScripts_Map>, GenericRepo<TestScripts_Map>>();
            services.AddTransient<ITestControllerRepo, TestControllerRepo>();
            services.AddTransient<IKeywordEntityRepo, KeywordEntityRepo>();
            services.AddTransient<IRepositoryEntityRepo, RepositoryEntityRepo>();
            services.AddTransient<ITestDataRepo, TestDataRepo>();
            services.AddTransient<IAuthProvider, AuthProvider>();
            services.AddTransient<IGenericRepo<UserTable>, GenericRepo<UserTable>>();
            services.AddTransient<IDashboardRepo, DashboardRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(sw =>
            {
                sw.SwaggerEndpoint("/swagger/V1/swagger.json", "Automation API");
            });

            app.UseCors(MyAllowSpecificOrigins);
            //app.UseHttpsRedirection();
            app.UseMvc(routeBuilder => {
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Expand().Select().OrderBy().Filter();
            });
        }
    }
}
