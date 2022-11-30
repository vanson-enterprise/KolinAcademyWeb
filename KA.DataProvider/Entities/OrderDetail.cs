using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }

        #region Relationship

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        #endregion
    }
}