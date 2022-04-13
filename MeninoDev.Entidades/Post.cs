using System;
using System.Collections.Generic;

namespace MeninoDev.Entidades
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
        }
        public long Id { get; set; }

        public DateTime Date { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
