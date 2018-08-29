using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpHackerAPI.Models;

namespace SharpHackerAPI
{
    /// <summary>
    /// Represents a client that the user can use to query HN API
    /// </summary>
    public class SharpHacker
    {
        /// <summary>
        /// Client to query the API
        /// </summary>
        protected HttpClient client;

        /// <summary>
        /// URL Base endpoint for API
        /// </summary>
        private const string URLBase = "https://hacker-news.firebaseio.com/v0/";

        /// <summary>
        /// General item endpoint
        /// </summary>
        private const string ItemBase = URLBase + "item/";

        /// <summary>
        /// General user endpoint
        /// </summary>
        private const string UserBase = URLBase + "user/";

        /// <summary>
        /// Endpoint to find the max item
        /// </summary>
        private const string MaxItem = URLBase + "maxitem.json";

        /// <summary>
        /// Endpoint to find the top (up to ~ 500) stories
        /// </summary>
        private const string TopStories = URLBase + "topstories.json";

        /// <summary>
        /// Endpoint to find the new (up to ~ 500) stories
        /// </summary>
        private const string NewStories = URLBase + "newstories.json";

        /// <summary>
        /// Endpoint to find the best (up to ~ 500) stories
        /// </summary>
        private const string BestStories = URLBase + "beststories.json";

        /// <summary>
        /// Endpoint to find the new (up to ~ 200) Ask HN stories
        /// </summary>
        private const string AskStories = URLBase + "askstories.json";

        /// <summary>
        /// Endpoint to find the new (up to ~ 200) Show HN stories
        /// </summary>
        private const string ShowStories = URLBase + "showstories.json";

        /// <summary>
        /// Endpoint to find the new (up to ~ 200) job postings
        /// </summary>
        private const string JobStories = URLBase + "jobstories.json";

        /// <summary>
        /// Endpoint to find the new (up to ~ 200) updates to profiles
        /// </summary>
        private const string Updates = URLBase + "updates.json";

        /// <summary>
        /// Empty constructor for SharpHacker client
        /// </summary>
        public SharpHacker() {
            client = new HttpClient();
        }

        /// <summary>
        /// Used to make item request easily
        /// </summary>
        /// <param name="ID">Unique int identifier for item</param>
        private static string MakeItemRequest(int ID) {
            return ItemBase + ID + ".json";
        }

        /// <summary>
        /// Used to make user request easily
        /// </summary>
        /// <param name="ID">Unique username</param>
        private static string MakeUserRequest(string ID) {
            return UserBase + ID + ".json";
        }

