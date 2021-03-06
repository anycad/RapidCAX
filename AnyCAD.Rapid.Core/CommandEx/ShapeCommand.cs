﻿using AnyCAD.Forms;
using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{


    class ShapeCommand : UICommand
    {
        public ShapeCommand()
        {
            this.Name = "Sphere";
        }

        public override bool Execute(UICommandContext ctx)
        {
            //创建事务
            var transaction = new UndoTransaction(ctx.Document);
            transaction.Start(this.Name);

            //创建个球
            var element = SphereElementSchema.Create(ctx.Document);
            // 显示
            ctx.ShowElement(element);

            //提交事务
            transaction.Commit();

            ctx.RequestUpdate();
           
            return true;
        }
    }
}
