using KA.Infrastructure.Enums;

using System.ComponentModel.DataAnnotations;


namespace KA.DataProvider.Entities
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public int? CartId { get; set; }

        // Giá tạm tính
        public decimal Price { get; set; }
        // Giá giảm (triết khấu)
        public decimal DiscountPrice { get; set; }
        // Thành tiền 
        public decimal TotalPrice { get; set; }
        // Phương thức thanh toán
        public PaymentMethod PaymentMethod { get; set; }
        // Ghi chú
        public string Note { get; set; }
        // Trạng thái order
        public OrderStatus OrderStatus { get; set; }
        // Trạng thái thanh toán
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        #region Relationship

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
        #endregion
    }
}
