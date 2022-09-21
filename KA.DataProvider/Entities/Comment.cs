using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public  int ParentId { get; set; }
    }
}
