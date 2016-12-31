using System;
using System.CodeDom;

namespace GoMan
{
    class ExampleProgram
    {
        static void Main(string[] args)
        {

            PrintEmails("short@googlemail.com");
            PrintEmails("longernotmuch@googlemail.com");
            Console.ReadLine();

        }

        private static int _lastSecondsRemaining = 0;

        static void OnEmailGenerated(object sender, GmailDotGeneratorEventArgs e)
        {
            //Console.Clear();
            TimeSpan ts = new TimeSpan(e.EstimatedCompletionTime);
            int secondsRemaining = (int) ts.TotalMilliseconds/1000;

            if (secondsRemaining != _lastSecondsRemaining)
            {
                _lastSecondsRemaining = secondsRemaining;
                Console.Clear();
                Console.WriteLine($"Estimated completion time is {_lastSecondsRemaining} seconds");
            }
        }

        static void PrintEmails(string email)
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
