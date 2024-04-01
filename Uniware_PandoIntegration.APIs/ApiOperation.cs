using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace Uniware_PandoIntegration.APIs
{




    public class ApiOperation
    {
        private readonly string t_BaseURL;
        public ApiOperation(string abc="")
        {
            t_BaseURL=abc; 
        }



        public T Get<T>(string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            client.Timeout = TimeSpan.FromMinutes(10);
            HttpResponseMessage response = client.GetAsync(uri).Result;
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
        public T Get<T>(string t1, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            client.Timeout = TimeSpan.FromMinutes(10);
            HttpResponseMessage response = client.GetAsync(uri + "?" + t1).Result;
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
        public T Get<T, t1>(t1 id, string Key, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(uri + "?" + Key + "=" + id).Result;
            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
        public string Get(string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            client.Timeout = TimeSpan.FromMinutes(10);
            HttpResponseMessage response = client.GetAsync(uri).Result;
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Dispose();
            return response.Content.ReadAsStringAsync().Result;
        }
		public T Get<T, t1, t2>(t1 id, t2 id1, string Key, string Key1, string uri)
		{
			HttpClient client = new HttpClient
			{
				BaseAddress = new Uri(t_BaseURL)
			};
			HttpResponseMessage response = client.GetAsync(uri).Result;
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			response = client.GetAsync(uri + "?" + Key + "=" + id + "&&" + Key1 + "=" + id1).Result;
			client.Dispose();
			return response.Content.ReadAsAsync<T>().Result;
		}
        public string Post1<T, U>(U Posted, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL)
            };
            client.Timeout = TimeSpan.FromMinutes(10);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = client.PostAsync<U>(uri, Posted, new JsonMediaTypeFormatter()).Result;
            client.Dispose();
            return response.Content.ReadAsStringAsync().Result;
        }
        //public string Post2<T,t1,t2>(t1 id, t2 id1, string Key, string Key1, string uri)
        //{
        //     HttpClient client = new HttpClient
        //    {
        //        BaseAddress = new Uri(t_BaseURL)
        //    };
        //    client.Timeout = TimeSpan.FromMinutes(10);

        //    //var httpContent = new StringContent("",Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = client.PostAsync(uri+"?"+ Key + "=" + id + "&&" + Key1 + "=" + id1, httpContent).Result;

        //    client.Dispose();
        //    return response.Content.ReadAsStringAsync().Result;
        //}

        public string Get1<T, t1, t2>(t1 id, t2 id1,  string Key, string Key1, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(uri + "?" + Key + "=" + id + "&&" + Key1 + "=" + id1).Result;
            client.Dispose();
            return response.Content.ReadAsStringAsync().Result;
        }


        public T Get<T, t1>(t1 id, string Key, string uri, string Token)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            HttpResponseMessage response = client.GetAsync(uri + "?" + Key + "=" + id).Result;
            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
        public T Get<T, t1, t2, t3>(t1 id, t2 id1, t3 id2, string Key, string Key1, string Key2, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(t_BaseURL),

            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(uri + "?" + Key + "=" + id + "&&" + Key1 + "=" + id1 + "&&" + Key2 + "=" + id2).Result;

            client.Dispose();
            return response.Content.ReadAsAsync<T>().Result;
        }
       

    }
}
