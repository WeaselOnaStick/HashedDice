using ConsoleTables;
using System.Data.Common;

// Also calculates chances since it's easy
static class HelpDisplayer
{
	static public string GenerateChancesTable(List<Dice> diceList)
	{
        string[] cols = new string[diceList.Count + 1];
        cols[0] = "User Dice v";
        for (int i = 0; i < diceList.Count; i++)
        {
            cols[i+1] = diceList[i].ToString();
        }
        ConsoleTable table = new ConsoleTable(cols);
        for (int i = 0; i < diceList.Count; i++)
        {
            string[] rowvals = new string[diceList.Count+1];
            rowvals[0] = diceList[i].ToString();
            for (int j = 0; j < diceList.Count; j++)
            {
                if (i == j)
                    rowvals[j + 1] = "---";
                else
                    rowvals[j + 1] = win_chance(diceList[i], diceList[j]).ToString("0.0000");
            }
            table.AddRow(rowvals);
        }


        return "Probability of user winning:\n"+table.ToMarkDownString();
    }

    static float win_chance(Dice user, Dice pc)
    {
        float res = 0;
        foreach (int sideU in user.Sides)
        {
            int count_wins = 0;
            foreach (int sideP in pc.Sides) if (sideU > sideP) count_wins++;
            res += count_wins / 6.0f;
        }
        return res / 6f;
    }
}