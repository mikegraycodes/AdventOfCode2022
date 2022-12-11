var input = File.ReadAllText(@"input.txt");


Console.WriteLine($"Part 1: {Solve(20, input, true)}");
Console.WriteLine($"Part 2: {Solve(10000, input, false)}");

long Solve(int numberOfRounds, string text, bool part1)
{
    var monkeys = text.Bind(ParseMonkeys);
    var commonDivisor = monkeys.Bind(CommonDivisor);


    foreach (var i in Enumerable.Range(1, numberOfRounds))
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                var worryLevel = PerformOperation(item, monkey.Operation);

                worryLevel = part1 ? Convert.ToInt64(Math.Floor((double)worryLevel / 3d)) : worryLevel % commonDivisor;

                if (worryLevel % monkey.DivisibleBy == 0)
                {
                    monkeys.First(x => x.Id == monkey.MonkeyIfTrue).Items.Add(worryLevel);
                }
                else
                {
                    monkeys.First(x => x.Id == monkey.MonkeyIfFalse).Items.Add(worryLevel);
                }
                monkey.NumberOfItemsInspected++;
            }
            monkey.Items.Clear();
        }
    }
    return CalculateMonkeyBusiness(monkeys);
}

List<Monkey> ParseMonkeys(string text) => text.Split("\r\n\r\n")
    .Select(x => x.Split("\r\n"))
    .Select(GetMonkey).ToList();

Monkey GetMonkey(string[] monkey) =>
    new(monkey.Bind(ParseId), monkey.Bind(StartingItems), monkey.Bind(ParseOperation),
        monkey.Bind(DivisibleBy), monkey.Bind(IfTrueMonkey), monkey.Bind(IfFalseMonkey));

int IfFalseMonkey(string[] monkey) =>
    monkey.First(x => x.Contains("false"))
        .Split(" ").Last()
        .Bind(int.Parse);


int IfTrueMonkey(string[] monkey) =>
    monkey.First(x => x.Contains("true"))
        .Split(" ").Last()
        .Bind(int.Parse);


long DivisibleBy(string[] monkey) =>
        monkey.First(x => x.Contains("Test"))
        .Split(" ").Last()
        .Bind(long.Parse);


IEnumerable<long> StartingItems(string[] monkey) =>
    monkey.First(x => x.Contains("Start"))
        .Split(":").Last().Split(",").Select(x => x.Trim())
        .Select(long.Parse);

Operation ParseOperation(string[] monkey) =>
    monkey.Select(x => x).First(x => x.Contains("Op"))
        .Split(":").Last()
        .Split("=").Last()
        .TrimStart()
        .Split(" ")
        .Bind(x => new Operation(x.ElementAt(0), x.ElementAt(1), x.ElementAt(2)));


int ParseId(string[] monkey) =>
    monkey.First().Replace(":", "")
        .Split(" ").Last()
        .Bind(int.Parse);


long CalculateMonkeyBusiness(List<Monkey> monkeys) =>
    monkeys.OrderByDescending(x => x.NumberOfItemsInspected).Take(2)
        .Select(x => x.NumberOfItemsInspected)
        .Aggregate(1L, (x, y) => x * y);

long CommonDivisor(List<Monkey> monkeys) =>
    monkeys.Select(x => x.DivisibleBy).Aggregate(1L, (x, y) => x * y);


void Print(List<Monkey> list, int round)
{
    Console.WriteLine($"== After round {round + 1} ==");
    foreach (var monkey in list)
    {
        Console.WriteLine($"Monkey {monkey.Id} inspected items {monkey.NumberOfItemsInspected} times.");
    }
}




long PerformOperation(long item, Operation operation) =>
    operation.Op switch
    {
        "+" => item + Right(item, operation),
        "*" => item * Right(item, operation),
        "/" => item / Right(item, operation),
        _ => throw new Exception()
    };

long Right(long old, Operation operation) =>
    operation.Right != "old" ? int.Parse(operation.Right) : old;


class Monkey
{
    public Monkey(int id, IEnumerable<long> startingItems, Operation operation, long divisibleBy, int monkeyIfTrue, int monkeyIfFalse)
    {
        Id = id;
        Items = startingItems.ToList();
        Operation = operation;
        DivisibleBy = divisibleBy;
        MonkeyIfFalse = monkeyIfFalse;
        MonkeyIfTrue = monkeyIfTrue;
    }

    public int NumberOfItemsInspected = 0;
    public long WorryLevel => Items.Sum();
    public int Id { get; set; }
    public List<long> Items;
    public Operation Operation { get; }
    public long DivisibleBy { get; }
    public int MonkeyIfTrue { get; }
    public int MonkeyIfFalse { get; }
}

record Operation(string Left, string Op, string Right);

public static class Extensions
{
    public static TOut Bind<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) => f(@this);
}