using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace IdentityServer4.ConsoleApp.Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        public static async Task MainAsync(string[] args)
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:7791");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");

            await DoSimpleApi1Async(tokenClient);
            await DoArbitraryAsync(tokenClient);
            
        }

        private static async Task DoArbitraryAsync(TokenClient tokenClient)
        {
            var customParams = new Dictionary<string, string>
            {
                { "handler", "arbitrary-claims-service" },
                { "arbitrary-claims", "{'naguid':'1234abcd','In':'Flames'}" },
                { "arbitrary-scopes", "A quick brown fox" }
            };

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("arbitrary", customParams);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:7791/api/IdentityApi");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        private static async Task DoSimpleApi1Async(TokenClient tokenClient)
        {
            var customParams = new Dictionary<string, string>
            {
                 
            };

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1", customParams);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:7791/api/IdentityApi");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
