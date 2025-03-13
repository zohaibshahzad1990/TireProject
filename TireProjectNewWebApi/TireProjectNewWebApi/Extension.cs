using System;

namespace TireProjectNewWebApi
{
    public static class Extensions
    {
        public static string ReplaceDomain(this string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? createdURI))
            {
                if (createdURI is not null)
                {
                    UriBuilder uriBuilder = new UriBuilder(createdURI)
                    {
                        Scheme = Uri.UriSchemeHttp, // Changing to http
                        Host = "3.133.136.76",
                        Port = -1 // Removing the port
                    };
                    return uriBuilder.Uri.ToString();
                }
                else
                {
                    return url;
                }
                
            }
            else
            {
                return url;
            }

        }
    }
}
