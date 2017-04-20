using DataModel;
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
using UICommon.Controls;

namespace Pad.Controls
{
    /// <summary>
    /// DeviceControl.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceControl : UserControl
    {
        /// <summary>
        /// 本控件代表哪台机器
        /// </summary>
        public string DeviceId;
        /// <summary>
        /// 这台机器被分为几行几列
        /// </summary>
        public int Rows = 1;
        public int Columns = 1;

        /// <summary>
        /// 当前操作的Border的Guid
        /// </summary>
        private Guid ActiveBorderGuid;
        /// <summary>
        /// 已经绘制在界面上的区域
        /// </summary>
        public Dictionary<Guid, SingleScreenItemInfo> BorderDic = new Dictionary<Guid, SingleScreenItemInfo>();
        /// <summary>
        /// 本次点击显示时要发送给大屏幕的数据项信息
        /// 发送完应当清空
        /// </summary>
        private List<SingleScreenItemInfo> ScreenItemsList = new List<SingleScreenItemInfo>();
        /// <summary>
        /// 删除按钮
        /// </summary>
        Button deleteBtn = new Button();
        public DeviceControl()
        {
            InitializeComponent();
            //设置删除按钮的位置等
            deleteBtn.Focusable = false;
            deleteBtn.Width = 30;
            deleteBtn.Height = 30;
            deleteBtn.Content = "";
            deleteBtn.FontFamily = new FontFamily("Segoe MDL2 Assets");
            deleteBtn.Click += DeleteBtn_Click;
            deleteBtn.Visibility = Visibility.Collapsed;
            cv.Children.Add(deleteBtn);
            this.Loaded += DeviceControl_Loaded;
        }

        private void DeviceControl_Loaded(object sender, RoutedEventArgs e)
        {
            DeviceId = Tag.ToString();
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
        /// 鼠标抬起时（矩形绘制完毕）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //如果鼠标抬起时是矩形或者Canvas，代表绘制矩形结束
            Rectangle target = e.OriginalSource as Rectangle;
            Canvas canvas = e.OriginalSource as Canvas;
            if (target == null && canvas == null)
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
                //根据绘制的矩形向Canvas中添加Border
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
            double gridWidth = Board.Width / (Board.ColumnDefinitions.Count == 0 ? 1 : Board.ColumnDefinitions.Count);
            double gridHeight = Board.Height / (Board.RowDefinitions.Count == 0 ? 1 : Board.RowDefinitions.Count);
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
            selectedRowSpan = endRows - selectedRow + 1;
            selectedColumnSpan = endColumns - selectedColumn + 1;
            //定位Border
            //搞一个Border放在指定位置
            Guid guid = Guid.NewGuid();
            Border border = CreateDragableBorder(true, guid, (BorderDic.Count + 1).ToString());
            border.GotFocus += Border_GotFocus;
            border.LostFocus += Border_LostFocus;
            Rect borderRegion = new Rect()
            {
                X = gridWidth * selectedColumn + 5,
                Y = gridHeight * selectedRow + 5,
                Width = selectedColumnSpan * gridWidth - 10,
                Height = selectedRowSpan * gridHeight - 10
            };
            PlaceBorder(border, cv, borderRegion);
            //绘制完成，添加到字典中
            BorderDic.Add(guid, new SingleScreenItemInfo() { Row = selectedRow, RowSpan = selectedRowSpan, Column = selectedColumn, ColumnSpan = selectedColumnSpan, ID = guid });
        }

        /// <summary>
        /// 区域失去焦点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_LostFocus(object sender, RoutedEventArgs e)
        {
            //隐藏删除按钮
            deleteBtn.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 正在拖动时
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void helper_DragChanging(object Sender, DragChangedEventArgs e)
        {
            Rect rect = e.NewBound;
            //定位删除按钮
            double top = rect.Top + 10;
            double left = rect.Left + rect.Width - 50 - 10;
            Canvas.SetLeft(deleteBtn, left);
            Canvas.SetTop(deleteBtn, top);
        }

        /// <summary>
        /// 点击某区域时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Border_GotFocus(object sender, RoutedEventArgs e)
        {
            Border target = sender as Border;
            if (target == null)
                return;
            ActiveBorderGuid = (Guid)target.Tag;
            //通知大屏选中了某个小屏
            await WCFServiceManager.Instance.SelectWindow(DeviceId, ActiveBorderGuid);
            //显示删除按钮
            double top = Canvas.GetTop(target) + 10;
            double left = Canvas.GetLeft(target) + target.ActualWidth - 50 - 10;
            Canvas.SetLeft(deleteBtn, left);
            Canvas.SetTop(deleteBtn, top);
            Panel.SetZIndex(deleteBtn, Panel.GetZIndex(target) + 1);
            deleteBtn.Visibility = Visibility.Visible;
            DeviceSelected?.Invoke(this, DeviceId);
        }

        /// <summary>
        /// DragHelper拖动完成事件
        /// 根据事件参数拿到选定区域位置和大小，从而起始点，跨行数、列数
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void helper_DragCompleted(object Sender, DragChangedEventArgs e)
        {
            Border activeBorder = (Sender as UICommon.Controls.DragControlHelper).TargetElement as Border;
            if (activeBorder == null)
                return;
            //如果未分行列，Board.ColumnDefinitions(RowDefinitions).Count为0，gridWidth计算结果为无穷，会引发异常，故判断下
            double gridWidth = Board.ActualWidth / (Board.ColumnDefinitions.Count == 0 ? 1 : Board.ColumnDefinitions.Count);
            double gridHeight = Board.ActualHeight / (Board.RowDefinitions.Count == 0 ? 1 : Board.RowDefinitions.Count);
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
            //定位Border
            Canvas.SetLeft(helper.TargetElement, gridWidth * selectedColumn + 5);
            Canvas.SetTop(helper.TargetElement, gridHeight * selectedRow + 5);
            helper.TargetElement.Width = (endColumns - selectedColumn + 1) * gridWidth - 10;
            helper.TargetElement.Height = (endRows - selectedRow + 1) * gridHeight - 10;
            //定位DragHelper
            Canvas.SetLeft(helper, gridWidth * selectedColumn + 5);
            Canvas.SetTop(helper, gridHeight * selectedRow + 5);
            helper.Width = (endColumns - selectedColumn + 1) * gridWidth - 10;
            helper.Height = (endRows - selectedRow + 1) * gridHeight - 10;
            //定位删除按钮
            double top = Canvas.GetTop(helper.TargetElement) + 10;
            double left = Canvas.GetLeft(helper.TargetElement) + helper.TargetElement.Width - 50 - 10;
            Canvas.SetLeft(deleteBtn, left);
            Canvas.SetTop(deleteBtn, top);
            //跨行列数等于头尾相减+1
            selectedRowSpan = endRows - selectedRow + 1;
            selectedColumnSpan = endColumns - selectedColumn + 1;

            Guid id = (Guid)activeBorder.Tag;
            //修改字典中对应项的位置信息
            if (id != null)
            {
                if (BorderDic.ContainsKey(id))
                {
                    BorderDic[id].Row = selectedRow;
                    BorderDic[id].RowSpan = selectedRowSpan;
                    BorderDic[id].Column = selectedColumn;
                    BorderDic[id].ColumnSpan = selectedColumnSpan;
                }
            }
        }

        #endregion

        /// <summary>
        /// 创建一个Border
        /// </summary>
        /// <param name="isDragable">是否支持拖动</param>
        /// <param name="tag">Border中内嵌的TextBlock的文字</param>
        /// <returns></returns>
        private Border CreateDragableBorder(bool isDragable, Guid id, string tag = null)
        {
            Border border = new Border()
            {
                Background = FindResource("RegionBackground") as SolidColorBrush,
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2),
                Tag = id,
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
        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Border target = null;
            foreach (UIElement item in cv.Children)
            {
                Border border = item as Border;
                if (border != null)
                {
                    if ((Guid)border.Tag == ActiveBorderGuid)
                    {
                        target = border;
                        break;
                    }
                }
            }
            if (target != null)
            {
                cv.Children.Remove(target);
                if (BorderDic.ContainsKey(ActiveBorderGuid))
                {
                    BorderDic.Remove(ActiveBorderGuid);
                }
                ActiveBorderGuid = default(Guid);
            }
            //cv.Children.Remove(deleteBtn);
            deleteBtn.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 重置各字段、属性
        /// </summary>
        private void Reset()
        {
            BorderDic = new Dictionary<Guid, SingleScreenItemInfo>();
            ActiveBorderGuid = default(Guid);
            //清空Canvas
            List<Border> list = new List<Border>();
            foreach (var item in cv.Children)
            {
                Border border = item as Border;
                if (border != null)
                    list.Add(border);
            }
            for (int i = 0; i < list.Count; i++)
            {
                cv.Children.Remove(list[i]);
            }
        }

        /// <summary>
        /// 划分示意区域并重置各字段
        /// </summary>
        public async Task SetLayout(string deviceId, int rows, int columns)
        {
            this.DeviceId = deviceId;
            this.Rows = rows;
            this.Columns = columns;
            //大屏布局
            await WCFServiceManager.Instance.SetLayout(deviceId, rows, columns);
            DataManager.SetBoardLayout(rows, columns, Board);
            //重新分区时应重置各字段
            Reset();
        }

        /// <summary>
        /// 显示方法，向设备发送信息
        /// </summary>
        public async void Show(bool isWebKitChecked)
        {
            try
            {
                foreach (SingleScreenItemInfo item in BorderDic.Values)
                {
                    item.IsWebKit = isWebKitChecked;
                    ScreenItemsList.Add(item);
                }
                //划分大屏
                await WCFServiceManager.Instance.SetLayout(DeviceId, Rows, Columns);
                //发送到大屏
                await WCFServiceManager.Instance.AddContentToScreen(DeviceId, ScreenItemsList.ToArray());
                ScreenItemsList.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="sourceUrl"></param>
        public void SetSource(string sourceUrl)
        {
            if (ActiveBorderGuid != default(Guid))
            {
                if (BorderDic.ContainsKey(ActiveBorderGuid))
                {
                    BorderDic[ActiveBorderGuid].TargetUrl = sourceUrl;
                }
            }
        }
        /// <summary>
        /// 根据模板添加Border
        /// </summary>
        /// <param name="list"></param>
        public void AddBorder(List<SingleScreenItemInfo> list)
        {
            double gridWidth = Board.Width / (Board.ColumnDefinitions.Count == 0 ? 1 : Board.ColumnDefinitions.Count);
            double gridHeight = Board.Height / (Board.RowDefinitions.Count == 0 ? 1 : Board.RowDefinitions.Count);
            for (int i = 0; i < list.Count; i++)
            {
                //定位Border
                //搞一个Border放在指定位置
                Border border = CreateDragableBorder(true, list[i].ID, (BorderDic.Count + 1).ToString());
                border.GotFocus += Border_GotFocus;
                border.LostFocus += Border_LostFocus;
                Rect borderRegion = new Rect()
                {
                    X = gridWidth * list[i].Column + 5,
                    Y = gridHeight * list[i].Row + 5,
                    Width = list[i].ColumnSpan * gridWidth - 10,
                    Height = list[i].RowSpan * gridHeight - 10
                };
                PlaceBorder(border, cv, borderRegion);
                //绘制完成，添加到字典中
                BorderDic.Add(list[i].ID, list[i]);
            }
        }
        /// <summary>
        /// 点击某区域时向用户控件外层传递消息，告知正在操作哪台机器
        /// </summary>

        public event EventHandler<object> DeviceSelected;
    }
}
