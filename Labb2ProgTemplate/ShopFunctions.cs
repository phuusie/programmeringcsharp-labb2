using System.Text;

namespace Labb2ProgTemplate;

public class ShopFunctions
{

    public int _selectOption;

    public void Title()
    {
        string title = @"                                                                                   
,-----.  ,--.  ,--.        ,--.                              ,--.            ,--.   
|  |) /_ `--',-'  '-. ,---.|  ,---. ,--,--,--. ,--,--.,--.--.|  |,-. ,---. ,-'  '-. 
|  .-.  \,--.'-.  .-'| .--'|  .-.  ||        |' ,-.  ||  .--'|     /| .-. :'-.  .-' 
|  '--' /|  |  |  |  \ `--.|  | |  ||  |  |  |\ '-'  ||  |   |  \  \\   --.  |  |   
`------' `--'  `--'   `---'`--' `--'`--`--`--' `--`--'`--'   `--'`--'`----'  `--'   
                                                                                   ";

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    public void OptionHighlight(string[] options, int selectOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i == selectOption)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(options[i]);

            Console.ResetColor();
        }
    }

    public void KeyOption(ConsoleKey keyPress, int selectedOption, Dictionary<int, Action> optionActions, int arrayLength)
    {

        switch (keyPress)
        {
            case ConsoleKey.UpArrow:
                if (selectedOption > 0)
                    _selectOption--;
                break;

            case ConsoleKey.DownArrow:
                if (selectedOption < arrayLength - 1)
                    _selectOption++;
                break;

            case ConsoleKey.Enter:
                if (optionActions.TryGetValue(selectedOption, out var action))
                {
                    action.Invoke();
                }
                break;
        }
    }

    public void BlackWhiteText(string highlight)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine(highlight);
        Console.ResetColor();
    }

    public string HidePassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
            else
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }

        } while (true);

        Console.WriteLine();

        return password.ToString();
    }

}