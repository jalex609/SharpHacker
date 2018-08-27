using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents an option for a poll on HN
    /// </summary>
    public class PollPart : Item
    {
        /// <summary>
        /// The author (username) of the poll part
        /// </summary>
        /// <value>The author.</value>
        [JsonProperty("by")]
        public string Author { get; set; }

        /// <summary>
        /// ID of the poll the part is linked to
        /// </summary>
        /// <value>The poll identifier.</value>
        [JsonProperty("poll")]
        public int PollID { get; set; }

        /// <summary>
        /// Votes on this option
        /// </summary>
        /// <value>The poll votes.</value>
        [JsonProperty("score")]
        public int PollVotes { get; set; }

        /// <summary>
        /// Text of the poll
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Creation time of poll option
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreationTime { get; set; }

        public PollPart()
        {
        }
    }
}
