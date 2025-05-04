using CustomerEmail.ViewModels;
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
using System.Windows.Shapes;

namespace CustomerEmail.Views
{
    /// <summary>
    /// Interaction logic for EmailLogView.xaml
    /// </summary>
    public partial class EmailLogView : Window
    {
        public EmailLogView()
        {
            DataContext = new EmailLogViewModel();
            ((EmailLogViewModel)DataContext).Window = this;
            InitializeComponent();
        }
    }
}
