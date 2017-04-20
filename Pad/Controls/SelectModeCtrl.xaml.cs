using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using UICommon.Controls;

namespace Pad.Controls
{
    /// <summary>
    /// SelectModeCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectModeCtrl : UserControl
    {
        /// <summary>
        /// 本次点击显示时要发送给大屏幕的数据项信息
        /// 发送完应当清空
        /// </summary>
        private List<SingleScreenItemInfo> currentItemsInfo;
        /// <summary>
        /// 区域列表数据源
        /// </summary>
        private ObservableCollection<int> RegionList { get; set; }

        /// <summary>
        /// 通过区域列表选中的某个区域
        /// </summary>
        private Border selectedBorder = null;
        /// <summary>
        /// 通过区域列表选中的某个区域的索引
        /// </summary>
        private int selectedIndex = 0;
        /// <summary>
        /// 配置每块小屏时小屏的索引，自增长
        /// </summary>
        private int ScreenItemIndex { get; set; } = 1;
        private bool IsUpdateInfo { get; set; } = false;
        public SelectModeCtrl()
        {
            InitializeComponent();
            this.Loaded += SelectModeCtrl_Loaded;
        }
        private void SelectModeCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            RegionList = new ObservableCollection<int>();
            cb_Sources.ItemsSource = DataManager.SourcesInfo.SourcesItems;
            lb_SetedRegion.ItemsSource = RegionList;
        }
        /// <summary>
        /// 重新划分示意区域并重置各字段
        /// </summary>
        public void SetLayout()
        {
            DataManager.SetBoardLayout(DataManager.Rows, DataManager.Columns, Board);
            //重新分区时应重置各字段
            Reset();
        }
        /// <summary>
        /// 重置各字段、属性
        /// </summary>
        private void Reset()
        {
            //allItemsInfoSent2Screen = null;
            currentItemsInfo = null;
            RegionList?.Clear();
            selectedBorder = null;
            selectedIndex = 0;
            ScreenItemIndex = 1;
            IsUpdateInfo = false;
            //清空Canvas
            List<Border> list = new List<Border>();
            foreach (var item in cv.Children)
            {
                Border border = item as Border;
                if (border != null)
                    //cv.Children.Remove(border);
                    list.Add(border);
            }
            for (int i = 0; i < list.Count; i++)
            {
                cv.Children.Remove(list[i]);
            }
        }
        /// <summary>
        /// 区域列表选中项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lb_SetedRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((sender as ListBox).SelectedIndex == -1)
            //    return;
            ////当前选中的是哪个区域，算出这个区域的索引
            //selectedIndex = int.Parse((sender as ListBox).SelectedValue.ToString());
            ////遍历示意区域，根据索引找到对应的Border
            //foreach (Border item in this.Board.Children)
            //{
            //    if (item.Tag != null)
            //        if (item.Tag.ToString() == selectedIndex.ToString())
            //        {
            //            selectedBorder = item;
            //            item.BorderBrush = new SolidColorBrush(Colors.Red);
            //        }
            //        else
            //        {
            //            item.BorderBrush = new SolidColorBrush(Colors.Black);
            //        }
            //}
            ////如果是更新操作，还要向大屏发消息
            //if (IsUpdateInfo)
            //{
            //    await WCFServiceManager.Instance.SelectWindow(selectedIndex);
            //}
            ////根据索引在已发送到屏幕信息列表中查找对应项，并填充到界面上
            //SingleScreenItemInfo indexitem = DataManager.ScreenItemsInfoOnScreen.ScreenItems?.FirstOrDefault(t => t.Index == selectedIndex);
            //if (indexitem != null)
            //{
            //    locationX.Text = (indexitem.Column + 1).ToString();
            //    locationY.Text = (indexitem.Row + 1).ToString();
            //    columnSpan.Text = indexitem.RowSpan.ToString();
            //    rowSpan.Text = indexitem.ColumnSpan.ToString();
            //    cb_Sources.Text = DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.SourceUrl == indexitem.TargetUrl)?.KeyName;
            //}
        }
        /// <summary>
        /// 显示按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Show_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoOnBigScreen();
        }
        /// <summary>
        /// 将要在大屏上显示的信息列表发送出去
        /// </summary>
        private async void ShowInfoOnBigScreen()
        {
            //初始化已发送到屏幕的数据列表
            if (DataManager.ScreenItemsInfoOnScreen == null)
            {
                DataManager.ScreenItemsInfoOnScreen = new ScreenItemsInfo()
                {
                    Rows = DataManager.Rows,
                    Columns = DataManager.Columns,
                    ScreenItems = new List<SingleScreenItemInfo>()
                };
            }
            //把本次要发送的添加到已发送列表中
            if (currentItemsInfo != null)
            {
                foreach (var item in currentItemsInfo)
                {
                    DataManager.ScreenItemsInfoOnScreen.ScreenItems.Add(item);
                }
                //发送内容
                await WCFServiceManager.Instance.AddContentToScreen("A",currentItemsInfo.ToArray());
                //发送完清空发送列表
                currentItemsInfo = null;
                //大屏上有内容了，接下来可以才更新某一块内容
                IsUpdateInfo = true;
            }
        }
        /// <summary>
        /// 保存为模板按钮事件
        /// 注意这里是保存的已发送到屏幕上的信息，即点击过显示按钮后才有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAsTemplate_Click(object sender, RoutedEventArgs e)
        {
            //DataManager.SaveAsTemplate(DataManager.ScreenItemsInfoOnScreen);
        }

        /// <summary>
        /// 保存该区域按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveOneScreen_Click(object sender, RoutedEventArgs e)
        {
            //示意区域处理和区域列表处理
            var item = SavaOneScreen(int.Parse(locationY.Text) - 1, int.Parse(columnSpan.Text), int.Parse(locationX.Text) - 1, int.Parse(rowSpan.Text), ScreenItemIndex, cb_Sources.Text, (bool)cb_IsWebKit.IsChecked);
            //把对应的实体对象添加到本次要发送的列表中
            AddOneItem2currentItemsInfo(item);
        }

        /// <summary>
        /// 添加一块屏幕的信息到本次发送列表中
        /// </summary>
        /// <param name="item"></param>
        private void AddOneItem2currentItemsInfo(SingleScreenItemInfo item)
        {
            if (currentItemsInfo == null)
            {
                currentItemsInfo = new List<SingleScreenItemInfo>();
            }
            currentItemsInfo.Add(item);
        }

        /// <summary>
        /// 保存该区域时示意区域的处理和区域列表的处理
        /// </summary>
        private SingleScreenItemInfo SavaOneScreen(int row, int rowSpan, int column, int columnSpan, int index, string keyName, bool isWebKit)
        {
            ////更新区域列表ItemSource
            //RegionList.Add(index);
            ////实例化对应数据模型
            //SingleScreenItemInfo item = new SingleScreenItemInfo()
            //{
            //    Index = index,
            //    Row = row,
            //    RowSpan = rowSpan,
            //    Column = column,
            //    ColumnSpan = columnSpan,
            //    TargetUrl = DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.KeyName == keyName).SourceUrl,
            //    IsWebKit = isWebKit
            //};
            //return item;
            return new SingleScreenItemInfo();
        }

        /// <summary>
        /// 更新该区域按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_UpdateOneScreen_Click(object sender, RoutedEventArgs e)
        {
            UpdateOneScreen();
        }

        /// <summary>
        /// 更新某块小屏幕区域
        /// </summary>
        private async void UpdateOneScreen()
        {
            #region OldCode
            ////先从示意区域移除
            //this.Board.Children.Remove(selectedBorder);
            ////重新算设置数据
            //Grid.SetRow(selectedBorder, int.Parse(locationY.Text) - 1);
            //Grid.SetRowSpan(selectedBorder, int.Parse(columnSpan.Text));
            //Grid.SetColumn(selectedBorder, int.Parse(locationX.Text) - 1);
            //Grid.SetColumnSpan(selectedBorder, int.Parse(rowSpan.Text));
            //this.Board.Children.Add(selectedBorder);
            ////这个区域的数据模型
            //SingleScreenItemInfo item = new SingleScreenItemInfo()
            //{
            //    Index = selectedIndex,
            //    Row = Grid.GetRow(selectedBorder),
            //    RowSpan = Grid.GetRowSpan(selectedBorder),
            //    Column = Grid.GetColumn(selectedBorder),
            //    ColumnSpan = Grid.GetColumnSpan(selectedBorder),
            //    TargetUrl = DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.KeyName == cb_Sources.Text).SourceUrl,
            //    IsWebKit = (bool)cb_IsWebKit.IsChecked
            //};
            ////通过WCF发送更新消息
            //await WCFServiceManager.Instance.UpdateWindowInfo(item);
            //foreach (var screenItem in DataManager.ScreenItemsInfoOnScreen.ScreenItems)
            //{
            //    if (screenItem.Index == selectedIndex)
            //    {
            //        screenItem.Row = item.Row;
            //        screenItem.RowSpan = item.RowSpan;
            //        screenItem.Column = item.Column;
            //        screenItem.ColumnSpan = item.ColumnSpan;
            //        screenItem.TargetUrl = item.TargetUrl;
            //        screenItem.IsWebKit = item.IsWebKit;
            //    }
            //}
            #endregion
            ////根据selectedRow，selectedColumn，selectedRowSpan，selectedColumnSpan创建数据模型
            //SingleScreenItemInfo item = new SingleScreenItemInfo()
            //{
            //    Index = selectedIndex,
            //    Row = selectedRow,
            //    RowSpan = selectedRowSpan,
            //    Column = selectedColumn,
            //    ColumnSpan = selectedColumnSpan,
            //    TargetUrl = DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.KeyName == cb_Sources.Text).SourceUrl,
            //    IsWebKit = (bool)cb_IsWebKit.IsChecked
            //};
            ////通过WCF发送更新消息
            //await WCFServiceManager.Instance.UpdateWindowInfo(item);
            //foreach (var screenItem in DataManager.ScreenItemsInfoOnScreen.ScreenItems)
            //{
            //    if (screenItem.Index == selectedIndex)
            //    {
            //        screenItem.Row = item.Row;
            //        screenItem.RowSpan = item.RowSpan;
            //        screenItem.Column = item.Column;
            //        screenItem.ColumnSpan = item.ColumnSpan;
            //        screenItem.TargetUrl = item.TargetUrl;
            //        screenItem.IsWebKit = item.IsWebKit;
            //    }
            //}
        }

        /// <summary>
        /// 返回按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            //TODO：返回设置数据页面
        }

        /// <summary>
        /// 使用DataManager中的数据初始化页面
        /// 使用模板时，将模板中保存的数据保存在DataManager中，如果用户手动点击了菜单栏的设置模式选项，设置模式页面应读取DataManager中的数据并填充到界面上
        /// </summary>
        public void SetSelectModeData()
        {
            SetLayout();
            if (DataManager.ScreenItemsInfoOnScreen?.ScreenItems != null)
            {
                foreach (var item in DataManager.ScreenItemsInfoOnScreen?.ScreenItems)
                {
                    //本地有大屏上的数据，只需处理示意区域和列表区域-
                    //-不向本次要发送列表添加数据，以免点击显示按钮时向大屏发生重复数据
                    //SavaOneScreen
                    //(
                    //    item.Row,
                    //    item.RowSpan,
                    //    item.Column,
                    //    item.ColumnSpan,
                    //    item.Index,
                    //    DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.SourceUrl == item.TargetUrl).KeyName,
                    //    item.IsWebKit
                    //);
                    //向示意区域添加Border
                    Border border = CreateDragableBorder(true,ScreenItemIndex.ToString());
                    border.GotFocus += Border_GotFocus;
                    Rect position = new Rect()
                    {
                        X = item.Column * Board.Width / Board.ColumnDefinitions.Count + 5,
                        Y = item.Row * Board.Height / Board.RowDefinitions.Count + 5,
                        Width = item.ColumnSpan * Board.Width / Board.ColumnDefinitions.Count - 10,
                        Height = item.RowSpan * Board.Height / Board.RowDefinitions.Count - 10
                    };
                    PlaceBorder(border, cv, position);
                    //ScreenItemIndex = item.Index + 1;
                }
            }
        }



        #region 鼠标选择区域
        /// <summary>
        /// 起点的Row和Column
        /// </summary>
        int selectedRow = 0, selectedColumn = 0;
        /// <summary>
        /// 选定区域的RowSpan和ColumnSpan
        /// </summary>
        int selectedRowSpan = 1, selectedColumnSpan = 1;
        /// <summary>
        /// 矩形开始绘制的起点
        /// </summary>
        private Point StartPoint;
        /// <summary>
        /// 是否是绘制状态
        /// </summary>
        private bool isDrawing = false;


        /// <summary>
        /// 鼠标按下，准备开始绘制矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //如果按下位置是Canvas代表要重新绘制矩形
            Canvas target = e.OriginalSource as Canvas;
            if (target != null)
            {
                rect.Width = 0;
                rect.Height = 0;
                //记录鼠标按钮位置，即为矩形的左上角位置
                StartPoint = e.GetPosition(sender as Canvas);
            }
        }
        /// <summary>
        /// 绘制矩形，大小根据鼠标移动改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //如果Drag Helper显示，代表是要移动矩形
            if (helper.Visibility == Visibility.Visible)
                return;
            //否则则是绘制矩形
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //如果是在Border上，即已划分的区域，不允许绘制矩形
                if ((e.OriginalSource as Border) != null)
                    return;
                Canvas.SetLeft(rect, StartPoint.X);
                Canvas.SetTop(rect, StartPoint.Y);
                rect.Visibility = Visibility.Visible;
                if (e.GetPosition(sender as Canvas).X > StartPoint.X && e.GetPosition(sender as Canvas).Y > StartPoint.Y)
                {
                    rect.Width = e.GetPosition(sender as Canvas).X - StartPoint.X;
                    rect.Height = e.GetPosition(sender as Canvas).Y - StartPoint.Y;
                }
                isDrawing = true;
            }
        }
        /// <summary>
        /// 鼠标抬起时（矩形绘制完毕）触发DragHeler的DragCompletedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //EndPoint = e.GetPosition(sender as Canvas);
            //如果鼠标抬起时是矩形，代表绘制矩形结束
            Debug.WriteLine(e.OriginalSource);
            Rectangle target = e.OriginalSource as Rectangle;
            if (target == null)
                return;
            if (isDrawing)
            {
                Rect r = new Rect
                {
                    Y = Canvas.GetTop(rect),
                    X = Canvas.GetLeft(rect),
                    Width = rect.ActualWidth,
                    Height = rect.ActualHeight
                };
                AddBorder2Canvas(r);
            }
            isDrawing = false;
            rect.Width = 0;
            rect.Height = 0;
            StartPoint = new Point();
        }

        /// <summary>
        /// 根据绘制的矩形向Canvas中加入一个规则的Border
        /// </summary>
        /// <param name="rect"></param>
        private void AddBorder2Canvas(Rect rect)
        {
            double gridWidth = Board.ActualWidth / Board.ColumnDefinitions.Count;
            double gridHeight = Board.ActualHeight / Board.RowDefinitions.Count;
            selectedColumn = (int)(rect.Left / gridWidth);
            if (selectedColumn > Board.ColumnDefinitions.Count - 1)
            {
                selectedColumn = Board.ColumnDefinitions.Count - 1;
            }
            if (selectedColumn < 0)
            {
                selectedColumn = 0;
            }
            selectedRow = (int)(rect.Top / gridHeight);
            if (selectedRow > Board.RowDefinitions.Count - 1)
            {
                selectedRow = Board.RowDefinitions.Count - 1;
            }
            if (selectedRow < 0)
            {
                selectedRow = 0;
            }
            int endColumns = (int)((rect.Left + rect.Width) / gridWidth);
            if (endColumns > Board.ColumnDefinitions.Count - 1)
            {
                endColumns = Board.ColumnDefinitions.Count - 1;
            }
            if (endColumns < 0)
            {
                endColumns = 0;
            }
            int endRows = (int)((rect.Top + rect.Height) / gridHeight);
            if (endRows > Board.RowDefinitions.Count - 1)
            {
                endRows = Board.RowDefinitions.Count - 1;
            }
            if (endRows < 0)
            {
                endRows = 0;
            }
            //定位Border
            //搞一个Border放在指定位置
            Border screenItem = CreateDragableBorder(true,ScreenItemIndex.ToString());
            screenItem.GotFocus += Border_GotFocus;
            Rect borderRegion = new Rect()
            {
                X = gridWidth * selectedColumn + 5,
                Y = gridHeight * selectedRow + 5,
                Width = (endColumns - selectedColumn + 1) * gridWidth - 10,
                Height = (endRows - selectedRow + 1) * gridHeight - 10
            };
            PlaceBorder(screenItem, cv, borderRegion);
            //TODO：到时候要去掉的
            //保存区域方法中目前是根据文本框文本计算值的
            locationY.Text = (selectedRow + 1).ToString();
            locationX.Text = (selectedColumn + 1).ToString();
            columnSpan.Text = selectedRowSpan.ToString();
            rowSpan.Text = selectedColumnSpan.ToString();
            ScreenItemIndex++;
        }


        /// <summary>
        /// 点击某区域时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Border_GotFocus(object sender, RoutedEventArgs e)
        {
            //Border target = sender as Border;
            //if (target == null)
            //    return;
            //selectedBorder = target;
            //selectedIndex = int.Parse(target.Tag.ToString());
            ////通知大屏选中了某个小屏
            //await WCFServiceManager.Instance.SelectWindow(selectedIndex);
            ////TODO:去掉右边的区域后得改
            ////根据索引在已发送到屏幕信息列表中查找对应项，并填充到界面上
            //SingleScreenItemInfo indexitem = DataManager.ScreenItemsInfoOnScreen?.ScreenItems?.FirstOrDefault(t => t.Index == selectedIndex);
            //if (indexitem != null)
            //{
            //    locationX.Text = (indexitem.Column + 1).ToString();
            //    locationY.Text = (indexitem.Row + 1).ToString();
            //    columnSpan.Text = indexitem.RowSpan.ToString();
            //    rowSpan.Text = indexitem.ColumnSpan.ToString();
            //    cb_Sources.Text = DataManager.SourcesInfo.SourcesItems.FirstOrDefault(t => t.SourceUrl == indexitem.TargetUrl)?.KeyName;
            //}
        }

        /// <summary>
        /// DragHelper拖动完成事件
        /// 根据事件参数拿到选定区域位置和大小，从而起始点，跨行数、列数
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void helper_DragCompleted(object Sender, DragChangedEventArgs e)
        {
            double gridWidth = Board.ActualWidth / Board.ColumnDefinitions.Count;
            double gridHeight = Board.ActualHeight / Board.RowDefinitions.Count;
            selectedColumn = (int)(e.NewBound.Left / gridWidth);
            if (selectedColumn > Board.ColumnDefinitions.Count - 1)
            {
                selectedColumn = Board.ColumnDefinitions.Count - 1;
            }
            if (selectedColumn < 0)
            {
                selectedColumn = 0;
            }
            selectedRow = (int)(e.NewBound.Top / gridHeight);
            if (selectedRow > Board.RowDefinitions.Count - 1)
            {
                selectedRow = Board.RowDefinitions.Count - 1;
            }
            if (selectedRow < 0)
            {
                selectedRow = 0;
            }
            int endColumns = (int)((e.NewBound.Left + e.NewBound.Width) / gridWidth);
            if (endColumns > Board.ColumnDefinitions.Count - 1)
            {
                endColumns = Board.ColumnDefinitions.Count - 1;
            }
            if (endColumns < 0)
            {
                endColumns = 0;
            }
            int endRows = (int)((e.NewBound.Top + e.NewBound.Height) / gridHeight);
            if (endRows > Board.RowDefinitions.Count - 1)
            {
                endRows = Board.RowDefinitions.Count - 1;
            }
            if (endRows < 0)
            {
                endRows = 0;
            }
            Canvas.SetLeft(helper.TargetElement, gridWidth * selectedColumn + 5);
            Canvas.SetTop(helper.TargetElement, gridHeight * selectedRow + 5);
            helper.TargetElement.Width = (endColumns - selectedColumn + 1) * gridWidth - 10;
            helper.TargetElement.Height = (endRows - selectedRow + 1) * gridHeight - 10;
            //定位DragHelper
            Canvas.SetLeft(helper, gridWidth * selectedColumn + 5);
            Canvas.SetTop(helper, gridHeight * selectedRow + 5);
            helper.Width = (endColumns - selectedColumn + 1) * gridWidth - 10;
            helper.Height = (endRows - selectedRow + 1) * gridHeight - 10;
            //跨行列数等于头尾相减+1
            selectedRowSpan = endRows - selectedRow + 1;
            selectedColumnSpan = endColumns - selectedColumn + 1;
            //TODO:到时候去掉
            //保存区域方法中目前是根据文本框文本计算值的
            locationY.Text = (selectedRow + 1).ToString();
            locationX.Text = (selectedColumn + 1).ToString();
            columnSpan.Text = selectedRowSpan.ToString();
            rowSpan.Text = selectedColumnSpan.ToString();
        }
        #endregion

        /// <summary>
        /// 创建一个Border
        /// </summary>
        /// <param name="isDragable">是否支持拖动</param>
        /// <param name="tag">Border中内嵌的TextBlock的文字</param>
        /// <returns></returns>
        private Border CreateDragableBorder(bool isDragable, string tag = null)
        {
            Border border = new Border()
            {
                Background = new SolidColorBrush(Colors.LightSeaGreen),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                Tag = ScreenItemIndex,
            };
            if (isDragable)
            {
                DragControlHelper.SetIsEditable(border, true);
                DragControlHelper.SetIsSelectable(border, true);
            }
            //在Border里添加Text Block以标识这是区域几
            if (!string.IsNullOrEmpty(tag))
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = "区域" + tag,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                border.Child = textBlock;
            }
            return border;
        }

        /// <summary>
        /// 按照Rect位置在Canvas中摆放一个Border
        /// </summary>
        /// <param name="border"></param>
        /// <param name="canvas"></param>
        /// <param name="rect"></param>
        private void PlaceBorder(Border border, Canvas canvas, Rect rect)
        {
            Canvas.SetLeft(border, rect.Left);
            Canvas.SetTop(border, rect.Top);
            border.Width = rect.Width;
            border.Height = rect.Height;
            canvas.Children.Add(border);
        }
    }
}
