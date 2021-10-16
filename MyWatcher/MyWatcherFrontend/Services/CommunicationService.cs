using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
        public Task<HttpResponseMessage> DeleteItemRequest(string endpoint, object item);
        public Task<HttpResponseMessage> UpdateItemRequest(string endpoint, object item);
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
                    var content = new StringContent(jsonString.ToString(), Encoding.UTF8, "application/json");
                    return await _client.PostAsync(baseUrl + endpoint, content);
                case Method.DELETE:
                    /*
                    var request = new HttpRequestMessage
                    {
                        Content = new StringContent(_jsonSerializer.Serialize(item), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(baseUrl + endpoint)
                    };
                    return await _client.SendAsync(request);
                    */
                    return await sendMessage(baseUrl + endpoint, item, HttpMethod.Delete);
                case Method.PATCH:
                    return await sendMessage(baseUrl + endpoint, item, HttpMethod.Patch);
                default:
                    return null;
            }
        }

        private async Task<HttpResponseMessage> sendMessage(string endpoint, object item, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent(_jsonSerializer.Serialize(item), Encoding.UTF8, "application/json"),
                Method = method,
                RequestUri = new Uri(endpoint)
            };
            return await _client.SendAsync(request);
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

        public async Task<HttpResponseMessage> DeleteItemRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.DELETE);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateItemRequest(string endpoint, object item)
        {
            var response = await SendRequestToApi(endpoint, item, Method.PATCH);
            return response;
        }
    }
}