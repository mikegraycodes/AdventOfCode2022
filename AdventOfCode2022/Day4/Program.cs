// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var ElfAssignments = new List<ElfAssignments>();

File.ReadLines(@"input.txt").ToList()
    .ForEach(x => ElfAssignments
    .Add(new ElfAssignments(new Assignment(int.Parse(x.Split(",")[0].Split("-")[0]), int.Parse(x.Split(",")[0].Split("-")[1])),
                            new Assignment(int.Parse(x.Split(",")[1].Split("-")[0]), int.Parse(x.Split(",")[1].Split("-")[1])))));

Console.WriteLine(ElfAssignments.Count(x => x.CompletelyOverlap));
Console.WriteLine(ElfAssignments.Count(x => x.SomewhatOverlap));


public class ElfAssignments
{
    private readonly Assignment _assignment1;
    private readonly Assignment _assignment2;

    public bool CompletelyOverlap => _assignment1.Numbers.All(x => _assignment2.Numbers.Contains(x)) ||
                                     _assignment2.Numbers.All(x => _assignment1.Numbers.Contains(x));

    public bool SomewhatOverlap => _assignment1.Numbers.Intersect(_assignment2.Numbers).Any();

    public ElfAssignments(Assignment assignment1, Assignment assignment2)
    {
        _assignment1 = assignment1;
        _assignment2 = assignment2;
    }
 }

public class Assignment
{
    public int Lower { get; }
    public int Upper { get; }

    public IEnumerable<int> Numbers => Enumerable.Range(Lower, Upper - Lower + 1);

    public Assignment(int lower, int upper)
    {
        Lower = lower;
        Upper = upper;
    }
}
    
