using AnyCAD.Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TransformPage.xaml
    /// </summary>
    public partial class TransformPage : UserControl, INotifyPropertyChanged
    {
        private GPnt mLocation;
        private DrawableElement mElement;
        private bool mModified = false;
        public TransformPage(DrawableElement element)
        {
            mElement = element;
            var loc = mElement.GetCoordinateSystem().Location();
            mLocation = new GPnt(loc.XYZ());
            InitializeComponent();

            this.DataContext = this;
        }

        public double LocationX
        {
            get { return mLocation.X(); }
            set { mLocation.SetX(value); LocationModified = true; }
        }

        public double LocationY
        {
            get { return mLocation.Y(); }
            set { mLocation.SetY(value); LocationModified = true; }
        }
        public double LocationZ
        {
            get { return mLocation.Z(); }
            set { mLocation.SetZ(value); LocationModified = true; }
        }

        public bool LocationModified
        {
            get { return mModified; }
            set { mModified = value; OnPropertyChanged("LocationModified"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public delegate void UpdateViewHandler();
        public event UpdateViewHandler UpdateViewEvent;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UndoTransaction undo = new UndoTransaction(mElement);
            undo.Start("Transform");
            var cs = new GAx3();
            cs.SetLocation(mLocation);
            mElement.SetCoordinateSystem(cs);
            undo.Commit();

            LocationModified = false;

            if (UpdateViewEvent != null)
                UpdateViewEvent();
        }
    }
}
