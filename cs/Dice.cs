public class Dice
{
	public int[] Sides {get; private set;}
	
	public Dice(int[] sides)
	{
		this.Sides = sides;
	}
	
	public override string ToString()
	{
		return string.Join(",", Sides);
	}
}