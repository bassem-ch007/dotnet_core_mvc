using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace autoChair.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, 1000000)]
        [Display(Name = "Prix (€)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "URL Image")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, 10000)]
        [Display(Name = "Stock disponible")]
        public int Stock { get; set; }

        // Clé étrangère
        [Required]
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        // Propriété de navigation
        [Display(Name = "Catégorie")]
        public Category? Category { get; set; }
    }
}
