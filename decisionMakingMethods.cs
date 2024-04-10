namespace N1_SO_KAZAR
{
    public class decisionMakingMethods()
    {
        public static List<string[]> ReadCSV(String filename)
        {
            List<string[]> result = new List<string[]>();

            string[] lines = File.ReadAllLines(filename);
            foreach (string vrstica in lines)
            {
                string[] vrednosti = vrstica.Split(',');
                result.Add(vrednosti);
            }
            return result;
        }
        static int GetIndexWithHighestAverage(List<Dictionary<string, int>> listOfDictionaries)
        {
            int numberOfElements = listOfDictionaries[0].Count;
            double[] averages = new double[numberOfElements];

            for (int i = 0; i < numberOfElements; i++)
            {
                averages[i] = listOfDictionaries.Average(dict => dict.Values.ElementAt(i));
            }
            return Array.IndexOf(averages, averages.Max());
        }
        public static List<Dictionary<string, int>> CSVtoDecision(List<string[]> lines)
        {
            List<Dictionary<string, int>> decisionTable = new();
            Dictionary<string, int> scenarionA = new();
            Dictionary<string, int> scenarionB = new();
            List<string> alternative = [.. lines[0]];
            lines.RemoveAt(0);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < lines[i].Length; j++)
                {
                    if (i == 0)
                        scenarionA.Add(alternative[j], int.Parse(lines[i][j]));
                    else
                        scenarionB.Add(alternative[j], int.Parse(lines[i][j]));
                }
            }
            decisionTable.Add(scenarionA);
            decisionTable.Add(scenarionB);

            return decisionTable;
        }
        public static KeyValuePair<string, int> Optimist(Decision decision)
        {
            KeyValuePair<string, int> s1 = decision.decisionTable[0].OrderByDescending(d => d.Value).First();
            KeyValuePair<string, int> s2 = decision.decisionTable[1].OrderByDescending(d => d.Value).First();
            if (s1.Value > s2.Value)
                return s1;
            else
                return s2;
        }
        public static KeyValuePair<string, int> Pesimist(Decision decision)
        {
            int alls1 = 0;
            int alls2 = 0;
            foreach (KeyValuePair<string, int> s in decision.decisionTable[0])
            {
                alls1 += s.Value;
            }
            foreach (KeyValuePair<string, int> s in decision.decisionTable[1])
            {
                alls2 += s.Value;
            }
            KeyValuePair<string, int> s1 = decision.decisionTable[0].OrderByDescending(d => d.Value).First();
            KeyValuePair<string, int> s2 = decision.decisionTable[1].OrderByDescending(d => d.Value).First();
            if (alls1 < alls2)
                return s1;
            else
                return s2;
        }
        public static KeyValuePair<string, double> Laplace(Decision decision)
        {
            int index = GetIndexWithHighestAverage(decision.decisionTable);
            string s1 = decision.decisionTable[0].ElementAt(index).Key;
            double avg = (decision.decisionTable[0].ElementAt(index).Value + decision.decisionTable[1].ElementAt(index).Value) / 2;
            KeyValuePair<string, double> k = new KeyValuePair<string, double>(s1, Math.Round(avg, 0));

            return k;
        }
        public static KeyValuePair<string, int> Savage(Decision decision)
        {
            KeyValuePair<string, int> s1 = decision.decisionTable[0].OrderByDescending(d => d.Value).First();
            KeyValuePair<string, int> s2 = decision.decisionTable[1].OrderByDescending(d => d.Value).First();

            List<int> s1ints = new();
            List<int> s2ints = new();

            foreach (KeyValuePair<string, int> s in decision.decisionTable[0])
            {
                s1ints.Add(s.Value);
            }
            foreach (KeyValuePair<string, int> s in decision.decisionTable[1])
            {
                s2ints.Add(s.Value);
            }
            Dictionary<int, int> disparities1 = new();
            Dictionary<int, int> disparities2 = new();
            Dictionary<int, int> disparities = new();
            for (int i = 0; i < s1ints.Count; i++)
            {
                disparities1.Add(i, Math.Abs(s1ints[i] - s1.Value));
            }
            for (int i = 0; i < s2ints.Count; i++)
            {
                disparities2.Add(i, Math.Abs(s2ints[i] - s2.Value));
            }
            for (int i = 0; i < s2ints.Count; i++)
            {
                if (disparities1[i] > disparities2[i])
                    disparities.Add(i, disparities1[i]);
                else
                    disparities.Add(i, disparities2[i]);
            }
            KeyValuePair<int, int> lowest = disparities.OrderBy(x => x.Value).First();
            int index = lowest.Key;
            KeyValuePair<string, int> result = decision.decisionTable[0].ElementAt(index);
            KeyValuePair<string, int> myValue = new KeyValuePair<string, int>(result.Key, lowest.Value);
            return myValue;
        }
        public static List<List<double>> Hurwitz(Decision decision)
        {
            List<Dictionary<string, int>> decisionTable = decision.decisionTable;
            Dictionary<string, int> s1 = decisionTable.ElementAt(0);
            Dictionary<string, int> s2 = decisionTable.ElementAt(1);
            List<string> alternatives = new();
            List<double> s1Values = new();
            List<int> s2Values = new();
            List<List <double>> hurwitz = new();
            List<double> increments = new();

            foreach (KeyValuePair<string, int> k in s1)
            {
                alternatives.Add(k.Key);
                s1Values.Add(k.Value);
            }
            foreach (KeyValuePair<string, int> k in s2)
            {
                s2Values.Add(k.Value);
            }
            for (int i = 0; i < s1Values.Count && i < s2Values.Count; i++)
            
            {
                increments.Add(Math.Abs((double)s1Values[i] - (double)s2Values[i]) / 10);
            }

            for (int i = 0; i < s1Values.Count; i++)
            {
                List<double> column = new();
                double point = s1Values[i];
                for (int j = 0; j <= 10; j++)
                {
                    column.Add(Math.Round(point, 1));
                    point += increments[i];
                }
                hurwitz.Add(column);
            }
            return hurwitz;
        }
        public static void DisplayTable(List<List<double>> data)
        {
            foreach (var row in data)
            {
                foreach (var cell in row)
                {
                    Console.Write($"{cell}\t");
                }
                Console.WriteLine();
            }
        }
        public static List<List<double>> Transpose(List<List<double>> matrix)
        {
            int rowCount = matrix.Count;
            int colCount = matrix[0].Count;

            List<List<double>> transposedMatrix = new List<List<double>>();

            for (int col = 0; col < colCount; col++)
            {
                List<double> newRow = new List<double>();
                for (int row = 0; row < rowCount; row++)
                {
                    newRow.Add(matrix[row][col]);
                }
                transposedMatrix.Add(newRow);
            }

            return transposedMatrix;
        }
    }
}