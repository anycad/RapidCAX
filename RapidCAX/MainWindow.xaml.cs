using AnyCAD.Foundation;
using AnyCAD.Rapid.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RapidCAX
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static readonly RoutedCommand RapidCommand = new RoutedCommand("Rapid", typeof(MainWindow));

        public MyDocumentViewer mDocumentView;
        public MyDocumentListener mDocumentListener;
        public MainWindow()
        {
            InitializeComponent();

            //命令绑定
            CommandBindings.Add(new CommandBinding(RapidCommand, RapidExecuted));
            // 注册文档消息
            mDocumentListener = new MyDocumentListener();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //创建文档视图控件
            mDocumentView = new MyDocumentViewer(this.documentViewHost, 
                (PickedItem item, Document doc, ElementId id)=>
                {
                    
                    propertyBrowser.Children.Clear();

                    if (!id.IsValid())
                        return;

                    //增加一条日志
                    this.outputCtrl.outputList.Items.Add(id.GetInteger());

                    //显示选中图元的属性
                    CreateElementPropertyUI(doc, id);
                });
            // 绑定构件浏览数据
            this.projectBrowser.ItemsSource = mDocumentListener.mProjectBrower;
        }

        void RapidExecuted(object sender, ExecutedRoutedEventArgs e)
        {            
            mDocumentView.ExecuteCommand(e.Parameter.ToString());
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="id"></param>
        void CreateElementPropertyUI(Document doc, ElementId id)
        {
            var element = doc.FindElement(id);
            if (element == null)
                return;
            {
                Expander expander = new Expander();
                expander.Header = "Element";
                expander.Content = new BasicPage(element);
                propertyBrowser.Children.Add(expander);
                expander.IsExpanded = true;
            }

            DrawableElement de = DrawableElement.Cast(element);
            if(de != null)
            {
                Expander expander = new Expander();
                expander.Header = "Location";
                var page = new TransformPage(de);
                page.UpdateViewEvent += mDocumentView.UpdateView;
                expander.Content = page;
                propertyBrowser.Children.Add(expander);
                expander.IsExpanded = true;
            }
        }

        private void projectBrowser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
        }
    }
}
