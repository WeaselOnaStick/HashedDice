public class DiceGame
{
	DiceParser diceParser = new DiceParser();
	List<Dice> dices = new List<Dice>();
	
	public void run(string[] args)
	{
		dices = diceParser.Parse(args);
		for (int i = 0; i < dices.Count; i++) Console.WriteLine($"Dice #{i} : {dices[i].ToString()}");
	}
}