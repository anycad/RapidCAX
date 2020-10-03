using AnyCAD.Forms;
using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    public class BrowerNodeItem
    {
        public string Icon { get; set; }
        public string DisplayName { get; set; }
        public Object Tag { get; set; }

        public ObservableCollection<BrowerNodeItem> Children { get; set; }

        public BrowerNodeItem()
        {
            Children = new ObservableCollection<BrowerNodeItem>();
        }
    }

    public class DocumentView
    {
        public RenderControl mRenderCtrl;
        public Document mDocument;
        public DbView mDbView;
        public ElementId mMaterialId;
        public DocumentSceneNode mRootSceneNode;

        public ObservableCollection<BrowerNodeItem> mProjectBrower = new ObservableCollection<BrowerNodeItem>();

        public UICommandContext mContext;

        public Dictionary<String, UICommand> mCommands = new Dictionary<string, UICommand>();

        public DocumentView(System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            mRenderCtrl = new RenderControl();
            host.Child = mRenderCtrl;
            mRenderCtrl.Load += MRenderCtrl_Load;

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(UICommand)))
                {
                    var command = Activator.CreateInstance(type) as UICommand;
                    if (command.Name.Length == 0)
                        continue;
                    mCommands[command.Name] = command;
                }
            }

        }
        public void ResetDocument(Document doc)
        {
            mDocument = doc;
            mRootSceneNode.SetDocument(mDocument);
            mDbView = DbView.Cast(mDocument.FindElement(mDocument.GetActiveDbViewId()));

            mContext.RequestUpdate();
            mRenderCtrl.ZoomAll(0.8f);
        }
        private void MRenderCtrl_Load(object sender, EventArgs e)
        {
            mDocument = new Document();
            mDbView = mDocument.Initialize("3D");

            mRootSceneNode = new DocumentSceneNode(mDocument);
            mRenderCtrl.ShowSceneNode(mRootSceneNode);

            mContext = new UICommandContext(this);

            mDocument.EnableTransaction(false);
            var material = new MaterialElement();
            material.SetName("Default");
            mMaterialId = mDocument.AddElement(material);

            mDocument.EnableTransaction(true);
        }      
       

        public void ExecuteCommand(string name)
        {
            UICommand command;
            if (mCommands.TryGetValue(name, out command))
            {
                command.Execute(mContext);
            }
        }
    }
}
