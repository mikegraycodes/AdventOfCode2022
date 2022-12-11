var input = File.ReadAllLines("input.txt");


Console.WriteLine($"Part 1: {Solve(input, 2)}");

Console.WriteLine($"Part 2: {Solve(input, 10)}");

int Solve(string[] lines, int snakeLength)
{
    var commands = ParseCommands(lines);
    var tailLocations = new List<Location>();
    var snake = Enumerable.Range(1, snakeLength).Select(x => new Location(0, 0)).ToList();
    foreach (var command in commands)
    {
        for (var i = 0; i < command.Amount; i++)
        {
            snake[0] = MoveHead(command.Direction, snake[0]);
            for (var j = 1; j < snake.Count; j++)
            {
                snake[j] = Follow(snake[j], snake[j - 1]);
            }
            tailLocations.Add(snake.Last());
            //Draw();
        }
    }
    return tailLocations.Distinct().Count();
}


List<Command> ParseCommands(string[] lines)
{
    return lines.Select(line => new Command(line.Split(" ")[0], int.Parse(line.Split(" ")[1]))).ToList();
}


Location MoveHead(string direction, Location head)
{
    return direction switch
    {
        "R" => head with { X = head.X + 1 },
        "L" => head with { X = head.X - 1 },
        "U" => head with { Y = head.Y + 1 },
        "D" => head with { Y = head.Y - 1 },
        _ => head
    };
}

Location Follow(Location current, Location target)
{
    var distanceX = target.X - current.X;
    var distanceY = target.Y - current.Y;

    if (Math.Abs(distanceX) <= 1 && Math.Abs(distanceY) <= 1)
    {
        return current;
    }

    return new Location(X: current.X + Math.Sign(distanceX), Y: current.Y + Math.Sign((distanceY)));
}

void Draw(IEnumerable<Location> locations)
{
    var size = 50;
    var toDraw = new string[size, size];


    for (var i = 0; i < locations.Count(); i++)
    {
        toDraw[(locations.ElementAt(i).Y * -1 + size/2), locations.ElementAt(i).X + size/2] = i.ToString();
    }

    for (var i = 0; i < toDraw.GetLength(0); i++)
    {
        for (var j = 0; j < toDraw.GetLength(1); j++)
        {
            switch (toDraw[i, j])
            {
                case null:
                    Console.Write("." + " ");
                    break;
                case "0":
                    Console.Write("H" + " ");
                    break;
                default:
                    Console.Write(toDraw[i, j] + " ");
                    break;
            }
        }
        Console.WriteLine();
    }
}




public record Location(int X, int Y);

public record Command(string Direction, int Amount);




