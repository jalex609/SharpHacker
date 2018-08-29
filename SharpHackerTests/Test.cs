using NUnit.Framework;
using SharpHackerAPI;
using SharpHackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpHackerTests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public async Task TestCase()
        {
            SharpHacker hn = new SharpHacker();
            Story s = (Story)(await hn.FindItemByID(17867863));
            List<Comment> comments = s.FindParentComments();
            List<Comment> flatten = s.FlattenComments();
            Assert.AreEqual(flatten.Count, s.CommentCount);
            Assert.AreEqual(s.FindParentComments().Count, s.Comments.Count);
        }
    }
}
