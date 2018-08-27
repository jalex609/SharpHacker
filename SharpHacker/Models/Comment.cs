using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a comment in the HackerNews API
    /// </summary>
    public class Comment : Item
    {
        /// <summary>
        /// Comment's author (username)
        /// </summary>
        [JsonProperty("by")]
        public string Author { get; set; }

        /// <summary>
        /// Represent's the children comments to this comments ID's
        /// </summary>
        [JsonProperty("kids")]
        public List<int> CommentChildrenID { get; set; }

        /// <summary>
        /// Represents the ID of parent (could be article or another comment)
        /// </summary>
        [JsonProperty("parent")]
        public int ParentID { get; set; }

        /// <summary>
        /// Text of the comment 
        /// </summary>
        /// <value>The body.</value>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Creation time of the comment
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// Returns amount of children comment has
        /// </summary>
        public int AmountChildren() {
            return CommentChildrenID.Count;
        }

        public Comment()
        {
        }

    }
}
