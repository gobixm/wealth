export interface AddTransactionRequest {
  SecurityId: string;
  OperationType: "Buy" | "Sell";
  date: Date;
  lots: number;
  currencyId: number;
  pricePerLot: number;
  fee: number;
}
