using System.Runtime.CompilerServices;

var input = File.ReadAllLines("input.txt");

var grid = ParseToGrid(input);

Print(grid);


var start = 'S';
var end = 'E';
var endPosition = EndPosition(grid);
var startPosition = StartPosition(grid, endPosition);

grid[startPosition.X, startPosition.Y] = '`';
grid[endPosition.X, endPosition.Y] = '{';

var moiByLocation = new Dictionary<Location, MoveOfInterest>() {
    {endPosition, new MoveOfInterest('{', endPosition, 0)}
};


var possiblePaths = new Queue<Location>();
possiblePaths.Enqueue(endPosition);
while (possiblePaths.Any())
{
    var location = possiblePaths.Dequeue();
    var thisMoi = moiByLocation[location];

    List<Location> possibleNextMoves = GetNextMoves(location, grid);

    foreach (var nextMove in possibleNextMoves)
    {
        if (moiByLocation.ContainsKey(nextMove))
        {
            continue;
        }

        var nextLetter = grid[nextMove.X, nextMove.Y];

        if (nextLetter == start)
        {
            nextLetter = '`';
        }
        else if (nextLetter == end)
        {
            nextLetter = '{';
        }

        if (thisMoi.Letter - nextLetter <= 1)
        {
            moiByLocation[nextMove] = new MoveOfInterest(
                Letter: nextLetter,
                Location: nextMove,
                DistanceFromGoal: thisMoi.DistanceFromGoal + 1
            );
            possiblePaths.Enqueue(nextMove);
        }
    }
}

Console.WriteLine($"Part 1: {moiByLocation.Values.Single(x => x.Letter == '`').DistanceFromGoal}" );

Console.WriteLine($"Part 2: {moiByLocation.Values.Where(x => x.Letter == 'a').Select(poi => poi.DistanceFromGoal).Min()}");




List<Location> GetNextMoves(Location last, char[,] grid)
{
    List<Location> possibleLocations = new List<Location>()
    {
        new Location(last.X, last.Y - 1),
        new Location(last.X, last.Y + 1),
        new Location(last.X - 1, last.Y),
        new Location(last.X + 1, last.Y)
    };

    var toRemove = new List<Location>();
    foreach (var location in possibleLocations)
    {
        if(location.X >=0 && location.Y >= 0 && location.X < grid.GetLength(0) && location.Y < grid.GetLength(1))
        {
            var temp = grid[last.X, last.Y];
            var temp1 = grid[location.X, location.Y];
        }
        else
        {
            toRemove.Add(location);
        }
    }
    toRemove.ForEach(x => possibleLocations.Remove(x));
    return possibleLocations;
}


Location StartPosition(char[,] grid, Location endPosition)
{
    for (var i = 0; i < grid.GetLength(0); i++)
    {
        for (var j = 0; j < grid.GetLength(1); j++)
        {
            if (grid[i, j] == 'S') return new Location(i, j);
        }
    }

    throw new Exception();
}

Location EndPosition(char[,] grid)
{
    for (var i = 0; i < grid.GetLength(0); i++)
    {
        for (var j = 0; j < grid.GetLength(1); j++)
        {
            if (grid[i, j] == 'E') return new Location(i, j);
        }
    }

    throw new Exception();
}


char[,] ParseToGrid(string[] lines)
{
    var i = 0;
    var grid = new char[lines.Length, lines.First().Length];
    foreach (var row in lines)
    {
        var j = 0;
        foreach (var col in row)
        {
            grid[i, j] = col;
            j++;
        }
        i++;
    }

    return grid;
}



void Print(char[,] grid)
{
    for (int i = 0; i < grid.GetLength(0); i++)
    {
        for (int j = 0; j < grid.GetLength(1); j++)
        {
            Console.Write(grid[i, j]);
        }
        Console.WriteLine();
    }
}

record Location(int X, int Y);

record MoveOfInterest(char Letter, Location Location, int DistanceFromGoal);