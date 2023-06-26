using Azure.Identity;
using LeadDBManagement.Repository;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var constring = builder.Configuration.GetSection("Endpoints:AppConfig").Value;
// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(constring)
        .ConfigureKeyVault(kvOptions =>
        {
            kvOptions.SetCredential(new DefaultAzureCredential());
        });

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{

    return new CosmosClient("AccountEndpoint=https:leaddataforpoc.documents.azure.com:443/;AccountKey=a2SoVZfUR9m1vGAghLjNhERPKIn44fE87UcYvvLVxylwOvPj347gdvqCFRSvMfUvuf26aumlMnk5ACDbNb570g==");
   
});
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddLogging();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
