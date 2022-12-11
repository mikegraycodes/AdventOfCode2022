

var input = File.ReadLines(@"input.txt");

var commandList = ParseCommands(input);

Part1(commandList, input).ToList().ForEach(Console.Write);
Console.WriteLine();
Part2(commandList, input).ToList().ForEach(Console.Write);




IEnumerable<string> Part1(IEnumerable<Command> commands, IEnumerable<string> text)
{
    var stacks = Stacks(text);
    foreach (var command in commands)
    {
        for (var i = 0; i < command.Amount; i++)
        {
            var crate = stacks.First(x => x.StackId == command.From).Pop();
            stacks.First(x => x.StackId == command.To).Push(crate);
        }
        
    }
    return stacks.Select(x => x.First);
}


IEnumerable<string> Part2(IEnumerable<Command> commands, IEnumerable<string> text)
{
    var stacks = Stacks(text);
    foreach (var command in commands)
    {
        var crate = stacks.First(x => x.StackId == command.From).PopRange(command.Amount);
        stacks.First(x => x.StackId == command.To).Push(crate);
    }
    return stacks.Select(x => x.First);
}


IEnumerable<Command> ParseCommands(IEnumerable<string> text)
{
    var commandList1 = text.Select(x => x)
        .Where(x => x.StartsWith("move"))
        .Select(x => new Command(int.Parse(x.Split(" ")[1]), int.Parse(x.Split(" ")[3]), int.Parse(x.Split(" ")[5])));
    return commandList1;
}

string CleanLetter(char[] chars)
{
    return new string(chars).Replace(" ", "").Replace("[", "").Replace("]", "");
}

List<Stack> Stacks(IEnumerable<string> enumerable)
{
    var columns = enumerable.Take(8).Select(x => x.Chunk(4));
    
    var list = Enumerable.Range(1, 9).ToList().Select(x => new Stack(x)).ToList();

    foreach (var col in columns)
    {
        var i = 1;
        foreach (var something in col)
        {
            var letter = CleanLetter(something);

            if (!string.IsNullOrEmpty(letter)) list.First(x => x.StackId == i).Push(new Crate(letter));
            i++;
        }
    }

    list.ForEach(x => x.Reverse());

    return list;
}

public record Command(int Amount, int From, int To);

public class Crate
{
    public string Letter { get; }
    public Crate(string letter)
    {
        Letter = letter;
    }
}


public class Stack
{
    public int StackId { get; }
    private readonly List<Crate> _crates;

    public string First => _crates.Last()?.Letter != null ? _crates.Last()?.Letter : "  ";

    public Stack(int id)
    {
        StackId = id;
        _crates = new List<Crate>();
    }

    public void Push(Crate crate) => _crates.Add(crate);

    public void Push(List<Crate> crates) => _crates.AddRange(crates);


    public Crate Pop()
    {
        if (_crates.Count == 0) return null;
        var ret = _crates.TakeLast(1).First();
        _crates.Remove(ret);
        return ret;
    }

    public List<Crate> PopRange(int amount)
    {
        var ret = _crates.TakeLast(amount).ToList();
        ret.ForEach(x => _crates.Remove(x));
        return ret;
    }
    public void Reverse() => _crates.Reverse();
}