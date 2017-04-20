using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Screen
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class Service1 : IService1
    {

        /// <summary>
        /// 设置屏幕上有几行几列
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public void SetLayout(int row, int column)
        {
            MainWindow.Instance.SetLayout(row, column);
        }
        /// <summary>
        /// 向屏幕上添加一组信息
        /// </summary>
        /// <param name="items"></param>
        public void AddContentToScreen(List<SingleScreenItemInfo> items)
        {
            MainWindow.Instance.AddContentToScreen(items);
        }
        /// <summary>
        /// 选择窗口
        /// </summary>
        /// <param name="index"></param>
        public void SelectWindow(Guid id)
        {
            MainWindow.Instance.SelectWindow(id);
        }
        /// <summary>
        /// 更新某窗口的信息
        /// </summary>
        /// <param name="index">该窗体的索引</param>
        /// <param name="screenInfo">信息</param>
        public void UpdateWindowInfo(SingleScreenItemInfo screenInfo)
        {
            MainWindow.Instance.UpdateWindow(screenInfo);
        }
        /// <summary>
        /// 获取当前屏幕上的信息
        /// </summary>
        /// <returns></returns>
        public ScreenItemsInfo GetCurrentWindowInfo()
        {
            return MainWindow.Instance.GetCurrentWindowInfoAsync().Result;
        }
    }
}
