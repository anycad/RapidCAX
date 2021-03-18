using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    /// <summary>
    /// 保存树节点信息
    /// </summary>
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

    /// <summary>
    /// 用于接收文档变化的监听器
    /// </summary>
    public class MyDocumentListener : DocumentListener
    {
        public ObservableCollection<BrowerNodeItem> mProjectBrower = new ObservableCollection<BrowerNodeItem>();

        public MyDocumentListener()
        {
            DocumentListener.SetListener(this);
        }

        /// <summary>
        /// 当增加图元的调用
        /// </summary>
        /// <param name="pDocument"></param>
        /// <param name="ids">图元的ID列表</param>
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

        /// <summary>
        /// 当删除图元时候调用
        /// </summary>
        /// <param name="pDocument"></param>
        /// <param name="ids">删除图元的ID列表</param>
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
