# Glimmer: Development Roadmap

This roadmap outlines the iterative development process for Glimmer, ensuring a functional and sleek local chat interface with minimal resource usage.

## Phase 1: Boilerplate & Connectivity (The "Hello World" POC)
*   **Goal:** Establish the foundation and prove the streaming capability from the .NET 10 backend to the frontend.
*   **Tasks:**
    *   Initialize .NET 10 Minimal API project.
    *   Create a simple GET endpoint that streams dummy text via `IAsyncEnumerable`.
    *   Create a basic `index.html` with a fetch request to consume the stream and display it in the console.

## Phase 2: Echo MVP (Backend Integration)
*   **Goal:** Establish bidirectional communication.
*   **Tasks:**
    *   Implement a POST endpoint in the backend to receive JSON payloads.
    *   Update the backend to accept input and return a mirrored ("echo") response using streaming.
    *   Connect the frontend to this POST endpoint and display the echoed response in the browser.

## Phase 3: Input Interface
*   **Goal:** Enable user interaction.
*   **Tasks:**
    *   Add a text input (`<textarea>`) and a "Send" button to the UI.
    *   Wire up the frontend to capture input on click/Enter key.
    *   Pass the input from the frontend to the backend endpoint.

## Phase 3.5: Initial UI Styling
*   **Goal:** Apply core aesthetics.
*   **Tasks:**
    *   Integrate Tailwind CSS (CDN).
    *   Implement the base obsidian/charcoal dark-mode theme.
    *   Refine input box and button layout.

## Phase 4: Ollama Integration & Scrollable View
*   **Goal:** Full LLM connectivity.
*   **Tasks:**
    *   Update backend to proxy requests to the local Ollama API (http://localhost:11434).
    *   Implement proper streaming of real LLM response chunks.
    *   Add a scrollable message container to the frontend to handle chat history and long responses.

## Phase 5: Markdown Rendering & Final Polish
*   **Goal:** Finalize the experience.
*   **Tasks:**
    *   Integrate `marked.js` to parse markdown from the LLM.
    *   Integrate `highlight.js` for syntax highlighting of code snippets.
    *   Refine the obsidian/charcoal theme for message bubbles, code blocks, and overall typography.
    *   Ensure all components (input, scroll, themes) are responsive and optimized.
