using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Tnomer.Net.Http.Rest
{
    public class RestClientConfig
    {
        public RestClientConfig(String serviceResourceUrl)
        {
            ServiceResourceUrl = serviceResourceUrl;
        }

        public RestClientConfig(String serviceResourceUrl, IEnumerable<KeyValuePair<String, String>> headers)
        {
            ServiceResourceUrl = serviceResourceUrl;
            Headers = headers;
        }

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public String ServiceResourceUrl { get; }
        public String Token { get; }
        public IEnumerable<KeyValuePair<String, String>> Headers { get; } = new Collection<KeyValuePair<String, String>>();

    }
}
