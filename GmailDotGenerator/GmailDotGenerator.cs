using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoMan
{
    public class GmailDotGenerator : IDisposable
    {
        public GmailDotGeneratorConfiguration Configuration { get; }
        public HashSet<GmailDotGeneratorEmailModel> GeneratedEmails { get; set; }

        public delegate void EstimatedCompletionTime(object sender, GmailDotGeneratorEventArgs e);

        public event EstimatedCompletionTime OnEstimatedCompletionTime;

        public GmailDotGenerator(string email, int maximumEmails = 0)
        {
            this.Configuration = new GmailDotGeneratorConfiguration(email, maximumEmails);
        }

        [JsonConstructor]
        public GmailDotGenerator(GmailDotGeneratorConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IEnumerator<GmailDotGeneratorEmailModel> GenerateEmails()
        {
            ClearEmails();
            this.GeneratedEmails = Generate();
            return GetEnumerable();
        }

        private HashSet<GmailDotGeneratorEmailModel> Generate()
        {
            var generatedEmails = new HashSet<GmailDotGeneratorEmailModel>();
            //StringBuilder sb = new StringBuilder();
            string username = "";
            Stopwatch sw = null;

            if (OnEstimatedCompletionTime != null)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            var totalCombos = 0;

            totalCombos = Configuration.MaximumEmails == 0 || Configuration.MaximumEmails > Configuration.TotalCombinations*2
                ? Configuration.TotalCombinations
                : Configuration.MaximumEmails/2;

            var isMaximumEmailsOdd = (Configuration.MaximumEmails%2 == 1);
            totalCombos += isMaximumEmailsOdd ? 1 : 0;

            for (int i = 0; i < totalCombos; i++)
            {
                string binaryString = Convert.ToString(i, 2).PadLeft(this.Configuration.UsernameLengthMinusOne, '0');
                for (int j = 0; j < (this.Configuration.UsernameLengthMinusOne); j++)
                {

                    username += this.Configuration.Username[j];
                   // sb.Append(this.Configuration.Username[j]);
                    if (binaryString[j] == '1')
                    {
                        username += ".";
                       // sb.Append(".");
                    }
                    
                }
                username += this.Configuration.Username[this.Configuration.UsernameLengthMinusOne];
                //sb.Append(this.Configuration.Username[this.Configuration.UsernameLengthMinusOne]);
                var generatorEmailModel1 = new GmailDotGeneratorEmailModel(username + "@gmail.com");
                var generatorEmailModel2 = new GmailDotGeneratorEmailModel(username + "@googlemail.com");

                //var generatorEmailModel1 = new GmailDotGeneratorEmailModel(sb.ToString() + "@gmail.com");
                //var generatorEmailModel2 = new GmailDotGeneratorEmailModel(sb.ToString() + "@googlemail.com");

                generatedEmails.Add(generatorEmailModel1);
                generatedEmails.Add(generatorEmailModel2);

                if (OnEstimatedCompletionTime != null)
                {
                    sw.Stop();
                    var remaining = Configuration.TotalCombinations*2 - generatedEmails.Count;
                    var estimatedCompletionTime = sw.ElapsedTicks*remaining;

                    var gmailDotGeneratorEventArgs = new GmailDotGeneratorEventArgs(generatedEmails.Count,
                        remaining, estimatedCompletionTime, generatorEmailModel1, generatorEmailModel2);

                    EstimatedCompletionTimeChanged(gmailDotGeneratorEventArgs);
                }
                username = "";
                //sb.Clear();
            }

            return generatedEmails;
        }

        public GmailDotGeneratorEmailModel GetSingleEmail()
        {
            if (this.GeneratedEmails == null)
                throw new GmailDotGeneratorExceptionEmailListIsEmptyOrNull(this.Configuration.ToString());

            try
            {
                return GeneratedEmails.First(x => x.Used == false);
            }
            catch
            {
                throw new GmailDotGeneratorExceptionNoAvaliableEmails(this.Configuration.ToString());
            }
        }

        public string GetSingleEmailAndMarkUsed()
        {
            return GetSingleEmail().UseEmail();
        }

        public IEnumerator<GmailDotGeneratorEmailModel> GetEnumerable()
        {
            if (this.GeneratedEmails == null || this.GeneratedEmails.Count == 0)
                throw new GmailDotGeneratorExceptionEmailListIsEmptyOrNull(this.Configuration.ToString());

            return this.GeneratedEmails.AsEnumerable().GetEnumerator();
        }

        public IEnumerator<GmailDotGeneratorEmailModel> SetMaximumEmails(int value)
        {
            this.Configuration.MaximumEmails = Math.Abs(value);
            this.GeneratedEmails = Generate();

            return GetEnumerable();
        }

        public void ClearEmails()
        {
            if (this.GeneratedEmails != null && this.GeneratedEmails.Count > 0)
                this.GeneratedEmails.Clear();
        }

        protected void EstimatedCompletionTimeChanged(GmailDotGeneratorEventArgs gmailDotGeneratorEventArgs)
        {
            this.OnEstimatedCompletionTime?.Invoke(this, gmailDotGeneratorEventArgs);
        }


        public bool Save()
        {
            try
            {
                if (!Directory.Exists("GmailDotGenerator")) Directory.CreateDirectory("GmailDotGenerator");

                string settings = JsonConvert.SerializeObject(this, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter($"./GmailDotGenerator/{this.Configuration.Email}.json", false)
                    )
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
            if (File.Exists($"./GmailDotGenerator/{email}.json"))
            {
                using (StreamReader sr = new StreamReader($"./GmailDotGenerator/{email}.json"))
                {
                    var gmailDotGenerator = JsonConvert.DeserializeObject<GmailDotGenerator>(sr.ReadToEnd());
                    if (gmailDotGenerator.Configuration.MaximumEmails != maximumEmails)
                        gmailDotGenerator.SetMaximumEmails(maximumEmails);

                    return gmailDotGenerator;
                }
            }
            else
                return new GmailDotGenerator(email, maximumEmails);
        }

        public void Dispose()
        {
            Configuration.Dispose();
            GeneratedEmails.Clear();
            OnEstimatedCompletionTime = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}