class DiceParser
{
	public List<Dice> Parse(string[] args)
	{
		List<Dice> dices = new List<Dice>();
		foreach (string arg in args)
		{
			string[] sides = arg.Split(',');
			if (sides.Length != 6)
			{
				Console.WriteLine($"Dice has incorrect number of sides ({sides.Length})");
                Environment.Exit(0);
            }
			int[] diceSides = new int[6];
			for (int i = 0; i < 6; i++)
			{
				string side = sides[i];
				if (!int.TryParse(side, out int sideInt))
				{
					Console.WriteLine($"Invalid dice side: {side}");
					Environment.Exit(0);
				}
				if (sideInt < 0)
				{
					Console.WriteLine($"Dice side cannot be negative: {side}");
					Environment.Exit(0);
				}
				diceSides[i] = sideInt;
			}
			dices.Add(new Dice(diceSides));
		}
		if (dices.Count < 2)
		{
            Console.WriteLine("At least two dices are required to play the game");
            Environment.Exit(0);
        }
		return dices;
	}
}