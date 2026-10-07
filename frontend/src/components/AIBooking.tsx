import { useState } from "react";

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

    return( 
        <section>
            <h2>AI Bokning</h2>

            <p>
                Beskriv vad du vill boka, till exempel: "Jag vill boka ett rum imorgon mellan 9-12."
            </p>

            <input type="text" value={question} onChange={(event) => setQuestion(event.target.value)} placeholder="Vad vill du boka? "/>
            <button type="button" onClick={handleAskAI} disabled={loading}> {loading ? "Letar..." : "Hitta bokning"} </button>
            {error && <p>{error}</p>}

            {suggestion && (
                <div> 
                    <h3>Bokningsförslag</h3>

                    <p>Resurs: {suggestion.intent.resourceType}</p>
                    <p>Datum: {suggestion.intent.date}</p>
                    <p>Tid: {suggestion.intent.startTime} - {suggestion.intent.endTime}</p>

                    <p>ResursID: {suggestion.resourceId}</p>
                </div>
            )}
        </section>
    )
}