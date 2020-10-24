using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Colorful;
using Konsole;
using Console = Colorful.Console;

namespace BigBlueButton_Video_Downloader.UI
{
    public static class ConsoleManager
    {
        static ConsoleManager()
        {
            Console.Clear();
        }

        public static void ShowWelcome()
        {
            FigletFont font = FigletFont.Load("chunky.flf");
            Figlet figlet = new Figlet(font);
            Console.WriteLine(figlet.ToAscii("BBB   Downloader"), Color.LimeGreen);
            Console.WriteLine(
                "For Documentation And Source Code Please Visit https://github.com/berkayalcin/BigBlueButton-Downloader",
                Color.LimeGreen);
        }

    }
}