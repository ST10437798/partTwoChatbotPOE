using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using partOneChatbot;

namespace EnhancedCybersecurityChatbot
{
    public class EnhancedResponseSystem
    {
        private Dictionary<string, List<string>> keywordResponses;
        private Dictionary<string, List<string>> topicResponses;
        private Dictionary<string, string> responses; // Keep the original responses
        private string currentTopic = null;

        // Sentiment-related data
        private Dictionary<string, SentimentResponse> sentimentMap;

        public EnhancedResponseSystem()
        {
            keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new List<string> { "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.", "A strong password typically includes a mix of uppercase and lowercase letters, numbers, and symbols.", "Consider using a password manager to generate and store complex passwords securely." } },
                { "scam", new List<string> { "Be cautious of online scams. Never share personal or financial information with untrusted sources.", "Look out for red flags like urgent requests, poor grammar, or demands for immediate payment.", "If something seems too good to be true, it probably is a scam." } },
                { "privacy", new List<string> { "Protecting your privacy online involves being mindful of the information you share and with whom.", "Review the privacy settings of your social media and online accounts.", "Be wary of websites that ask for excessive personal information." } }
            };

            topicResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "phishing tips", new List<string> {
                    "Be wary of emails from unknown senders, especially if they ask for personal information.",
                    "Never click on links or download attachments from suspicious emails.",
                    "Verify the sender's identity through official channels if you are unsure about an email's legitimacy.",
                    "Pay attention to the email's language and grammar; phishing attempts often contain errors.",
                    "Enable two-factor authentication on your email account for added security."
                }},
                { "malware advice", new List<string> {
                    "Install and keep your antivirus software up to date.",
                    "Be cautious when downloading files from the internet, especially from untrusted sources.",
                    "Avoid clicking on suspicious links or opening unexpected attachments.",
                    "Regularly scan your system for malware.",
                    "Keep your operating system and other software updated to patch security vulnerabilities."
                }},
                { "safe browsing habits", new List<string> {
                    "Ensure that websites you visit, especially when entering sensitive information, use HTTPS.",
                    "Be careful about the permissions you grant to websites.",
                    "Consider using browser extensions that enhance security and privacy.",
                    "Keep your web browser updated to the latest version.",
                    "Avoid clicking on suspicious ads or pop-ups."
                }}
            };

            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you", "As a large language model, I'm functioning well!" },
                { "what's your purpose", "My purpose is to assist you with information and tasks." },
                { "what can i ask you about", "You can ask me about a wide range of topics, including security topics." },
                { "password safety", "Use strong, unique passwords. Enable 2FA." },
                { "phishing", "Be wary of suspicious emails. Verify senders." },
                { "safe browsing", "Keep software updated. Use antivirus." },
                { "tell me a joke", "Why don't scientists trust atoms? Because they make up everything!" },
                { "hello", "Hello there!" },
                { "hi", "Hi!" },
                { "goodbye", "Goodbye!" },
                { "bye", "Bye!" },
                { "thank you", "You're welcome!" },
                { "thanks", "You're welcome!" },
                { "what is the weather", "I do not have access to real time weather information." },
                { "what time is it", "I do not have access to real time information like the current time." },
                { "strong password", "A strong password should be long and complex." },
                { "two factor authentication", "2FA adds an extra layer of security." },
                { "https", "HTTPS indicates a secure connection." },
                { "ad blocker", "Ad blockers improve browsing speed and security." },
                { "malware", "Malware can harm your system. Use antivirus." }
            };

            sentimentMap = new Dictionary<string, SentimentResponse>(StringComparer.OrdinalIgnoreCase)
            {
                { "worried", new SentimentResponse(Sentiment.Negative, "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.", "Don't worry, I'm here to help you understand how to protect yourself.") },
                { "curious", new SentimentResponse(Sentiment.Neutral, "That's a great question! Let's explore that further.", "That's an interesting point. Here's some more information.") },
                { "frustrated", new SentimentResponse(Sentiment.Negative, "I understand it can be frustrating. Cybersecurity can seem complex, but we can break it down. What specifically is causing frustration?", "Take a deep breath. We'll work through this together. Tell me what's confusing you.") },
                { "unsure", new SentimentResponse(Sentiment.Negative, "It's alright to feel unsure. Let me clarify things for you.", "No problem at all. Let's go over it again.") },
                { "overwhelmed", new SentimentResponse(Sentiment.Negative, "I can see that this might seem like a lot. We can take it one step at a time.", "Let's focus on the most important aspects first.") }
                // Add more sentiment keywords and responses as needed
            };
        }

        private enum Sentiment
        {
            Positive,
            Negative,
            Neutral
        }

        private class SentimentResponse
        {
            public Sentiment SentimentType { get; }
            public string SupportiveResponse { get; }
            public string EncouragingResponse { get; }

            public SentimentResponse(Sentiment sentimentType, string supportiveResponse, string encouragingResponse)
            {
                SentimentType = sentimentType;
                SupportiveResponse = supportiveResponse;
                EncouragingResponse = encouragingResponse;
            }
        }

        public string GetResponse(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            string lowerQuestion = question.ToLower();

            // Check for sentiment keywords first
            foreach (var sentimentPair in sentimentMap)
            {
                if (lowerQuestion.Contains(sentimentPair.Key))
                {
                    currentTopic = null; // Reset topic as the focus is on sentiment
                    Random random = new Random();
                    // For now, let's just return the supportive response
                    return sentimentPair.Value.SupportiveResponse;
                }
            }

            // Check for follow-up questions on the current topic
            if (!string.IsNullOrEmpty(currentTopic))
            {
                if (lowerQuestion.Contains("more detail") || lowerQuestion.Contains("explain further") || lowerQuestion.Contains("i'm confused"))
                {
                    if (topicResponses.ContainsKey(currentTopic))
                    {
                        Random random = new Random();
                        return topicResponses[currentTopic][random.Next(topicResponses[currentTopic].Count)];
                    }
                    else if (keywordResponses.ContainsKey(currentTopic))
                    {
                        Random random = new Random();
                        return keywordResponses[currentTopic][random.Next(keywordResponses[currentTopic].Count)];
                    }
                    else if (responses.ContainsKey(currentTopic))
                    {
                        return responses[currentTopic]; // Provide the original response again
                    }
                    return "Could you be more specific about what you'd like more details on?";
                }
            }

            // Check for specific topic queries with multiple responses
            foreach (var topicPair in topicResponses)
            {
                if (lowerQuestion.Contains(topicPair.Key))
                {
                    currentTopic = topicPair.Key;
                    Random random = new Random();
                    return topicPair.Value[random.Next(topicPair.Value.Count)];
                }
            }

            // Check for keyword matches with multiple responses
            foreach (var keywordPair in keywordResponses)
            {
                if (lowerQuestion.Contains(keywordPair.Key))
                {
                    currentTopic = keywordPair.Key;
                    Random random = new Random();
                    return keywordPair.Value[random.Next(keywordPair.Value.Count)];
                }
            }

            // Fallback to the original response system for exact matches
            if (responses.TryGetValue(lowerQuestion, out string response))
            {
                currentTopic = lowerQuestion;
                return response;
            }

            currentTopic = null; // Reset current topic if no match
            return "I didn't quite understand that. Could you rephrase?";
        }

        public void DisplayHeader(string headerText)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine($"  {headerText}");
            Console.WriteLine(new string('=', 40) + "\n");
            Console.ResetColor();
        }

        public void DisplayDivider()
        {
            Console.WriteLine(new string('-', 40));
        }

        public void TypeWriterEffect(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            //creating instance for the class playNow
            new playNow() { };

            EnhancedResponseSystem responseSystem = new EnhancedResponseSystem();
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "ascii-art.txt");

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("\n");
                }
                else
                {
                    responseSystem.DisplayHeader("Cybersecurity Awareness Chatbot");
                }

                Console.WriteLine("Welcome to the Enhanced Chatbot!");

                Console.Write("Please enter your name: ");
                string userName = Console.ReadLine();

                Console.WriteLine($"\nHello, {userName}! I'm here to help you learn about cybersecurity.");

                Console.WriteLine("\nHere are some questions you can ask (try these for varied responses and follow-ups):");
                Console.WriteLine("- Tell me some phishing tips.");
                Console.WriteLine("- What is your malware advice?");
                Console.WriteLine("- What are some safe browsing habits?");
                Console.WriteLine("- Tell me about password safety.");
                Console.WriteLine("- What should I know about online scams?");
                Console.WriteLine("- Can you give me some tips on privacy online?");
                Console.WriteLine("- How are you?");
                Console.WriteLine("- What's your purpose?");
                Console.WriteLine("- What can I ask you about?");
                Console.WriteLine("- Phishing?");
                Console.WriteLine("- Safe browsing?");
                Console.WriteLine("- Tell me a joke?");
                Console.WriteLine("- Hello/Hi/Goodbye/Bye");
                Console.WriteLine("- Thank you/Thanks");
                Console.WriteLine("- What is the weather?");
                Console.WriteLine("- What time is it?");
                Console.WriteLine("- Strong password?");
                Console.WriteLine("- Two factor authentication?");
                Console.WriteLine("- HTTPS?");
                Console.WriteLine("- Ad blocker?");
                Console.WriteLine("- Malware?");

                Console.WriteLine("\nTry asking these follow-up questions after a cybersecurity topic:");
                Console.WriteLine("- More detail.");
                Console.WriteLine("- Explain further.");
                Console.WriteLine("- I'm confused about that.");

                Console.WriteLine("\nTry asking these keyword-related questions:");
                Console.WriteLine("- Tell me about passwords.");
                Console.WriteLine("- What should I know about scams?");
                Console.WriteLine("- Any tips on online privacy?");

                Console.WriteLine("\nTry expressing your feelings about cybersecurity:");
                Console.WriteLine("- I'm worried about online scams");
                Console.WriteLine("- I'm curious about how malware works");
                Console.WriteLine("- I feel frustrated with password rules");
                Console.WriteLine("- I'm unsure about how to identify phishing emails");
                Console.WriteLine("- I feel overwhelmed by all this security advice");

                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("You: ");
                    Console.ResetColor();
                    string userInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        break;
                    }

                    string response = responseSystem.GetResponse(userInput);

                    responseSystem.DisplayDivider();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Bot: ");
                    Console.ResetColor();
                    responseSystem.TypeWriterEffect(response);
                    responseSystem.DisplayDivider();
                }

                responseSystem.DisplayHeader("End of Conversation");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nPress any key to exit...");
                Console.ResetColor();
                Console.ReadKey();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: File not found at {filePath}");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Error: Directory not found in {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An IO Exception occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}