using KA.Infrastructure.Enums;

using System.ComponentModel.DataAnnotations;


namespace KA.DataProvider.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? CartId { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Note { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        #region Relationship

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}