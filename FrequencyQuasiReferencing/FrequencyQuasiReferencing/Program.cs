using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyQuasiReferencing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //char[] endSeparators = new char[] { '.', '?', '!'};
            string[] wordsMarkers = new string[] { "вывод", "заключение", "начало", "начать", "вступление" };

            List<string> listSentences = ReadText.Sentences("input.txt");
            List<string> listAutoref = new List<string>();

            Dictionary<string, double> significanceSentences = Zipfa.RankFrequency("input.txt");

            Console.Write("Выходной размер автореферата (20-80%): ");
            int sizeAutoref = Convert.ToInt32(Console.ReadLine());

            sizeAutoref = Convert.ToInt32((double)(listSentences.Count) / 100 * sizeAutoref);

            int count = 0;
            bool check;

            // Поиск предложений в которых есть слова-маркеры
            foreach (string marker in wordsMarkers)
            {
                foreach (var sentence in significanceSentences)
                {
                    if (sentence.Key.Contains(marker) && count < sizeAutoref)
                    {
                        check = true;

                        foreach (string outputSentence in listAutoref)
                        {
                            if (outputSentence.Equals(sentence.Key))
                            {
                                check = false;
                                break;
                            }
                        }

                        if (check)
                        {
                            listAutoref.Add(sentence.Key);
                            count++;
                        }
                    }
                }
            }

            // Добавление в список предложений по убыванию значимости
            if (count < sizeAutoref)
            {
                foreach (var sentence in significanceSentences)
                {
                    if (count < sizeAutoref)
                        listAutoref.Add(sentence.Key);
                    else
                        break;

                    count++;
                }
            }

            // Заполнения словаря итоговыми предложениями и их позициями в тексте
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (var outputSentence in listAutoref)
            {
                for (int i = 0; i < listSentences.Count; i++)
                {
                    if (listSentences[i].Contains(outputSentence))
                    {
                        dict[outputSentence] = i;
                        break;
                    }
                }
            }

            // Сортировка предложений в исходный порядок
            Dictionary<string, int> outputAutoref = dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);


            string text = "";
            Console.WriteLine("\nВыходной автореферат:");

            foreach (var sentence in outputAutoref)
               text += sentence.Key + " ";

            Console.WriteLine(text + "\n");
        }
    }
}
