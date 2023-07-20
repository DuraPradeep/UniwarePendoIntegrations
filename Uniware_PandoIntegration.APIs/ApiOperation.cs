using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace Uniware_PandoIntegration.APIs
{
    public class ApiOperation
    {
        public T Get<T>(string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128"),

            };

            HttpResponseMessage response = client.GetAsync(uri).Result;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
        public string Get(string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7128"),

            };

            HttpResponseMessage response = client.GetAsync(uri).Result;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Dispose();
            return response.Content.ReadAsAsync<string>().Result;
        }
		public T Get<T, t1, t2>(t1 id, t2 id1, string Key, string Key1, string uri)
		{
			HttpClient client = new HttpClient
			{
				BaseAddress = new Uri("https://localhost:7128")
			};
			HttpResponseMessage response = client.GetAsync(uri).Result;
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			response = client.GetAsync(uri + "?" + Key + "=" + id + "&&" + Key1 + "=" + id1).Result;
			client.Dispose();
			return response.Content.ReadAsAsync<T>().Result;
		}
	}
}
