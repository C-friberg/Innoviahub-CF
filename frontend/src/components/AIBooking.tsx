import { useState } from "react";
import styles from "./css/AIBooking.module.css";

const API_URL = import.meta.env.VITE_API_URL; 

type BookingIntent = {
    resourceType: string;
    date: string;
    startTime: string; 
    endTime:string;
}; 

type AIBookingResponse = {
    intent: BookingIntent; 
    resourceId: number; 
}

export default function AIBooking(){
    const [question, setQuestion] = useState(""); 
    const [suggestion, setSuggestion] = useState<AIBookingResponse | null>(null); 
    const [loading, setLoading] = useState(false); 
    const [error, setError] = useState(""); 
    const [bookingLoading, setBookingLoading] = useState(false);
    const [bookingMessage, setBookingMessage] = useState(""); 

    async function handleConfirmBooking() {
        if(!suggestion) {
            return; 
        }

        const token = localStorage.getItem("token"); 

        if(!token) {
            setError("Du måste vara inloggad för att kunna boka.")
            return; 
        }

        setBookingLoading(true); 
        setError(""); 
        setBookingMessage(""); 

        try {
            const bookingData = {
                resourceId: suggestion.resourceId,
                startTime: `${suggestion.intent.date}T${suggestion.intent.startTime}`,
                endTime: `${suggestion.intent.date}T${suggestion.intent.endTime}`,
            }; 

            const response = await fetch(`${API_URL}/api/Bookings`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                }, 
                body: JSON.stringify(bookingData),
            }); 
            if(response.status === 401) {
                throw new Error("Du är inte inloggad eller din inloggning har gått ut"); 
            }

            if (response.status === 409) {
                throw new Error("Resursen hann bli bokad. Försök igen");
            }

            if(!response.ok) {
                const message = await response.text(); 
                throw new Error(message || "Bokning kunde inte genomföras")
            }

            setBookingMessage("Bokningen är genomförd"); 
            setSuggestion(null); 
            setQuestion(""); 
        } catch(error) {
            if (error instanceof Error) {
                setError(error.message); 
            } else {
                setError("Okänt fel uppstod.")
            }
        } finally {
            setBookingLoading(false); 
        }

    }

    async function handleAskAI() {
        if(!question.trim()) {
            setError("Skriv vad du vill boka"); 
            return; 
        }

        setLoading(true); 
        setError(""); 
        setSuggestion(null); 

        try {
            const response = await fetch(`${API_URL}/api/ai/chat`, {
                method: "POST", 
                headers: {
                    "Content-Type": "application/json", 
                },
                body: JSON.stringify({
                    question: question,
                }),
            });  
            /* if (!response.ok) {
                throw new Error("Kunde inte hitta ledig resurs"); 
            } */
           if (!response.ok) {
                const message = await response.text();

                console.error("AI endpoint error:", response.status, message);

                throw new Error(
                    `AI-anrop misslyckades (${response.status}): ${message}`);
}

            const data: AIBookingResponse = await response.json(); 
            setSuggestion(data); 
        } catch(error) {
            if(error instanceof Error) {
                setError(error.message); 
            } else {
                setError("Ett okänt fel uppstod"); 
            }
        }  finally {
            setLoading(false); 
        }
    }

    return (
        <section className={styles.aiBookingWrapper}>

            <div className={styles.header}>
                <div className={styles.icon}>
                    ✦
                </div>

                <div className={styles.headerText}>
                    <div className={styles.titleRow}>
                        <h2>AI-assistent</h2>

                        <span className={styles.beta}>
                            BETA
                        </span>
                    </div>

                    <p className={styles.description}>
                        Beskriv vad du vill boka så hjälper
                        AI-assistenten dig.
                    </p>
                </div>
            </div>


            <div className={styles.inputWrapper}>
                <input
                    className={styles.input}
                    type="text"
                    value={question}
                    onChange={(event) =>
                        setQuestion(event.target.value)
                    }
                    onKeyDown={(event) => {
                        if (event.key === "Enter" && !loading) {
                            handleAskAI();
                        }
                    }}
                    placeholder="T.ex. boka ett VR-headset imorgon mellan 09 och 12..."
                />

                <button
                    className={styles.sendButton}
                    type="button"
                    onClick={handleAskAI}
                    disabled={loading}
                >
                    {loading ? "Letar..." : "Skicka"}
                </button>
            </div>

            {error && (
                <div className={styles.error}>
                    {error}
                </div>
            )}


            {suggestion && (
                <div className={styles.suggestion}>

                    <h3>Bokningsförslag</h3>

                    <div className={styles.details}>

                        <div className={styles.detail}>
                            <span className={styles.label}>
                                Resurs
                            </span>

                            <span className={styles.value}>
                                {suggestion.intent.resourceType}
                            </span>
                        </div>


                        <div className={styles.detail}>
                            <span className={styles.label}>
                                Datum
                            </span>

                            <span className={styles.value}>
                                {suggestion.intent.date}
                            </span>
                        </div>


                        <div className={styles.detail}>
                            <span className={styles.label}>
                                Tid
                            </span>

                            <span className={styles.value}>
                                {suggestion.intent.startTime.slice(0, 5)}
                                {" – "}
                                {suggestion.intent.endTime.slice(0, 5)}
                            </span>
                        </div>

                    </div>


                    <button
                        className={styles.confirmButton}
                        type="button"
                        onClick={handleConfirmBooking}
                        disabled={bookingLoading}
                    >
                        {bookingLoading
                            ? "Bokar..."
                            : "Bekräfta bokning"}
                    </button>

                </div>
            )}


            {bookingMessage && (
                <div className={styles.success}>
                    {bookingMessage}
                </div>
            )}

        </section>
    );
}