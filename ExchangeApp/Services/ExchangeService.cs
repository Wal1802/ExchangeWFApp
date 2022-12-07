using ExchangeApp.Models;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Web.Helpers;

namespace ExchangeApp.Services
{
    public class ExchangeService
    {
        private const string _url = "https://www.bancentral.gov.do/Home/GetActualExchangeRate";
        private readonly HttpClient _Client;

        public ExchangeService()
        {
            _Client = new HttpClient();
        }

        public async Task<ResultModel> GetTodayExchange()
        {
            ResultModel result = null;

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_url);

            try
            {
                HttpResponseMessage response =  await _Client.SendAsync(httpRequestMessage);
                var resultModel= await response.Content.ReadFromJsonAsync<ResponseModel>();
                
                if(resultModel != null)
                    result = resultModel?.result;

            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        
        }
    }
}
