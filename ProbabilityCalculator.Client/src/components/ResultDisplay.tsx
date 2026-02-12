import { useState } from "react";
import type { CalculationResult } from "../types";

interface Props {
    result: CalculationResult;
}

export default function ResultDisplay({ result }: Props) {
    const [animate, setAnimate] = useState(false);

    useState(() => {
        setAnimate(true);
        const t = setTimeout(() => setAnimate(false), 600);
        return () => clearTimeout(t);
    });

    const label =
        result.calculationType === "CombinedWith"
            ? "Combined With (P(A) × P(B))"
            : "Either (P(A) + P(B) − P(A)×P(B))";

    return (
        <div className={`result-card ${animate ? "result-animate" : ""}`}>
            <span className="result-label">{label}</span>
            <div className="result-values">
                <span className="result-input">P(A) = {result.probabilityA}</span>
                <span className="result-input">P(B) = {result.probabilityB}</span>
            </div>
            <div className="result-number">{result.result}</div>
        </div>
    );
}
