using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitCI.Models;

namespace BitCI.ViewModels
{
    /// <summary>
    /// ViewModel representing related Builds to particular Project's Dashoard
    /// </summary>
    public class DashboardBuilds
    {
        //fields
        public Dashboard _dashboard =  new Dashboard();
        public IList<Build> _buildsHistory = new List<Build>();
        public IList<Build> _buildQueue = new List<Build>();

        //properties
        public Dashboard Dashboard
        {
            get { return _dashboard; }
            set { _dashboard = value; }
        }

        public IList<Build> BuildsHistory
        { 
            get { return _buildsHistory; }
            set { _buildsHistory = value; } 
        }

        public IList<Build> BuildQueue
        {
            get { return _buildQueue; }
            set { _buildQueue = value; }
        } 
 
    }
}