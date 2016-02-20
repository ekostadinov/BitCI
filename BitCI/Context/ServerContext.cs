using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using BitCI.Models;
using BitCI.Models.BuildSteps;

namespace BitCI.Context
{
    public class ServerContext : DbContext
    {
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<BuildStep> BuildSteps { get; set; }
        public DbSet<PostBuildEmailStep> EmailSteps { get; set; }
        public DbSet<RunTestsStep> RunTestsSteps { get; set; }
        public DbSet<TriggerStep> TriggerSteps { get; set; }
        public DbSet<VersionControlStep> VCSteps { get; set; } 
    }
}