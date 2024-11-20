using System.Text.Json.Serialization;

namespace FreshInventory.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    Vegetables,
    Grains,
    Spices,
    Dairy,
    Meat,
    Seafood,
    General
}
