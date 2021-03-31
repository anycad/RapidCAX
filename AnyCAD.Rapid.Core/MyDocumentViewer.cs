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
    public class MyDocumentViewer
    {
        //数据
        public Document mDocument;
        public DbView mDbView;
        public ElementId mMaterialId;

        //显示
        public RenderControl mRenderCtrl;
        public DocumentSceneNode mRootSceneNode;

        //交互
        public UICommandContext mContext;

        public delegate void SelectElementHandler(PickedItem item, Document doc, ElementId id);
        SelectElementHandler mSelectionCallback;
        public MyDocumentViewer(System.Windows.Forms.Integration.WindowsFormsHost host, SelectElementHandler selector)
        {
            //创建三维控件
            mRenderCtrl = new RenderControl();
            host.Child = mRenderCtrl;
            mRenderCtrl.Load += InitializeRenderControlOnLoad;

            //用于选择的回调方法
            mSelectionCallback = selector;

            //注册命令
            UICommandManager.LoadCommands(Assembly.GetExecutingAssembly());
            //注册图元模板
            GlobalInstance.RegisterElementSchema(Assembly.GetExecutingAssembly());
        }

        private void InitializeRenderControlOnLoad(object sender, EventArgs e)
        {
            // 初始化文档
            mDocument = new Document();
            mDbView = mDocument.Initialize("3D");

            //创建一个默认的材质，先禁用事务
            mDocument.EnableTransaction(false);
            var material = new MaterialElement();
            material.SetName("Default");
            mMaterialId = mDocument.AddElement(material);
            mDocument.EnableTransaction(true);

            //初始化显示节点
            mRootSceneNode = new DocumentSceneNode(mDocument);
            mRenderCtrl.ShowSceneNode(mRootSceneNode);

            //初始化交互
            mContext = new UICommandContext(this);

            mRenderCtrl.SetSelectCallback((PickedItem item) =>
            {
                var node = item.GetNode();
                var elementId = node == null ? ElementId.InvalidId : new ElementId(node.GetUserId());
                mSelectionCallback(item, mDocument, elementId);
            });
        }

        /// <summary>
        /// 换个文档显示
        /// </summary>
        /// <param name="doc"></param>
        public void ResetDocument(Document doc)
        {
            mDocument = doc;
            mRootSceneNode.SetDocument(mDocument);
            mDbView = DbView.Cast(mDocument.FindElement(mDocument.GetActiveDbViewId()));

            var table = mDocument.FindTable("UserElement");
            var userIds = table.GetIds();
            var scene = mContext.RenderView.GetScene();
            foreach (var id in userIds)
            {
                var element = mDocument.FindElement(id);
                if(MyTextElement.IsKindOf(element))
                {
                    var text = new MyTextElement();
                    text.Load(UserElement.Cast(element));
                    text.Show(scene);
                }
            }

            mContext.RequestUpdate();
            mRenderCtrl.ZoomAll(0.8f);
        }
        public void UpdateView()
        {
            mRootSceneNode.RequstUpdate();
            mRenderCtrl.RequestDraw(EnumUpdateFlags.Scene);
        }
        public void ExecuteCommand(string name)
        {
            UICommandManager.ExecuteCommand(name, mContext);
        }
    }
}
