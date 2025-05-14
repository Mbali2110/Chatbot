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
            {
                "password", new List<string>()
                {
                    "Passwords should be long, unique, and contain a mix of characters.",
                    "Consider using a password manager to keep your credentials safe.",
                    "Avoid using personal info like birthdays or pet names in your passwords."
                }
            },
            {
                "phishing", new List<string>()
                {
                    "Phishing emails often create a sense of urgency. Always double-check the sender's email.",
                    "Never click suspicious links, even if they look official.",
                    "Verify requests for personal information by contacting the organization directly."
                }
            },
            {
                "privacy", new List<string>()
                {
                    "Review your privacy settings regularly and be cautious about what you share online.",
                    "Use strong, unique passwords for different accounts.",
                    "Be aware of what data you share on social media."
                }
            },
            {
                "safe browsing", new List<string>()
                {
                    "Use HTTPS websites for secure browsing.",
                    "Avoid downloading files from unknown or suspicious websites.",
                    "Keep your browser and plugins up to date for better security."
                }
            },
            {
                "social engineering", new List<string>()
                {
                    "Always verify identities before sharing sensitive info.",
                    "Be cautious of unsolicited requests for personal data.",
                    "Remember, if it sounds too good to be true, it probably is."
                }
            },
            {
                "two-factor", new List<string>()
                {
                    "Enable two-factor authentication whenever possible.",
                    "Use authenticator apps over SMS for better security.",
                    "Keep your 2FA backup codes in a safe place."
                }
            },
            {
                "scam", new List<string>()
                {
                    "Be cautious of scams asking for your personal or financial info.",
                    "Always verify the identity of anyone requesting sensitive data.",
                    "Remember, legitimate organizations will never ask for your password or PIN via email."
                }
            }
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
            { "negative", "I'm sorry to hear that. Remember, staying informed helps!" }
        };

        static List<string> positiveWords = new List<string> { "happy", "excited", "confident", "relieved", "glad" };
        static List<string> negativeWords = new List<string> { "worried", "scared", "frustrated", "anxious", "nervous", "unsure" };
        static List<string> interestWords = new List<string> { "interested", "intrigued", "curious", "know" };

        static string userName = "";
        static string favoriteTopic = "";
        static string currentTopic = "";
        static string lastMentionedTopic = "";
        static string lastResponseTopic = "";

        static void Main(string[] args)
        {
            DisplayASCIIArt();
            PlayWelcomeMessage();

            // Ask user for their name first
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Please enter your name: ");
            Console.ResetColor();
            userName = Console.ReadLine().Trim();

            // Show the welcome message with a yellow border and cyan text
            string welcomeMsg = $"Welcome to the Cybersecurity Bot {userName}";
            string borderLine = new string('*', welcomeMsg.Length + 4);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(borderLine);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"* {welcomeMsg} *");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(borderLine);
            Console.ResetColor();

            // Instruct user about topics
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nAsk me about: password safety, phishing, privacy, safe browsing, social engineering, two-factor, and scams.");
            Console.ResetColor();

            // Main chat loop
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{userName}: ");
                Console.ResetColor();
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("I didn't catch that. Could you rephrase?");
                    continue;
                }
                if (input.ToLower().Contains("exit"))
                {
                    break;
                }
                HandleUserInput(input);
            }

            // Cheeky farewell
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nOh well, sad to see you go, but I'm glad I helped you. See you later, alligator :)");
            Console.ResetColor();
            Environment.Exit(0);
        }

        static void HandleUserInput(string input)
        {
            string normalizedInput = NormalizeInput(input);
            string lowerInput = input.ToLower();

            // 1. Static phrases detection
            foreach (var phrase in staticPhrases.Keys)
            {
                if (normalizedInput.Contains(phrase))
                {
                    TypeWrite($"Chatbot: {staticPhrases[phrase]}", ConsoleColor.Cyan);
                    return;
                }
            }

            // 2. Set favorite topic
            if (lowerInput.Contains("my favorite topic is"))
            {
                int idx = lowerInput.IndexOf("my favorite topic is");
                string favTopicCandidate = input.Substring(idx + "my favorite topic is".Length).Trim().ToLower();
                foreach (var topic in topicResponses.Keys)
                {
                    if (favTopicCandidate.Contains(topic))
                    {
                        favoriteTopic = topic;
                        TypeWrite($"Chatbot: Got it! Your favorite topic is {topic}.", ConsoleColor.Magenta);
                        return;
                    }
                }
            }

            // 3. Detect topics
            var detectedTopics = topicResponses.Keys.Where(k => lowerInput.Contains(k)).ToList();
            if (detectedTopics.Count > 0)
            {
                foreach (var topic in detectedTopics)
                {
                    if (currentTopic != topic)
                    {
                        currentTopic = topic;
                    }
                    lastMentionedTopic = topic;
                    GiveTopicResponse(topic);
                }
                return;
            }

            // 4. About it / about that / it
            string[] aboutItPhrases = { "tell me about it", "about it", "about that", "it" };
            if (aboutItPhrases.Any(phrase => lowerInput.Contains(phrase)))
            {
                if (!string.IsNullOrEmpty(currentTopic))
                {
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

            // 5. More about / tell me more about / know more about
            string[] moreAboutPhrases = { "more about", "know more about", "tell me more about" };
            if (moreAboutPhrases.Any(phrase => lowerInput.Contains(phrase)))
            {
                bool foundTopic = false;
                foreach (var topic in topicResponses.Keys)
                {
                    if (lowerInput.Contains(topic))
                    {
                        currentTopic = topic;
                        lastMentionedTopic = topic; // update lastMentionedTopic
                        GiveTopicResponse(topic);
                        foundTopic = true;
                        break;
                    }
                }
                if (!foundTopic)
                {
                    if (!string.IsNullOrEmpty(currentTopic))
                    {
                        lastMentionedTopic = currentTopic; // update lastMentionedTopic
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

            // 6. Interested in topic
            if (interestWords.Any(w => lowerInput.Contains(w)))
            {
                foreach (var topic in topicResponses.Keys)
                {
                    if (lowerInput.Contains(topic))
                    {
                        currentTopic = topic;
                        lastMentionedTopic = topic;
                        TypeWrite($"Chatbot: I see you're interested in {topic}. Would you like to know more?", ConsoleColor.Yellow);
                        string reply = Console.ReadLine().ToLower().Trim();
                        if (reply == "yes") GiveTopicResponse(topic);
                        return;
                    }
                }
            }

            // 7. Explicit mention of a topic (like "Tell me about privacy")
            foreach (var topic in topicResponses.Keys)
            {
                if (lowerInput.Contains(topic))
                {
                    currentTopic = topic; // update current topic
                    lastMentionedTopic = topic;
                    GiveTopicResponse(topic);
                    return;
                }
            }

            // 8. Follow-up "tell me about it" / "about it"
            string[] continuationPhrases = { "tell me about it", "about it", "about that", "it" };
            if (continuationPhrases.Any(phrase => lowerInput.Contains(phrase)))
            {
                if (!string.IsNullOrEmpty(lastMentionedTopic))
                {
                    currentTopic = lastMentionedTopic; // continue with last
                    GiveTopicResponse(currentTopic);
                }
                else if (!string.IsNullOrEmpty(favoriteTopic))
                {
                    currentTopic = favoriteTopic;
                    GiveTopicResponse(currentTopic);
                }
                else
                {
                    TypeWrite("Chatbot: Which topic would you like to learn more about? Please specify.", ConsoleColor.Red);
                }
                return;
            }

            // 9. Sentiment detection
            if (negativeWords.Any(w => lowerInput.Contains(w)))
            {
                TypeWrite($"Chatbot: {sentimentResponses["negative"]}", ConsoleColor.Magenta);
                return;
            }
            if (positiveWords.Any(w => lowerInput.Contains(w)))
            {
                TypeWrite($"Chatbot: {sentimentResponses["positive"]}", ConsoleColor.Yellow);
                return;
            }

            // 10. Interest + no sentiment
            if (interestWords.Any(w => lowerInput.Contains(w)))
            {
                foreach (var topic in topicResponses.Keys)
                {
                    if (lowerInput.Contains(topic))
                    {
                        currentTopic = topic;
                        lastMentionedTopic = topic;
                        TypeWrite($"Chatbot: I see you're interested in {topic}. Would you like to know more?", ConsoleColor.Yellow);
                        string reply = Console.ReadLine().ToLower().Trim();
                        if (reply == "yes") GiveTopicResponse(topic);
                        return;
                    }
                }
            }

            // fallback
            TypeWrite("Chatbot: I didn't quite understand that. Could you rephrase?", ConsoleColor.Red);
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

        static void GiveTopicResponse(string topic)
        {
            if (topicResponses.ContainsKey(topic))
            {
                var responses = topicResponses[topic];
                var random = new Random();
                string response = responses[random.Next(responses.Count)];
                TypeWrite($"Chatbot ({topic}): {response}", ConsoleColor.DarkYellow);
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
                // ignore if no sound file
            }
        }
    }
}
