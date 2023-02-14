using Ask.Models;
using Ask.Services;
using Microsoft.Extensions.Configuration;

namespace Ask;

public class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Input is required");
                return;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("Only one input is required");
                return;
            }

            var configBuilder = new ConfigurationBuilder().
                    AddJsonFile("appsettings.json").Build();

            var apiUrl = configBuilder["Api:Url"];
            var apiKey = configBuilder["Api:Key"];

            var askService = new AskService(apiUrl, apiKey);
            var answer = await askService.AskAsync(args[0]);

            GuessAndCopyCommand(answer);
            PrintAnswer(answer);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    static void PrintAnswer(Answer answer)
    {
        Console.ForegroundColor = answer.Success ? ConsoleColor.Green : ConsoleColor.Red;

        string text = answer.Text;

        if (text.StartsWith("\n"))
        {
            text = text[2..];
        }

        if (text.EndsWith("\n"))
        {
            text = text[..^2];
        }

        Console.WriteLine(text);
        Console.ResetColor();
    }

    static void GuessAndCopyCommand(Answer answer)
    {
        if (answer.Success)
        {
            var lastIndex = answer.Text.LastIndexOf('\n');
            string guess = answer.Text[(lastIndex + 1)..];

            TextCopy.ClipboardService.SetText(guess);
        }
    }
}