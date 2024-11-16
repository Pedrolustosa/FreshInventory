using FreshInventory.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FreshInventory.Application.DTO.IngredientDTO;

public record IngredientCreateDto(string Name,
                                  int Quantity,
                                  Unit Unit,
                                  decimal UnitCost,
                                  Category Category,
                                  string Supplier,
                                  [property: DataType(DataType.Date)] DateTime PurchaseDate,
                                  [property: DataType(DataType.Date)] DateTime ExpiryDate,
                                  bool IsPerishable,
                                  int ReorderLevel);

