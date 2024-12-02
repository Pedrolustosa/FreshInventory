namespace FreshInventory.Domain.Entities
{
    public class Supplier
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ContactPerson { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }
        public string Category { get; private set; }
        public bool Status { get; private set; } = true;
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public ICollection<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();

        private Supplier() { }

        public Supplier(string name, string contactPerson, string email, string phone, string address, string category, bool status)
        {
            SetName(name);
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Address = address;
            Category = category;
            Status = status;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Update(string name, string contactPerson, string email, string phone, string address, string category, bool status)
        {
            SetName(name);
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Address = address;
            Category = category;
            Status = status;
            UpdatedDate = DateTime.UtcNow;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
        }

        public void Deactivate()
        {
            if (!Status)
                throw new InvalidOperationException("Supplier is already inactive.");
            Status = false;
        }
    }
}
