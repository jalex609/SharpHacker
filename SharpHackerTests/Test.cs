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
            Story s = await hn.FindStoryByID(17853129);
            List<Comment> comments = s.FindParentComments();
            Assert.AreEqual(s.Comments.Count, s.CommentCount);
            Assert.AreEqual(s.FindParentComments().Count, s.ParentCommentsID.Count);
        }
    }
}
