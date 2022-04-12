using System;

namespace MeninoDev.Entidades
{
    public class Post
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
