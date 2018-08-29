using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<List<Comment>> Comments { get; set; }

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
        /// Returns the comments at the top level (the parent comments)
        /// </summary>
        /// <returns>The parent comments.</returns>
        public List<Comment> FindParentComments()
        {
            List<Comment> parentComments = new List<Comment>();
            foreach (List<Comment> comments in Comments)
            {
                foreach (Comment c in comments)
                {
                    if (c.ParentID == this.ItemID)
                    {
                        parentComments.Add(c);
                    }
                }
            }
            return parentComments;
        }

        /// <summary>
        /// Flattens all of the comments into one list of comment
        /// </summary>
        /// <returns>The comments.</returns>
        public List<Comment> FlattenComments()
        {
            return this.Comments.SelectMany(x => x).ToList();
        }

        /// <summary>
        /// Returns a list of comments at each level (parent of thread comments are 0) for each thread
        /// </summary>
        /// <returns>A dictionary of levels to comments. The 0th level is comments that are directly on the thread
        /// while the other comments increase in level. Does this for each thread</returns>
        public List<Dictionary<int, List<Comment>>> LevelComments() {
            int currentParent = this.ItemID;
            List<Dictionary<int, List<Comment>>> levelComments = new List<Dictionary<int, List<Comment>>>();
            foreach (List<Comment> thread in this.Comments) {
                Dictionary<int, List<Comment>> threadComments = new Dictionary<int, List<Comment>>();
                Dictionary<int, int> IDToLevel = new Dictionary<int, int>();
                int level = 0;
                currentParent = this.ItemID;
                threadComments[level] = new List<Comment>();
                foreach (Comment c in thread) {
                    if (c.ParentID == this.ItemID) {
                        threadComments[0] = new List<Comment>();
                        threadComments[0].Add(c);
                        IDToLevel[c.ItemID] = 0;
                    } else {
                        level = IDToLevel[c.ParentID] + 1;
                        try  {
                            if (threadComments[level] == null) {
                                threadComments[level] = new List<Comment>(); 
                            }
                        } catch (KeyNotFoundException e) {
                            threadComments[level] = new List<Comment>();
                        }
                        IDToLevel[c.ItemID] = level;
                        threadComments[level].Add(c);
                    }
                }
                levelComments.Add(threadComments);
            }
            return levelComments;
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
