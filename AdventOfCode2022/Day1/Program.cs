var elves = ParseFile();

Console.WriteLine($"Top Elf Calories: {elves.Max(x => x.TotalCalories)}");
Console.WriteLine($"Top 3 Elf Calories: {elves.OrderByDescending(x => x.TotalCalories).Take(3).Sum(x => x.TotalCalories)}");



static List<Elf> ParseFile()
{
    var elves = new List<Elf>();
    Elf elf = null;

    foreach (string line in File.ReadLines(@"input.txt"))
    {
        if (elf == null || string.IsNullOrEmpty(line))
        {
            elf = new Elf();
            elves.Add(elf);
        }

        if (!string.IsNullOrEmpty(line))
        {
            elf.AddFood(int.Parse(line));
        }
    }

    return elves;
}

public class Elf
{
    private List<int> calories = new List<int>();
    public int TotalCalories => calories.Sum();

    public void AddFood(int food)
    {
        calories.Add(food);
    }
}

