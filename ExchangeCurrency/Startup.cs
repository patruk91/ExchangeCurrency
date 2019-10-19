﻿using ExchangeCurrency.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeCurrency
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureBankServices(services);
        }

        private static void ConfigureBankServices(IServiceCollection services)
        {
            IExchange exchange = new Exchange();
            var exchangeData = exchange.GetExchangeRatesData(ApiBankConfiguration.UriStringToNbpApi,
                ApiBankConfiguration.RequestUriToGetCurrentExchangeRates).Result;
            var codeCurrencies = exchange.GetCodesForExchangeRates(exchangeData);
            services.Add(new ServiceDescriptor(typeof(IExchange), exchange));
            services.Add(new ServiceDescriptor(typeof(string), codeCurrencies));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
