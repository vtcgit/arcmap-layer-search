using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using arcmap_layer_search.Utilities;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using arcmap_layer_search.Models;

namespace arcmap_layer_search.ViewModels
{
    public class MxdSearchViewModel : Utilities.ViewModelBase
    {
        #region Private Variablesp
        private string _name;
        private RelayCommand _commandBrowseMxd;

        private List<FileInfo> _mxdList = new List<FileInfo>();
        private List<LayerEntry> _knownLayers = new List<LayerEntry>();
        #endregion

        #region Properties
        public ObservableCollection<FileInfo> MxdList { get { return new ObservableCollection<FileInfo>(_mxdList); } }
        public ObservableCollection<LayerEntry> KnownLayers { get { return new ObservableCollection<LayerEntry>(_knownLayers); } }
        public string LayerName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(() => LayerName);
            }
        }
        #region Commands
        public RelayCommand CommandBrowseMxd
        {
            get
            {
                if (_commandBrowseMxd == null)
                    _commandBrowseMxd = new RelayCommand(CommandBrowseMxdExecute, CommandBrowseMxdCanExecute);
                return _commandBrowseMxd;
            }
        }
        #endregion
        #endregion

        #region Private Helpers
        private void AddLayersFromMxd(FileInfo fileInfo)
        {
            IMapDocument pMapDoc = new MapDocumentClass();
            pMapDoc.Open(fileInfo.FullName);
            IDocumentInfo2 pDocInfo = (IDocumentInfo2)pMapDoc;
            var map = pMapDoc.get_Map(0);
            for (int i = 0; i < map.LayerCount; i++)
            {
                try
                {
                    ILayer layer = (ILayer)map.get_Layer(i);
                    IDataLayer2 dataLayer = (IDataLayer2)layer;
                    IDatasetName name = (IDatasetName)dataLayer.DataSourceName;
                    IWorkspaceName workspace = name.WorkspaceName;
                    IPropertySet propSet = workspace.ConnectionProperties;
                    object obj1 = new object[1];
                    object obj2 = new object[1];
                    propSet.GetAllProperties(out obj1, out obj2);

                    object[] array1 = (object[])obj1;
                    object[] array2 = (object[])obj2;

                    _knownLayers.Add(new LayerEntry(layer.Name, array2[0].ToString(), fileInfo.Name));
                }
                catch (Exception e)
                {

                }
            }
            pMapDoc.Close();
            OnPropertyChanged(() => KnownLayers);
        }

        #endregion

        #region Command Helpers
        private void CommandBrowseMxdExecute(object sender)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".mxd";
            fileDialog.Filter = "ArcMap Documents (.mxd)|*.mxd";
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                LayerName = fileDialog.SafeFileName;
                var newFile = new FileInfo(fileDialog.FileName);
                _mxdList.Add(newFile);
                OnPropertyChanged(() => MxdList);
                AddLayersFromMxd(newFile);
            }
        }
        private bool CommandBrowseMxdCanExecute(object sender)
        {
            return true;
        }

        #endregion
    }
}
