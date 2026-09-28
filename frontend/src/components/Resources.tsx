import { useEffect, useState } from "react";
import styles from "./css/Resources.module.css";
const API_URL = import.meta.env.VITE_API_URL;

type Resource = {
    resourceId: number;
    resourceType: string;
};

type ResourcesProps = {
    selectedResourceType: string | null;
    onResourceTypeSelect: (resourceType: string) => void;
};

export default function Resources({
    selectedResourceType,
    onResourceTypeSelect
}: ResourcesProps) {
    const [resources, setResources] = useState<Resource[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function fetchResources() {
            try {
                const response = await fetch(
                    `${API_URL}/api/Resources`
                );

                if (!response.ok) {
                    throw new Error("Kunde inte hämta resurser.");
                }

                const data: Resource[] = await response.json();
                setResources(data);
            } catch (error) {
                console.error("Fel vid hämtning av resurser:", error);
            } finally {
                setLoading(false);
            }
        }

        fetchResources();
    }, []);

    const resourceTypes = [
        ...new Set(resources.map((resource) => resource.resourceType))
    ];

    if (loading) {
        return (
            <section className={styles.resourcesWrapper}>
                <div className={styles.heading}>
                    <p className={styles.eyebrow}>Resurser</p>
                </div>

                <div className={styles.placeholder}>
                    <p>Hämtar resurser...</p>
                </div>
            </section>
        );
    }

    function formatResourceType(resourceType: string) {
        switch (resourceType) {
            case "VRHeadset":
                return "VR Headset";
            case "AIServer":
                return "AI Server";
            default:
                return resourceType;
        }
    }

    return (
        <section className={styles.resourcesWrapper}>
            <div className={styles.heading}>
                <p className={styles.eyebrow}>Resurser</p>
            </div>

            <div className={styles.placeholder}>
                <div className={styles.resourceButtonWrapper}>
                    {resourceTypes.map((resourceType) => (
                        <button
                            key={resourceType}
                            type="button"
                            className={
                                selectedResourceType === resourceType
                                    ? styles.selected
                                    : ""
                            }
                            onClick={() => {
                                onResourceTypeSelect(resourceType);
                            }}
                        >
                            <span>{formatResourceType(resourceType)}</span>

                            <span
                                className={`${styles.selectCircle} ${
                                    selectedResourceType === resourceType
                                        ? styles.circleSelected
                                        : ""
                                }`}
                            >
                                {selectedResourceType === resourceType ? "✓" : ""}
                            </span>
                        </button>
                    ))}
                </div>
            </div>
        </section>
    );
}