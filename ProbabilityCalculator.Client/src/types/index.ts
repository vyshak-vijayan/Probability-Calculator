export const CalculationType = {
  CombinedWith: "CombinedWith",
  Either: "Either",
} as const;

export type CalculationType =
  (typeof CalculationType)[keyof typeof CalculationType];

export interface CalculationRequest {
  probabilityA: number;
  probabilityB: number;
  calculationType: CalculationType;
}

export interface CalculationResult {
  result: number;
  calculationType: CalculationType;
  probabilityA: number;
  probabilityB: number;
}

export interface ApiError {
  statusCode: number;
  message: string;
  detail?: string;
}
