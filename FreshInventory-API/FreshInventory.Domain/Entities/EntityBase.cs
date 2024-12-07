namespace FreshInventory.Domain.Entities
{
    public abstract class EntityBase
    {
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        protected EntityBase()
        {
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
        public void SetCreatedDate()
        {
            if (CreatedDate == default)
            {
                CreatedDate = DateTime.UtcNow;
            }
        }
        public void UpdateTimestamp()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
