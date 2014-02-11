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
using FolderSelect;

namespace arcmap_layer_search.ViewModels
{
    public class MxdSearchViewModel : Utilities.ViewModelBase
    {
        #region Private Variablesp
        private RelayCommand _commandBrowseMxd;
        private RelayCommand _commandBrowseFolder;
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
        public RelayCommand CommandBrowseFolder
        {
            get
            {
                if (_commandBrowseFolder == null)
                    _commandBrowseFolder = new RelayCommand(CommandBrowseFolderExecute, CommandBrowseFolderCanExecute);
                return _commandBrowseFolder;
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

        private void AddMxd(string filename)
        {
            var newFile = new MxdEntry(new FileInfo(filename));
            _mxdList.Add(newFile);
            OnPropertyChanged(() => MxdList);
            var bg = new BackgroundWorker();
            bg.DoWork += delegate
            {
                newFile.FetchLayers(UpdateLayersList);
            };
            bg.RunWorkerAsync();
        }

        #endregion

        #region Command Helpers
        private void CommandBrowseMxdExecute(object sender)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.DefaultExt = ".mxd";
            fileDialog.Filter = "ArcMap Documents (.mxd)|*.mxd";
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (var file in fileDialog.FileNames)
                {
                    AddMxd(file);
                }
            }
        }
        private bool CommandBrowseMxdCanExecute(object sender)
        {
            return true;
        }
        private void CommandBrowseFolderExecute(object sender)
        {
            var folderDialog = new FolderSelectDialog();
            var result = folderDialog.ShowDialog();
            if (result)
            {
                var fileName = folderDialog.FileName;
                var files = Directory.GetFiles(fileName, "*.mxd", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    AddMxd(file);
                }
            }
        }
        private bool CommandBrowseFolderCanExecute(object sender)
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
