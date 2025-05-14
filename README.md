🛡️ Cybersecurity Awareness Chatbot (C# Console App)

Welcome to the Cybersecurity Awareness Bot — a friendly console chatbot that helps users stay safe online while delivering a sleek, interactive experience.

> “Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.”

---

📌 Features

This is **Part 2** of the project — expanding on the base chatbot from Part 1 by adding:

 🧠 Smart Interaction
- **Keyword detection** to understand user queries (e.g., *"passwords"*, *"phishing"*, *"safe browsing"*).
- **Sentiment recognition**: detects phrases like “I’m not okay” or “feeling sad” and replies empathetically.
- **Topic interest memory**: if a user shows curiosity (e.g., "I'm intrigued by phishing"), the bot asks if it’s their favorite. If so, it remembers!

🗨️ Natural Flow
- Handles follow-up phrases like “Tell me more” or “What about it?”
- Avoids repeating fallback messages after giving good answers.
- Smooth conversation transitions and friendly language.

 🎨 Enhanced Console UI
- **ASCII Art** splash screen with “LiZulu” branding.
- **Voice greeting** using a `.wav` file (plays on launch).
- **Color-coded text** for bot messages, warnings, and prompts.
- **Typing effects** and spacing for realistic responses.

---

🛠️ Tech Stack

- **Language:** C# (.NET Console Application)
- **Voice:** Uses `System.Media.SoundPlayer` for `.wav` playback
- **Memory & Flow:** Dictionary-based response system with user state tracking
- **UI:** ASCII art, color formatting with `Console.ForegroundColor`

---

🧩 Sample Topics You Can Ask

```text
- How are you?
- What's your purpose?
- What can I ask you about?
- Tell me about password safety
- I'm curious about phishing
- I'm worried
