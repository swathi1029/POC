using LeadDBManagement.Model;
using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LeadDBManagement.Repository
{
    public class LeadRepository : ILeadRepository
    {
       private readonly IConfiguration _configuration;
        //public readonly string CosmosDbConnectionString = "AccountEndpoint=https://leaddataforpoc.documents.azure.com:443/;AccountKey=a2SoVZfUR9m1vGAghLjNhERPKIn44fE87UcYvvLVxylwOvPj347gdvqCFRSvMfUvuf26aumlMnk5ACDbNb570g==";
        public readonly string CosmosDbName = "LeadManagementDB";
        public readonly string CosmosDbContainerName = "Leads";
        
        public LeadRepository(IConfiguration configuration)
        {
            _configuration = configuration ;
        }
        
        private Microsoft.Azure.Cosmos.Container GetContainerClient()
        {
            var endpointref = _configuration["leaddb-endpoint"];
            var cosmosDbClient = new CosmosClient(endpointref);
            var container = cosmosDbClient.GetContainer(CosmosDbName, CosmosDbContainerName);
            return container;
        }

        public async Task<Leaddata> CreateLeadAsync(Leaddata lead)
        {
            if (string.IsNullOrEmpty(lead.id))
            {
                lead.id = Guid.NewGuid().ToString();
            }
            lead.id = lead.id.ToString();

            var container = GetContainerClient();
            var response = await container.CreateItemAsync(lead, new PartitionKey(lead.programId));
            return response.Resource;


        }
        public async Task<List<Leaddata>> GetAllLeadsAsync()
        {
            var container = GetContainerClient();
            var query = container.GetItemQueryIterator<Leaddata>(new QueryDefinition("SELECT * FROM c"));
            List<Leaddata> results = new List<Leaddata>();

            while (query.HasMoreResults)
            {
                Microsoft.Azure.Cosmos.FeedResponse<Leaddata> response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Leaddata> GetLeadByIdAsync(string programId)
        {
            var container = GetContainerClient();
            try
            {
                ItemResponse<Leaddata> response = await container.ReadItemAsync<Leaddata>(programId, new PartitionKey(programId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<Leaddata> UpdateLeadAsync(Leaddata lead)
        {
            var container = GetContainerClient();
            ItemResponse<Leaddata> response = await container.ReplaceItemAsync(lead, lead.programId, new PartitionKey(lead.programId));

            return response.Resource;
        }

        public async Task DeleteLeadAsync(string programId)
        {
            var container = GetContainerClient();
            await container.DeleteItemAsync<Leaddata>(programId, new PartitionKey(programId));
        }
    }
}
