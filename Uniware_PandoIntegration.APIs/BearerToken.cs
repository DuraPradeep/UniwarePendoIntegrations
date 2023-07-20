using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uniware_PandoIntegration.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Security.Policy;
using Serilog;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Data;

namespace Uniware_PandoIntegration.APIs
{
    public class BearerToken
    {
        //private static readonly ILogger log = LogManager.GetLogger(typeof(SendEmail));
        public void CreateLog(string message)
        {
            Log.Information(message);
        }
        public async Task<Uniware_PandoIntegration.Entities.PandoUniwariToken> GetTokens()
        {
            PandoUniwariToken rootobject;
            try
            {
                CreateLog(" Token api Started");
                string URL = "https://sleepyhead.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";

                HttpClient _client = new HttpClient()
                {
                    BaseAddress = new Uri(URL)
                };
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = _client.GetAsync(URL).Result;
                var responses = response.Content.ReadAsStringAsync().Result;
                rootobject = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(responses);
                CreateLog($" Response:{responses}");
                if (response.IsSuccessStatusCode)
                {
                    return rootobject;

                }                

            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }

            return rootobject;
        }

        public async Task<ServiceResponse<string>> GetCode(string Details, string Token)
        {
           
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
             
                CreateLog(" Api saleOrder Search Started" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/saleOrder/search");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($" Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errdesc=await response.Content.ReadAsStringAsync();
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetCodeDetails(string Code, string Token)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {
                CreateLog(" Api saleorder get:- " + Code + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/saleorder/get");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Code, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCodeDetails(Code, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> GetSkuDetails(string SkuCode, string Token)
        {
            ServiceResponse<string> serviceResponse= new ServiceResponse<string>();
            try
            {
                CreateLog(" Api itemType_Get -" + SkuCode + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/catalog/itemType/get");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(SkuCode, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetSkuDetails(SkuCode, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> PostDataToDeliverypackList(List<Data> data)
        {
            var responses = "";
            //var jsondeserial = JsonSerializer.Deserialize<List<sendRoot>>(data);
            ServiceResponse<string> serviceResponse=new ServiceResponse<string>();

             var jsonre = JsonConvert.SerializeObject(new { data =data});
            string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
            CreateLog($" Post Data to Pando:-  {jsonre}");
            //var dejson=JsonConvert.DeserializeObject<Datum>(jsonre);
            try
            {
                var client = new HttpClient();
                var credentials = Encoding.ASCII.GetBytes(_credentials);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");
               
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);                
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");               
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);                    
                }
            }
            catch (Exception ex)
            {
                CreateLog("$ Error: " + ex.Message);
                throw ex;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> PostDataTomaterialinvoice(List<WaybillSend> data)
        {
            
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            var jsonre = JsonConvert.SerializeObject(new { data = data });
            string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
            CreateLog($" Way Bill Material Invoice Data:-  {jsonre}");
            
            try
            {
                var client = new HttpClient();
                var credentials = Encoding.ASCII.GetBytes(_credentials);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/order/material-invoice ");
                
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
             
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
             
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                   
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error:{ex.Message}");
                throw ex;
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> ReturnOrderGetCode(string Details, string Token)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {

                CreateLog("Return Order API Search Request" + Details + ": " + Token);
                var client = new HttpClient(); 
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/search");
                request.Headers.Add("Facility", "Hosur_Avigna");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($" Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> ReturnOrderGet(string Details, string Token)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {

                CreateLog("Return Order API Get Request" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://sleepyhead.unicommerce.com/services/rest/v1/oms/return/get");
                request.Headers.Add("Facility", "Hosur_Avigna");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($" Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> PostDataReturnOrderAPI(List<ReturnOrderSendData> data)
        {
            var responses = "";
            //var jsondeserial = JsonSerializer.Deserialize<List<sendRoot>>(data);
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            var jsonre = JsonConvert.SerializeObject(new { data = data });
            string _credentials = "system+demoduro@pando.ai:Pandowelcome@123";
            CreateLog($" Post Data of Return Order API:-  {jsonre}");
            //var dejson=JsonConvert.DeserializeObject<Datum>(jsonre);
            try
            {
                var client = new HttpClient();
                var credentials = Encoding.ASCII.GetBytes(_credentials);
                var request = new HttpRequestMessage(HttpMethod.Post, "https://duroflex-mitm.gopando.in/inbound/api/transactions/optima/delivery-picklist");
                //request.Headers.Add("Authorization", "Bearer" + Token);
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
                var content = new StringContent(jsonre, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response- : {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //PostDataToDeliverypaclList(data);
                }
            }
            catch (Exception ex)
            {
                CreateLog("$ Error: " + ex.Message);
                throw ex;
            }
            return serviceResponse;

        }
        public async Task<Uniware_PandoIntegration.Entities.PandoUniwariToken> GetTokensSTO()
        {
            PandoUniwariToken rootobject;
            try
            {
                CreateLog(" GetToken For STOWaybill");
                string URL = "https://stgsleepyhead.unicommerce.com/oauth/token?grant_type=password&client_id=my-trusted-client&username=analytics@mysleepyhead.com&password=Unisleepy@123";

                HttpClient _client = new HttpClient()
                {
                    BaseAddress = new Uri(URL)
                };
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = _client.GetAsync(URL).Result;
                var responses = response.Content.ReadAsStringAsync().Result;
                rootobject = JsonConvert.DeserializeObject<Uniware_PandoIntegration.Entities.PandoUniwariToken>(responses);
                CreateLog($" Response:{responses}");
                if (response.IsSuccessStatusCode)
                {
                    return rootobject;

                }

            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }

            return rootobject;
        }

        public async Task<ServiceResponse<string>> FetchingGetPassCode(string Details, string Token)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {

                CreateLog("STO WaybillGetPass Code" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/search");
                request.Headers.Add("Facility", "stgsleepyhead");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($" Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> FetchingGetPassElements(string Details, string Token)
        {

            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {

                CreateLog("STO WaybillGetPass Code" + Details + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://stgsleepyhead.unicommerce.com/services/rest/v1/purchase/gatepass/get");
                request.Headers.Add("Facility", "stgsleepyhead");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(Details, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                serviceResponse.Errcode = ((int)response.StatusCode);
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync(); ;
                CreateLog($" Response:{JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetCode(Details, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($" Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> GetSTOSkuDetails(string SkuCode, string Token)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                CreateLog("STO itemType_Get -" + SkuCode + ": " + Token);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://stgsleepyhead.unicommerce.com/services/rest/v1/catalog/itemType/get");
                request.Headers.Add("Authorization", "Bearer" + Token);
                var content = new StringContent(SkuCode, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                serviceResponse.ObjectParam = await response.Content.ReadAsStringAsync();
                CreateLog($" Response: {JsonConvert.SerializeObject(serviceResponse.ObjectParam)}");
                //return responses;
                if (response.IsSuccessStatusCode)
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    return serviceResponse;
                }
                else
                {
                    serviceResponse.Errcode = ((int)response.StatusCode);
                    //GetSkuDetails(SkuCode, Token);
                }
            }
            catch (Exception ex)
            {
                CreateLog($"Error: {ex.Message}");
                throw ex;
            }
            return serviceResponse;
        }


    }
}
