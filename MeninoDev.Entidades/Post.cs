using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        [AllowHtml]
        [Display(Name = "Conteúdo")]
        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
