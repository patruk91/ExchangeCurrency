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
        private readonly string _uriString;
        private readonly string _requestUri;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _uriString = ApiBankConfiguration.GetUriLink(ApiBankConfiguration.UriToNbpApi);
            _requestUri = ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRates,
                TableNames.A.ToString());
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ExchangeDbEntities>(
                context => context.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var stringBuilder = new StringBuilder();
            var exchangeHelper = new ExchangeHelper();
            IExchange exchange = new Exchange(exchangeHelper, stringBuilder);

            var statusCode = exchange.GetStatusCode(_uriString, _requestUri).Result.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                ConfigureBankServices(services, exchange);
            }
            services.Add(new ServiceDescriptor(typeof(HttpStatusCode), statusCode));
        }

        private void ConfigureBankServices(IServiceCollection services,
                                                IExchange exchange)
        {
            IConversionDao conversionDao = new ConversionSql();
            var exchangeData = exchange.GetExchangeRatesData(_uriString, _requestUri).Result;
            var codeCurrencies = exchange.GetCodesForExchangeRates(exchangeData);
            services.Add(new ServiceDescriptor(typeof(IExchange), exchange));
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, int>), codeCurrencies));
            services.Add(new ServiceDescriptor(typeof(IConversionDao), conversionDao));
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
