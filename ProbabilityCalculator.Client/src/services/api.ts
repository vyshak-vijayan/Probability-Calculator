import type { CalculationRequest, CalculationResult, ApiError } from "../types";

const API_BASE = "http://localhost:5062";

export async function calculate(
    request: CalculationRequest
): Promise<CalculationResult> {
    const response = await fetch(`${API_BASE}/api/probability/calculate`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(request),
    });

    if (!response.ok) {
        const error: ApiError = await response.json().catch(() => ({
            statusCode: response.status,
            message: response.statusText,
        }));
        throw error;
    }

    return response.json();
}
