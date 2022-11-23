using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyQuasiReferencing
{
    static class Zipfa
    {
        static public Dictionary<string, double> RankFrequency(string pathToFile)
        {
            Dictionary<string, int> dictFrequencyWords = ReadText.FrequencyWords(pathToFile);
            Dictionary<string, int> countWordsInSentences = ReadText.CountWordsInSentences(pathToFile);
            Dictionary<string, double> dict = new Dictionary<string, double>();

            List<string> listSentences = ReadText.Sentences(pathToFile);

            foreach (string sentence in listSentences)
            {
                dict[sentence] = 0;

                foreach (var word in dictFrequencyWords)
                {
                    if (sentence.Contains(word.Key))
                        dict[sentence] += (double)(word.Value) / countWordsInSentences[sentence];
                }

                dict[sentence] = Math.Round(dict[sentence], 2);
            }

            Dictionary<string, double> significanceSentences = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return significanceSentences;
        }
    }
}
