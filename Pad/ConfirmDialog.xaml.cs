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

namespace Pad
{
    /// <summary>
    /// ConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog()
        {
            InitializeComponent();
        }


        public string Result
        {
            get { return (string)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Result.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(string), typeof(ConfirmDialog), new PropertyMetadata(""));


        private void btnn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb.Text))
            {
                Result = tb.Text;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btnn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
