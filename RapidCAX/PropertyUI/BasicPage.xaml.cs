using AnyCAD.Foundation;
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
    /// Interaction logic for BasicPage.xaml
    /// </summary>
    public partial class BasicPage : UserControl
    {
        private Element mElement;
        public BasicPage(Element element)
        {
            mElement = element;

            InitializeComponent();

            DataContext = this;
        }

        public string ElementName
        {
            get { return mElement.GetName(); }
            set {
                mElement.SetName(value);
            }
        }

        public string ElementSchemaName
        {
            get { return mElement.GetSchemaName(); }
        }
    }
}
