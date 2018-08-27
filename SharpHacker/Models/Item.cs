using System;
using Newtonsoft.Json;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a generic item in HackerRank API
    /// Inherited by Comment, Poll, PollPart, and Story
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Unique ID of item
        /// </summary>
        [JsonProperty("id")]
        public int ItemID {get; set;}

        /// <summary>
        /// Type of Item, for easy casting
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(ItemConverter))]
        public ItemType TypeItem { get; set; }

        /// <summary>
        /// Whether the item has been deleted (by user) from site
        /// </summary>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Whether the item is dead on site
        /// </summary>
        /// <value><c>true</c> if dead; otherwise, <c>false</c>.</value>
        [JsonProperty("dead")]
        public bool Dead { get; set; }

        public Item() {}

        /// <summary>
        /// Gets URL to access this item on news.ycombinator
        /// </summary>
        /// <returns>URL on news.ycombinator</returns>
        public string GetRealURL() {
            return "https://news.ycombinator.com/item?id=" + this.ItemID; 
        }

        /// <summary>
        /// Converts the type from API into enum ItemType
        /// </summary>
        internal class ItemConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ItemType);
            }

            /// <summary>
            /// Returns the correct ItemType
            /// </summary>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string type = (string)reader.Value;
                if (type.Equals("story") || type.Equals("job")) {
                    return ItemType.Story;
                } else if (type.Equals("comment")) {
                    return ItemType.Comment;
                } else if (type.Equals("poll")) {
                    return ItemType.Poll;
                } else {
                    return ItemType.PollPart;
                }

            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
