// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using net.hempux.kabuto.Bot;
using net.hempux.kabuto.database;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Options;
using net.hempux.kabuto.Teamsoptions;
using net.hempux.kabuto.VaultOptions;
using System.Collections.Concurrent;

namespace net.hempux.kabuto
{
    public class Startup
    {
    
        private NinjaApiv2 ninjaApi { get; }
        private SqliteEngine sqlite { get; }
        // Instantiate options in NinjaOptions
        public Startup(IConfiguration configuration)
        {

            KeyVaultOptions.Initialize(configuration);
            NinjaOptions.Initialize(configuration);
            TeamsbotOptions.Initialize(configuration);
            KeyVaultOptions.Initialize(configuration);
            Optionsvalidator.Validate(configuration);


            // Create Sqlite instance and initialize database
            sqlite = new SqliteEngine();
            sqlite.InitDb();
            
            
            ninjaApi = new NinjaApiv2();
            // Set references to the sqlite and ninjaApi objects 
            sqlite.setApi(ninjaApi);
            ninjaApi.setSqliteEngine(sqlite);
            // Init DB and run migrations 

            NinjaMessageBot.Init(ninjaApi);
            NinjaMessageBot.TokenInit();


        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddHttpClient().AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            services.AddSingleton<INinjaApiv2, NinjaApiv2>();
            // Create a global hashset for our ConversationReferences
            services.AddSingleton<ConcurrentDictionary<string, ConversationReference>>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, NinjaMessageBot>();

            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                // .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthorization()
                .UseStatusCodePages(
        "application/json", "Status code page, status code: {0}")
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });

        }
    }
}
