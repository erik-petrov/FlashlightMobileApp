using System;
using System.Collections.Generic;
using System.Text;

namespace ProstoDajteMne3
{
    public class MorseCode
    {
        public static Dictionary<char, string> _morseAlphabetDictionary;

        static void Main()
        {
            InitializeDictionary();

            Console.WriteLine("What did you want to say?");
            string userInput = GetUserInput();
            Console.WriteLine("Morse alphabet output is: " + Translate(userInput));

            Console.WriteLine("[Press ANY KEY to exit]");
            Console.ReadLine();
        }

        public static void InitializeDictionary()
        {
            _morseAlphabetDictionary = new Dictionary<char, string>()
                                   {
                                       {'a', ".-"},
                                       {'b', "-..."},
                                       {'c', "-.-."},
                                       {'d', "-.."},
                                       {'e', "."},
                                       {'f', "..-."},
                                       {'g', "--."},
                                       {'h', "...."},
                                       {'i', ".."},
                                       {'j', ".---"},
                                       {'k', "-.-"},
                                       {'l', ".-.."},
                                       {'m', "--"},
                                       {'n', "-."},
                                       {'o', "---"},
                                       {'p', ".--."},
                                       {'q', "--.-"},
                                       {'r', ".-."},
                                       {'s', "..."},
                                       {'t', "-"},
                                       {'u', "..-"},
                                       {'v', "...-"},
                                       {'w', ".--"},
                                       {'x', "-..-"},
                                       {'y', "-.--"},
                                       {'z', "--.."},
                                       {'а', ".-" },
                                       {'б', "-..." },
                                       {'в', ".--" },
                                       {'г', "--." },
                                       {'д', "-.." },
                                       {'е', "." },
                                       {'ж', "...-" },
                                       {'з', "--.." },
                                       {'и', ".." },
                                       {'й', ".--" },
                                       {'к', "-.-" },
                                       {'л', ".-.." },
                                       {'м', "--" },
                                       {'н', "-." },
                                       {'о', "---" },
                                       {'п', ".--." },
                                       {'р', ".-." },
                                       {'с', "..." },
                                       {'т', "-" },
                                       {'у', "..-" },
                                       {'ф', "..-." },
                                       {'х', "..." },
                                       {'ц', "-.-." },
                                       {'ч', "---." },
                                       {'ш', "----" },
                                       {'щ', "--.-" },
                                       {'ъ', ".--.-." },
                                       {'ы', "-.--" },
                                       {'ь', "-..-" },
                                       {'э', "...-..." },
                                       {'ю', ".-" },
                                       {'я', ".-.-" },
                                       {'0', "-----"},
                                       {'1', ".----"},
                                       {'2', "..---"},
                                       {'3', "...--"},
                                       {'4', "....-"},
                                       {'5', "....."},
                                       {'6', "-...."},
                                       {'7', "--..."},
                                       {'8', "---.."},
                                       {'9', "----."}
                                   };
        }

        private static string GetUserInput()
        {
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                input = input.ToLower();
            }

            return input;
        }

        public static string Translate(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char character in input)
            {
                if (_morseAlphabetDictionary.ContainsKey(character))
                {
                    stringBuilder.Append(_morseAlphabetDictionary[character] + " ");
                }
                else if (character == ' ')
                {
                    stringBuilder.Append("/ ");
                }
                else
                {
                    stringBuilder.Append(character + " ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
