export interface WasteMetrics {
  totalWastage: number;
  spoiledProductsCount: number;
  totalWasteCost: number;
}

export interface FinancialMetrics {
  totalProfit: number;
  totalExpenses: number;
  netProfit: number;
}

export interface IngredientMetrics {
  totalEntries: number;
  totalAvailableStock: number;
  lowStockCount: number;
}

export interface DishMetrics {
  totalDishOutputs: number;
  mostPopularDish: string;
  leastPopularDish: string;
  mostProfitableDish: string;
  leastProfitableDish: string;
}

export interface CustomerMetrics {
  totalCustomersServed: number;
  repeatCustomers: number;
  averageSpendingPerCustomer: number;
}