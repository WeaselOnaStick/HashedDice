public class DiceGame
{
	DiceParser diceParser = new DiceParser();
	List<Dice> dices = new List<Dice>();
	OptionsGiver optionsGiver;

	Dice dicePlayer;
	Dice dicePC;

	int scorePlayer, scorePC;

	// Main Game "loop"

	public void run(string[] args)
	{
		dices = diceParser.Parse(args);
		optionsGiver = new OptionsGiver(HelpDisplayer.GenerateChancesTable(dices));
		Console.WriteLine($"Starting a game with following dice: {string.Join(' ', dices.Select(x => x.ToString()))}");
		RandPickFirst();
		ThrowStage(true);
		ThrowStage(false);
		GameResult();
    }

	// Game stages v

	public void RandPickFirst()
	{
		Console.WriteLine("Let's determine who makes the first move...");
		SecureRandomGen srgbin = new SecureRandomGen(0, 1);
		Console.WriteLine($"I've selected a random value in the range 0..1 (HMAC={srgbin.Hmac})\nTry to guess my selection. If you guess correctly - you pick your dice first.");
		int playerPick = optionsGiver.ReadOptions(new List<string> { "0", "1" });
		if (playerPick == srgbin.secretInt)
		{
			Console.WriteLine($"Congrats! You guessed my number (KEY={srgbin.SecretKey})");
			DicePicking(true);
            DicePicking(false);
		}
		else
		{
			Console.WriteLine($"Sorry! You didn't guess my number (KEY={srgbin.SecretKey})");
            DicePicking(false);
            DicePicking(true);
        }
	}

	public void ThrowStage(bool playersTurn)
	{
        Console.WriteLine(playersTurn ? "Time for your throw..." : "Time for my throw...");
        SecureRandomGen pcThrowRand = new SecureRandomGen(0, 5);
        Console.WriteLine($"I've selected a random value in 0..5 range (HMAC={pcThrowRand.Hmac})");
        int userModSix = ReadModSix();
        Console.WriteLine($"You've selected {userModSix}");
        Console.WriteLine($"My number was {pcThrowRand.secretInt} (KEY={pcThrowRand.SecretKey})");
        int res = (pcThrowRand.secretInt + userModSix) % 6;
        Console.WriteLine($"Resulting sum: {pcThrowRand.secretInt} + {userModSix} = {res} (mod 6)");
        if (playersTurn)
            scorePlayer = dicePlayer.Sides[res];
        else
            scorePC = dicePC.Sides[res];
        Console.WriteLine(playersTurn ? $"You got a score of {scorePlayer}!" : $"I got a score of {scorePC}!");
    }

	public void GameResult()
	{
		if (scorePlayer > scorePC)
            Console.WriteLine($"You win! {scorePlayer} > {scorePC}");
        else if (scorePlayer < scorePC)
            Console.WriteLine($"I win! {scorePlayer} < {scorePC}");
        else
            Console.WriteLine($"It's a draw! {scorePlayer} = {scorePC}");
    }

	// Helper functions

	public void DicePicking(bool playerPicks)
	{
		Console.WriteLine(playerPicks ? "Pick your poison..." : "Let me pick a die...");
		int pick_index;
		Dice picked_dice;
		if (playerPicks)
		{
			pick_index = optionsGiver.ReadOptions(dices.Select(x => x.ToString()).ToList());
            picked_dice = dices[pick_index];
			dicePlayer = picked_dice;
            Console.WriteLine($"You've picked die #{pick_index} ({picked_dice})");
        }
		else
		{
            pick_index = new Random().Next(0, dices.Count);
            picked_dice = dices[pick_index];
			dicePC = picked_dice;
            Console.WriteLine($"I've picked die #{pick_index} ({picked_dice})");
        }
        dices.RemoveAt(pick_index);
    }

	public int ReadModSix()
	{
		Console.WriteLine("Add your number modulo 6.");
		return optionsGiver.ReadOptions(new List<string> { "0", "1", "2", "3", "4", "5" });
	}
}