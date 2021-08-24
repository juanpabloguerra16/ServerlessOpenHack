using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRating.Models
{

    public class Rating
    {
        public Guid Id { get; set; }
        [JsonProperty("UserId")]
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime Timestamp { get; set; }
        public string LocationName { get; set; }
        [JsonProperty("rating")]
        public int RatingValue { get; set; }
        public string UserNotes { get; set; }
    }


}
