using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitCI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public Dashboard Dashboard { get; set; }
        public string Name { get; set; }
        public IList<Build> BuildsHistory { get; set; }
        public Queue<Build> CurrentBuilds { get; set; } 
 
    }
}