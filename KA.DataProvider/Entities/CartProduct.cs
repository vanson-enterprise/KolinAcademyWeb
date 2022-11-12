using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int CourseId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}