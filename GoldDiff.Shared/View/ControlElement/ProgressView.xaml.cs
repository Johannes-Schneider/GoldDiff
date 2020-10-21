using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using GoldDiff.Shared.View.Controller;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.Shared.View.ControlElement
{
    /// <summary>
    /// Interaction logic for ProgressView.xaml
    /// </summary>
    public partial class ProgressView : UserControl
    {
        private static readonly DependencyProperty PrivateModelProperty = DependencyProperty.Register(nameof(PrivateModel), typeof(ProgressViewViewModel), MethodBase.GetCurrentMethod().DeclaringType);
        
        private ProgressViewViewModel PrivateModel
        {
            get => GetValue(PrivateModelProperty) as ProgressViewViewModel ?? throw new NullReferenceException($"{nameof(PrivateModel)} must not be {null}!");
            set => SetValue(PrivateModelProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }
        
        public ProgressViewViewModel Model { get; }
        public ProgressViewController Controller { get; }
        
        public ProgressView()
        {
            InitializeComponent();
            
            Model = new ProgressViewViewModel();
            Controller = new ProgressViewController(Model);

            PrivateModel = Model;
        }
    }
}
