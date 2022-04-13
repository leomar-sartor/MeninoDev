using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeninoDev.Entidades
{
    public class Comment
    {
        public long Id { get; set; }
        public long PostId { get; set; }

        public DateTime Date { get; set; }
        public string Content { get; set; }
    }
}


