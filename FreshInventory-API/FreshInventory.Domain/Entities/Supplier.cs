namespace FreshInventory.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string ContactInfo { get; private set; }
    public string Address { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }

    private Supplier() { }

    public Supplier(string name, string contactInfo, string address)
    {
        SetName(name);
        SetContactInfo(contactInfo);
        SetAddress(address);
        CreatedDate = DateTime.Now;
        UpdatedDate = DateTime.Now;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name cannot be null or empty.");

        Name = name;
        UpdateTimestamp();
    }

    public void SetContactInfo(string contactInfo)
    {
        if (string.IsNullOrWhiteSpace(contactInfo))
            throw new ArgumentException("Contact information cannot be null or empty.");

        ContactInfo = contactInfo;
        UpdateTimestamp();
    }

    public void SetAddress(string address)
    {
        Address = address;
        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        UpdatedDate = DateTime.Now;
    }
}
