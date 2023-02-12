using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Blogs
{
    public class BlogItem
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public string CreatedDate { get; set; }
        public string CreateUser { get; set; }
        public string BlogType { get; set; }

    }
}