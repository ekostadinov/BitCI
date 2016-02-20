using System;
using System.Collections.Generic;
using System.Linq;
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
            //todo: powershell schedule support
            // http://blogs.technet.com/b/heyscriptingguy/archive/2015/01/13/use-powershell-to-create-scheduled-tasks.aspx
            throw new NotImplementedException();
        }
    }
}