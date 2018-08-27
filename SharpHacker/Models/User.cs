using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a user on HN
    /// </summary>
    public class User
    {
        /// <summary>
        /// Username of user
        /// </summary>
        [JsonProperty("id")]
        public string Username { get; set; }

        /// <summary>
        /// Delay in comment posting and visibility to other users (if punished)
        /// </summary>
        [JsonProperty("delay")]
        public int Delay { get; set; }

        /// <summary>
        /// Creation time of account
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Karma (upvotes) the user has
        /// </summary>
        [JsonProperty("karma")]
        public int Karma { get; set; }

        /// <summary>
        /// Optional About space HTML that the user can set
        /// </summary>
        [JsonProperty("about")]
        public string AboutHTML { get; set; }

        /// <summary>
        /// ID's of submitted items
        /// </summary>
        [JsonProperty("submitted")]
        public List<int> SubmittedIDs { get; set; }

        /// <summary>
        /// Items the user has submitted 
        /// </summary>
        public List<Item> SubmittedItems { get; set; }


        /// <summary>
        /// Amount of items the user submitted
        /// </summary>
        public int AmountSubmitted() {
            return this.SubmittedIDs.Count;
        }

        /// <summary>
        /// Finds amount of each type of item the user has submitted
        /// </summary>
        public Dictionary<ItemType, int> AmountEachType() {
            Dictionary<ItemType, int> amountPerType = new Dictionary<ItemType, int>();
            amountPerType[ItemType.Comment] = 0;
            amountPerType[ItemType.Poll] = 0;
            amountPerType[ItemType.Story] = 0;
            foreach (Item item in this.SubmittedItems) {
                amountPerType[item.TypeItem] += 1;
            }
            return amountPerType;
        }

        /// <summary>
        /// Returns URL for actual users profile
        /// </summary>
        public string GetRealURL() {
            return "https://news.ycombinator.com/user?id=" + this.Username;
        }

        public User() { }


    }
}
