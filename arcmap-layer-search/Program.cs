using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace arcmap_layer_search
{
    static class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new arcmap_layer_search.LicenseInitializer();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeBasic },
            new esriLicenseExtensionCode[] { esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst, esriLicenseExtensionCode.esriLicenseExtensionCodeDataInteroperability });
            //IMapDocument pMapDoc = new MapDocumentClass();
            //pMapDoc.Open(@"E:\Documents\GradSchool\CGIT\DMV\NewFetches_2013_v2\Output\Henrico_20130101_20131122\Henrico_20130101_20131122\Henrico.mxd");
            //IDocumentInfo2 pDocInfo = (IDocumentInfo2)pMapDoc;
            //var map = pMapDoc.get_Map(0);
            //for (int i = 0; i < map.LayerCount; i++)
            //{
            //    ILayer layer = (ILayer)map.get_Layer(i);
            //    IDataLayer2 dataLayer = (IDataLayer2)layer;
            //    IDatasetName name = (IDatasetName)dataLayer.DataSourceName;
            //    IWorkspaceName workspace = name.WorkspaceName;
            //    IPropertySet propSet = workspace.ConnectionProperties;
            //    object obj1 = new object[5];
            //    object obj2 = new object[5];
            //    propSet.GetAllProperties(out obj1, out obj2);

            //    object[] array1 = (object[])obj1;
            //    object[] array2 = (object[])obj2;


            //    for (int j = 0; j < array1.Length; j++)
            //    {
            //        //System.Windows.Forms.MessageBox.Show(array1[j] + " = " + array2[j]);
            //    }
            //}
            //pMapDoc.Close();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
            m_AOLicenseInitializer.ShutdownApplication();
        }
    }
}