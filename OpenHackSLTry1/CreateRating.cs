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
using System.Net.Http;

namespace OpenHackSLTry1
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "ohjuguerra",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection")]
                IAsyncCollector<Rating> ratingitems,
            ILogger log)
        {
            log.LogInformation("Ratings HTTP trigger function processed a request.");

            IceCreamRating.Models.CreateRatingRequest createRating;

            try
            {
                createRating = JsonConvert.DeserializeObject<IceCreamRating.Models.CreateRatingRequest>(await req.ReadAsStringAsync());
            }
            catch {
                return new BadRequestObjectResult("Invalid Request");
            }

            if (createRating.UserId != null)
            {
                var client = new HttpClient();
                string userUrl = $"https://serverlessohapi.azurewebsites.net/api/GetUser?userId={createRating.UserId}";
                var response = await client.GetAsync(userUrl);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new BadRequestObjectResult("UserId is not valid");
                }
            }
            else 
            {
                return new BadRequestObjectResult("user Id not found");            
            }

            if (createRating.ProductId != null)
            {
                var client = new HttpClient();
                string productUrl = $"https://serverlessohapi.azurewebsites.net/api/GetProduct?productId={createRating.ProductId}";
                var response = await client.GetAsync(productUrl);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new BadRequestObjectResult("ProductId is not valid");
                }

            }
            else
            {
                return new BadRequestObjectResult("Product Id not found");
            }

            if (createRating.Rating < 0 || createRating.Rating > 5)
            {
                return new BadRequestObjectResult("rating must be between 0 and 5");
            }

            var rating = new Rating
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.Now,
                UserId = createRating.UserId,
                ProductId = createRating.ProductId,
                LocationName = createRating.LocationName,
                RatingValue = createRating.Rating,
                UserNotes = createRating.UserNotes,

            };

            await ratingitems.AddAsync(rating);

            return new OkObjectResult(rating);
        }
    }
}
