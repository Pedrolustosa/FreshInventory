using System.Text.Json.Serialization;

namespace FreshInventory.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Unit
{
    Kilogram,
    Gram,
    Liter,
    Milliliter,
    Piece
}
