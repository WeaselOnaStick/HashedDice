using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

class OptionsGiver
{
    string helpString = "";
    public OptionsGiver(string HelpString)
    {
        this.helpString = HelpString;
    }

    public int ReadOptions(List<string> options)
    {
        int x;
        string input;
        string optionsList = "";
        for (int i = 0; i < options.Count; i++)
        {
            optionsList += $"{i} - {options[i]}\n";
        }
        optionsList += "? - Help\n";
        optionsList += "X - Exit";

        while (true)
        {
            Console.WriteLine(optionsList);
            input = Console.ReadLine();
            if (input == "X")
                Environment.Exit(0);
            if (input == "?")
            {
                Console.WriteLine(helpString);
                continue;
            }
            if (input != "" && int.TryParse(input, out int res) && res >= 0 && res < options.Count)
                return res;
            else
                Console.WriteLine("Invalid input! Please try again");
        }
    }
}
