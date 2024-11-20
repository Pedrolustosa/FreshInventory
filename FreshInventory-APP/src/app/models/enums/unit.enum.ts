export enum Unit {
  Kilogram = 0,
  Gram = 1,
  Liter = 2,
  Milliliter = 3,
  Piece = 4,
  Unit = 5
}

export const UnitLabels: Record<Unit, string> = {
  [Unit.Kilogram]: 'Kilogram',
  [Unit.Gram]: 'Gram',
  [Unit.Liter]: 'Liter',
  [Unit.Milliliter]: 'Milliliter',
  [Unit.Piece]: 'Piece',
  [Unit.Unit]: 'Unit'
};