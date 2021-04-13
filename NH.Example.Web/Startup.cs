using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NH.Example.Web.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace NH.Example.Web
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NH.Example.Web", Version = "v1" });
            });

            services.AddSingleton<ISessionFactory>(serviceProvider =>
            {
                var configuration = new Configuration();
                configuration.DataBaseIntegration(db =>
                {
                    db.ConnectionString = "Data Source=NHExample.db";
                    db.Driver<SQLite20Driver>();
                    db.Dialect<SQLiteDialect>();
                    db.LogSqlInConsole = true;
                    db.LogFormattedSql = true;
                });

                var modelMapper = new ModelMapper();
                modelMapper.AddMapping<UserMapping>();

                var mappings = modelMapper.CompileMappingForAllExplicitlyAddedEntities();

                configuration.AddMapping(mappings);

                var sessionFactory = configuration.BuildSessionFactory();

                var schemaUpdate = new SchemaUpdate(configuration);
                schemaUpdate.Execute(true, true);

                return sessionFactory;
            });

            services.AddScoped<ISession>(serviceProvider =>
            {
                var sessionFactory = serviceProvider.GetService<ISessionFactory>();
                var session = sessionFactory.OpenSession();

                return session;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NH.Example.Web v1"));
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
