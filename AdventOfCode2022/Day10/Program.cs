var lines = File.ReadAllLines("input.txt");

var xDuringCycles = GetXDuringCycles(lines);

Console.WriteLine("================ Part 1 ================");
Console.WriteLine(Calculate(20, 40, 220, xDuringCycles.ToArray()));
Console.WriteLine();
Console.WriteLine("================ Part 2 ================");
Console.WriteLine();
xDuringCycles.Chunk(40).ToList().ForEach(Draw);


int Calculate(int start, int difference, int end, IReadOnlyList<int> during)
{
    var values = new List<int>();
    for (var i = start; i <= end; i += difference)
    {
        values.Add(i * during[i - 1]);
    }

    return values.Sum();
}

void Draw(int[] during)
{
    var row = "";
    for (var i = 0; i < during.Length; i++)
    {
        row = new List<int> { during[i] - 1, during[i] + 1, during[i] }.Any(x => x == i) ? row + "#" : row + ".";
    }

    Console.WriteLine(row);
}

List<int> GetXDuringCycles(IEnumerable<string> commands)
{
    var xDuringCyclesLocal = new List<int>();

    var x = 1;
    foreach (var command in commands)
    {
        if (command == "noop")
        {
            xDuringCyclesLocal.Add(x);
        }
        else if(!command.StartsWith("addx"))
        {
            xDuringCyclesLocal.Add(x);
            xDuringCyclesLocal.Add(x);
            x += int.Parse(command.Split(" ")[1]);
        }
    }

    return xDuringCyclesLocal;
}