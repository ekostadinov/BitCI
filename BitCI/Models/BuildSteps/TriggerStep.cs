using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace BitCI.Models.BuildSteps
{
    public class TriggerStep : IBuildStep
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
        public StepStatus Status { get; set; }
        public string Value { get; set; }

        public void Execute()
        {
            DateTime resultTime;
            DateTime.TryParse(Value, out resultTime);

            var fromHours = TimeSpan.FromHours(resultTime.Hour).TotalMilliseconds;
            var fromMinutes = TimeSpan.FromMinutes(resultTime.Minute).TotalMilliseconds;
            var fromSeconds = TimeSpan.FromSeconds(resultTime.Second).TotalMilliseconds;
            var milisecondsDelay = fromHours + fromMinutes + fromSeconds;

            //update log
            object locker = new Object();
            lock (locker)
            {
                using (FileStream file = new FileStream(Build.Log, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.WriteLine();
                    writer.WriteLine("Step 1:");
                    writer.WriteLine("Build trigger has delay of - " + milisecondsDelay + "miliseconds");
                }
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(milisecondsDelay));
        }
    }
}