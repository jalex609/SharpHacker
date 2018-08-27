﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a poll on HN
    /// </summary>
    public class Poll : Item
    {
        /// <summary>
        /// Author (username) of the poll
        /// </summary>
        [JsonProperty("by")]
        public string Author { get; set; }

        /// <summary>
        /// The amount of comments in the poll
        /// </summary>
        [JsonProperty("descendants")]
        public int CommentCount { get; set; }

        /// <summary>
        /// ID's for top level comments  
        /// </summary>
        [JsonProperty("kids")]
        public List<int> ParentCommentsID { get; set; }

        /// <summary>
        /// Comments on the poll
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// ID of parts on the poll
        /// </summary>
        [JsonProperty("parts")]
        public List<int> PartsID { get; set; }

        /// <summary>
        /// PollPart objects for all of the options in the poll
        /// </summary>
        public List<PollPart> Parts { get; set; }

        /// <summary>
        /// Votes on the poll
        /// </summary>
        [JsonProperty("score")]
        public int PollScore { get; set; }

        /// <summary>
        /// Text for the poll
        /// </summary>
        [JsonProperty("text")]
        public string Body { get; set; }

        /// <summary>
        /// Creation time for the poll
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Title of the poll
        /// </summary>
        [JsonProperty("title")]
        public String PollTitle { get; set; }

        /// <summary>
        /// Returns the comments at the top level (the parent comments)
        /// </summary>
        /// <returns>The parent comments.</returns>
        public List<Comment> FindParentComments()
        {
            List<Comment> parentComments = new List<Comment>();
            foreach (Comment c in Comments)
            {
                if (c.ParentID == this.ItemID)
                {
                    parentComments.Add(c);
                }
            }
            return parentComments;
        }

        /// <summary>
        /// Finds the index of the poll with the best score
        /// </summary>
        public int WinnerIndex() {
            int max = -1;
            int bestIndex = -1;
            for (int i = 0; i < Parts.Count; i++) {
                if (Parts[i].PollVotes > max) {
                    max = Parts[i].PollVotes;
                    bestIndex = i;
                }
            }
            return max;
        }

        /// <summary>
        /// Finds the text of the poll with the best score
        /// </summary>
        public string WinnerText()
        {
            string s = "";
            int max = 0;
            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i].PollVotes > max)
                {
                    max = Parts[i].PollVotes;
                    s = Parts[i].Text;
                }
            }
            return s;
        }

        /// <summary>
        /// Finds the votes of the poll with the best score
        /// </summary>
        public int WinnerVotes()
        {
            int max = 0;
            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i].PollVotes > max)
                {
                    max = Parts[i].PollVotes;
                }
            }
            return max;
        }

        public Poll()
        {
        }
    }
}
