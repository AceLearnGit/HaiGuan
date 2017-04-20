using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Screen
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        /*****有用到*****/

        /// <summary>
        /// 设置屏幕上有几行几列
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        [OperationContract]
        void SetLayout(int row, int column);

        /// <summary>
        /// 向大屏幕上增加一组数据
        /// </summary>
        /// <param name="items"></param>
        [OperationContract]
        void AddContentToScreen(List<SingleScreenItemInfo> items);

        /// <summary>
        /// 选择窗口
        /// </summary>
        /// <param name="index"></param>
        [OperationContract]
        void SelectWindow(Guid id);

        /// <summary>
        /// 更新某块屏幕的信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="screenInfo"></param>
        [OperationContract]
        void UpdateWindowInfo(SingleScreenItemInfo screenInfo);

        /// <summary>
        /// 获取当前屏幕上的信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ScreenItemsInfo GetCurrentWindowInfo();

    }
}
