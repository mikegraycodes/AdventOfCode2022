var ruckSacks = new List<Rucksack>();

File.ReadLines(@"input.txt").ToList().
    ForEach(line => ruckSacks.Add(new Rucksack(line[..(line.Length/2)], line[(line.Length/2)..])));


var elveGroups = new List<ElvesGroup>();

ruckSacks.Chunk(3).ToList().ForEach(x => elveGroups.Add(new ElvesGroup(x.ToList())));


Console.WriteLine($"Part 1: {ruckSacks.Sum(x => x.ItemPriorityTotal)}");
Console.WriteLine($"Part 2: {elveGroups.Sum(x => x.ItemPriorityTotal)}");



public class ElvesGroup
{
    private readonly List<Rucksack> _ruckSacks;

    public int ItemPriorityTotal => ItemPriority(_ruckSacks[0].BothCompartements.Select(x => x).Where(x => x.ToString().Intersect(_ruckSacks[1].BothCompartements).Any() && x.ToString().Intersect(_ruckSacks[2].BothCompartements).Any()).First());

    public ElvesGroup(List<Rucksack> ruckSacks)
    {
        if (ruckSacks.Count != 3) throw new Exception("Elves must be in groups of 3");

        _ruckSacks = ruckSacks;
    }
    private int ItemPriority(char input)
    {
        return char.IsUpper(input) ? input - 38 : input - 96;
    }
}



public class Rucksack
{
    private readonly string _compartement1;
    private readonly string _compartement2;

    public string BothCompartements => _compartement1 + _compartement2;

    public int ItemPriorityTotal => ItemPriority(_compartement1.Select(x => x).Where(x => _compartement2.Contains(x)).First());


    public Rucksack(string compartement1, string compartement2)
    {
        _compartement1 = compartement1;
        _compartement2 = compartement2;
    }

    private int ItemPriority(char input)
    {
        return char.IsUpper(input) ? input - 38 : input - 96;
    }

}



