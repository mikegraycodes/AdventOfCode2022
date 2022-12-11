var input = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {Solve(input).Where(size => size < 100000).Sum()}");



var freeSpace = 70000000 - Solve(input).Max();
Console.WriteLine($"Part 2: {Solve(input).Where(size => size + freeSpace >= 30000000).Min()}");


IEnumerable<int> Solve(string input)
{
    var iterator = input.Split("\n").AsEnumerable().GetEnumerator();
    iterator.MoveNext();
    return Flatten(GetDir(iterator)).Select(dir => dir.size);
}

Dir GetDir(IEnumerator<string> iterator)
{
    string name = iterator.Current.Split(' ')[2];
    iterator.MoveNext();

    var files = 0;
    var subdirs = new List<Dir> { };

    while (iterator.MoveNext() && !iterator.Current.StartsWith("$ cd .."))
    {
        if (iterator.Current.StartsWith("$ cd"))
        {
            subdirs.Add(GetDir(iterator));
        }
        else if (!iterator.Current.StartsWith("dir "))
        {
            files += int.Parse(iterator.Current.Split(" ")[0]);
        }
    }
    return new Dir(name, files + subdirs.Select(dir => dir.size).Sum(), subdirs);
}

IEnumerable<Dir> Flatten(Dir dir) => dir.subdirs.SelectMany(Flatten).Append(dir);

record struct Dir(string name, int size, List<Dir> subdirs);