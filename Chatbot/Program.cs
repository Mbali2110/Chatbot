using System;
using System.Media;
using System.Threading;

namespace Chatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Display the ASCII art logo  
            DisplayASCIIArt();

            // Play the welcome message  
            PlayWelcomeMessage();

            // Ask for the user's name  
            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();

            // Display a personalized welcome message  
            DisplayWelcomeMessage(userName);

            Console.WriteLine("Chatbot is ready to chat! Ask away.");

            string userInput;
            while (true)
            {
                Console.Write($"\n{(string.IsNullOrWhiteSpace(userName) ? "User" : userName)}: ");
                userInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter a valid input.");
                    continue;
                }

                if (userInput.ToLower() == "exit")
                {
                    TypeWrite("Chatbot: Goodbye! Stay safe online!", ConsoleColor.Cyan);
                    break;
                }

                RespondToUser(userInput, userName);
            }
        }

        static void DisplayASCIIArt()
        {
            Console.WriteLine("===========================================");
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
            Console.WriteLine("===========================================");
            Console.WriteLine("            LIZULU Cybersecurity Awareness Bot");
            Console.WriteLine("===========================================");
        }

        static void DisplayWelcomeMessage(string userName)
        {
            // Decorative Border
            string border = new string('*', 60);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(border);

            // Welcome Message with color
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"*        Welcome to the Cybersecurity Awareness Bot, {userName}!        *");

            // Decorative Border
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(border);
        }

        static void PlayWelcomeMessage()
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer("greeting.wav"))
                {
                    player.Load();
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }

        static void RespondToUser(string userInput, string userName)
        {
            userInput = userInput.ToLower();
            switch (userInput)
            {
                case "how are you?":
                    TypeWrite("Chatbot: I'm just a program, but I'm here and ready to help you!", ConsoleColor.Cyan);
                    break;
                case "what can i ask you about?":
                    TypeWrite("Chatbot: You can ask me about password safety, phishing, safe browsing, social engineering and two-factor authentication", ConsoleColor.Cyan);
                    break;
                case "what is your purpose?":
                    TypeWrite("Chatbot: My purpose is to raise awareness about cybersecurity and help you stay safe online.", ConsoleColor.Cyan);
                    break;
                case "password safety":
                    TypeWrite("Chatbot: Always use strong, unique passwords for each of your accounts, and consider using a password manager.", ConsoleColor.Green);
                    break;
                case "phishing":
                    TypeWrite("Chatbot: Be cautious of emails or messages that ask for personal information. Always verify the sender before clicking links.", ConsoleColor.Green);
                    break;
                case "safe browsing":
                    TypeWrite("Chatbot: Make sure to use secure websites (look for 'https://') and avoid entering sensitive information on public Wi-Fi.", ConsoleColor.Green);
                    break;
                case "social engineering":
                    TypeWrite("Chatbot: Social engineering is a tactic used by cybercriminals to manipulate people into giving up confidential information. Be cautious of unsolicited messages or phone calls.", ConsoleColor.Green);
                    break;
                case "two-factor authentication":
                    TypeWrite("Chatbot: Two-factor authentication adds an extra layer of security to your accounts by requiring a second form of verification. Always enable 2FA when available.", ConsoleColor.Green);
                    break;
                case "thank you":
                    TypeWrite("Chatbot: You're welcome! Do you have more cybersecurity questions you'd like me to answer?", ConsoleColor.Cyan);
                    break;
                default:
                    TypeWrite("Chatbot: I didn't quite understand that. Could you rephrase?", ConsoleColor.Red);
                    break;
            }
        }

        static void TypeWrite(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            Console.ResetColor();
            Console.WriteLine(); // Move to the next line after the message is displayed  
        }
    }
}
