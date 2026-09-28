import { useState } from "react"
import styles from "./css/Input.module.css";

type InputProps = {
    label: string;
    type?: string;
    value: string;
    placeholder?: string;
    onChange: (value: string) => void;
};

export default function Input({
    label,
    type = "text",
    value,
    placeholder,
    onChange
}: InputProps) {

    const [showPassword, setShowPassword] = useState(false);

    return (
        <div className={styles.inputGroup}>
            <label className={styles.inputLabel}>
                {label}
            </label>

            <div className={styles.inputWrapper}>
                <input
                    className={styles.inputField}
                    type={
                        type === "password" && showPassword
                            ? "text"
                            : type
                    }
                    value={value}
                    placeholder={placeholder}
                    onChange={(event) => onChange(event.target.value)}
                />

                {type === "password" && (
                    <button
                        className={styles.passwordButton}
                        type="button"
                        onClick={() => setShowPassword(!showPassword)}
                    >
                        {showPassword ? "Dölj" : "Visa"}
                    </button>
                )}
            </div>
        </div>
    );
}