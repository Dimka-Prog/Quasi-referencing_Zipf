using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FrequencyQuasiReferencing
{
    static class ReadText
    {
        static private string sentence = "";
        static private int symbol;

        static private char[] endSeparators = new char[] { '.', '?', '!' };

        /// <summary>
        /// Считывает текстовый файл и возвращает list предложений.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        static public List<string> Sentences(string pathToFile)
        {
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                List<string> listSentences = new List<string>();

                while ((symbol = reader.Read()) != -1)
                {
                    if (sentence == "" && Convert.ToChar(symbol) == ' ')
                        continue;

                    sentence += Convert.ToChar(symbol);

                    foreach (var endSymbol in endSeparators)
                    {
                        if (Convert.ToInt32(endSymbol) == symbol)
                        {
                            listSentences.Add(sentence);
                            sentence = "";
                        }
                    }
                }

                reader.Close();
                return listSentences;
            }
        }

        /// <summary>
        /// Считывает текстовый файл и возвращает list предложений, где кадое преложение оканчивается на символ из массива endCharacters.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="endSeparators"></param>
        /// <returns></returns>
        static public List<string> Sentences(string pathToFile, in char[] endSeparators)
        {
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                List<string> listSentences = new List<string>();

                while ((symbol = reader.Read()) != -1)
                {
                    if (sentence == "" && Convert.ToChar(symbol) == ' ')
                        continue;

                    sentence += Convert.ToChar(symbol);

                    foreach (var endSymbol in endSeparators)
                    {
                        if (Convert.ToInt32(endSymbol) == symbol)
                        {
                            listSentences.Add(sentence);
                            sentence = "";
                        }
                    }
                }

                reader.Close();
                return listSentences;
            }
        }

        /// <summary>
        /// Считывает текстовый файл и возвращает массив слов.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        static public string[] Words(string pathToFile)
        {
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                string text = reader.ReadToEnd();
                char[] separators = new char[] { ' ', ',', '.', '?', '!', ':', ';', '"' };

                string[] words = text.Split(separators);
                words = Array.FindAll(words, x => !string.IsNullOrEmpty(x) && x.Length > 2 && x != "что" && x != "или");

                reader.Close();
                return words;
            }
        }

        /// <summary>
        /// Считывает текстовый файл и возвращает коллекцию слов с количеством их повторений.  
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="words"></param>
        static public Dictionary<string, int> FrequencyWords(string pathToFile)
        {
            using(StreamReader reader = new StreamReader(pathToFile))
            {
                string[] words = Words(pathToFile);

                Dictionary<string, int> dict = new Dictionary<string, int>();
                foreach (var value in words)
                {
                    dict.TryGetValue(value, out int count);
                    dict[value] = count + 1;
                }
                Dictionary<string, int> dictFrequencyWords = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                reader.Close();
                return dictFrequencyWords;
            }
        }

        /// <summary>
        /// Считывает текстовый файл и возвращает колекцию предложений с количеством слов в этих предложениях.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        static public Dictionary<string, int> CountWordsInSentences(string pathToFile)
        {
            char[] separators = new char[] { ' ', ',', '.', '?', '!', ':', ';', '"' };
            string[] words;

            List<string> listSentences = Sentences(pathToFile);
            Dictionary<string, int> countWordsInSentences = new Dictionary<string, int>();

            foreach (string sentence in listSentences)
            {
                words = sentence.Split(separators);
                words = Array.FindAll(words, x => !string.IsNullOrEmpty(x) && x.Length > 2 && x != "что" && x != "или");

                countWordsInSentences[sentence] = words.Length;
            }
            
            return countWordsInSentences;
        }
    }
}
