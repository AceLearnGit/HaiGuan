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
using System.Xml.Serialization;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using System.Runtime.InteropServices;
using Common;
using System.Diagnostics;

namespace Screen
{
    /// <summary>
    /// SingleScreenItem.xaml 的交互逻辑
    /// </summary>
    public partial class SingleScreenItem : UserControl
    {
        public SingleScreenItem()
        {
            InitializeComponent();
            this.Unloaded += SingleScreenItem_Unloaded;
        }

        private void SingleScreenItem_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //释放webbrowser资源
                (windowsFormsHost.Child as System.Windows.Forms.WebBrowser)?.Dispose();
                //释放ChromiumWebBrowser资源
                (windowsFormsHost.Child as ChromiumWebBrowser)?.Dispose();
                //不放心，再设为null
                windowsFormsHost.Child = null;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "SingleScreenItem_Unloaded");
            }
        }

        /// <summary>
        /// 是否启用WebKit内核
        /// </summary>
        public bool IsWebKit = true;
        /// <summary>
        /// 是否已初始化Cef
        /// </summary>
        private static bool CefInitialized = false;
        public SingleScreenItem(bool isWebKit) : this()
        {
            IsWebKit = isWebKit;
            if (!CefInitialized)
            {
                var settings = new CefSettings();
                settings.Locale = "zh-CN";
                //支持Flash，非常重要
                settings.CefCommandLineArgs.Add("ppapi-flash-path", System.AppDomain.CurrentDomain.BaseDirectory + "PepperFlash\\pepflashplayer.dll"); //指定flash的版本，不使用系统安装的flash版本
                settings.CefCommandLineArgs.Add("ppapi-flash-version", "24.0.0.221");
                Cef.Initialize(settings);
                CefInitialized = true;
            }
        }

        public Guid ID
        {
            get { return (Guid)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("ID", typeof(Guid), typeof(SingleScreenItem), new PropertyMetadata(default(Guid)));



        public string TargetUrl
        {
            get { return (string)GetValue(TargetUrlProperty); }
            set { SetValue(TargetUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetUrlProperty =
            DependencyProperty.Register("TargetUrl", typeof(string), typeof(SingleScreenItem), new PropertyMetadata(null, TargetUrlPropertyChanged));

        private static void TargetUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleScreenItem current = d as SingleScreenItem;
            if (current.IsWebKit)
            {
                (current.windowsFormsHost.Child as ChromiumWebBrowser)?.Dispose();
                current.windowsFormsHost.Child = null;
                ChromiumWebBrowser browser = new ChromiumWebBrowser(e.NewValue.ToString());
                current.windowsFormsHost.Child = browser;
            }
            else
            {
                //当Source改变时先释放资源，再搞个新的，防止出现“来自网页的消息，内存不足，位置第一行”
                System.Windows.Forms.WebBrowser broswer = current.windowsFormsHost.Child as System.Windows.Forms.WebBrowser;
                if (broswer != null)
                {
                    broswer.Dispose();
                    broswer = null;
                    current.windowsFormsHost.Child = null;
                }
                broswer = new System.Windows.Forms.WebBrowser();
                broswer.ScriptErrorsSuppressed = true;
                broswer.IsWebBrowserContextMenuEnabled = false;
                current.windowsFormsHost.Child = broswer;
                broswer.Navigate(e.NewValue.ToString());
            }
        }
    }


}
