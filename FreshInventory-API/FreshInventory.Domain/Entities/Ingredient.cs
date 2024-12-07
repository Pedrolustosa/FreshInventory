namespace FreshInventory.Domain.Entities
{
    public class Ingredient : EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitCost { get; private set; }
        public int SupplierId { get; private set; }
        public Supplier Supplier { get; private set; }

        private Ingredient() { }

        public Ingredient(string name, int quantity, decimal unitCost, Supplier supplier)
        {
            SetName(name);
            SetQuantity(quantity);
            SetUnitCost(unitCost);
            SetSupplier(supplier);

            SetCreatedDate();
        }

        public void Update(string name, int quantity, decimal unitCost, Supplier supplier)
        {
            SetName(name);
            SetQuantity(quantity);
            SetUnitCost(unitCost);
            SetSupplier(supplier);

            UpdateTimestamp();
        }

        public void AdjustQuantity(int adjustment)
        {
            if (Quantity + adjustment < 0)
                throw new InvalidOperationException("Not enough stock for the adjustment.");

            Quantity += adjustment;
            UpdateTimestamp();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");

            Name = name;
        }

        private void SetQuantity(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Quantity = quantity;
        }

        private void SetUnitCost(decimal unitCost)
        {
            if (unitCost < 0)
                throw new ArgumentException("Unit cost cannot be negative.");

            UnitCost = unitCost;
        }

        private void SetSupplier(Supplier supplier)
        {
            Supplier = supplier ?? throw new ArgumentNullException(nameof(supplier));
            SupplierId = supplier.Id;
        }
    }
}
