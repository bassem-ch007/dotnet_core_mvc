using System.ComponentModel.DataAnnotations;

namespace autoChair.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Display(Name = "Session ID")]
        public string SessionId { get; set; } = string.Empty;

        [Display(Name = "Produit")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }
    }
}
