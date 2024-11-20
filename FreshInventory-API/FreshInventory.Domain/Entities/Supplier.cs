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
        public bool Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        // Navigation properties
        public ICollection<Ingredient> Ingredients { get; private set; }

        private Supplier() { }

        public Supplier(string name, string address, string contactPerson, string email, string phone, string category, bool status)
        {
            SetName(name);
            Address = address;
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Category = category;
            Status = status;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public void Update(string name, string address, string contactPerson, string email, string phone, string category, bool status)
        {
            SetName(name);
            Address = address;
            ContactPerson = contactPerson;
            Email = email;
            Phone = phone;
            Category = category;
            Status = status;
            UpdatedDate = DateTime.Now;
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be null or empty.");
            Name = name;
        }
    }
}
