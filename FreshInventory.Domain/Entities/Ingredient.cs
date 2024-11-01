﻿using FreshInventory.Domain.Enums;

namespace FreshInventory.Domain.Entities;

public class Ingredient
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required int Quantity { get; set; }

    public required Unit Unit { get; set; }

    public decimal UnitCost { get; set; }

    public Category Category { get; set; } = Category.General;

    public string Supplier { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsPerishable { get; set; } = true;

    public int ReorderLevel { get; set; } = 10;
}
