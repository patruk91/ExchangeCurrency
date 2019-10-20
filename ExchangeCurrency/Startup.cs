using System.Text;
using ExchangeCurrency.Model;
using ExchangeCurrency.Model.Enums;
using ExchangeCurrency.Model.ExchangeCurrency;
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
            StringBuilder stringBuilder = new StringBuilder();
            ExchangeHelper exchangeHelper = new ExchangeHelper();
            IExchange exchange = new Exchange(exchangeHelper, stringBuilder);

            var uriString = ApiBankConfiguration.GetUriLink(ApiBankConfiguration.UriToNbpApi);
            var requestUri = ApiBankConfiguration.GetRequestUri(ApiBankConfiguration.UriToExchangeRates,
                TableNames.A.ToString());

            var exchangeData = exchange.GetExchangeRatesData(uriString, requestUri).Result;
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
