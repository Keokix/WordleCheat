using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolverForHany
{
    class WordleSolver
    {
        private const string WordListPath = @"C:\Users\marce_hpoatx\source\repos\WordleSolver\WordleSolver\Assests\WordleWordList.txt";
        private static List<char> _allowedLetters;
        private static List<char> _invalidLetters;
        private static List<string> _possibleWords1;
        private static List<string> _possibleWords2;
        private static List<string> _possibleWords3;
        private static List<string> _possibleWords4;
        private static int _wordLength;
        private static char[] _wordPattern;
        private static List<char> _multipleExistingLetters;

        static void Main(string[] args)
        {
            Console.WriteLine("Da Bobby es nicht glaubt, hier ein Worldesolver made by Keokix :P");
            _allowedLetters = new List<char>();
            _invalidLetters = new List<char>();
            _possibleWords1 = new List<string>();
            _possibleWords2 = new List<string>();
            _possibleWords3 = new List<string>();
            _possibleWords4 = new List<string>();
            SetWordLength();
            Menu();
        }



        private static void Menu()
        {
            Console.WriteLine("");
            Console.WriteLine("Wähle aus was du tun möchtest: ");
            Console.WriteLine("1:  Erlaubte Buchstaben eingeben");
            Console.WriteLine("2:  Ungültige Buchstaben eingeben");
            Console.WriteLine("3:  Wordmuster für bekannte Buchstaben Eingeben");
            Console.WriteLine("4:  Alle möglichen Wörter anzeigen lassen (kann ein paar Sekunden dauern)");
            Console.WriteLine("5:  Alle Filter entfernen");


            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SetAllowedLetters();
                    break;
                case "2":
                    SetInvalidLetters();
                    break;
                case "3":
                    SetWordPattern();
                    break;
                case "4":
                    SearchForValidWords();
                    break;
                case "5":
                    ResetFilter();
                    break;
                default:
                    Console.WriteLine("Eingabe Ungültigt");
                    break;
            }
        }

        private static void ResetFilter()
        {
            SetWordLength();
            _allowedLetters.Clear();
            _invalidLetters.Clear();
            _possibleWords1.Clear();
            _possibleWords2.Clear();
            _possibleWords3.Clear();
            _possibleWords4.Clear();

            Menu();
        }

        private static void UpdateConsoleText()
        {
            Console.Clear();
            Console.WriteLine($"Wortlänge: {_wordLength}");
            Console.WriteLine(" ");
            Console.Write("Erlaubte Buchstaben:   ");
            if (_allowedLetters.Count > 0)
            {
                foreach (char letter in _allowedLetters)
                {
                    Console.Write("  " + letter + "  ");
                }
            }

            Console.WriteLine(" ");
            Console.Write("Ungültige Buchstaben:   ");
            if (_invalidLetters.Count > 0)
            {
                foreach (char letter in _invalidLetters)
                {
                    Console.Write("  " + letter + "  ");
                }
            }
            Console.WriteLine(" ");
            Console.Write("Aktuelles Muster:   ");
            if (_wordPattern.Length > 0)
            {
                foreach (char letter in _wordPattern)
                {
                    if (char.IsLetter(letter))
                    {
                        Console.Write(letter);
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }
            }

            Menu();
        }

        private static void SearchForValidWords()
        {


            if (_possibleWords1.Count < 1)
            {
                var words = File.ReadAllLines(WordListPath);
                _possibleWords1 = words.ToList();
            }

            foreach (string word in _possibleWords1.ToList())
            {
                var UpperCaseWord = word.ToUpper();
                if (word.Length != _wordLength)
                {
                    _possibleWords1.Remove(word);
                    continue;
                }
                foreach (char c in _invalidLetters)
                {
                    if (UpperCaseWord.Contains(c))
                    {
                        _possibleWords1.Remove(word);
                        continue;
                    }
                }
                foreach (char c in _allowedLetters)
                {
                    if (!UpperCaseWord.Contains(c))
                    {
                        _possibleWords1.Remove(word);
                        continue;
                    }
                }
            }

            foreach (string w in _possibleWords1)
            {
                Console.WriteLine(w);
            }
            Menu();
        }


        private static void SetWordLength()
        {
            Console.WriteLine("Wie lang ist das gesuchte Wort?");

            var input = Console.ReadLine();

            if (!int.TryParse(input, out _wordLength))
            {
                Console.WriteLine("Ungültige Eingabe, nur Zahlen sind erlaubt");
                SetWordLength();
            }
            _wordPattern = new char[_wordLength];
            UpdateConsoleText();
        }


        private static void SetWordPattern()
        {
            Console.WriteLine("Bitte gib die Buchstaben, welche bereits richtig sind in dem Muster ein: ");
            Console.WriteLine("Das Wort hat z.B. 6 Buchstaben,  und Lautet 'Saufen',  das a f und n sind bekannt.");
            Console.WriteLine("Die Eingabe muss also so aussehen:   _a_f_n    ein '_' ist hierbei ein Platzhalter für unbekannte Buchstaben");

            var input = Console.ReadLine();
            if (input.Length != _wordLength)
            {
                Console.WriteLine($"Das Muster muss genau aus {_wordLength} Zeichen bestehen!");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");

            }

            var pattern = input.ToCharArray();
            for (int i = 0; i < _wordLength; i++)
            {
                if (char.IsLetter(pattern[i]) || pattern[i].Equals("_"))
                {
                    _wordPattern[i] = pattern[i];
                }
                else
                {
                    Console.WriteLine($"Das Zeichen {pattern[i]} wurde ignoriert, da es keine gültige eingabe ist.");
                }
            }
            UpdateConsoleText();
        }

        private static void SetAllowedLetters()
        {
            Console.WriteLine("Bitte gib alle Buchstaben die in dem Wort enthalten sind ein");
            Console.WriteLine("Beispiel:  BOBBY   wenn ein Buchstabe mehrfach vorkommt, gib ihn auch mehrfach ein.");
            var input = Console.ReadLine();

            var allowedChars = input.ToCharArray();

            foreach (char letter in allowedChars)
            {
                if (char.IsLetter(letter))
                {
                    var upperLetter = char.ToUpper(letter);
                    if (_allowedLetters.Contains(upperLetter))
                    {
                        _multipleExistingLetters.Add(upperLetter);
                        _multipleExistingLetters.Add(upperLetter);
                    }
                    _allowedLetters.Add(upperLetter);
                }
                else
                {
                    Console.WriteLine($"Das Zeichen {letter} wurde ignoriert, da es keine gültige eingabe ist.");
                }
            }
            UpdateConsoleText();
        }

        private static void SetInvalidLetters()
        {
            Console.WriteLine("Bitte gib alle Buchstaben die NICHT in dem Wort enthalten sind ein");
            Console.WriteLine("Beispiel:  SKL  ");
            var input = Console.ReadLine();

            var invalidChars = input.ToCharArray();

            foreach (char letter in invalidChars)
            {
                if (char.IsLetter(letter))
                {
                    var upperLetter = char.ToUpper(letter);
                    _invalidLetters.Add(upperLetter);
                }
                else
                {
                    Console.WriteLine($"Das Zeichen {letter} wurde ignoriert, da es keine gültige eingabe ist.");
                }
            }
            UpdateConsoleText();
        }

        private static bool DoesFileExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}
