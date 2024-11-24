export enum Unit {
  Kilogram = 1,
  Gram = 2,
  Liter = 3,
  Milliliter = 4,
  Piece = 5,
  Unit = 6
}

export const UnitLabels: Record<Unit, string> = {
  [Unit.Kilogram]: 'Kilogram',
  [Unit.Gram]: 'Gram',
  [Unit.Liter]: 'Liter',
  [Unit.Milliliter]: 'Milliliter',
  [Unit.Piece]: 'Piece',
  [Unit.Unit]: 'Unit'
};