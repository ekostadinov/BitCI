using System;
using System.Diagnostics;

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
            command = command.Replace("http://", "http://ekostadinov:Test1234");
            command += " " + Build.Workspace.Replace("\\", "/");

            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c " + command;
            p.StartInfo = startInfo;
            p.Start();
        }
    }
}