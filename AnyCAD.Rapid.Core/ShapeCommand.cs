using AnyCAD.Forms;
using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    class SphereElementSchema : ElementSchema
    {
        public SphereElementSchema()
            :base("Sphere")
        {
            AddParameter("Radius", ParameterCreator.Create(5.0f));
        }

        public override bool OnParameterChanged(Element instance, ParameterDict parameters)
        {
            var shape = CastShapeElement(instance);
            var radius = parameters.Cast("Radius", 5.0f);
            shape.SetShape(ShapeBuilder.MakeSphere(GP.Origin(), radius));
            return true;
        }

        static public ShapeElement Create(Document doc)
        {
            return ElementSchemaManager.Instance().CreateShapeInstance("Sphere", doc);
        }
    }

    class ShapeCommand : UICommand
    {
        public ShapeCommand()
        {
            this.Name = "Sphere";
        }

        public override bool Execute(UICommandContext ctx)
        {
            var transaction = new UndoTransaction(ctx.Document);
            transaction.Start(this.Name);

            var element = SphereElementSchema.Create(ctx.Document);
            ctx.ShowElement(element);

            transaction.Commit();

            ctx.RequestUpdate();
           
            return true;
        }
    }
}
