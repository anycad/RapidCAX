using AnyCAD.Forms;
using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    public class UICommandContext
    {
        public DocumentView mDocView;

        public UICommandContext(DocumentView docView)
        {
            mDocView = docView;
        }


        public Document Document
        {
            get { return mDocView.mDocument; }
        }

        public ElementId DefaultMaterialId
        {
            get { return mDocView.mMaterialId; }
        }

        public RenderControl RenderView
        {
            get { return mDocView.mRenderCtrl; }
        }

        public void ShowElement(DrawableElement element)
        {
            mDocView.mDbView.ShowElement(element);
        }

        public void RequestUpdate()
        {
            mDocView.mRootSceneNode.ComputeBoundingBox();
            mDocView.mRootSceneNode.RequstUpdate();
            mDocView.mRenderCtrl.GetContext().UpdateWorld();
            mDocView.mRenderCtrl.RequestDraw();
        }
    }
}
