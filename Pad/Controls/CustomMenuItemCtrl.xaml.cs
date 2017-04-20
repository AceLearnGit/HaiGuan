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

namespace Pad.Controls
{
    /// <summary>
    /// CustomMenuItemCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class CustomMenuItemCtrl : RadioButton
    {
        public CustomMenuItemCtrl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 文字前Icon的路径
        /// </summary>
        public string IconUrl
        {
            get { return (string)GetValue(IconUrlProperty); }
            set { SetValue(IconUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconUrlProperty =
            DependencyProperty.Register("IconUrl", typeof(string), typeof(CustomMenuItemCtrl), new PropertyMetadata(""));



        /// <summary>
        /// 文字后箭头的透明度
        /// </summary>
        public int ArrowOpacity
        {
            get { return (int)GetValue(ArrowOpacityProperty); }
            set { SetValue(ArrowOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowOpacityProperty =
            DependencyProperty.Register("ArrowOpacity", typeof(int), typeof(CustomMenuItemCtrl), new PropertyMetadata(1));


    }
}
