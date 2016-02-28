using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using BitCI.Models.BuildSteps;

namespace BitCI.Models
{
    public class Build
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public BuildStatus Status { get; set; }
        public string Workspace { get; set; }
        public string Duration { get; set; }
        public string TriggeredBy { get; set; }
        public string Log { get; set; }
        public DateTime StartDate { get; set; }
        
        public ICollection<IBuildStep> Steps { get; set; }
        public ICollection<Object> Artifacts { get; set; }
 
        public enum BuildStatus
        {
            Pending,
            Running,
            Passed,
            Failed
        }
    }
}