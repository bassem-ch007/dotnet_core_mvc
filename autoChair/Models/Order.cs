using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace autoChair.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Utilisateur")]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "Date de commande")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Numéro de commande")]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Display(Name = "Montant total")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nom du destinataire")]
        public string ShippingName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Adresse")]
        public string ShippingAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Ville")]
        public string ShippingCity { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Display(Name = "Code postal")]
        public string ShippingPostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Pays")]
        public string ShippingCountry { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Statut")]
        public string Status { get; set; } = "En attente";

        public ICollection<OrderItem>? OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        [Display(Name = "Commande")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Display(Name = "Produit")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }

        [Display(Name = "Prix unitaire")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }

}
