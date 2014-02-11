using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.esriSystem;

namespace arcmap_layer_search.Models
{
    public class MxdEntry
    {
        private Dictionary<string, string> _layerUids = new Dictionary<string, string>()
        {
            {"IDataLayer","{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}"},
            {"IFeatureLayer","{40A9E885-5533-11d0-98BE-00805F7CED21}"},
            {"IGeoFeatureLayer","{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}"},
            {"IGraphicsLayer","{34B2EF81-F4AC-11D1-A245-080009B6F22B}"},
            {"IFDOGraphicsLayer","{5CEAE408-4C0A-437F-9DB3-054D83919850}"},
            {"ICoverageAnnotationLayer","{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}"},
            {"IGroupLayer","{EDAD6644-1810-11D1-86AE-0000F8751720}"},
        };

        public delegate void FetchLayersDelegate();

        private List<LayerEntry> _knownLayers = new List<LayerEntry>();

        public MxdEntry(FileInfo fileInfo)
        {
            this.MxdInfo = fileInfo;
            this.Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; private set; }
        public FileInfo MxdInfo { get; private set; }
        public string Status { get; private set; }
        public List<LayerEntry> KnownLayers { get { return _knownLayers; } }

        public void FetchLayers(FetchLayersDelegate updateKnownLayers)
        {
            Status = "Fetching...";
            IMapDocument pMapDoc = new MapDocumentClass();
            try
            {
                pMapDoc.Open(MxdInfo.FullName);
                IDocumentInfo2 pDocInfo = (IDocumentInfo2)pMapDoc;
                var map = pMapDoc.get_Map(0);
                var count = map.LayerCount;
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = (ILayer)map.get_Layer(i);
                    try
                    {
                        IDataLayer2 dataLayer = (IDataLayer2)layer;
                        IDatasetName name = (IDatasetName)dataLayer.DataSourceName;
                        IWorkspaceName workspace = name.WorkspaceName;
                        IPropertySet propSet = workspace.ConnectionProperties;
                        object obj1 = new object[1];
                        object obj2 = new object[1];
                        propSet.GetAllProperties(out obj1, out obj2);
                         
                        object[] array1 = (object[])obj1;
                        object[] array2 = (object[])obj2;

                        _knownLayers.Add(new LayerEntry(layer.Name, array2[0].ToString(), MxdInfo.Name));
                        Status = string.Format("Found {0}/{1}", (i + 1), count);
                        updateKnownLayers();
                    }
                    catch (Exception e)
                    {
                        _knownLayers.Add(new LayerEntry(layer.Name, "Unknown - not a feature layer", MxdInfo.Name));
                        Status = string.Format("Found {0}/{1}", (i + 1), count);
                        updateKnownLayers();
                    }
                }
            }
            catch (Exception e)
            {
                Status = "An error occurred"; 
            }
            finally
            {
                pMapDoc.Close();
                updateKnownLayers();
            }
        }
    }
}
 