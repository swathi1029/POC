using System.Net.Http;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Newtonsoft.Json.Linq;


namespace RouteService
{
    public class RouteService
    {
        private static readonly HttpClient client = new HttpClient();
       
        [FunctionName("RouteService")]
        public static async Task RunAsync([ServiceBusTrigger("lead-data-queue", Connection = "LeadDataConnection")]string myQueueItem,
            ILogger log)        {
            try
            {
                var myContent = JsonConvert.SerializeObject(myQueueItem);
                var requestContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://scoreapipoc.azurewebsites.net/api/getscore", requestContent);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(myQueueItem);
                JObject scoreResponse = JObject.Parse(responseBody);
                jsonResponse.Merge(scoreResponse);
                var mergedJson = jsonResponse.ToString();
                if (requestContent!=null)
                {                    
                    var CosmosDbName = "LeadManagementDB";
                    var CosmosDbContainerName = "Leads";
                    var keyVaultUrl = Environment.GetEnvironmentVariable("VaultUri");
                    SecretClient sc = new(new Uri(keyVaultUrl), new DefaultAzureCredential());
                    KeyVaultSecret kvsecret = sc.GetSecret("leaddata-endpoint");
                    //DB connectionstring
                    //var cosmosdbEndpoint = "AccountEndpoint=https://leaddataforpoc.documents.azure.com:443/;AccountKey=a2SoVZfUR9m1vGAghLjNhERPKIn44fE87UcYvvLVxylwOvPj347gdvqCFRSvMfUvuf26aumlMnk5ACDbNb570g==";
                    var cosmosDbClient = new CosmosClient(kvsecret.Value);
                    var container = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);            
                    Leaddata data = Newtonsoft.Json.JsonConvert.DeserializeObject<Leaddata>(mergedJson);
                    var dataresponse = await container.CreateItemAsync(data, new PartitionKey(data.id));
                    Console.WriteLine(dataresponse.ToString());
                }
            }
            catch (HttpRequestException e)
            {

                log.LogError("\nException Caught!\n Message :{0} ", e.Message);
            }
            
        }
    }
}
