using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidCAX
{
    public class BrowerNodeItem
    {
        public string Icon { get; set; }
        public string DisplayName { get; set; }
        public Object Tag { get; set; }

        public ObservableCollection<BrowerNodeItem> Children { get; set; }

        public BrowerNodeItem()
        {
            Children = new ObservableCollection<BrowerNodeItem>();
        }
    }

    public class RapidDocumentListener : DocumentListener
    {
        public ObservableCollection<BrowerNodeItem> mProjectBrower = new ObservableCollection<BrowerNodeItem>();

        public RapidDocumentListener()
        {
            DocumentListener.SetListener(this);
        }

        public override void OnAddElements(Document pDocument, ElementIdSet ids)
        {
            foreach(var id in ids)
            {
               var element = DrawableElement.Cast(pDocument.FindElement(id));
                if (element == null)
                    continue;

                var item = new BrowerNodeItem();
                item.DisplayName = String.Format("{0}({1})", element.GetName(), element.GetId().GetInteger());
                item.Tag = element.GetId();
                mProjectBrower.Add(item);
            }

        }

        public override void OnRemoveElements(Document pDocument, ElementIdSet ids)
        {
            var nodes = mProjectBrower.Where(x => ids.Contains((ElementId)x.Tag)).ToList();
            foreach (var node in nodes)
            {
                mProjectBrower.Remove(node);
            }
        }
    }
}
