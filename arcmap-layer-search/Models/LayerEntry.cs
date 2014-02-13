using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace arcmap_layer_search.Models
{
    public class LayerEntry
    {
        public LayerEntry(string layerName, string location, string mxd)
        {
            this.LayerName = layerName;
            this.Location = location;
            this.MxdOrigin = mxd;
            this.LayerExists = Directory.Exists(location) ? "Yes" : "No";
        }

        public string LayerName { get; private set; }
        public string Location { get; private set; }
        public string MxdOrigin { get; private set; }
        public string LayerExists { get; private set; }
    }
}
