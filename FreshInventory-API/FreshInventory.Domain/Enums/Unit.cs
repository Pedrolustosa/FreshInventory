using System.Text.Json.Serialization;

namespace FreshInventory.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Unit
{
    Kilogram = 1,
    Gram = 2,
    Liter = 3,
    Milliliter = 4,
    Piece = 5,
    Unit = 6
}
