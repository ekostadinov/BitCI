using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitCI.Models.BuildSteps
{
    public class PostBuildEmailStep : IBuildStep
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
        public StepStatus Status { get; set; }
        public string Value { get; set; }

        public void Execute()
        {
            //todo: implement smtp support 
            // https://www.siteground.com/kb/google_free_smtp_server/
            throw new NotImplementedException();
        }
    }
}