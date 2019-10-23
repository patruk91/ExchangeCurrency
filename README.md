# ExchangeCurrency

Using the available ASP.NET WebAPI and Entity Framework functionalities, prepare the application
providing methods for converting user-specified currencies according to current exchange rates. The services have
be made available through REST. To obtain current exchange rates use the available ones
API by the National Bank of Poland at http://api.nbp.pl.

The application must provide the following functionalities:
- list of available currencies on which conversions can be made (min 5)
- conversion based on the exchange rate for the following parameters: amount from which currency, to which currency
- providing current exchange rates for the currency list

In addition, all calls made on shared services should be stored in any database
and an external website with exchange rates.
You do not need a GUI for the described functionality, just provide sample requests to call
with the help of client REST or documentation describing how the services are named and what parameters should be transferred.
Alternatively, you can embed the Swagger tool or other into the application to build documentation for the shared API.

Examples of queries that we can use are:\
https://exchangecurrency2019.azurewebsites.net/exchange - displaying the list of available currencies,\
https://exchangecurrency2019.azurewebsites.net/exchange/rates - current exchange rates to the list of currencies,\
https://exchangecurrency2019.azurewebsites.net/exchange/150/EUR/NOK - exchange rates,\
https://exchangecurrency2019.azurewebsites.net/exchange/150/CZK/GBP - exchange rate conversion.\
To convert rates, you can use all the currencies available in the first link.
