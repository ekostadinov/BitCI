using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        }
    }
}