import { useState, type FormEvent } from "react";
import { CalculationType } from "../types";
import type { CalculationResult, ApiError } from "../types";
import { calculate } from "../services/api";
import ResultDisplay from "./ResultDisplay";

function validateProbability(value: string, label: string): string | null {
    if (value.trim() === "") return `${label} is required.`;
    const n = Number(value);
    if (isNaN(n)) return `${label} must be a number.`;
    if (n < 0 || n > 1) return `${label} must be between 0 and 1.`;
    return null;
}

export default function Calculator() {
    const [probA, setProbA] = useState("");
    const [probB, setProbB] = useState("");
    const [calcType, setCalcType] = useState<CalculationType>(
        CalculationType.CombinedWith
    );

    const [result, setResult] = useState<CalculationResult | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const errorA = probA !== "" ? validateProbability(probA, "Probability A") : null;
    const errorB = probB !== "" ? validateProbability(probB, "Probability B") : null;

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();
        setError(null);
        setResult(null);

        const errA = validateProbability(probA, "Probability A");
        const errB = validateProbability(probB, "Probability B");

        if (errA || errB) {
            setError([errA, errB].filter(Boolean).join(" "));
            return;
        }

        setLoading(true);

        try {
            const res = await calculate({
                probabilityA: Number(probA),
                probabilityB: Number(probB),
                calculationType: calcType,
            });
            setResult(res);
        } catch (err: unknown) {
            const apiErr = err as ApiError;
            setError(apiErr?.detail ?? apiErr?.message ?? "An unexpected error occurred.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="calculator-wrapper">
            <div className="calculator-card">
                <div className="card-header">
                    <h1>Probability Calculator</h1>
                    <p className="subtitle">
                        Enter two probabilities (0–1) and choose an operation
                    </p>
                </div>

                <form onSubmit={handleSubmit} className="calc-form">
                    <div className="input-group">
                        <label htmlFor="probA">Probability A</label>
                        <input
                            id="probA"
                            type="number"
                            step="any"
                            min="0"
                            max="1"
                            placeholder="e.g. 0.5"
                            value={probA}
                            onChange={(e) => setProbA(e.target.value)}
                            className={errorA ? "input-error" : ""}
                        />
                        {errorA && <span className="field-error">{errorA}</span>}
                    </div>

                    <div className="input-group">
                        <label htmlFor="probB">Probability B</label>
                        <input
                            id="probB"
                            type="number"
                            step="any"
                            min="0"
                            max="1"
                            placeholder="e.g. 0.5"
                            value={probB}
                            onChange={(e) => setProbB(e.target.value)}
                            className={errorB ? "input-error" : ""}
                        />
                        {errorB && <span className="field-error">{errorB}</span>}
                    </div>

                    <fieldset className="operation-group">
                        <legend>Operation</legend>
                        <div className="radio-options">
                            <label
                                className={`radio-card ${calcType === CalculationType.CombinedWith ? "selected" : ""}`}
                            >
                                <input
                                    type="radio"
                                    name="calcType"
                                    value={CalculationType.CombinedWith}
                                    checked={calcType === CalculationType.CombinedWith}
                                    onChange={() => setCalcType(CalculationType.CombinedWith)}
                                />
                                <div className="radio-content">
                                    <strong>Combined With</strong>
                                    <span className="formula">P(A) × P(B)</span>
                                </div>
                            </label>

                            <label
                                className={`radio-card ${calcType === CalculationType.Either ? "selected" : ""}`}
                            >
                                <input
                                    type="radio"
                                    name="calcType"
                                    value={CalculationType.Either}
                                    checked={calcType === CalculationType.Either}
                                    onChange={() => setCalcType(CalculationType.Either)}
                                />
                                <div className="radio-content">
                                    <strong>Either</strong>
                                    <span className="formula">P(A) + P(B) − P(A)×P(B)</span>
                                </div>
                            </label>
                        </div>
                    </fieldset>

                    <button type="submit" className="calc-button" disabled={loading}>
                        {loading ? "Calculating…" : "Calculate"}
                    </button>
                </form>

                {error && (
                    <div className="error-banner">
                        <span className="error-icon">⚠</span>
                        {error}
                    </div>
                )}

                {result && <ResultDisplay result={result} />}
            </div>
        </div>
    );
}
