using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CybersecurityChatbot
{
    class Program
    {
        static Dictionary<string, List<string>> topicResponses = new Dictionary<string, List<string>>()
        {
            { "password", new List<string>() {
                "Passwords should be long, unique, and contain a mix of characters.",
                "Consider using a password manager to keep your credentials safe.",
                "Avoid using personal info like birthdays or pet names in your passwords."
            }},
            { "phishing", new List<string>() {
                "Phishing emails often create a sense of urgency. Always double-check the sender's email.",
                "Never click suspicious links, even if they look official.",
                "Verify requests for personal information by contacting the organization directly."
            }},
            { "privacy", new List<string>() {
                "Review your privacy settings regularly and be cautious about what you share online.",
                "Use strong, unique passwords for different accounts.",
                "Be aware of what data you share on social media."
            }},
            { "safe browsing", new List<string>() {
                "Use HTTPS websites for secure browsing.",
                "Avoid downloading files from unknown or suspicious websites.",
                "Keep your browser and plugins up to date for better security."
            }},
            { "social engineering", new List<string>() {
                "Always verify identities before sharing sensitive info.",
                "Be cautious of unsolicited requests for personal data.",
                "Remember, if it sounds too good to be true, it probably is."
            }},
            { "two-factor", new List<string>() {
                "Enable two-factor authentication whenever possible.",
                "Use authenticator apps over SMS for better security.",
                "Keep your 2FA backup codes in a safe place."
            }},
            { "scam", new List<string>() {
                "Be cautious of scams asking for your personal or financial info.",
                "Always verify the identity of anyone requesting sensitive data.",
                "Remember, legitimate organizations will never ask for your password or PIN via email."
            }}
        };

        static Dictionary<string, string> staticPhrases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "how are you", "I'm running smoothly, thank you!" },
            { "what's your purpose", "I'm here to keep you safe and informed in the digital world." },
            { "what can i ask you about", "You can ask me about password safety, phishing, privacy, safe browsing, social engineering, two-factor, and scams." }
        };

        static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
        {
            { "positive", "I'm glad to hear that! Keep up the good cybersecurity habits." },
            { "negative", "I'm sorry to hear that. Remember, staying informed helps! You're doing great by asking questions." },
            { "overwhelmed", "That's understandable. Cybersecurity can be overwhelming, but take it step by step. I'm here to help." },
            { "unsure", "It's okay to feel unsure. Knowledge is power—ask me anything to learn more." }
        };

        static List<string> positiveWords = new List<string>() { "happy", "excited", "confident", "relieved", "glad" };
        static List<string> negativeWords = new List<string>() { "worried", "scared", "frustrated", "anxious", "nervous", "unsure" };
        static List<string> interestWords = new List<string>() { "interested", "intrigued", "curious", "know" };
        static List<string> overwhelmedWords = new List<string>() { "overwhelmed", "confused", "lost", "unsure" };

        static string userName = "";
        static string favoriteTopic = "";
        static string currentTopic = "";
        static string lastMentionedTopic = "";
        static Random rand = new Random();

        static void Main()
        {
            DisplayASCIIArt();
            PlayWelcomeMessage();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Please enter your name: ");
            Console.ResetColor();
            userName = Console.ReadLine().Trim();

            string welcomeMsg = $"Welcome to the Cybersecurity Bot {userName}";
            string borderLine = new string('*', welcomeMsg.Length + 4);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(borderLine);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"* {welcomeMsg} *");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(borderLine);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nAsk me about: password safety, phishing, privacy, safe browsing, social engineering, two-factor, and scams.");
            Console.ResetColor();

            // Possibly introduce favorite topic once at start
            if (!string.IsNullOrEmpty(favoriteTopic))
            {
                if (rand.NextDouble() < 0.5) // 50% chance
                {
                    TypeWrite($"By the way, I remember you’re interested in {favoriteTopic}. Would you like to hear more about it?", ConsoleColor.Yellow);
                    string reply = Console.ReadLine().ToLower().Trim();
                    if (reply == "yes" || reply == "y") GiveTopicResponse(favoriteTopic);
                }
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{userName}: ");
                Console.ResetColor();
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("I didn't catch that. Could you rephrase?");
                    continue;
                }
                if (input.ToLower().Contains("exit")) break;
                HandleUserInput(input);
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nOh well, sad to see you go, but I'm glad I helped you. See you later, alligator :)");
            Console.ResetColor();
        }

        static void HandleUserInput(string input)
        {
            string lower = input.ToLower();
            string normalized = NormalizeInput(input);

            // --- Exact "tell me more" ---

            if (lower.Trim() == "tell me more")
            {
                if (!string.IsNullOrEmpty(favoriteTopic))
                {
                    currentTopic = favoriteTopic;
                    lastMentionedTopic = favoriteTopic;
                    GiveTopicResponse(currentTopic);
                }
                else if (!string.IsNullOrEmpty(currentTopic))
                {
                    GiveTopicResponse(currentTopic);
                }
                else
                {
                    TypeWrite("Chatbot: Which topic would you like to learn more about? Please specify.", ConsoleColor.Red);
                }
                return;
            }

            // --- "I want to know more about ..." or "I want to learn about ..." ---

            if (lower.StartsWith("i want to know more about") || lower.StartsWith("i want to learn about"))
            {
                string topicStr = input.Substring(input.ToLower().IndexOf("about") + "about".Length).Trim();
                foreach (var topic in topicResponses.Keys)
                {
                    if (topicStr.ToLower().Contains(topic))
                    {
                        // Respond immediately
                        GiveTopicResponse(topic);
                        // Ask if it's favorite
                        TypeWrite($"Is {topic} your favorite? (yes/no)", ConsoleColor.Yellow);
                        string reply = Console.ReadLine().ToLower().Trim();
                        if (reply == "yes" || reply == "y")
                        {
                            favoriteTopic = topic;
                            TypeWrite($"Great! I'll remember that you're interested in {topic}.", ConsoleColor.Green);
                        }
                        else
                        {
                            TypeWrite($"Oh, my bad. What else would you like to know about?", ConsoleColor.Red);
                        }
                        return;
                    }
                }
            }

            // --- Detect specific topics mentioned in input ---

            var detectedTopics = topicResponses.Keys.Where(k => lower.Contains(k)).ToList();
            if (detectedTopics.Count > 0)
            {
                foreach (var t in detectedTopics)
                {
                    if (currentTopic != t) currentTopic = t;
                    lastMentionedTopic = t;
                    GiveTopicResponse(t);
                }
                return;
            }

            // --- "about it / about that / it" ---

            string[] aboutItPhrases = { "tell me about it", "about it", "about that", "it" };
            if (aboutItPhrases.Any(phrase => lower.Contains(phrase)))
            {
                if (!string.IsNullOrEmpty(lastMentionedTopic))
                {
                    currentTopic = lastMentionedTopic;
                    GiveTopicResponse(currentTopic);
                }
                else if (!string.IsNullOrEmpty(favoriteTopic))
                {
                    currentTopic = favoriteTopic;
                    GiveTopicResponse(currentTopic);
                }
                else
                {
                    TypeWrite("Chatbot: Which topic are you referring to? Please specify.", ConsoleColor.Red);
                }
                return;
            }

            // --- "more about" / "tell me more about" / "know more about" ---

            string[] moreAboutPhrases = { "more about", "know more about", "tell me more about" };
            if (moreAboutPhrases.Any(phrase => lower.Contains(phrase)))
            {
                bool foundTopic = false;
                foreach (var t in topicResponses.Keys)
                {
                    if (lower.Contains(t))
                    {
                        currentTopic = t;
                        lastMentionedTopic = t;
                        GiveTopicResponse(t);
                        foundTopic = true;
                        break;
                    }
                }
                if (!foundTopic)
                {
                    if (!string.IsNullOrEmpty(currentTopic))
                    {
                        lastMentionedTopic = currentTopic;
                        GiveTopicResponse(currentTopic);
                    }
                    else if (!string.IsNullOrEmpty(favoriteTopic))
                    {
                        currentTopic = favoriteTopic;
                        lastMentionedTopic = favoriteTopic;
                        GiveTopicResponse(currentTopic);
                    }
                    else
                    {
                        TypeWrite("Chatbot: Which topic would you like to learn more about? Please specify.", ConsoleColor.Red);
                    }
                }
                return;
            }

            // --- "I'm interested in ..." or "I'm curious about ..." ---

            if (lower.Contains("interested in") || lower.Contains("curious about"))
            {
                foreach (var t in topicResponses.Keys)
                {
                    if (lower.Contains($"interested in {t}") || lower.Contains($"about {t}"))
                    {
                        currentTopic = t;
                        lastMentionedTopic = t;
                        TypeWrite($"Chatbot: I see you're interested in {t}. Would you like to know more?", ConsoleColor.Yellow);
                        string reply = Console.ReadLine().ToLower().Trim();
                        if (reply == "yes") GiveTopicResponse(t);
                        return;
                    }
                }
            }

            // --- "tell me more" (no specific topic) ---

            if (lower.Trim() == "tell me more")
            {
                if (!string.IsNullOrEmpty(favoriteTopic))
                {
                    currentTopic = favoriteTopic;
                    lastMentionedTopic = favoriteTopic;
                    GiveTopicResponse(currentTopic);
                }
                else if (!string.IsNullOrEmpty(currentTopic))
                {
                    GiveTopicResponse(currentTopic);
                }
                else
                {
                    TypeWrite("Chatbot: Which topic would you like to learn more about? Please specify.", ConsoleColor.Red);
                }
                return;
            }

            // --- "I want to know more about ..." (explicitly) ---

            if (lower.StartsWith("i want to know more about") || lower.StartsWith("i want to learn about"))
            {
                string topicStr = input.Substring(input.ToLower().IndexOf("about") + "about".Length).Trim();
                foreach (var t in topicResponses.Keys)
                {
                    if (topicStr.ToLower().Contains(t))
                    {
                        // Ask if favorite
                        TypeWrite($"Is {t} your favorite? (yes/no)", ConsoleColor.Yellow);
                        string reply = Console.ReadLine().ToLower().Trim();
                        if (reply == "yes" || reply == "y")
                        {
                            favoriteTopic = t;
                            TypeWrite($"Great! I'll remember that you're interested in {t}.", ConsoleColor.Green);
                        }
                        return;
                    }
                }
            }

            // --- Sentiment detection ---

            if (negativeWords.Any(w => lower.Contains(w)))
            {
                TypeWrite($"Chatbot: {sentimentResponses["negative"]}", ConsoleColor.Magenta);
                return;
            }
            if (positiveWords.Any(w => lower.Contains(w)))
            {
                TypeWrite($"Chatbot: {sentimentResponses["positive"]}", ConsoleColor.Yellow);
                return;
            }
            if (overwhelmedWords.Any(w => lower.Contains(w)))
            {
                TypeWrite($"Chatbot: {sentimentResponses["overwhelmed"]}", ConsoleColor.Magenta);
                return;
            }

            // --- Randomly mention favorite topic once ---

            if (!string.IsNullOrEmpty(favoriteTopic) && rand.NextDouble() < 0.05)
            {
                TypeWrite($"By the way, as someone interested in {favoriteTopic}, you might want to review the latest tips.", ConsoleColor.Yellow);
                GiveTopicResponse(favoriteTopic);
            }

            // fallback
            TypeWrite("Chatbot: I didn't understand that. Could you rephrase?", ConsoleColor.Red);
        }

        static string NormalizeInput(string input)
        {
            var charsToRemove = new char[] { '.', ',', '!', '?', ';', ':', '\'', '\"' };
            foreach (var c in charsToRemove)
            {
                input = input.Replace(c.ToString(), "");
            }
            return input.ToLower().Trim();
        }

        static string ExtractName(string input)
        {
            string lower = input.ToLower();
            if (lower.Contains("my name is"))
            {
                int idx = lower.IndexOf("my name is") + "my name is".Length;
                return input.Substring(idx).Trim().Split(' ')[0];
            }
            if (lower.Contains("i am"))
            {
                int idx = lower.IndexOf("i am") + "i am".Length;
                return input.Substring(idx).Trim().Split(' ')[0];
            }
            if (lower.Contains("it's"))
            {
                int idx = lower.IndexOf("it's") + "it's".Length;
                return input.Substring(idx).Trim().Split(' ')[0];
            }
            return userName;
        }

        static void GiveTopicResponse(string topic)
        {
            if (topicResponses.ContainsKey(topic))
            {
                var responses = topicResponses[topic];
                var random = new Random();
                if (responses.Count >= 2)
                {
                    var resp1 = responses[random.Next(responses.Count)];
                    var resp2 = responses[random.Next(responses.Count)];
                    while (resp2 == resp1 && responses.Count > 1)
                        resp2 = responses[random.Next(responses.Count)];
                    string jointResponse = resp1 + " " + resp2;
                    TypeWrite($"Chatbot ({topic}): {jointResponse}", ConsoleColor.DarkYellow);
                }
                else
                {
                    TypeWrite($"Chatbot ({topic}): {responses[0]}", ConsoleColor.DarkYellow);
                }
            }
        }

        static void TypeWrite(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(25);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        static void DisplayASCIIArt()
        {
            // ASCII art omitted for brevity
            Console.WriteLine("  █████          █████                                 ");
            Console.WriteLine("                                       █                    █                                        ");
            Console.WriteLine("                                      █   ████████████████   █                                       ");
            Console.WriteLine("                                    ███  █                █  █                                       ");
            Console.WriteLine("                                    █ █  █                █  █ █                                     ");
            Console.WriteLine("                               █ █    █ ▓█                ██ █    █ █                                ");
            Console.WriteLine("                             █  ████ ██  █  ▓███    ▓███  █     ████  █                              ");
            Console.WriteLine("                            ██ █    ██ █ ███▓          ▓███ █ ██    █ ██                             ");
            Console.WriteLine("                               █  █   ███      ██████      ██    █  █                                ");
            Console.WriteLine("                                 ▓█     █ ████        ████ █     ██                                 ");
            Console.WriteLine("                          █       █ ██  █     ▒            █  ██            ▒█                       ");
            Console.WriteLine("                        █        █    █████               ████    █      ░                           ");
            Console.WriteLine("                     ██   █   ██ ██       █   █      █           ██  ████████                        ");
            Console.WriteLine("                           ██     ░█      █   █      █   █      ██      █                            ");
            Console.WriteLine("                                    █     █   █      █   █     █           ███                       ");
            Console.WriteLine("                                     █    ███          ███    █                                      ");
            Console.WriteLine("                                      █    ██     ░     █    ██                                      ");
            Console.WriteLine("                                       █     ██      ██     █   █   ██                               ");
            Console.WriteLine("                                     █  █                  █  ██  █                                 ");
            Console.WriteLine("                                 █ █    ███████████████████     █    █                               ");
            Console.WriteLine("                                 ██ ███             █             ██                                 ");
            Console.WriteLine("                                 ██    ██           █ █            ██                                ");
            Console.WriteLine("                                   █                ███           █                                 ");
            Console.WriteLine("                                                 ██ █                                                ");
            Console.WriteLine("                                                      ███    ");
            Console.WriteLine("======================================================================================================");
            Console.WriteLine("                               LiZulu Cybersecurity Awareness Bot                                                            ");
            Console.WriteLine("=======================================================================================================");
        }

        static void PlayWelcomeMessage()
        {
            try
            {
                using (System.Media.SoundPlayer player = new System.Media.SoundPlayer("greeting.wav"))
                {
                    player.Load();
                    player.PlaySync();
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}
