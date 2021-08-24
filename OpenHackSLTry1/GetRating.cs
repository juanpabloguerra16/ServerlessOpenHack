using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using IceCreamRating.Models;

namespace IceCreamRating
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ratingId/{Id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "ohjuguerra",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{Id}",
                PartitionKey = "{Id}")]
                Rating rating,
                
            ILogger log)
        {   
            
            log.LogInformation("processing request");
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
