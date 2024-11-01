using FreshInventory.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FreshInventory.Application.DTO;

public class IngredientCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public Unit Unit { get; set; }

    [Required(ErrorMessage = "UnitCost is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "UnitCost must be greater than zero.")]
    public decimal UnitCost { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Category Category { get; set; }

    [StringLength(100, ErrorMessage = "Supplier cannot exceed 100 characters.")]
    public string Supplier { get; set; }

    [Required(ErrorMessage = "PurchaseDate is required.")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    [Required(ErrorMessage = "ExpiryDate is required.")]
    [DataType(DataType.Date)]
    public DateTime ExpiryDate { get; set; }

    public bool IsPerishable { get; set; } = true;

    [Range(0, int.MaxValue, ErrorMessage = "ReorderLevel cannot be negative.")]
    public int ReorderLevel { get; set; } = 10;
}
