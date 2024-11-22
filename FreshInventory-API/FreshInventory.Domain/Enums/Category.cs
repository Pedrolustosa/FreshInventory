using System.Text.Json.Serialization;

namespace FreshInventory.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    Vegetables = 1,
    Fruits = 2,
    Meat = 3,
    Dairy = 4,
    Grains = 5,
    Spices = 6,
    Other = 7
}
