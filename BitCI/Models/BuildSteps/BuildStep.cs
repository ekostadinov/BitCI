using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace BitCI.Models.BuildSteps
{
    public class BuildStep : IBuildStep
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
        public StepStatus Status { get; set; }
        public string Value { get; set; }

        public void Execute()
        {
            //update log
            object locker = new Object();
            lock (locker)
            {
                using (FileStream file = new FileStream(Build.Log, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.WriteLine();
                    writer.WriteLine("Step 2:");
                    writer.WriteLine("Build .Net project...");
                }
            }
            
            string value = Value;
            string gitDirrectory = Build.Workspace.Replace("\\", "/") + "/source";
            //build project
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/c ""C:/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe"" " + gitDirrectory 
                + "/" + value.Replace("\\", "/") + " /t:build"
                + " >> " + Build.Log;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}