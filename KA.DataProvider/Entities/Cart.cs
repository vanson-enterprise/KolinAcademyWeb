using KA.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        public CartStatus CartStatus { get; set; } = CartStatus.PreOrder;

        #region Relationship
        public ICollection<CartProduct> CartProducts { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
        public Order Order { get; set; }
        #endregion
    }
}