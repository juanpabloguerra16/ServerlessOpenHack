using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IceCreamRating.Models;

namespace IceCreamRating
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "userid/{UserId}")] HttpRequest req,
            [CosmosDB(
                databaseName: "ohjuguerra",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{UserId}")]
                //PartitionKey = "{Id}")]
                Rating rating,
            ILogger log)

        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            
            if (rating == null)
            {
                
                return new NotFoundObjectResult("not found");
            }
            else
            {
                return new OkObjectResult(rating);
            }



        }
    }
}
