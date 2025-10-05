// wwwroot/js/speech.js
window.SpeechInterop = (function () {
    let recognition = null;
    let dotnetRef = null;

    function ensureApi() {
        const SR = window.SpeechRecognition || window.webkitSpeechRecognition;
        if (!SR) throw new Error("Web Speech API no soportada en este navegador.");
        return SR;
    }

    function buildRecognition(options) {
        const SR = ensureApi();
        const rec = new SR();
        rec.lang = options?.lang || "es-ES"; // o "es-CO" si prefieres
        rec.interimResults = options?.interimResults ?? true;
        rec.maxAlternatives = 1;
        // Si continuous=false, se detiene solo cuando detecta silencio (speechend).
        rec.continuous = options?.continuous ?? false;
        return rec;
    }

    function init(dotnetRefFromBlazor, options) {
        dotnetRef = dotnetRefFromBlazor;
        recognition = buildRecognition(options);

        recognition.onstart = () => {
            dotnetRef.invokeMethodAsync("OnRecognitionStarted");
        };

        recognition.onresult = (event) => {
            // Agrupa el último resultado
            let finalText = "";
            let interimText = "";

            for (let i = event.resultIndex; i < event.results.length; i++) {
                const res = event.results[i];
                if (res.isFinal) finalText += res[0].transcript;
                else interimText += res[0].transcript;
            }

            if (interimText) {
                dotnetRef.invokeMethodAsync("OnTranscriptPartial", interimText.trim());
            }
            if (finalText) {
                dotnetRef.invokeMethodAsync("OnTranscriptFinal", finalText.trim());
            }
        };

        // Dispara cuando el usuario deja de hablar y el motor corta
        recognition.onspeechend = () => {
            try { recognition.stop(); } catch { /* ignore */ }
        };

        recognition.onend = () => {
            dotnetRef.invokeMethodAsync("OnRecognitionEnded");
        };

        recognition.onerror = (e) => {
            dotnetRef.invokeMethodAsync("OnRecognitionError", e.error || "unknown");
        };
    }

    function start() {
        if (!recognition) throw new Error("Llama a init() primero.");
        recognition.start();
    }

    function stop() {
        if (!recognition) return;
        recognition.stop();
    }

    return { init, start, stop };
})();

async function speakText(text) {
    if (!text || !text.trim()) {
        console.error("❌ Texto vacío");
        return;
    }

    try {
        // Llamar al endpoint de la API .NET
        const resp = await fetch("https://api.linplatform.com/tts/api/voice", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ text })
        });

        if (!resp.ok) {
            console.error("❌ Error en respuesta:", resp.status);
            return;
        }

        // Recibir el audio como blob
        const blob = await resp.blob();

        // Crear un URL temporal y reproducirlo
        const audio = new Audio(URL.createObjectURL(blob));
        await audio.play();
    } catch (err) {
    }
}
