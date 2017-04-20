using Common;
using DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Screen
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        private readonly static string configFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.Xml");
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            App.Current.Exit += Current_Exit;
            var task = new Task(() =>
              {
                  var host = new ServiceHost(typeof(Service1));
                  host.Open();
              });
            task.Start();
            //应用启动时先找本地存储的上次的文件
            LoadLocalSetting();
        }


        /// <summary>
        /// 加载本地保存的数据
        /// </summary>
        private void LoadLocalSetting()
        {
            if (File.Exists(configFileName))
            {
                var config = Common.SerializationHelper.DeserializeXmlFile2Obj<ScreenItemsInfo>(configFileName);
                if (config != null)
                {
                    //布局
                    int Rows = config.Rows;
                    int Columns = config.Columns;
                    SetLayout(Rows, Columns);
                    //填充Source
                    AddContentToScreen(config.ScreenItems);
                }
            }
        }

        /// <summary>
        /// 获取当前屏幕上的信息异步方法
        /// WCF调用时必须使用Dispatcher，然后就得使用 async和await
        /// </summary>
        internal async Task<ScreenItemsInfo> GetCurrentWindowInfoAsync()
        {
            ScreenItemsInfo ScreenItemList = new ScreenItemsInfo();
            await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                //此处加try catch 是因为，之前出现 偶发性 平板通过GetCurrentWindowInfoAsync方法拉取大屏现有信息时，Rows和Columns有值，但是ScreenItems的Count为0的情况
                //在catch中捕获异常保存在本地，以验证是否是这里出现的问题
                try
                {
                    ScreenItemList.Rows = this.LayoutRoot.RowDefinitions.Count;
                    ScreenItemList.Columns = this.LayoutRoot.ColumnDefinitions.Count;
                    ScreenItemList.ScreenItems = new List<SingleScreenItemInfo>();
                    foreach (var child in this.LayoutRoot.Children)
                    {
                        SingleScreenItem singleScreenItem = child as SingleScreenItem;
                        if (singleScreenItem != null)
                        {
                            SingleScreenItemInfo itemInfo = new SingleScreenItemInfo()
                            {
                                ID = singleScreenItem.ID,
                                TargetUrl = singleScreenItem.TargetUrl,
                                Row = Grid.GetRow(singleScreenItem),
                                RowSpan = Grid.GetRowSpan(singleScreenItem),
                                Column = Grid.GetColumn(singleScreenItem),
                                ColumnSpan = Grid.GetColumnSpan(singleScreenItem),
                                IsWebKit = singleScreenItem.IsWebKit
                            };
                            ScreenItemList.ScreenItems.Add(itemInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                }
            }));
            return ScreenItemList;
        }

        /// <summary>
        /// 获取当前屏幕上的信息同步方法
        /// 在Current_Exit中不能await
        /// </summary>
        /// <returns></returns>
        internal ScreenItemsInfo GetCurrentWindowInfo()
        {
            ScreenItemsInfo ScreenItemList = new ScreenItemsInfo();
            ScreenItemList.Rows = this.LayoutRoot.RowDefinitions.Count;
            ScreenItemList.Columns = this.LayoutRoot.ColumnDefinitions.Count;
            ScreenItemList.ScreenItems = new List<SingleScreenItemInfo>();
            foreach (var child in this.LayoutRoot.Children)
            {
                SingleScreenItem singleScreenItem = child as SingleScreenItem;
                if (singleScreenItem != null)
                {
                    SingleScreenItemInfo itemInfo = new SingleScreenItemInfo()
                    {
                        ID = singleScreenItem.ID,
                        TargetUrl = singleScreenItem.TargetUrl,
                        Row = Grid.GetRow(singleScreenItem),
                        RowSpan = Grid.GetRowSpan(singleScreenItem),
                        Column = Grid.GetColumn(singleScreenItem),
                        ColumnSpan = Grid.GetColumnSpan(singleScreenItem),
                        IsWebKit = singleScreenItem.IsWebKit
                    };
                    ScreenItemList.ScreenItems.Add(itemInfo);
                }
            }
            return ScreenItemList;
        }

        /// <summary>
        /// 向屏幕上添加一组信息
        /// </summary>
        /// <param name="items"></param>
        internal async void AddContentToScreen(List<SingleScreenItemInfo> items)
        {
            await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                try
                {
                    foreach (SingleScreenItemInfo item in items)
                    {
                        SingleScreenItem uiItem = new SingleScreenItem(item.IsWebKit);
                        Grid.SetRow(uiItem, item.Row);/*uiItem.Row = item.RowPosition;*/
                        Grid.SetRowSpan(uiItem, item.RowSpan);/* uiItem.RowSpan = item.RowCount;*/
                        Grid.SetColumn(uiItem, item.Column); /*uiItem.Column = item.ColumnPosition;*/
                        Grid.SetColumnSpan(uiItem, item.ColumnSpan); /*uiItem.ColumnSpan = item.ColumnCount;*/
                        uiItem.TargetUrl = item.TargetUrl;
                        uiItem.ID = item.ID;
                        this.LayoutRoot.Children.Add(uiItem);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                }
            }));
        }

        /// <summary>
        /// 保存屏幕上的信息到Xml
        /// </summary>
        private void SaveCurrentInfoToXml()
        {
            var ScreenItemList = GetCurrentWindowInfo();
            //序列化这个对象  
            Common.SerializationHelper.SerializerObj2XmlFile(ScreenItemList, configFileName);
        }

        /// <summary>
        /// 应用退出前保存当前配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                SaveCurrentInfoToXml();
                CefSharp.Cef.Shutdown();
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "Current_Exit");
            }
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        public async void SetLayout(int rows, int columns)
        {
            await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                try
                {
                    this.LayoutRoot.Children.Clear();
                    this.LayoutRoot.RowDefinitions.Clear();
                    this.LayoutRoot.ColumnDefinitions.Clear();
                    for (int j = 0; j < rows; j++)
                    {
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(1, GridUnitType.Star);
                        this.LayoutRoot.RowDefinitions.Add(row);
                    }
                    for (int k = 0; k < columns; k++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        column.Width = new GridLength(1, GridUnitType.Star);
                        this.LayoutRoot.ColumnDefinitions.Add(column);
                    }
                    //每行
                    for (int j = 0; j < rows; j++)
                    {
                        //每列
                        for (int k = 0; k < columns; k++)
                        {
                            //放个Border进行占位，方便呈现视觉效果
                            Border placeholder = new Border()
                            {
                                BorderBrush = new SolidColorBrush(Colors.Black),
                                BorderThickness = new Thickness(1)
                            };
                            Grid.SetColumn(placeholder, k);
                            Grid.SetRow(placeholder, j);
                            this.LayoutRoot.Children.Add(placeholder);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                }
            }));
        }

        /// <summary>
        /// 选择某元素
        /// </summary>
        /// <param name="index">元素序数</param>
        public async void SelectWindow(Guid id)
        {
            await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                try
                {
                    foreach (var child in this.LayoutRoot.Children)
                    {
                        SingleScreenItem singleScreenItem = child as SingleScreenItem;
                        if (singleScreenItem != null)
                        {
                            if (singleScreenItem.ID == id)
                            {
                                singleScreenItem.BorderBrush = new SolidColorBrush(Colors.Red);
                                System.Windows.Controls.Panel.SetZIndex(singleScreenItem, 1);
                            }
                            else
                            {
                                singleScreenItem.BorderBrush = new SolidColorBrush(Colors.Black);
                                System.Windows.Controls.Panel.SetZIndex(singleScreenItem, 0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                }
            }));
        }

        /// <summary>
        /// 更新某窗口信息
        /// </summary>
        /// <param name="itemInfo"></param>
        public async void UpdateWindow(SingleScreenItemInfo itemInfo)
        {
            await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                try
                {
                    Guid index = itemInfo.ID;
                    SingleScreenItem aimItem = null;
                    foreach (var child in this.LayoutRoot.Children)
                    {
                        SingleScreenItem singleScreenItem = child as SingleScreenItem;
                        if (singleScreenItem != null)
                        {
                            if (singleScreenItem.ID == index)
                            {
                                aimItem = singleScreenItem;
                                break;
                            }
                        }
                    }
                    if (aimItem != null)
                    {
                        bool iswebkit = aimItem.IsWebKit;
                        this.LayoutRoot.Children.Remove(aimItem);
                        aimItem = null;
                        SingleScreenItem newItem = new SingleScreenItem(iswebkit);
                        Grid.SetRow(newItem, itemInfo.Row);
                        Grid.SetRowSpan(newItem, itemInfo.RowSpan);
                        Grid.SetColumn(newItem, itemInfo.Column);
                        Grid.SetColumnSpan(newItem, itemInfo.ColumnSpan);
                        newItem.TargetUrl = itemInfo.TargetUrl;
                        newItem.ID = itemInfo.ID;
                        this.LayoutRoot.Children.Add(newItem);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex, new StackTrace(true).GetFrame(0).GetMethod().Name);
                }
            }));
        }
    }
}
