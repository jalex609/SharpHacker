using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a story post on HN
    /// </summary>
    public class Story : Item
    {
        /// <summary>
        /// Username that added the story
        /// </summary>
        [JsonProperty("by")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Comments in story
        /// </summary>
        [JsonProperty("descendants")]
        public int CommentCount { get; set; }

        /// <summary>
        /// Parent comments ID on story
        /// </summary>
        [JsonProperty("kids")]
        public List<int> ParentCommentsID { get; set; }

        /// <summary>
        /// Comments on story
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Number of votes the story has
        /// </summary>
        [JsonProperty("score")]
        public int StoryScore { get; set; }

        /// <summary>
        /// Creation time for story
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Title for story
        /// </summary>
        [JsonProperty("title")]
        public string StoryTitle { get; set; }

        /// <summary>
        /// Text for story (if includes self-post elements)
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [JsonProperty("url")]
        public string URL { get; set; }

        /// <summary>
        /// Type of the story (job, ask, show, default)
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StoryTypeConverter))]
        public StoryType Type { get; set; }

        public Story()
        {
        }

        /// <summary>
        /// Finds the parent-level comments
        /// </summary>
        public List<Comment> FindParentComments()
        {
            List<Comment> parentComments = new List<Comment>();
            foreach (Comment c in Comments) {
                if (c.ParentID == this.ItemID) {
                    parentComments.Add(c);
                }
            }
            return parentComments;
        }

        /// <summary>
        /// Converts the text of story type into StoryType enum
        /// </summary>
        internal class StoryTypeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(StoryType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string type = (string)reader.Value;
                if (type.Equals("job"))
                {
                    return StoryType.Job;
                }
                else
                {
                    return StoryType.Default;
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
