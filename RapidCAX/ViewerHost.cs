using AnyCAD.Forms;
using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RapidCAX
{
    public class ViewerHost
    {
        public RenderControl mRenderCtrl;
        public Document mDocument;
        public DbView mDbView;
        public ViewerHost(System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            mRenderCtrl = new RenderControl();
            host.Child = mRenderCtrl;

            mDocument = new Document();
            mDbView = mDocument.Initialize("3D");

            var node = new DocumentSceneNode(mDocument);
            mRenderCtrl.ShowSceneNode(node);
        }

        public void ExecuteCommand(string name)
        {
            MessageBox.Show(name);
        }
    }
}
