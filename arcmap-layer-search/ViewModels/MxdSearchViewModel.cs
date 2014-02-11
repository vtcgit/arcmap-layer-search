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
using System.ComponentModel;

namespace arcmap_layer_search.ViewModels
{
    public class MxdSearchViewModel : Utilities.ViewModelBase
    {
        #region Private Variablesp
        private string _name;
        private RelayCommand _commandBrowseMxd;
        private RelayCommand _commandClearAll;
        private RelayCommand _commandRemoveItem;

        private List<MxdEntry> _mxdList = new List<MxdEntry>();
        #endregion

        #region Properties
        public ObservableCollection<MxdEntry> MxdList { get { return new ObservableCollection<MxdEntry>(_mxdList); } }
        public ObservableCollection<LayerEntry> KnownLayers
        {
            get
            {
                var list = new List<LayerEntry>();
                _mxdList.ForEach(a => list.AddRange(a.KnownLayers));
                return new ObservableCollection<LayerEntry>(list);
            }
        }
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
        public RelayCommand CommandClearAll
        {
            get
            {
                if (_commandClearAll == null)
                    _commandClearAll = new RelayCommand(CommandClearAllExecute, CommandClearAllCanExecute);
                return _commandClearAll;
            }
        }
        public RelayCommand CommandRemoveItem
        {
            get
            {
                if (_commandRemoveItem == null)
                    _commandRemoveItem = new RelayCommand(CommandRemoveItemExecute, CommandRemoveItemCanExecute);
                return _commandRemoveItem;
            }
        }
        #endregion
        #endregion

        #region Private Helpers
        private void UpdateLayersList()
        {
            OnPropertyChanged(() => KnownLayers);
            OnPropertyChanged(() => MxdList);
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
                var newFile = new MxdEntry(new FileInfo(fileDialog.FileName));
                _mxdList.Add(newFile);
                OnPropertyChanged(() => MxdList);
                var bg = new BackgroundWorker();
                bg.DoWork += delegate
                {
                    newFile.FetchLayers(UpdateLayersList);
                };
                bg.RunWorkerAsync();
            }
        }
        private bool CommandBrowseMxdCanExecute(object sender)
        {
            return true;
        }
        private void CommandClearAllExecute(object sender)
        {
            _mxdList.Clear();
            UpdateLayersList();
        }
        private bool CommandClearAllCanExecute(object sender)
        {
            return MxdList.Count > 0;
        }
        private void CommandRemoveItemExecute(object sender)
        {
            var item = sender as MxdEntry;
            if(item != null)
            {
                _mxdList.Remove(_mxdList.Find(a => a.Guid == item.Guid));
                UpdateLayersList();
            }
        }
        private bool CommandRemoveItemCanExecute(object sender)
        {
            return true;
        }

        #endregion
    }
}
