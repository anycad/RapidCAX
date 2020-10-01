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

        public DocumentView mDocumentView;
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(RapidCommand, RapidExecuted));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mDocumentView = new DocumentView(this.documentViewHost);
        }

        void RapidExecuted(object sender, ExecutedRoutedEventArgs e)
        {            
            mDocumentView.ExecuteCommand(e.Parameter.ToString());
        }

        private void projectBrowser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
