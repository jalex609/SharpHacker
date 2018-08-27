using System;
namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a type of story on HackerNews
    /// </summary>
    public enum StoryType
    {
        /// <summary>
        /// Default story
        /// </summary>
        Default,
        /// <summary>
        /// Job posting
        /// </summary>
        Job,
        /// <summary>
        /// Ask HN post
        /// </summary>
        Ask,
        /// <summary>
        /// Show HN post
        /// </summary>
        Show
    }
}
