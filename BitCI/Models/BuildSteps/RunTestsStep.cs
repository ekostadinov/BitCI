using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BitCI.Context;

namespace BitCI.Models.BuildSteps
{
    public class RunTestsStep : IBuildStep
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
                    writer.WriteLine("Step 4:");
                    writer.WriteLine("Run NUnit tests...");
                }
            }

            var db = new ServerContext();
            Build build = db.Builds.Find(BuildId);
            var project = db.Projects.FirstOrDefault(p => p.Id.Equals(build.ProjectId));

            string value = Value;
            string gitDirrectory = Build.Workspace.Replace("\\", "/") + "/source";
            //run NUnit tests
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/c "+ gitDirrectory + @"/packages/NUnit.Runners.2.6.3/tools/nunit-console.exe " + gitDirrectory
                + value 
                + " /include:" + project.Name.Replace(" team", String.Empty).Trim()
                + " >> " + Build.Log;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}