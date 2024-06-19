using System;
using System.IO;
using System.Threading;

class Program
{
    private static string targetFilePath;
    private static string lastFileContent;

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the full path of the target text file:");
        targetFilePath = Console.ReadLine();

        if (!File.Exists(targetFilePath))
        {
            Console.WriteLine("File does not exist. Exiting...");
            return;
        }

        lastFileContent = File.ReadAllText(targetFilePath);

        Timer timer = new Timer(CheckFileChanges, null, 0, 15000);

        Console.WriteLine("Monitoring file changes. Press Enter to stop.");
        Console.ReadLine();
    }

    private static void CheckFileChanges(object state)
    {
        try
        {
            string currentFileContent = File.ReadAllText(targetFilePath);
            if (currentFileContent != lastFileContent)
            {
                Console.WriteLine($"File changed at {DateTime.Now}");
                Console.WriteLine("Changes:");
                Console.WriteLine(GetChanges(lastFileContent, currentFileContent));
                lastFileContent = currentFileContent;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
        }
    }

    private static string GetChanges(string oldContent, string newContent)
    {
        using (StringReader oldReader = new StringReader(oldContent))
        using (StringReader newReader = new StringReader(newContent))
        {
            string oldLine;
            string newLine;
            string changes = string.Empty;
            int lineNumber = 1;

            while ((oldLine = oldReader.ReadLine()) != null && (newLine = newReader.ReadLine()) != null)
            {
                if (oldLine != newLine)
                {
                    changes += $"Line {lineNumber}:\nOld: {oldLine}\nNew: {newLine}\n";
                }
                lineNumber++;
            }

            while ((oldLine = oldReader.ReadLine()) != null)
            {
                changes += $"Line {lineNumber} removed: {oldLine}\n";
                lineNumber++;
            }

            while ((newLine = newReader.ReadLine()) != null)
            {
                changes += $"Line {lineNumber} added: {newLine}\n";
                lineNumber++;
            }

            return changes;
        }
    }
}
