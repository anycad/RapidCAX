using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnyCAD.Foundation;

namespace AnyCAD.Rapid.Core
{
    /// <summary>
    /// 球类型的模板
    /// </summary>
    class SphereElementSchema : ElementSchema
    {
        public SphereElementSchema()
            : base("Sphere")
        {
            //增加参数
            AddParameter("Radius", ParameterCreator.Create(5.0f));
        }

        /// <summary>
        /// 参数化驱动构造模型
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override bool OnParameterChanged(Element instance, ParameterDict parameters)
        {
            var shape = CastShapeElement(instance);
            var radius = parameters.Cast("Radius", 5.0f);
            shape.SetShape(ShapeBuilder.MakeSphere(GP.Origin(), radius));
            return true;
        }

        /// <summary>
        /// 创建一个实例对象
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        static public ShapeElement Create(Document doc)
        {
            return ElementSchemaManager.Instance().CreateShapeInstance("Sphere", doc);
        }
    }
}
