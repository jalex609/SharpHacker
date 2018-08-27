using System;
namespace SharpHackerAPI.Models
{
    /// <summary>
    /// Represents a way to sort stories by
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// Most votes
        /// </summary>
        Top,
        /// <summary>
        /// Most votes + time sensitivity
        /// </summary>
        Best,
        /// <summary>
        /// Newest stories
        /// </summary>
        New
    }
}
