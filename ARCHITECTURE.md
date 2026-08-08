# Glimmer: Architectural Guidelines

## 1. Overview
Glimmer is a high-performance, ultra-lightweight chat interface designed to provide a seamless, low-resource experience for interacting with local Ollama instances. By avoiding heavy UI wrappers and complex build pipelines, Glimmer prioritizes system stability, minimal thermal footprint, and native performance.

## 2. Architectural Principles
*   **Minimalism:** Avoid bloatware, heavy frameworks, and unnecessary dependencies.
*   **Efficiency:** Leverage low-footprint technologies to ensure responsiveness on resource-constrained hardware (e.g., Lenovo E14).
*   **Asynchronous Streaming:** Maximize the use of non-blocking I/O to handle LLM token generation in real-time.
*   **Maintainability:** Clean, idiomatic .NET code and modular frontend components.

## 3. Technology Stack
*   **Backend:** .NET 10 Minimal API.
    *   *Rationale:* Native performance, low memory overhead, and seamless integration with C#'s `IAsyncEnumerable` for streaming.
*   **Frontend:** Vanilla JavaScript + Tailwind CSS (via CDN).
    *   *Rationale:* Eliminates the need for massive `node_modules` and complex build tools.
*   **Rendering:** `marked.js` and `highlight.js`.
    *   *Rationale:* Provides robust, lightweight Markdown and syntax highlighting.
*   **Engine:** Ollama (localhost:11434).

## 4. Backend Strategy (.NET 10)
*   **Proxy Pattern:** Act as a clean conduit between the frontend and the Ollama REST API.
*   **Streaming:** Implement streaming using `IAsyncEnumerable<string>` in endpoint handlers.
*   **Configuration:** Externalize model settings and thread constraints (OLLAMA_NUM_THREADS) to minimize hardcoded dependencies.

## 5. Frontend Strategy
*   **Style:** Permanent obsidian/charcoal theme using Tailwind utility classes.
*   **Interaction:** Simple `fetch` API calls with `ReadableStream` to process chunks from the .NET proxy.
*   **State Management:** Minimalist approach; maintain chat history in memory or local browser storage if persistence is required.

## 6. Performance Constraints
*   **Thermal Management:** Respect OLLAMA_NUM_THREADS settings to avoid thermal throttling.
*   **Resource Usage:** Aim for <100MB memory footprint for the combined backend/frontend lifecycle.
Glimmer_Architectural_Guidelines.md
Displaying Glimmer_Architectural_Guidelines.md.
