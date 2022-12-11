var text = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {Solve(text, 4)}");
Console.WriteLine($"Part 2: {Solve(text, 14)}");


static int Solve(string text, int numberOfMarkers)
{
    var position = 0;
    for (var i = 0; i < text.Length; i++)
    {
        var currentString = text.Substring(i, numberOfMarkers).Distinct();

        var distinct = currentString.Distinct();

        if (distinct.Count() == numberOfMarkers)
        {
            position = i + numberOfMarkers;
            break;
        }
    }
    return position;
}