using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationsPOC
{
    class Program
    {
        const string BASE_URL = "https://api.parse.com";
        const string PUSH_RESOURCE_PATH = "/1/push";

        const string HEADER_APP_ID = "X-Parse-Application-Id";
        const string PARSE_APP_ID = "<Parse APP ID goes here>";
        const string HEADER_REST_API_KEY = "X-Parse-REST-API-Key";
        const string PARSE_REST_API_KEY = "<Parse REST API Key goes here>";

        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Program.BASE_URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            
            // The request
            var request = getPushNotificationToUserRequest("<TripFiles UserID goes here>", "Hello from BJ");

            // List data response.
            HttpResponseMessage response = client.SendAsync(request).Result;  // Blocking call!

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Push Successful!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Console.WriteLine("{0}", response.Content.ReadAsStringAsync().Result);
            }

            //Wait for enter
            Console.ReadLine();
        }

        static HttpRequestMessage getPushNotificationToUserRequest(string userId, string alert)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Program.PUSH_RESOURCE_PATH);
            request.Headers.Add(Program.HEADER_APP_ID, Program.PARSE_APP_ID);
            request.Headers.Add(Program.HEADER_REST_API_KEY, Program.PARSE_REST_API_KEY);
            var jsonData = new
            {
                where = new
                {
                    userId = userId
                },
                data = new
                {
                    alert = alert
                }
            };
            var serializedData = JObject.FromObject(jsonData).ToString();

            request.Content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            return request;
        }

        static HttpRequestMessage getPushNotificationToChannelsRequest(List<string> channels, string alert)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Program.PUSH_RESOURCE_PATH);
            request.Headers.Add(Program.HEADER_APP_ID, Program.PARSE_APP_ID);
            request.Headers.Add(Program.HEADER_REST_API_KEY, Program.PARSE_REST_API_KEY);
            var jsonData = new
            {
                channels = channels.ToArray(),
                data = new
                {
                    alert = alert
                }
            };
            var serializedData = JObject.FromObject(jsonData).ToString();

            request.Content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            return request;
        }
    }
}
