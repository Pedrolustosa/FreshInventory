using FreshInventory.Domain.Enums;

namespace FreshInventory.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public Unit Unit { get; private set; }
        public decimal UnitCost { get; private set; }
        public Category Category { get; private set; }
        public int SupplierId { get; private set; }
        public Supplier Supplier { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsPerishable { get; private set; }
        public int ReorderLevel { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        private Ingredient() { }

        public Ingredient(
            string name,
            int quantity,
            Unit unit,
            decimal unitCost,
            Category category,
            int supplierId,
            DateTime purchaseDate,
            DateTime expiryDate,
            bool isPerishable,
            int reorderLevel)
        {
            SetName(name);
            SetQuantity(quantity);
            SetUnit(unit);
            SetUnitCost(unitCost);
            SetCategory(category);
            SetSupplier(supplierId);
            SetPurchaseDate(purchaseDate);
            SetExpiryDate(expiryDate);
            SetIsPerishable(isPerishable);
            SetReorderLevel(reorderLevel);
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public void ReduceQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity to reduce must be positive.");
            if (quantity > Quantity)
                throw new InvalidOperationException("Insufficient quantity to reduce.");
            Quantity -= quantity;
            UpdatedDate = DateTime.Now;
        }

        public void SetSupplier(int supplierId)
        {
            SupplierId = supplierId;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            Quantity = newQuantity;
        }

        private void UpdateTimestamp()
        {
            UpdatedDate = DateTime.Now;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
            UpdateTimestamp();
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            Quantity = quantity;
            UpdateTimestamp();
        }

        public void SetUnit(Unit unit)
        {
            Unit = unit;
            UpdateTimestamp();
        }

        public void SetUnitCost(decimal unitCost)
        {
            if (unitCost < 0)
                throw new ArgumentException("Unit cost cannot be negative.");
            UnitCost = unitCost;
            UpdateTimestamp();
        }

        public void SetCategory(Category category)
        {
            Category = category;
            UpdateTimestamp();
        }

        public void SetPurchaseDate(DateTime purchaseDate)
        {
            if (purchaseDate > DateTime.Now)
                throw new ArgumentException("Purchase date cannot be in the future.");
            PurchaseDate = purchaseDate;
            UpdateTimestamp();
        }

        public void SetExpiryDate(DateTime expiryDate)
        {
            if (expiryDate <= PurchaseDate)
                throw new ArgumentException("Expiry date must be after the purchase date.");
            ExpiryDate = expiryDate;
            UpdateTimestamp();
        }

        public void SetIsPerishable(bool isPerishable)
        {
            IsPerishable = isPerishable;
            UpdateTimestamp();
        }

        public void SetReorderLevel(int reorderLevel)
        {
            if (reorderLevel < 0)
                throw new ArgumentException("Reorder level cannot be negative.");
            ReorderLevel = reorderLevel;
            UpdateTimestamp();
        }
    }
}
