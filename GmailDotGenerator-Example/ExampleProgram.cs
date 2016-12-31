using System;
using GoMan;

namespace GmailDotGenerator_Example
{
    internal class ExampleProgram
    {
        private static int _lastSecondsRemaining;

        private static void Main()
        {
            PrintEmails("short@googlemail.com");
            PrintEmails("longernotmuch@googlemail.com");
            Console.ReadLine();
        }

        private static void OnEmailGenerated(object sender, GmailDotGeneratorEventArgs e)
        {
            //Console.Clear();
            var ts = new TimeSpan(e.EstimatedCompletionTime);
            var secondsRemaining = (int)ts.TotalMilliseconds / 1000;

            if (secondsRemaining != _lastSecondsRemaining)
            {
                _lastSecondsRemaining = secondsRemaining;
                Console.Clear();
                Console.WriteLine($"Estimated completion time is {_lastSecondsRemaining} seconds");
            }
        }

        private static void PrintEmails(string email)
        {
            try
            {
                using (var gmailDotGenerator = GmailDotGenerator.Load(email))
                {
                    gmailDotGenerator.OnEstimatedCompletionTime += OnEmailGenerated;

                    Console.WriteLine(gmailDotGenerator.Configuration);
                    Console.WriteLine("Press any key to continue....");
                    Console.ReadLine();

                    if (gmailDotGenerator.GeneratedEmails == null)
                        gmailDotGenerator.GenerateEmails();
                    try
                    {
                        while (true)
                        {
                            Console.WriteLine(gmailDotGenerator.GetSingleEmailAndMarkUsed());
                        }

                        Console.WriteLine("Done! Press any key to continue....");
                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    gmailDotGenerator.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}