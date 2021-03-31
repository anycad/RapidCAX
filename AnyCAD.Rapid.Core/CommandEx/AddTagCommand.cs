using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    class AddTagCommand : UICommand
    {
        public AddTagCommand()
        {
            this.Name = "AddTag";
        }

        public override bool Execute(UICommandContext ctx)
        {
            //创建事务
            var transaction = new UndoTransaction(ctx.Document);
            transaction.Start(this.Name);

            var text = new MyTextElement();
            text.Text = "这是一个好引擎！";
            text.Position = new Vector3(100, 100, 100);

            text.AddElement(ctx.Document);

            text.Show(ctx.RenderView.GetScene());

            
            //提交事务
            transaction.Commit();

            ctx.RequestUpdate();

            return true;
        }
    }
}
