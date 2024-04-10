using N1_SO_KAZAR;
using System.IO;

List<string[]> lines = decisionMakingMethods.ReadCSV(@"C:\fax frfr\2.letnik\SO\prodaja.csv");
foreach (string[] line in lines)
{
    foreach (string value in line)
    {
        Console.Write(value + " ");
    }
    Console.WriteLine();
}
Console.WriteLine();
Decision s = new(lines);
KeyValuePair<string, int> optimistic = decisionMakingMethods.Optimist(s);
Console.WriteLine("Optimist : " + optimistic.Key + "(" + optimistic.Value + ")");
Console.WriteLine();

KeyValuePair<string, int> pesimistic = decisionMakingMethods.Pesimist(s);
Console.WriteLine("Pesimist : " + pesimistic.Key + "(" + pesimistic.Value + ")");
Console.WriteLine();

KeyValuePair<string, double> laplance = decisionMakingMethods.Laplace(s);
Console.WriteLine("Laplance : " + laplance.Key + "(" + laplance.Value + ")");
Console.WriteLine();
KeyValuePair<string, int> savage = decisionMakingMethods.Savage(s);
Console.WriteLine("Savage : " + savage.Key + "(" + savage.Value + ")");

List<List<double>> hurwitz = decisionMakingMethods.Transpose(decisionMakingMethods.Hurwitz(s));
Console.WriteLine();
Console.WriteLine("Hurwitz : ");
decisionMakingMethods.DisplayTable(hurwitz);

List<string> categories = new();

foreach (KeyValuePair<string, int> sit in s.decisionTable[0])
{
    categories.Add(sit.Key);
}
