using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using XamNative.Core.Entities;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XamNative.Core.Services
{
    public class POIService : IPOIService
    {
        private const string GET_POIS = "http://poitestapp.azurewebsites.net/api/values/";
        private const string CREATE_POI = "http://poitestapp.azurewebsites.net/api/values/Create/";
        private const string DELETE_POI = "http://poitestapp.azurewebsites.net/api/values/Delete/";

        public async Task<List<PointOfInterest>> GetPOIListAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await httpClient.GetAsync(GET_POIS);
                if (response != null || response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<List<PointOfInterest>>(content);
                    return results;
                }
                else
                {
                    Console.Out.WriteLine("failed to fetch data, try again later");
                    return null;
                }
            }
        }

        public async Task<string> CreateOrUpdatePOIAsync(PointOfInterest poi)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new POIContractResolver();
            var poiJSON = JsonConvert.SerializeObject(poi, Formatting.Indented, settings);

            using (HttpClient httpClient = new HttpClient())
            {
                var jsonContent = new StringContent(poiJSON, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(CREATE_POI, jsonContent);
                if (response.IsSuccessStatusCode && response != null)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    return content;
                }
            }


            return null;
        }

        public async Task<string> DeletePOIAsync(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(DELETE_POI + id);
                if (response.IsSuccessStatusCode || response != null)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("One record deleted");
                    return content;
                }
            }

            return null;
        }
    }

    public interface IPOIService
    {
        Task<List<PointOfInterest>> GetPOIListAsync();
        Task<string> CreateOrUpdatePOIAsync(PointOfInterest poi);
        Task<string> DeletePOIAsync(int id);
    }


    public class POIContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }

}