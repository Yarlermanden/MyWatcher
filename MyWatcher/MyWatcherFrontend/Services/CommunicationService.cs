using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Serialization.Json;

namespace MyWatcherFrontend.Services
{
    public interface ICommunicationService
    {
        public Task<HttpResponseMessage> SendGetRequest(string endpoint, object item);
        public Task<HttpResponseMessage> PostItemRequest(string endpoint, object item);
    }
    
    public class CommunicationService : ICommunicationService
    {
        private readonly HttpClient _client;
        private JsonSerializer _jsonSerializer;

        public CommunicationService()
        {
            _client = new HttpClient();
            _jsonSerializer = new JsonSerializer();
        }
        
        public async Task<HttpResponseMessage> SendRequestToApi(string endpoint, object item, Method requestType)
        {
            var baseUrl = "http://localhost:5000";
            switch (requestType)
            {
                case Method.GET:
                    return await _client.GetAsync(baseUrl + endpoint);
                case Method.POST:
                    string jsonString = _jsonSerializer.Serialize(item);
                    Console.WriteLine(jsonString);
                    var content = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/json");
                    //var content = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/text");
                    return await _client.PostAsync(baseUrl + endpoint, content);
                default:
                    return null;
            }
        }

        public async Task<HttpResponseMessage> SendGetRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.GET);
            return response;
        }

        public async Task<HttpResponseMessage> PostItemRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.POST);
            return response;
        }
    }
}