using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitCI.Models
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Project> Projects { get; set; } 
    }
}