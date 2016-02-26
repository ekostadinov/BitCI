using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BitCI.Models.BuildSteps
{
    public class VersionControlStep : IBuildStep
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
        public StepStatus Status { get; set; }
        public string Value { get; set; }

        public void Execute()
        {
            string command = Value;
            //todo: remove username:password
            command = command.Replace("https://", "http://ekostadinov:Test1234@");
            string gitDirrectory = Build.Workspace.Replace("\\", "/") + "/source";
            command += " " + gitDirrectory;

            //update log
            object locker = new Object();
            lock (locker)
            {
                using (FileStream file = new FileStream(Build.Log, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.WriteLine();
                    writer.WriteLine("Step 2:");
                    writer.WriteLine("Downloading Git project...");
                }
            }

            //clone project
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/c git clone " + command + " >> " + Build.Log;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}