namespace FreshInventory.Domain.Entities
{
    public class Supplier : EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Contact { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public bool Status { get; private set; }

        public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();
        private readonly List<Ingredient> _ingredients = new();

        private Supplier() { }

        public Supplier(string name, string address, string contact, string email, string phone, bool status)
        {
            Update(name, address, contact, email, phone, status);
            SetCreatedDate();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            ArgumentNullException.ThrowIfNull(ingredient);

            if (_ingredients.Any(i => i.Id == ingredient.Id))
                throw new InvalidOperationException($"Ingredient with ID {ingredient.Id} is already associated with this supplier.");

            _ingredients.Add(ingredient);
            UpdateTimestamp();
        }

        public void RemoveIngredient(int ingredientId)
        {
            var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId)??throw new KeyNotFoundException($"Ingredient with ID {ingredientId} not found.");
            _ingredients.Remove(ingredient);
            UpdateTimestamp();
        }

        public void Update(string name, string address, string contact, string email, string phone, bool status)
        {
            SetName(name);
            SetAddress(address);
            SetContact(contact);
            SetEmail(email);
            SetPhone(phone);
            SetStatus(status);

            UpdateTimestamp();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
        }

        private void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be null or empty.");
            Address = address;
        }

        private void SetContact(string contact)
        {
            if (string.IsNullOrWhiteSpace(contact)) throw new ArgumentException("Contact cannot be null or empty.");
            Contact = contact;
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("Invalid email address.");
            Email = email;
        }

        private void SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone cannot be null or empty.");
            Phone = phone;
        }

        private void SetStatus(bool status)
        {
            Status = status;
        }
    }
}
