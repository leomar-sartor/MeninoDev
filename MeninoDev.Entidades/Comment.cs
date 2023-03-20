using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeninoDev.Entidades
{
    public class Comment
    {
        public Comment()
        {
            SubComments = new List<Comment>();
        }
        public long Id { get; set; }
        public long PostId { get; set; }

        public DateTime Date { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }
        public long CommentId { get; set; }
        public string CommentUser { get; set; }

        public List<Comment> SubComments { get; set; }
    }
}


