using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnyCAD.Foundation;

namespace AnyCAD.Rapid.Core
{
    class ZoomAllCommand : UICommand
    {
        public ZoomAllCommand()
        {
            this.Name = "ZoomAll";
        }

        public override bool Execute(UICommandContext ctx)
        {
            ctx.RenderView.ZoomAll(1.2f);
            return true;
        }
    }

    class UndoCommand : UICommand
    {
        public UndoCommand()
        {
            this.Name = "Undo";
        }

        public override bool Execute(UICommandContext ctx)
        {
            ctx.Document.Undo();
            ctx.RequestUpdate();

            return true;
        }
    }
    class RedoCommand : UICommand
    {
        public RedoCommand()
        {
            this.Name = "Redo";
        }

        public override bool Execute(UICommandContext ctx)
        {
            ctx.Document.Redo();
            ctx.RequestUpdate();
            return true;
        }
    }

    class ColorBackgroundCommand : UICommand
    {
        public ColorBackgroundCommand()
        {
            this.Name = "BackgroundColor";
        }

        public override bool Execute(UICommandContext ctx)
        {
            var dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ctx.RenderView.SetBackgroundColor(dlg.Color.R / 255.0f, dlg.Color.G / 255.0f, dlg.Color.B / 255.0f, 1);
            }
            return true;
        }
    }

    class ImageBackgroundCommand : UICommand
    {
        public ImageBackgroundCommand()
        {
            this.Name = "BackgroundImage";
        }

        public override bool Execute(UICommandContext ctx)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files(*.jpg;*.png)|*.jpg;*.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var bkg = new ImageBackground(ImageTexture2D.Create(dlg.FileName));
                ctx.RenderView.GetViewer().SetBackground(bkg);
            }
            return true;
        }
    }
}
