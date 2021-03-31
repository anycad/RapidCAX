using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    class MyTextElement
    {
        public string Text = "";
        public Vector3 Position = new Vector3(0);
        public ElementId Id;
        public TextSceneNode Node;
        public MyTextElement()
        {

        }

        public MyTextElement(string text, Vector3 position)
        {
            Text = text;
            Position = position;
        }

        static public bool IsKindOf(Element element)
        {
            return element.GetSchemaName() == "TextSceneNode";
        }

        /// <summary>
        /// 只能加一次
        /// </summary>
        /// <param name="doc"></param>
        public void AddElement(Document doc)
        {
            var element = new UserElement();
            element.SetName(Text);
            element.SetSchemaName("TextSceneNode");
            Id = doc.AddElement(element);

            element.AddParameter("PositionX", ParameterCreator.Create(Position.x));
            element.AddParameter("PositionY", ParameterCreator.Create(Position.y));
            element.AddParameter("PositionZ", ParameterCreator.Create(Position.z));
        }

        public void Load(UserElement element)
        {
            Text = element.GetName();
            Position.x = ParameterCast.Cast(element.FindParameter("PositionX"), 0.0f);
            Position.y = ParameterCast.Cast(element.FindParameter("PositionY"), 0.0f);
            Position.z = ParameterCast.Cast(element.FindParameter("PositionZ"), 0.0f);
            Id = element.GetId();
        }

        public void Show(Scene scene)
        {
            if (Node != null)
                return;

            Node = new TextSceneNode(Text, 10, Vector3.Blue, new Vector3(-1), true);
            Node.SetTransform(Matrix4.makeTranslation(Position));
            Node.SetUserId(Id.GetInteger());
            Node.RequstUpdate();

            scene.AddNode(Node);
        }
    }
}
