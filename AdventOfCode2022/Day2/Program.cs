var games = new List<Game>();
var fixedGames = new List<FixedGame>();


foreach (string line in File.ReadLines(@"input.txt"))
{
    games.Add(new Game(Choice.Create(line[0]), Choice.Create(line[2])));

    fixedGames.Add(new FixedGame(Choice.Create(line[0]), DesiredResult.Create(line[2])));

}

Console.WriteLine(games.Sum(x => x.MyScore));
Console.WriteLine(fixedGames.Sum(x => x.MyScore));

public class Game
{
    private Choice _opponentChoice;
    private Choice _myChoice;

    public int OpponentScore => _oppenentWinLoseDrawScore + _opponentChoice.Score;
    public int MyScore => _myWinLoseDrawScore + _myChoice.Score;

    private int _oppenentWinLoseDrawScore;
    private int _myWinLoseDrawScore;

    public Game(Choice opponentChoice, Choice myChoice)
    {
        _opponentChoice = opponentChoice;
        _myChoice = myChoice;
        Play();
    }

    private void Play()
    {
        if(_opponentChoice.Equals(_myChoice))
        {
            _oppenentWinLoseDrawScore = 3;
            _myWinLoseDrawScore = 3;
        }

        if((_opponentChoice.Equals(Choice.Rock) && _myChoice.Equals(Choice.Scissors)) ||
            (_opponentChoice.Equals(Choice.Scissors) && _myChoice.Equals(Choice.Paper)) ||
            (_opponentChoice.Equals(Choice.Paper) && _myChoice.Equals(Choice.Rock)))
                
        {
            _oppenentWinLoseDrawScore = 6;
            _myWinLoseDrawScore = 0;
        }

        if ((_myChoice.Equals(Choice.Rock) && _opponentChoice.Equals(Choice.Scissors)) ||
            (_myChoice.Equals(Choice.Scissors) && _opponentChoice.Equals(Choice.Paper)) ||
            (_myChoice.Equals(Choice.Paper) && _opponentChoice.Equals(Choice.Rock)))

        {
            _oppenentWinLoseDrawScore = 0;
            _myWinLoseDrawScore = 6;
        }
    }
}

public class FixedGame
{
    private Choice _opponentChoice;
    private Choice _myChoice;
    private DesiredResult _desiredResult;

    public int OpponentScore => _oppenentWinLoseDrawScore + _opponentChoice.Score;
    public int MyScore => _myWinLoseDrawScore + _myChoice.Score;

    private int _oppenentWinLoseDrawScore;
    private int _myWinLoseDrawScore;

    public FixedGame(Choice opponentChoice, DesiredResult desiredResult)
    {
        _desiredResult = desiredResult;
        _opponentChoice = opponentChoice;
        Play();
    }

    private void Play()
    {
        if (_desiredResult.Equals(DesiredResult.Draw))
        {
            _oppenentWinLoseDrawScore = 3;
            _myWinLoseDrawScore = 3;
            _myChoice = _opponentChoice;
        }

        if (_desiredResult.Equals(DesiredResult.Lose))
        {
            _oppenentWinLoseDrawScore = 6;
            _myWinLoseDrawScore = 0;
            if (_opponentChoice.Equals(Choice.Rock)) _myChoice = Choice.Scissors;
            if (_opponentChoice.Equals(Choice.Scissors)) _myChoice = Choice.Paper;
            if (_opponentChoice.Equals(Choice.Paper)) _myChoice = Choice.Rock;
        }

        if (_desiredResult.Equals(DesiredResult.Win))
        {
            _oppenentWinLoseDrawScore = 0;
            _myWinLoseDrawScore = 6;

            if (_opponentChoice.Equals(Choice.Rock)) _myChoice = Choice.Paper;
            if (_opponentChoice.Equals(Choice.Scissors)) _myChoice = Choice.Rock;
            if (_opponentChoice.Equals(Choice.Paper)) _myChoice = Choice.Scissors;
        }
    }
}

public class DesiredResult : IEquatable<DesiredResult>
{
    public static DesiredResult Win => new DesiredResult(0);
    public static DesiredResult Lose => new DesiredResult(1);
    public static DesiredResult Draw => new DesiredResult(2);

    public int Value { get; }
    private DesiredResult(int result)
    {
        Value = result;
    }

    public static DesiredResult Create(char input)
    {
        if (input == 'A' || input == 'X') return DesiredResult.Lose;
        if (input == 'B' || input == 'Y') return DesiredResult.Draw;
        if (input == 'C' || input == 'Z') return DesiredResult.Win;

        throw new Exception();
    }

    public bool Equals(DesiredResult? other)
    {
        if (other == null) return false;

        return Value == other.Value;
    }
}


public class Choice : IEquatable<Choice>
{
    public static Choice Rock => new Choice(0, 1);
    public static Choice Paper => new Choice(1, 2);
    public static Choice Scissors => new Choice(2, 3);

    public int Value { get; }
    public int Score { get; }
    private Choice(int choice, int score)
    {
        Value = choice;
        Score = score;
    }

    public static Choice Create(char input)
    {
        if (input == 'A' || input == 'X') return Choice.Rock;
        if (input == 'B' || input == 'Y') return Choice.Paper;
        if (input == 'C' || input == 'Z') return Choice.Scissors;

        throw new Exception();
    }

    public bool Equals(Choice? other)
    {
        if(other == null) return false;

        return Value == other.Value;
    }
}