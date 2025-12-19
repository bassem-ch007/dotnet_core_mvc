using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace autoChair.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nom de la catégorie")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Relation : Une catégorie = plusieurs produits
        public ICollection<Product>? Products { get; set; }
    }
}
