using AnyCAD.Exchange;
using AnyCAD.Forms;
using AnyCAD.Foundation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace RapidCAX
{
    public class DocumentView
    {
        public RenderControl mRenderCtrl;
        public Document mDocument;
        public DbView mDbView;
        public MeshStandardMaterial mDefaultFaceMaterial;
        public BasicMaterial mDefaultEdgeMaterial;
        public DocumentView(System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            mRenderCtrl = new RenderControl();
            host.Child = mRenderCtrl;
            mRenderCtrl.Load += MRenderCtrl_Load;
        }

        private void MRenderCtrl_Load(object sender, EventArgs e)
        {
            mDocument = new Document();
            mDbView = mDocument.Initialize("3D");

            var node = new DocumentSceneNode(mDocument);
            mRenderCtrl.ShowSceneNode(node);

            mDefaultFaceMaterial = MeshStandardMaterial.Create("pbr-default");
            mDefaultFaceMaterial.SetColor(new Vector3(1.0f, 0.9f, 0.8f));
            mDefaultFaceMaterial.SetFaceSide(EnumFaceSide.DoubleSide);
            mDefaultFaceMaterial.SetMetalness(0.1f);
            mDefaultFaceMaterial.SetRoughness(0.6f);

            mDefaultEdgeMaterial = BasicMaterial.Create("edge");
            mDefaultEdgeMaterial.SetColor(new Vector3(0));
            mDefaultEdgeMaterial.SetLineWidth(2);
        }

        public void ExecuteCommand(string name)
        {
            if(name == "ImportSTEP")
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "STEP Files(*.step;*.stp)|*.step;*.stp";
                if(dlg.ShowDialog() == true)
                {
                   var shape =  StepIO.Open(dlg.FileName);
                    if(shape != null)
                    {
                       var node = BrepSceneNode.Create(shape, mDefaultFaceMaterial, mDefaultEdgeMaterial);
                       mRenderCtrl.ShowSceneNode(node);
                        mRenderCtrl.ZoomAll(0.9f);
                    }
                        
                }
            }
            else if(name == "ImportIGES")
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "IGES Files(*.iges;*.igs)|*.iges;*.igs";
                if (dlg.ShowDialog() == true)
                {
                    var shape = IgesIO.Open(dlg.FileName);
                    if (shape != null)
                    {
                        var node = BrepSceneNode.Create(shape, mDefaultFaceMaterial, mDefaultEdgeMaterial);
                        mRenderCtrl.ShowSceneNode(node);
                        mRenderCtrl.ZoomAll(0.9f);
                    }
                }
            }
            else if(name == "ZoomAll")
            {
                mRenderCtrl.ZoomAll(0.9f);
            }
            else if(name == "BackgroundColor")
            {
                var dlg = new System.Windows.Forms.ColorDialog();
                if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mRenderCtrl.SetBackgroundColor(dlg.Color.R / 255.0f, dlg.Color.G / 255.0f, dlg.Color.B / 255.0f, 1);
                }
            }
            else if(name == "BackgroundImage")
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image Files(*.jpg;*.png)|*.jpg;*.png";
                if (dlg.ShowDialog() == true)
                {
                    var bkg = new ImageBackground(ImageTexture2D.Create(dlg.FileName));
                    mRenderCtrl.GetViewer().SetBackground(bkg);
                }
            }
            else
            {
                MessageBox.Show(name);
            }
        }
    }
}
