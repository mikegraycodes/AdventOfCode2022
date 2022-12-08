var lines = File.ReadAllLines("input.txt");

var grid = ParseToGrid(lines);

var countVisibleTress = CountVisibleTrees(lines, grid) + ((lines.First().Length + lines.Length) * 2) - 4;

var maxScenicScore = CountScenicScore(lines, grid);

Console.WriteLine($"Part 1: {countVisibleTress}");

Console.WriteLine($"Part 2: {maxScenicScore}");


static int[,] ParseToGrid(string[] lines)
{
    var i = 0;
    var grid = new int[lines.First().Length, lines.Length];
    foreach (var row in lines)
    {
        var j = 0;
        foreach (var col in row)
        {
            grid[i, j] = int.Parse(col.ToString());
            j++;
        }
        i++;
    }

    return grid;
}


int CountScenicScore(string[] lines, int[,] grid)
{
    var values = new List<int>();

    for (int k = 0; k < lines.First().Length; k++)
    {
        for (int l = 0; l < lines.Length; l++)
        {
            int treeHeight = grid[k, l];

            var leftDistance = GetLeftViewingDistance(k, l, treeHeight, grid);
            var rightDistance = GetRightViewingDistance(k, l, treeHeight, grid);
            var downDistance = GetDownViewingDistance(k, l, treeHeight, grid);
            var upDistance = GetUpViewingDistance(k, l, treeHeight, grid);

            values.Add(leftDistance*rightDistance*downDistance*upDistance);
        }
    }

    return values.Max();
}

int CountVisibleTrees(string[] lines, int[,] grid)
{
    var count = 0;

    for (int k = 1; k < lines.First().Length - 1; k++)
    {
        for (int l = 1; l < lines.Length - 1; l++)
        {
            int treeHeight = grid[k, l];

            if (!AreLeftTreesTaller(k, l, treeHeight, grid) ||
                !AreRightTreesTaller(k, l, treeHeight, grid) ||
                !AreDownTreesTaller(k, l, treeHeight, grid) ||
                !AreUpTreesTaller(k, l, treeHeight, grid)) count++;
        }
    }

    return count;
}

int GetLeftViewingDistance(int k, int l, int treeHeight, int[,] grid)
{
    var distance = 0;
    for (int a = l-1; a >= 0; a--)
    {
        var height = grid[k, a];

        if (height >= treeHeight && (a != 0))
        {
            distance++; 
            break;
        }

        distance++;
    }

    return distance;
}

int GetRightViewingDistance(int k, int l, int treeHeight, int[,] grid)
{
    var distance = 0;
    for (int a = l + 1; a < grid.GetLength(0); a++)
    {
        var height = grid[k, a];

        if (height >= treeHeight && (a != grid.GetLength(0)))
        {
            distance++;
            break;
        }
        distance++;
    }

    return distance;
}

int GetDownViewingDistance(int k, int l, int treeHeight, int[,] grid)
{
    int distance = 0;
    for (int a = k + 1; a < grid.GetLength(1); a++)
    {
        var height = grid[a, l];

        if (height >= treeHeight && (a != grid.GetLength(1)))
        {
            distance++;
            break;
        }
        distance++;
    }

    return distance;
}

int GetUpViewingDistance(int k, int l, int treeHeight, int[,] grid)
{
    var distance = 0;
    for (int a = k-1; a >= 0; a--)
    {
        var height = grid[a, l];

        if (height >= treeHeight && (a != 0))
        {
            distance++;
            break;
        }
        distance++;
    }

    return distance;
}



bool AreLeftTreesTaller(int k, int l, int treeHeight, int[,] grid)
{
    for (int a = 0; a < l; a++)
    {
        var height = grid[k, a];

        if (height >= treeHeight)
            return true;
    }

    return false;
}

bool AreRightTreesTaller(int k, int l, int treeHeight, int[,] grid)
{
    for (int a = l + 1; a < grid.GetLength(0); a++)
    {
        var height = grid[k, a];

        if (height >= treeHeight)
            return true;
    }

    return false;
}

bool AreDownTreesTaller(int k, int l, int treeHeight, int[,] grid)
{
    for (int a = k + 1; a < grid.GetLength(1); a++)
    {
        var height = grid[a, l];

        if (height >= treeHeight)
            return true;
    }

    return false;
}

bool AreUpTreesTaller(int k, int l, int treeHeight, int[,] grid)
{
    for (int a = 0; a < k; a++)
    {
        var height = grid[a, l];

        if (height >= treeHeight)
            return true;
    }

    return false;
}