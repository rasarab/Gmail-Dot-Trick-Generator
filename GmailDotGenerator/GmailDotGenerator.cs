using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GoMan
{
    public sealed class GmailDotGenerator : IDisposable
    {
        public delegate void EstimatedCompletionTime(object sender, GmailDotGeneratorEventArgs e);

        public GmailDotGenerator(string email, int maximumEmails = 0)
        {
            Configuration = new GmailDotGeneratorConfiguration(email, maximumEmails);
        }

        [JsonConstructor]
        public GmailDotGenerator(GmailDotGeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public GmailDotGeneratorConfiguration Configuration { get; }
        public HashSet<GmailDotGeneratorEmailModel> GeneratedEmails { get; set; }

        public void Dispose()
        {
            GeneratedEmails.Clear();
            OnEstimatedCompletionTime = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public event EstimatedCompletionTime OnEstimatedCompletionTime;

        public IEnumerator<GmailDotGeneratorEmailModel> GenerateEmails()
        {
            ClearEmails();
            GeneratedEmails = Generate();
            return GetEnumerable();
        }

        private HashSet<GmailDotGeneratorEmailModel> Generate()
        {
            var generatedEmails = new HashSet<GmailDotGeneratorEmailModel>();
            //StringBuilder sb = new StringBuilder();
            var username = "";
            Stopwatch sw = null;

            if (OnEstimatedCompletionTime != null)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            var totalCombos = Configuration.MaximumEmails == 0 ||
                              Configuration.MaximumEmails > Configuration.TotalCombinations*2
                ? Configuration.TotalCombinations
                : Configuration.MaximumEmails/2;

            var isMaximumEmailsOdd = Configuration.MaximumEmails%2 == 1;
            totalCombos += isMaximumEmailsOdd ? 1 : 0;

            for (var i = 0; i < totalCombos; i++)
            {
                var binaryString = Convert.ToString(i, 2).PadLeft(Configuration.UsernameLengthMinusOne, '0');
                for (var j = 0; j < Configuration.UsernameLengthMinusOne; j++)
                {
                    username += Configuration.Username[j];
                    // sb.Append(this.Configuration.Username[j]);
                    if (binaryString[j] == '1')
                    {
                        username += ".";
                        // sb.Append(".");
                    }
                }
                username += Configuration.Username[Configuration.UsernameLengthMinusOne];
                //sb.Append(this.Configuration.Username[this.Configuration.UsernameLengthMinusOne]);
                var generatorEmailModel1 = new GmailDotGeneratorEmailModel(username + "@gmail.com");
                var generatorEmailModel2 = new GmailDotGeneratorEmailModel(username + "@googlemail.com");

                //var generatorEmailModel1 = new GmailDotGeneratorEmailModel(sb.ToString() + "@gmail.com");
                //var generatorEmailModel2 = new GmailDotGeneratorEmailModel(sb.ToString() + "@googlemail.com");

                generatedEmails.Add(generatorEmailModel1);
                generatedEmails.Add(generatorEmailModel2);

                if (OnEstimatedCompletionTime != null)
                {
                    if (sw != null)
                    {
                        sw.Stop();
                        var remaining = Configuration.TotalCombinations*2 - generatedEmails.Count;
                        var estimatedCompletionTime = sw.ElapsedTicks*remaining;

                        var gmailDotGeneratorEventArgs = new GmailDotGeneratorEventArgs(generatedEmails.Count,
                            remaining, estimatedCompletionTime, generatorEmailModel1, generatorEmailModel2);

                        EstimatedCompletionTimeChanged(gmailDotGeneratorEventArgs);
                    }
                }
                username = "";
                //sb.Clear();
            }

            return generatedEmails;
        }

        public GmailDotGeneratorEmailModel GetSingleEmail()
        {
            if (GeneratedEmails == null)
                throw new GmailDotGeneratorExceptionEmailListIsEmptyOrNull(Configuration.ToString());

            try
            {
                return GeneratedEmails.First(x => x.Used == false);
            }
            catch
            {
                throw new GmailDotGeneratorExceptionNoAvaliableEmails(Configuration.ToString());
            }
        }

        public string GetSingleEmailAndMarkUsed()
        {
            return GetSingleEmail().UseEmail();
        }

        public IEnumerator<GmailDotGeneratorEmailModel> GetEnumerable()
        {
            if (GeneratedEmails == null)
                throw new GmailDotGeneratorExceptionEmailListIsEmptyOrNull(Configuration.ToString());

            return GeneratedEmails.AsEnumerable().GetEnumerator();
        }

        public IEnumerator<GmailDotGeneratorEmailModel> SetMaximumEmails(int value)
        {
            Configuration.MaximumEmails = Math.Abs(value);
            GeneratedEmails = Generate();

            return GetEnumerable();
        }

        public void ClearEmails()
        {
            if (GeneratedEmails != null && GeneratedEmails.Count > 0)
                GeneratedEmails.Clear();
        }

        private void EstimatedCompletionTimeChanged(GmailDotGeneratorEventArgs gmailDotGeneratorEventArgs)
        {
            OnEstimatedCompletionTime?.Invoke(this, gmailDotGeneratorEventArgs);
        }


        public bool Save()
        {
            try
            {
                if (!Directory.Exists("GmailDotGenerator")) Directory.CreateDirectory("GmailDotGenerator");

                var settings = JsonConvert.SerializeObject(this, Formatting.Indented);
                using (var sw = new StreamWriter($"./GmailDotGenerator/{Configuration.Email}.json", false))
                    sw.WriteLine(settings);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static GmailDotGenerator Load(string email, int maximumEmails = 0)
        {
            if (!EmailDataJsonExists(email)) return new GmailDotGenerator(email, maximumEmails);

            var emailDataFromJson = GetEmailDataFromJson(email);
            var gmailDotGenerator = DeserializeEmailJson(emailDataFromJson);

            if (gmailDotGenerator.Configuration.MaximumEmails != maximumEmails)
                gmailDotGenerator.SetMaximumEmails(maximumEmails);

            return gmailDotGenerator;
        }

        private static GmailDotGenerator DeserializeEmailJson(string data)
        {
            return JsonConvert.DeserializeObject<GmailDotGenerator>(data);
        }
        private static string GetEmailDataFromJson(string email)
        {
            using (var sr = new StreamReader($"./GmailDotGenerator/{email}.json"))
                return sr.ReadToEnd();
        }

        private static bool EmailDataJsonExists(string email)
        {
            return File.Exists($"./GmailDotGenerator/{email}.json");
        }
    }
}