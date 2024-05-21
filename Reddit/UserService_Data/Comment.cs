using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService_Data
{
    public class Comment
    {
        public string Content { get; set; }
        public int Upvote { get; set; }
        public int Downote { get; set; }

        public Comment(string content, int upvote, int downote)
        {
            Content = content;
            Upvote = upvote;
            Downote = downote;
        }

        public override string ToString()
        {
            return Content;
        }

    }
}
