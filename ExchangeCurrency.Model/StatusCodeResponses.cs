namespace ExchangeCurrency.Model
{
    public static class StatusCodeResponses
    {
        public static string GetResponseMessage(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return "Bad request. Please, check your link.\n";
                case 403:
                    return "Forbidden. You don't have permission to access this resource.\n'";
                case 404:
                    return "Not found. Link can be broken or dead. Please, check your link.\n'";
                case 500:
                    return "Internal Server Error. Please, try again later\n";
                case 502:
                    return "Bad Gateway. Invalid response from the upstream server. Please try again later.\n";
                default:
                    return $"Error {statusCode}. Please contact by email.\n";
            }
        }
    }
}