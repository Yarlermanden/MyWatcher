using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using RestSharp;

namespace MyWatcherFrontend.Services
{
    public interface ICommunicationService
    {
        //public Task<IRestResponse> SendGetRequest(string endpoint, object item);
        public Task<HttpResponseMessage> SendGetRequest(string endpoint, object item);
        public Task PostItemRequest(string endpoint, object item);
    }
    
    public class CommunicationService : ICommunicationService
    {
        //private readonly RestClient _client;
        private readonly HttpClient _client;

        public CommunicationService()
        {
            //_client = new RestClient("http://localhost:5000");
            //_client = new HttpClient("http://localhost:5000");
            _client = new HttpClient();
        }
        
        //public async Task<IRestResponse> SendRequestToApi(string endpoint, object item, Method requestType)
        public async Task<HttpResponseMessage> SendRequestToApi(string endpoint, object item, Method requestType)
        {
            var request = new RestRequest("http://localhost:5000" + endpoint, requestType)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(item);
            //var response = await _client.ExecuteAsync(request);

            switch (requestType)
            {
                case Method.GET:
                    return await _client.GetAsync("https://localhost:5001" + endpoint);
                case Method.POST:
                    //return await _client.PostAsync()
                default:
                    return null;
            }
        }

        //public async Task<IRestResponse> SendGetRequest(string endpoint, object item)
        public async Task<HttpResponseMessage> SendGetRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.GET);
            return response;
        }

        public async Task PostItemRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.POST);
        }
    }
}