using System;
using System.Collections.Generic;
using System.Linq;
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
            //todo: implement MSbuild support, may use .build file 
            // located in the project structure and just trigger MSbuild.exe
            throw new NotImplementedException();
        }
    }
}