        /// <summary>
        /// Used to find a general purpose item. Returned as specifc type
        /// so ItemType can be used to safely cast
        /// </summary>
        /// <param name="ItemID">Unique item ID.</param>
        public async Task<Item> FindItemByID(int ItemID) {
            using (var client = this.client) {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));
                Item item = JsonConvert.DeserializeObject<Item>(content);
                if (item.Dead || item.Deleted)
                {
                    return item;
                }
                else
                {
                    switch (item.TypeItem)
                    {
                        case ItemType.Story:
                            Story story = await SharpHacker.SetUpStory(JsonConvert.DeserializeObject<Story>(content));
                            return story;
                        case ItemType.Comment:
                            Comment comment = JsonConvert.DeserializeObject<Comment>(content);
                            return comment;
                        case ItemType.Poll:
                            Poll poll = await SharpHacker.SetUpPoll(JsonConvert.DeserializeObject<Poll>(content));
                            return poll;
                        case ItemType.PollPart:
                            PollPart pollPart = JsonConvert.DeserializeObject<PollPart>(content);
                            return pollPart;
                        default:
                            throw new ArgumentException("Item is deleted or nonexistent");
                    }
                }
            }
        }

        /// <summary>
        /// Private helper doing same as above but when HttpClient needs to be reused
        /// </summary>
        private async Task<Item> FindItemByID(int ItemID, HttpClient clientOld)
        {
            using (var client = clientOld)
            {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));   
                Item item = JsonConvert.DeserializeObject<Item>(content);
                if (item.Dead || item.Deleted)
                {
                    return item;
                }
                else
                {
                    switch (item.TypeItem)
                    {
                        case ItemType.Story:
                            Story story = await SharpHacker.SetUpStory(JsonConvert.DeserializeObject<Story>(content));
                            return story;
                        case ItemType.Comment:
                            Comment comment = JsonConvert.DeserializeObject<Comment>(content);
                            return comment;
                        case ItemType.Poll:
                            Poll poll = await SharpHacker.SetUpPoll(JsonConvert.DeserializeObject<Poll>(content));
                            return poll;
                        case ItemType.PollPart:
                            PollPart pollPart = JsonConvert.DeserializeObject<PollPart>(content);
                            return pollPart;
                        default:
                            throw new ArgumentException("Item is deleted or nonexistent");
                    }
                }
            }
        }

        /// <summary>
        /// Finds a specific user by ID
        /// </summary>
        /// <param name="UserID">Ussername of user</param>
        public async Task<User> FindUserByID(string UserID)
        {
            using (var client = this.client) {
                var content = await client.GetStringAsync(SharpHacker.MakeUserRequest(UserID));
                User user = await SharpHacker.SetUpUser(JsonConvert.DeserializeObject<User>(content));
                return user;
            }
        }


        /// <summary>
        /// Sets up <paramref name="user"/>:
        /// Fills in the SubmittedItems parameter
        /// </summary>
        private static async Task<User> SetUpUser(User user) {
            user.SubmittedItems = new List<Item>();
            SharpHacker sharp = new SharpHacker();
            foreach (int ID in user.SubmittedIDs) {
                user.SubmittedItems.Add(await sharp.FindItemByID(ID, new HttpClient()));
            }
            return user;
        } 


        /// <summary>
        /// Returns the story with the unique ID
        /// </summary>
        /// <returns>Story with unique ID.</returns>
        /// <param name="ItemID">Unique ID for story.</param>
        public async Task<Story> FindStoryByID(int ItemID)
        {
            using (var client = this.client)
            {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));
                Story story = await SharpHacker.SetUpStory(JsonConvert.DeserializeObject<Story>(content));
                return story;
            }
        }

        /// <summary>
        /// Sets up story:
        /// -Adds correct StoryType
        /// -Adds comments to story
        /// </summary>
        /// <returns>Newly set up story.</returns>
        /// <param name="story">Story to be changed.</param>
        private static async Task<Story>  SetUpStory(Story story) {
            Story storyWithType = SharpHacker.SetTypeCorrectly(story);
            if (storyWithType.ParentCommentsID == null) {
                storyWithType.ParentCommentsID = new List<int>();
            }
            storyWithType.Comments = await SharpHacker.GetComments(storyWithType.ParentCommentsID);
            return storyWithType;
        }

        /// <summary>
        /// Sets up poll:
        /// -Adds options to poll
        /// -Adds comments to Poll
        /// </summary>
        /// <returns>The new set-up poll.</returns>
        /// <param name="poll">Poll to be changed.</param>
        private static async Task<Poll> SetUpPoll(Poll poll) {
            Poll pollWithParts = await SharpHacker.SetParts(poll, new HttpClient());
            if (pollWithParts.ParentCommentsID == null)
            {
                pollWithParts.ParentCommentsID = new List<int>();
            }
            pollWithParts.Comments = await SharpHacker.GetComments(pollWithParts.ParentCommentsID);
            return pollWithParts;
        }

        /// <summary>
        /// Sets up the type for a story
        /// </summary>
        /// <returns>Story with type changed.</returns>
        /// <param name="story">Story to be changed.</param>
        private static Story SetTypeCorrectly(Story story)
        {
            if (story.Dead || story.Deleted || story.CreatedBy == null)
            {
                story.Type = StoryType.Default;
            }
            else
            {
                if (story.Type != StoryType.Job)
                {
                    if (story.StoryTitle.Contains("Ask HN:"))
                    {
                        story.Type = StoryType.Ask;
                    }
                    else if (story.StoryTitle.Contains("Show HN:"))
                    {
                        story.Type = StoryType.Show;
                    }
                    else
                    {
                        story.Type = StoryType.Default;
                    }
                }
            }
            return story;
        }

        /// <summary>
        /// Finds the comments for an article
        /// </summary>
        /// <returns>The comments for the article organized into lists of different threads.</returns>
        /// <param name="ParentComments">ParentComments of article</param>
        private static async Task<List<List<Comment>>> GetComments(List<int> ParentComments)
        {
            using (var client = new HttpClient())
            {
                List<List<Comment>> comments = new List<List<Comment>>();
                foreach (int ID in ParentComments)
                {
                    List<Comment> thread = new List<Comment>();
                    var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ID));
                    Comment comment = JsonConvert.DeserializeObject<Comment>(content);
                    if (comment.CommentChildrenID == null) {
                        comment.CommentChildrenID = new List<int>();
                    }
                    if (!comment.Dead && !comment.Deleted) {
                        thread.Add(comment);
                    }
                    List<Comment> childComments = await GetChildrenCommentsAsync(comment.CommentChildrenID);
                    thread.AddRange(childComments);
                    comments.Add(thread);
                }
                return comments;
            }
        }

        /// <summary>
        /// Gets the children comments of a thread.
        /// </summary>
        /// <returns>Children comments of a thread.</returns>
        /// <param name="ChildIDs">ID's of original children.</param>
        private static async Task<List<Comment>> GetChildrenCommentsAsync(List<int> ChildIDs)
        {
            using (var client = new HttpClient())
            {
                List<Comment> comments = new List<Comment>();
                foreach (int ID in ChildIDs)
                {
                    var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ID));
                    Comment comment = JsonConvert.DeserializeObject<Comment>(content);
                    if (comment.CommentChildrenID == null)
                    {
                        comment.CommentChildrenID = new List<int>();
                    }
                    if (!comment.Dead && !comment.Deleted)
                    {
                        comments.Add(comment);
                    }
                    List<Comment> childComments = await GetChildrenCommentsAsync(comment.CommentChildrenID);
                    comments.AddRange(childComments);
                }
                return comments;
            }
        }

        /// <summary>
        /// Gets the poll and sets the options object
        /// </summary>
        /// <returns>The poll with parts changed.</returns>
        /// <param name="poll">Poll to be changed.</param>
        /// <param name="oldClient">Old client to reuse.</param>
        private static async Task<Poll> SetParts(Poll poll, HttpClient oldClient)
        {
            using (var client = oldClient)
            {
                poll.Parts = new List<PollPart>();
                if(poll.PartsID == null) {
                    poll.PartsID = new List<int>();
                }
                foreach (int ID in poll.PartsID)
                {
                    var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ID));
                    PollPart pollPart = JsonConvert.DeserializeObject<PollPart>(content);
                    poll.Parts.Add(pollPart);
                }
                return poll;
            }
        }

        /// <summary>
        /// Find a specifc comment by ID
        /// </summary>
        /// <returns>Specifc comment by ID</returns>
        /// <param name="ItemID">Unique comment identifer.</param>
        public async Task<Comment> FindCommentByID(int ItemID)
        {
            using (var client = this.client)
            {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));
                Comment comment = JsonConvert.DeserializeObject<Comment>(content);
                return comment;
            }

        }

        /// <summary>
        /// Finds specific poll by ID
        /// </summary>
        /// <returns>The poll with unique identifier.</returns>
        /// <param name="ItemID">Unique poll identifier.</param>
        public async Task<Poll> FindPollByID(int ItemID)
        {
            using (var client = this.client)
            {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));
                Poll poll =  await SharpHacker.SetUpPoll(JsonConvert.DeserializeObject<Poll>(content));
                return poll;
            }
        }

        /// <summary>
        /// Finds a poll option by idenitifer
        /// </summary>
        /// <returns>The specific poll options.</returns>
        /// <param name="ItemID">Unique item identifier.</param>
        public async Task<PollPart> FindPollOptionByID(int ItemID)
        {
            using (var client = this.client)
            {
                var content = await client.GetStringAsync(SharpHacker.MakeItemRequest(ItemID));
                PollPart option = JsonConvert.DeserializeObject<PollPart>(content);
                return option;
            }
        }

        /// <summary>
        /// Finds max item currently on HN
        /// </summary>
        /// <returns>The object fo the max item.</returns>
        public async Task<Item> FindMaxItem() {
            using (var client = this.client) {
                var content = await client.GetStringAsync(MaxItem);
                return await FindItemByID(Int32.Parse(content));
            }
        }

        /// <summary>
        /// Finds recent stories sorted by a certain metric
        /// </summary>
        /// <returns>The amount of stories according to a certain metric.</returns>
        /// <param name="sortType">The type (new, best, top) to sort by.</param>
        /// <param name="amount">The amount of stories to fetch. If greater
        /// than the amount of stories in endpoint then it will just fetch all
        /// stories at that endpoint</param>
        public async Task<List<Story>> FindSortedStories(SortType sortType, int amount)
        {
            string storyBase = "";
            switch (sortType)
            {
                case SortType.Best:
                    storyBase = BestStories;
                    break;
                case SortType.New:
                    storyBase = NewStories;
                    break;
                case SortType.Top:
                    storyBase = TopStories;
                    break;
            }

            List<Story> retStories = new List<Story>();


            using (var client = this.client)
            {
                var content = await client.GetStringAsync(storyBase);
                int[] stories = JsonConvert.DeserializeObject<int[]>(content);
                if (amount > stories.Length) {
                    amount = stories.Length;
                }
                for (int i = 0; i < amount; i++)
                {
                    var storyJSON = await client.GetStringAsync(SharpHacker.MakeItemRequest(stories[i]));
                    retStories.Add(await SharpHacker.SetUpStory(JsonConvert.DeserializeObject<Story>(content)));
                }
                return retStories;
            }
        }

        /// <summary>
        /// Finds recent stories categorizd by a certain type
        /// </summary>
        /// <returns>Stories categorized by a certain type.</returns>
        /// <param name="storyType">Type of story (ask, show, job).</param>
        /// <param name="amount">Amount of stories, follows same as last method.</param>
        public async Task<List<Story>> FindTypeStories(StoryType storyType, int amount)
        {
            string storyBase = "";
            switch (storyType)
            {
                case StoryType.Ask:
                    storyBase = AskStories;
                    break;
                case StoryType.Show:
                    storyBase = ShowStories;
                    break;
                case StoryType.Job:
                    storyBase = JobStories;
                    break;
                case StoryType.Default:
                    storyBase = NewStories;
                    break;
                default:
                    break;
            }

            List<Story> retStories = new List<Story>();


            using (var client = this.client)
            {
                var content = await client.GetStringAsync(storyBase);
                int[] stories = JsonConvert.DeserializeObject<int[]>(content);
                if (amount > stories.Length)
                {
                    amount = stories.Length;
                }
                for (int i = 0; i < amount; i++)
                {
                    var storyJSON = await client.GetStringAsync(SharpHacker.MakeItemRequest(stories[i]));
                    retStories.Add(await SharpHacker.SetUpStory(JsonConvert.DeserializeObject<Story>(content)));
                }
                return retStories;
            }
        }
    }
}