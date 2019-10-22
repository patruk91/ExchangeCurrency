﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExchangeCurrency.AccessLayer;
using ExchangeCurrency.AccessLayer.dao;
using ExchangeCurrency.AccessLayer.dao.sql;
using ExchangeCurrency.Model.ExchangeCurrency;
using ExchangeCurrency.ModelExchangeCurrency;
using ExchangeCurrency.ModelExchangeCurrency.Enums;
using ExchangeCurrency.ModelExchangeCurrency.ExchangeCurrency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExchangeCurrency
{
    public class Startup
    {
        private readonly string _uriString =
            ApiBankConfiguration.GetUriLink(ApiBankConfiguration.UriToNbpApi);
        private readonly string _requestUriAllRates =
            ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRates);
        private readonly string _requestUriSingleRate =
            ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRate);
        private readonly IExchange _exchange;
        private readonly ApiConnections _apiConnections;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var exchangeHelper = new ExchangeHelper();
            _exchange = new Exchange(exchangeHelper);
            _apiConnections = new ApiConnections()
            {
                UriString = _uriString,
                RequestUriAllRates = _requestUriAllRates,
                RequestUriSingleRate = _requestUriSingleRate
            };
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ExchangeDbEntities>(
                context => context.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var statusCode = _exchange.GetStatusCode(_uriString, _requestUriAllRates).Result.StatusCode;
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
            ICurrencyDao currencyDao = new CurrencySql();
            services.Add(new ServiceDescriptor(typeof(IExchange), _exchange));
            services.Add(new ServiceDescriptor(typeof(HttpStatusCode), statusCode));
            services.Add(new ServiceDescriptor(typeof(IConversionDao), conversionDao));
            services.Add(new ServiceDescriptor(typeof(ICurrencyDao), currencyDao));
            services.Add(new ServiceDescriptor(typeof(ApiConnections), _apiConnections));
        }

        private void EmptyCodeCurrencies(IServiceCollection services)
        {
            var codeCurrencies = new Dictionary<string, int>();
            services.Add(new ServiceDescriptor(typeof(Dictionary<string, int>), codeCurrencies));
        }

        private void LoadCodeCurrencies(IServiceCollection services,
                                                IExchange exchange)
        {
            var exchangeData = exchange.GetExchangeRatesData(_uriString, _requestUriAllRates).Result;
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
