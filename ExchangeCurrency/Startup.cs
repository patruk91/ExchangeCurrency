using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer;
using ExchangeCurrency.AccessLayer.dao;
using ExchangeCurrency.AccessLayer.dao.sql;
using ExchangeCurrency.Model;
using ExchangeCurrency.Model.Enums;
using ExchangeCurrency.Model.ExchangeCurrency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeCurrency
{
    public class Startup
    {
        private static readonly string UriString = ApiBankConfiguration
            .GetUriLink(ApiBankConfiguration.UriToNbpApi);
        private static readonly string RequestUri = ApiBankConfiguration
            .GetRequestUri(ApiBankConfiguration.UriToExchangeRates,
            TableNames.A.ToString());
        private readonly IExchange _exchange;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var stringBuilder = new StringBuilder();
            var exchangeHelper = new ExchangeHelper();
            _exchange = new Exchange(exchangeHelper, stringBuilder);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ExchangeDbEntities>(
                context => context.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var statusCode = _exchange.GetStatusCode(UriString, RequestUri).Result.StatusCode;
            CodeCurrencies(services, statusCode);
            BankExchange(services, statusCode);
        }

        private void CodeCurrencies(IServiceCollection services, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                LoadCodeCurrencies(services, _exchange);
            }
            else
            {
                EmptyCodeCurrencies(services);
            }
        }

        private void BankExchange(IServiceCollection services, HttpStatusCode statusCode)
        {
            IConversionDao conversionDao = new ConversionSql();
            services.Add(new ServiceDescriptor(typeof(IExchange), _exchange));
            services.Add(new ServiceDescriptor(typeof(HttpStatusCode), statusCode));
            services.Add(new ServiceDescriptor(typeof(IConversionDao), conversionDao));
        }

        private void EmptyCodeCurrencies(IServiceCollection services)
        {
            var codeCurrencies = new Dictionary<string, int>();
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, int>), codeCurrencies));
        }

        private void LoadCodeCurrencies(IServiceCollection services,
                                                IExchange exchange)
        {
            var exchangeData = exchange.GetExchangeRatesData(UriString, RequestUri).Result;
            var codeCurrencies = exchange.GetCodesForExchangeRates(exchangeData);
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, int>), codeCurrencies));
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
