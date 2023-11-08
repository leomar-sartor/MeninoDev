using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Web.Mvc;

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

        [Required(ErrorMessage = "É necessário um título")]
        [Display(Name = "Título")]
        public string Title { get; set; }


        [Required(ErrorMessage = "É necessário um título")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Categoria")]
        public long CategoriaId { get; set; }

        //[AllowHtml]
        //[DataType(DataType.MultilineText)]
        [DisplayFormat(HtmlEncode = true)]
        [Display(Name = "Conteúdo")]
        public string? Content { get; set; }

        [Display(Name = "Imagem")]
        public string Url { get; set; }

        public long PostPage { get; set; }

        public List<Comment> Comments { get; set; }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
