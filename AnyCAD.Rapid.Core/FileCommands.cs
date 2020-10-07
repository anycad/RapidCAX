using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyCAD.Rapid.Core
{
    class ImportStepCommand : UICommand
    {
        public ImportStepCommand()
        {
            this.Name = "ImportSTEP";
        }

        public static String ExtractName(String safeFileName)
        {
            return safeFileName.Substring(0, safeFileName.LastIndexOf('.'));
        }
        public override bool Execute(UICommandContext ctx)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "STEP Files(*.step;*.stp)|*.step;*.stp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var shape = StepIO.Open(dlg.FileName);
                if (shape != null)
                {
                    var transaction = new UndoTransaction(ctx.Document);
                    transaction.Start(this.Name);

                    var shapeElement = new ShapeElement();
                    shapeElement.SetName(ExtractName(dlg.SafeFileName));
                    ctx.Document.AddElement(shapeElement);
                    shapeElement.SetMaterialId(ctx.DefaultMaterialId);
                    shapeElement.SetShape(shape);
                    ctx.ShowElement(shapeElement);
                    transaction.Commit();

                    ctx.RequestUpdate();

                }

            }
            return true;
        }
    }

    class ImportIgesCommand : UICommand
    {
        public ImportIgesCommand()
        {
            this.Name = "ImportIGES";
        }

        public override bool Execute(UICommandContext ctx)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "IGES Files(*.iges;*.igs)|*.iges;*.igs";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var shape = IgesIO.Open(dlg.FileName);
                if (shape != null)
                {
                    var transaction = new UndoTransaction(ctx.Document);
                    transaction.Start(this.Name);

                    var shapeElement = new ShapeElement();
                    shapeElement.SetName(ImportStepCommand.ExtractName(dlg.SafeFileName));
                    ctx.Document.AddElement(shapeElement);
                    shapeElement.SetMaterialId(ctx.DefaultMaterialId);
                    shapeElement.SetShape(shape);
                    ctx.ShowElement(shapeElement);
                    transaction.Commit();

                    ctx.RequestUpdate();
                }

            }
            return true;
        }
    }


    class SaveFileCommand : UICommand
    {
        public SaveFileCommand()
        {
            this.Name = "Save";
        }

        public override bool Execute(UICommandContext ctx)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "AnyCAD Rapid Files(*.acad)|*.acad";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DocumentIO.Save(ctx.Document, dlg.FileName);
            }
            return true;
        }
    }

    class OpenFileCommand : UICommand
    {
        public OpenFileCommand()
        {
            this.Name = "Open";
        }

        public override bool Execute(UICommandContext ctx)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "AnyCAD Rapid Files(*.acad)|*.acad";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var doc = DocumentIO.Load(dlg.FileName);
                ctx.mDocView.ResetDocument(doc);
            }
            return true;
        }
    }
}
