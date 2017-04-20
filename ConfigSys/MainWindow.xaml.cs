using DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConfigSys
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GSDM");
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GSDM", "testConfig.xml");
        SourcesInfo sourcesInfo;
        public MainWindow()
        {
            InitializeComponent();
            //如果不存在就创建文件夹
            if (Directory.Exists(folderName) == false)
            {
                Directory.CreateDirectory(folderName);
            }
            //读取配置文件，包括物理屏幕大小和源数据
            if (File.Exists(fileName))
            {
                //using (FileStream stream = new FileStream(fileName, FileMode.Open))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(SourcesInfo));
                //    sourcesInfo = (SourcesInfo)serializer.Deserialize(stream);
                //}
                sourcesInfo = Common.SerializationHelper.DeserializeXmlFile2Obj<SourcesInfo>(fileName);
                tbScreenWidth.Text = sourcesInfo.Width.ToString();
                tbScreenHeight.Text = sourcesInfo.Height.ToString();
            }
            else
            {
                sourcesInfo = new SourcesInfo();
                sourcesInfo.SourcesItems = new List<SingleSourceInfo>();
            }
        }
        //物理屏水平有几块
        public int ScreenWidth = 0;
        //物理屏竖直有几块
        public int ScreenHeight = 0;

        //大屏配置
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChangeContent("ConfigScreen");
            //ConfigScreen.Visibility = Visibility.Visible;
            //AddData.Visibility = Visibility.Collapsed;           
        }

        //数据录入
        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        {
            ChangeContent("AddData");
            //ConfigScreen.Visibility = Visibility.Collapsed;
            //AddData.Visibility = Visibility.Visible;
        }

        //大屏配置区域保存按钮事件
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(tbScreenWidth.Text, out ScreenWidth);
            int.TryParse(tbScreenHeight.Text, out ScreenHeight);

            sourcesInfo.Width = ScreenWidth;
            sourcesInfo.Height = ScreenHeight;
        }

        //数据录入区域添加按钮事件
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            sourcesInfo.SourcesItems.Add(new SingleSourceInfo() { KeyName = tbKey.Text, SourceUrl = tbSource.Text, TargetImg = targetImg });
        }

        //配好大屏录完数据后点击保存的事件
        private void btnAddCompleted_Click(object sender, RoutedEventArgs e)
        {
            //序列化这个对象  
            //using (FileStream stream = new FileStream(fileName, FileMode.Create))
            //{
            //    XmlSerializer serializer = new XmlSerializer(typeof(SourcesInfo));
            //    serializer.Serialize(stream, sourcesInfo);
            //}
            Common.SerializationHelper.SerializerObj2XmlFile(sourcesInfo,fileName);
        }

        /// <summary>
        /// 根据Grid名字控制Visibility
        /// </summary>
        /// <param name="gridName"></param>
        private void ChangeContent(string gridName)
        {
            foreach (var child in ContentGrid.Children)
            {
                Grid grid = child as Grid;
                if (grid != null)
                {
                    if (grid.Name == gridName)
                    {
                        grid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        string targetImg = null;
        //上传缩略图
        private void btn_selectImg_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "picture files (*.png/*.jpg)|*.jpg;*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事 ...=openFileDialog1.FileName; 
                string fileName = openFileDialog1.FileName;
                img_source.Source = new BitmapImage(new Uri(fileName));
                File.Copy(fileName, Path.Combine(folderName, Path.GetFileName(fileName)), true);
                targetImg = Path.Combine(folderName, Path.GetFileName(fileName));
            }
        }

    }
}
