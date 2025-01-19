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
		PCThrow();
		PlayerThrow();
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
			PlayerPicksDice();
			PCPicksDice();
		}
		else
		{
			Console.WriteLine($"Sorry! You didn't guess my number (KEY={srgbin.SecretKey})");
			PCPicksDice();
			PlayerPicksDice();
		}
	}

	public void PlayerThrow()
	{
        Console.WriteLine("Time for your throw...");
        SecureRandomGen pcThrowRand = new SecureRandomGen(0, 5);
        Console.WriteLine($"I've selected a random value in 0..5 range (HMAC={pcThrowRand.Hmac})");
        int userModSix = ReadModSix();
        Console.WriteLine($"You've selected {userModSix}");
        Console.WriteLine($"My number was {pcThrowRand.secretInt} (KEY={pcThrowRand.SecretKey})");
        int res = (pcThrowRand.secretInt + userModSix) % 6;
        Console.WriteLine($"Resulting sum: {pcThrowRand.secretInt} + {userModSix} = {res} (mod 6)");
        scorePlayer = dicePlayer.Sides[res];
        Console.WriteLine($"You got a score of {scorePlayer}");
    }

	public void PCThrow()
	{
		Console.WriteLine("Time for my throw...");
		SecureRandomGen pcThrowRand = new SecureRandomGen(0, 5);
		Console.WriteLine($"I've selected a random value in 0..5 range (HMAC={pcThrowRand.Hmac})");
		int userModSix = ReadModSix();
        Console.WriteLine($"You've selected {userModSix}");
		Console.WriteLine($"My number was {pcThrowRand.secretInt} (KEY={pcThrowRand.SecretKey})");
		int res = (pcThrowRand.secretInt + userModSix) % 6;
        Console.WriteLine($"Resulting sum: {pcThrowRand.secretInt} + {userModSix} = {res} (mod 6)");
		scorePC = dicePC.Sides[res];
		Console.WriteLine($"I got a score of {scorePC}");
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

	public void PlayerPicksDice()
	{
		Console.WriteLine("Pick your poison...");
		int pick_index = optionsGiver.ReadOptions(dices.Select(x => x.ToString()).ToList());
		dicePlayer = dices[pick_index];
		Console.WriteLine($"You've picked die #{pick_index} ({dicePlayer})");
		dices.RemoveAt(pick_index);
	}

	public void PCPicksDice()
	{
		Console.WriteLine("Let me pick a die...");
		int pick_index = new Random().Next(0, dices.Count);
		Console.WriteLine($"I've picked die #{pick_index} ({dicePC})");
		dicePC = dices[pick_index];
		dices.RemoveAt(pick_index);
	}

	public int ReadModSix()
	{
		Console.WriteLine("Add your number modulo 6.");
		return optionsGiver.ReadOptions(new List<string> { "0", "1", "2", "3", "4", "5" });
	}
}