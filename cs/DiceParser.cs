class DiceParser
{
	public List<Dice> Parse(string[] args)
	{
		List<Dice> dices = new List<Dice>();
		foreach (string arg in args)
		{
			string[] sides = arg.Split(',');
			int[] diceSides = new int[sides.Length];
			for (int i = 0; i < sides.Length; i++)
			{
				string side = sides[i];
				if (!int.TryParse(side, out int sideInt))
				{
					Console.WriteLine($"Invalid dice side: {side}");
					Environment.Exit(1);
				}
				if (sideInt < 0)
				{
					Console.WriteLine($"Dice side cannot be negative: {side}");
					Environment.Exit(1);
				}
				diceSides[i] = sideInt;
			}
			dices.Add(new Dice(diceSides));
		}
		return dices;
	}
}