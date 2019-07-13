using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;//要用到DllImport
using System.IO;
using System.Threading;//线程
using System.Net.Sockets;//通信
using System.Net;//通信
using Models;
using System.Collections;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.UI;
using System.Data.SqlClient;
namespace JieMei
{
    public partial class Mainform : Form
    {
        #region 外部函数-相机：初始化 获取图像 关闭相机
        [DllImport("tuyang.dll", EntryPoint = "camera_init", CallingConvention = CallingConvention.Cdecl)]
        public extern static int camera_init(bool isTrig, int power, int gain);//初始化camera

        //读取( mycolor, myp3d)值
        [DllImport("tuyang.dll", EntryPoint = "cameraFun", CallingConvention = CallingConvention.Cdecl)]
        public extern static void cameraFun([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  byte[] color_temp, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]  float[] p3d_temp, int num);

        //软触发取图像--softTrigg:   前提是: camera_init(true)
        [DllImport("tuyang.dll", EntryPoint = "softTrigg", CallingConvention = CallingConvention.Cdecl)]
        public extern static void softTrigg(int num);

        [DllImport("tuyang.dll", EntryPoint = "close_camera", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool close_camera();//停止Camera
        #endregion

        #region 变量声明
        bool isListen = false;
        public static string mystring = "";
        string myIP = "192.168.1.123";// 自己的IP,客户端查找此IP;
        int myPORT = 8080; //自己的端口,客户端查找此PORT
        double toD = 180d / Math.PI;
        double toR = Math.PI / 180.0;
        Socket tcpServer;//服务器
        Client myclient;//机器人客户端
        private Thread My_Thread;//监听
        private Thread Robot_Thread;//机器人运行
        private Thread checkZone_Thread;//查看托盘
        public static int runMode =0;//运行模式
        public static bool runStatus = false;//运行是否完成
        public static bool boxFall = false;//箱子掉落
        public static bool boxFallDown = false;//箱子掉落地上
        delegate void boxFallCallback(bool value); //声名委托:
        string myQRCInfo = "";
        string myQRCInfoLast = "";
        int NG_Type = 0;//不良品类型
        int my_start = 0;//启停
        public static int CarryMode = 0;//运送模式
        bool zoneFull = false;//托盘满了
        public static bool ProduceArrive = false;
        int my_checkZone = 0;//是否停止检测托盘
        int zoneTemp = 0;//6个区的编号
        BOX box1 = new BOX(540, 540, 108);//箱子尺寸
        ProductInfo qrcProduce = new ProductInfo();//识别到的产品
        string globalBarcode = "";//识别到的条码
        public static P6D cameraPos = new P6D(1024, -717, 1100, 0, 180, 35);//拍照位置
        public static P6D qrcPos = new P6D(970, -1000, 725, 137, 180, 141);//二维码识别位置
        public static P6D qrcNGPos = new P6D(1980, 100, -400, 0, 180, -30);//不良区
        P6D putPos = new P6D();//放置位置        
        P6D passTempPos1 = new P6D();//跨越位置
        P6D passTempPos2 = new P6D();
        P6D passTempPos3 = new P6D();
        P6D passTempPos4 = new P6D();
        P6D[] zoneCenter = new P6D[6];//托盘中心*注意*zoneCenter[0]不使用
        //P6D zone1Center = new P6D(0, -1680, -420, 0, 180, 90);//托盘1中心
        //P6D zone2Center = new P6D(-1455, -840, -420, 0, 180, 150);//托盘2中心
        //P6D zone3Center = new P6D(-1455, 840, -420, 0, 180, -150);//托盘3中心
        //P6D zone4Center = new P6D(0, 1680, -420, 0, 180, -90);//托盘4中心
        //P6D zone5Center = new P6D(1455, 840, -420, 0, 180, -30);//托盘5中心
        P6D[] zoneCheckCenter = new P6D[6]{//检测托盘中心*注意*zoneCheckCenter[0]不使用
            new P6D(),
            new P6D(0, -1250, -430, 0, 180, 90),
            new P6D(-1083, -625, -420, 0, 180, 150),
            new P6D(-1083, 625, -420, 0, 180, -150),
            new P6D(0, 1250, -390, 0, 180, -90),
            new P6D(1083, 625, -420, 0, 180, -30),           
        };
        private ProductMethod productMethod = new ProductMethod();//实例化产品操作的类
        private FileOperation fileOperation = new FileOperation();

        private ShowImage showImage = new ShowImage();
        private ScanerHook listener = new ScanerHook();//二维码钩子
        #endregion

        #region 窗口事件
        /// <summary>
        /// 窗体入口
        /// </summary>
        public Mainform()
        {
            InitializeComponent();
            camera_init(false,100,100);//初始化camera
            setfrom(this.Controls);
            Thread.Sleep(200);
            Img_modeBox.SelectedIndex = 0;//初始化显示采样
            listener.ScanerEvent += Listener_ScanerEvent; //二维码监听
            timer1.Interval = 100;
            timer1.Start();//启动定时器1            
        }
        private void setfrom(Control.ControlCollection IControls)
        {
            foreach (Control item in IControls)
            {
                item.Top = (int)(item.Top * 0.9);
                item.Left = (int)(item.Left * 0.9);
                item.Width = (int)(item.Width * 0.9);
                item.Height = (int)(item.Height * 0.9);
                if (item.HasChildren)
                {
                    setfrom(item.Controls);
                }
            }
        }
        private void Mainform_Load(object sender, EventArgs e)
        {
            //【1】禁止跨线程访问检测
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            //【2】背景
            btnWorking.BackColor = Color.Green;
            this.tabControl1.SelectedIndex = 2;
            btnModeChange.BackColor = Color.LawnGreen;
            btnModeChange.Enabled = false;//运送模式
            btnClearZone0.Enabled = false;
            btnClearZone1.Enabled = false;
            btnClearZone2.Enabled = false;
            btnClearZone3.Enabled = false;
            btnClearZone4.Enabled = false;
            btnClearZone5.Enabled = false;
            //【3】产品设置更新到界面
            GLB.objListProduct.Clear();
            productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
            
            #region // 【4】检测托盘满没
            for (int i = 0; i < 6; i++)
            {
                if (SearchZoneFullStatus(i))//对应垛满了
                {
                    zoneFull = true;
                    //清空托盘按键使能
                    switch (i)
                    {
                        case 0:
                            btnClearZone0.Enabled = true;
                            break;
                        case 1:
                            btnClearZone1.Enabled = true;
                            break;
                        case 2:
                            btnClearZone2.Enabled = true;
                            break;
                        case 3:
                            btnClearZone3.Enabled = true;
                            break;
                        case 4:
                            btnClearZone4.Enabled = true;
                            break;
                        case 5:
                            btnClearZone5.Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion

            //【5】初始化每垛的数量
            for (int i = 0; i < 6; i++)
            {
                ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
            }
            //【6】初始化每个托盘的中心位置
            string[] line = fileOperation.ReadTxtFile("zoneCenterPos.txt");
            for (int i = 0; i < line.Length; i++)
            {
                string[] split = line[i].Split(',');
                zoneCenter[i + 1] = new P6D(double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]), double.Parse(split[4]), double.Parse(split[5]), double.Parse(split[6]));
                //***注意***zoneCenter[0]不使用
            }
            //【7】设置机器人状态            
            SetRobotStatus(2, "等待中");
            //二维码监听
            listener.Start();
            //【8】读取5个托盘识别状态
            for (int i = 0; i <6; i++)
            {
                if (SearchZoneCheckStatus(i))//对应垛成功识别
                {
                    GLB.zoneCheckStatus[i] = true;
                }
            }  
        }

        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (my_start == 1)//机器人运行中
            {
                DialogResult result = MessageBox.Show("机器人正在运行，确定关闭？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Robot_Thread.Abort();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            listener.Stop(); //二维码监听
            SetRobotStatus(0, "机器人停止");
            timer1.Stop();
            close_camera();//停止Camera  
            if (isListen == true)//停止tcp服务
            {
                My_Thread.Abort();
                myclient.close();
                tcpServer.Close();
            }
        }

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            byte[] mycolor = new byte[1280 * 960 * 3];//彩色一维数数;
            float[] myp3d = new float[1280 * 960 * 3];//真3D数据            

            //查询运送是否来了
            if (my_start == 1 && runMode == 1 && ProduceArrive == false)
            {
                if (CarryMode == 1)//手动运送
                {
                    ProduceArrive = true;  //默认产品送到
                }
                else//小车运送
                {
                    string sql = "select * from StowRobotStatus where carryArrive=1 and StowNo ='" + GLB.RobotId + "'";
                    DataTable dt = MyDataLib.SearchItem(sql);
                    if (dt.Rows.Count > 0)
                    {
                        ProduceArrive = true;//产品到达
                        SetRobotStatus(1, "小车运送到达");
                    }
                }
            }
            //if (runMode == 1 || runMode == 6)
            {
                //softTrigg(0);//触发
                //List<float[]> myp3d_list=new List<float[]>();//五次采样
                //for (int i = 0; i < 5; i++)
                //{
                //    cameraFun(mycolor, myp3d, 0);//启动Camera,整数表示相机编号 
                //    myp3d_list.Add(myp3d);
                //    Thread.Sleep(20);
                //}
                //for (int i = 0; i < GLB.BUFH; i++)
                //{
                //    for (int j = 0; j < GLB.BUFW; j++)
                //    {
                //        myp3d[(i * GLB.BUFW + j) * 3 + 2] = myp3d_list.Average(o => o[(i * GLB.BUFW + j) * 3 + 2]);
                //        myp3d[(i * GLB.BUFW + j) * 3 + 1] = myp3d_list.Average(o => o[(i * GLB.BUFW + j) * 3 + 1]);
                //        myp3d[(i * GLB.BUFW + j) * 3 + 0] = myp3d_list.Average(o => o[(i * GLB.BUFW + j) * 3 + 0]);
                //    }
                //}
                cameraFun(mycolor, myp3d, 0);//启动Camera,整数表示相机编号  
              
                if (GLB.img_mode == 0) showImage.DisplayDepthImg3(myp3d, ptbDisplay, txtTypeName); //匹配模式
                if (GLB.img_mode == 1) showImage.DisplayDepthImg2(myp3d, ptbDisplay, txtTypeName);    //添加工件 单对象识别
                if (GLB.img_mode == 2) showImage.DisplayDepthImg(myp3d, ptbDisplay);     //深度图像显示
                if (GLB.img_mode == 3) showImage.DisplayColorImg(mycolor, ptbDisplay);   //彩色图像显示

                if (GLB.img_mode == 4)
                {
                    for (int i = 0; i < GLB.BUFH; i++)
                    {
                        for (int j = 0; j < GLB.BUFW; j++)
                        {
                            GLB.myp3d[(i * GLB.BUFW + j) * 3 + 2] = myp3d[(i * GLB.BUFW + j) * 3 + 2];//局部变量--》3D全局变量
                            GLB.myp3d[(i * GLB.BUFW + j) * 3 + 1] = myp3d[(i * GLB.BUFW + j) * 3 + 1];
                            GLB.myp3d[(i * GLB.BUFW + j) * 3 + 0] = myp3d[(i * GLB.BUFW + j) * 3 + 0];
                            GLB.mycolor[(i * GLB.BUFW + j) * 3 + 2] = mycolor[(i * GLB.BUFW + j) * 3 + 2];
                            GLB.mycolor[(i * GLB.BUFW + j) * 3 + 1] = mycolor[(i * GLB.BUFW + j) * 3 + 1];
                            GLB.mycolor[(i * GLB.BUFW + j) * 3 + 0] = mycolor[(i * GLB.BUFW + j) * 3 + 0];
                        }
                    }
                }
            }
            this.Text = GLB.TitleStr;
            timer1.Enabled = true;
        }



        #endregion

        #region 按键操作
        /// <summary>
        /// 图像模式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_modeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GLB.img_mode = Img_modeBox.SelectedIndex;
        }

        /// <summary>
        /// 采样距离选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            GLB.Sample_Distance = trackBar1.Value;
        }

        /// <summary>
        /// 采样并保存样品数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSampling_Click(object sender, EventArgs e)
        {
            string sample_info = "";
            if (GLB.img_mode == 1)//必须在样品识别模式下
            {
                if (GLB.obj.Count == 1)//只有一个样品存在
                {
                    if (GLB.obj[0].typName.Length == 0 && txtTypeName.Text == "")
                    {
                        MessageBox.Show("请输入工件名称！");
                        return;
                    }
                    else if (GLB.obj[0].typName.Length == 0)
                    {
                        MessageBox.Show("没有发现样品");
                    }
                    else
                    {
                        sample_info += GLB.obj[0].typName;//工件分类与名称
                        //识别特征:
                        sample_info += " " + string.Format("{0:#.##}", GLB.obj[0].axisLong);//长轴
                        sample_info += " " + string.Format("{0:#.##}", GLB.obj[0].axisShort);//短轴
                        sample_info += " " + string.Format("{0:#.##}", GLB.obj[0].L2S);//长轴比短轴
                        sample_info += " " + string.Format("{0:#.##}", GLB.obj[0].SZ);//面积
                        sample_info += "\r\n";
                        fileOperation.WriteTxtFile("obj_info.txt", sample_info);
                        txtSampleResult.Text = "工件：" + GLB.obj[0].typName + "\r\n长：" + string.Format("{0:#.##}", GLB.obj[0].axisLong) + "\r\n宽：" + string.Format("{0:#.##}", GLB.obj[0].axisShort)
                            + "\r\n长vs短: " + string.Format("{0:#.##}", GLB.obj[0].L2S) + "\r\n面积：" + string.Format("{0:#.##}", GLB.obj[0].SZ);
                    }
                }
                else if (GLB.obj.Count > 1)
                {
                    MessageBox.Show("请只留下一件样品");
                }
            }
            else
            {
                MessageBox.Show("请选择图像模式为：样品学习！");
            }
        }

        /// <summary>
        /// 启动3d
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3DForm_Click(object sender, EventArgs e)
        {
            GLB.img_mode = 4;
            MessageBox.Show("打开3D窗口");
            Thread.Sleep(20);
            Ax3D f3d = new Ax3D();
            f3d.Show();
            GLB.img_mode = 0;
        }

        /// <summary>
        /// //接收机器人服务器的连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartServers_Click(object sender, EventArgs e)
        {
            Socket clientsocket;
            btnStartServers.Enabled = false;
            try
            {
                tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpServer.Bind(new IPEndPoint(IPAddress.Parse(myIP), myPORT));//端口绑定
                txtTCPInfo.Text = "服务器已经启动,IP=" + myIP + ", PORT=" + myPORT + "\r\n客户端连接中......\r\n";
                tcpServer.Listen(100);
                clientsocket = tcpServer.Accept();//进行客户端连接

                myclient = new Client(clientsocket);//把每个客户端放client类处理  
                isListen = true;
                My_Thread = new Thread(My_Process);
                My_Thread.Start();
            }
            catch (Exception ex)
            {
                isListen = false;
                btnStartServers.Enabled = true;
                MessageBox.Show("服务器启动失败！" + ex.Message);
            }
        }

        /// <summary>
        /// //初始位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitPos_Click(object sender, EventArgs e)
        {
            if (isListen == true)//连上服务器
            {
                //初始拍照位置
                runToPos(0, 2, 0, (int)cameraPos.x, (int)cameraPos.y, (int)cameraPos.z, (int)cameraPos.a, (int)cameraPos.b, (int)cameraPos.c, false);
                //runToPos(0, 2, 0, (int)qrcPos.x, (int)qrcPos.y, (int)qrcPos.z, (int)qrcPos.a, (int)qrcPos.b, (int)qrcPos.c, false);
            }
        }
        private void zone1Btn_Click(object sender, EventArgs e)//识别托盘1位置
        {

            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (my_start == 1)
            {
                MessageBox.Show("请先停止机器人运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                btnInitPos.Enabled = false;//禁用
                btnWorking.Enabled = false;
                btnClearAlarm.Enabled = false;//不能再清除报警
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                my_checkZone = 1;
                runMode = 5;
                zoneTemp = 1;
                checkZone_Thread = new Thread(checkZone);
                checkZone_Thread.Start();
            }
        }

        private void zone2Btn_Click(object sender, EventArgs e)//识别托盘2位置
        {

            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (my_start == 1)
            {
                MessageBox.Show("请先停止机器人运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                btnInitPos.Enabled = false;//禁用
                btnClearAlarm.Enabled = false;//不能再清除报警
                btnWorking.Enabled = false;
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                my_checkZone = 1;
                runMode = 5;
                zoneTemp = 2;
                checkZone_Thread = new Thread(checkZone);
                checkZone_Thread.Start();

            }
        }

        private void zone3Btn_Click(object sender, EventArgs e)//识别托盘3位置
        {
            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (my_start == 1)
            {
                MessageBox.Show("请先停止机器人运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                btnInitPos.Enabled = false;//禁用
                btnWorking.Enabled = false;
                btnClearAlarm.Enabled = false;//不能再清除报警
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                my_checkZone = 1;
                runMode = 5;
                zoneTemp = 3;
                checkZone_Thread = new Thread(checkZone);
                checkZone_Thread.Start();
            }
        }

        private void zone4Btn_Click(object sender, EventArgs e)//识别托盘4位置
        {
            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (my_start == 1)
            {
                MessageBox.Show("请先停止机器人运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                btnInitPos.Enabled = false;//禁用
                btnWorking.Enabled = false;
                btnClearAlarm.Enabled = false;//不能再清除报警
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                my_checkZone = 1;
                runMode = 5;
                zoneTemp = 4;
                checkZone_Thread = new Thread(checkZone);
                checkZone_Thread.Start();
            }
        }

        private void zone5Btn_Click(object sender, EventArgs e)//识别托盘5位置
        {
            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (my_start == 1)
            {
                MessageBox.Show("请先停止机器人运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                btnInitPos.Enabled = false;//禁用
                btnWorking.Enabled = false;
                btnClearAlarm.Enabled = false;//不能再清除报警
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                my_checkZone = 1;
                runMode = 5;
                zoneTemp = 5;
                checkZone_Thread = new Thread(checkZone);
                checkZone_Thread.Start();
            }
        }
        private void btnClearAlarm_Click(object sender, EventArgs e)//清除报警
        {
            if (isListen == false)
            {
                MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            runToPos(1, 0, 0, 0, 0, 0, 0, 180, 0, false);//报警
        }

        /// <summary>
        ///机器=== 启动/停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorking_Click(object sender, EventArgs e)
        {
            if (my_start == 0)
            {
                if (isListen == false)
                {
                    MessageBox.Show("请先启动服务器！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (boxFallDown == true)//箱子掉地上
                {
                    MessageBox.Show("箱子掉地上，请将机器人手动移回原点，重新启动");
                    return;
                }
                my_start = 1;

                this.tabControl1.SelectedIndex = 2;//信息显示
                SetRobotStatus(1, "工作中");
                btnWorking.Text = "停止";
                btnInitPos.Enabled = false;//不能再点击初始位置
                btnClearAlarm.Enabled = false;//不能再清除报警
                zone1Btn.Enabled = false;
                zone2Btn.Enabled = false;
                zone3Btn.Enabled = false;
                zone4Btn.Enabled = false;
                zone5Btn.Enabled = false;
                QrcCheckBtn.Enabled = false;
                btnModeChange.Enabled = true;
                carArriveStatusBtn.Enabled = false;
                btnWorking.BackColor = Color.Red;
                Robot_Thread = new Thread(robot_run);
                Robot_Thread.Start();
                txtQRC.Focus();
            }
            else
            {
                Robot_Thread.Abort();//终止线程
                //亮红灯
                if (boxFallDown == false || runMode != 3 || runMode != 4)//箱子掉地上
                {
                    runToPos(1, 2, 0, (int)cameraPos.x, (int)cameraPos.y, (int)cameraPos.z, (int)cameraPos.a, (int)cameraPos.b, (int)cameraPos.c, false);
                }
                btnWorking.Text = "启动";
                btnWorking.BackColor = Color.Green;
                my_start = 0;
                runMode = 0;
                //设置机器人状态
                SetRobotStatus(2, "等待中");
                btnInitPos.Enabled = true;
                btnClearAlarm.Enabled = true;
                zone1Btn.Enabled = true;
                zone2Btn.Enabled = true;
                zone3Btn.Enabled = true;
                QrcCheckBtn.Enabled = true;
                zone4Btn.Enabled = true;
                zone5Btn.Enabled = true;
                btnModeChange.Enabled = false;
                carArriveStatusBtn.Enabled = true;
            }
        }

        private void btnProSet_Click(object sender, EventArgs e)//产品设置
        {
            if (my_start == 1)
            {
                MessageBox.Show("请先停止运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (runMode == 0)
            {
                using (ProductSetForm f = new ProductSetForm())
                {
                    f.ShowDialog();
                }
            }
        }

        private void btnModeChange_Click(object sender, EventArgs e)//运送模式切换
        {
            if (CarryMode == 0)
            {
                MessageBox.Show("手动运送?");
                CarryMode = 1;
                btnModeChange.Text = "手动运送";
                ProduceArrive = true;
                btnModeChange.BackColor = Color.Yellow;
            }
            else
            {
                MessageBox.Show("小车运送?");
                btnModeChange.Text = "小车运送";
                CarryMode = 0;
                ProduceArrive = false;
                btnModeChange.BackColor = Color.LawnGreen;
                SetRobotStatus(2, "等待小车运送"); //设置机器人状态
            }
        }

        private void btnClearZone0_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空不良区托盘数量：确保清空托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone0.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(0, 0);//托盘状态
                //string sql = "update  dbo.StowMissionn_list set Fupqty =0,fTuoQty=0  where StowNo =1 and area =0";
                //MyDataLib.ExecNoneQueryBySql(sql);//托盘数量清零
                cleanZoneCurrentNum(0, int.Parse(GLB.RobotId));//托盘数量
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
                //删除不良品记录
                string sql = "delete from StowNgStatus ";
                MyDataLib.ExecNoneQueryBySql(sql);
            }
            else
            {
                return;
            }
        }

        private void btnClearZone1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空托盘1数量：确定移除并更换托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone1.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(1, 0);//托盘状态
                cleanZoneCurrentNum(1, int.Parse(GLB.RobotId));//托盘数量
                //【*】修改对应托盘识别状态
                SetZoneCheckStatus(1, 0);
                GLB.zoneCheckStatus[1] = false;
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
            }
            else
            {
                return;
            }
        }

        private void btnClearZone2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空托盘2数量：确定移除并更换托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone2.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(2, 0);//托盘状态
                cleanZoneCurrentNum(2, int.Parse(GLB.RobotId));//托盘数量
                //【*】修改对应托盘识别状态
                SetZoneCheckStatus(2, 0);
                GLB.zoneCheckStatus[2] = false;
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
            }
            else
            {
                return;
            }
        }

        private void btnClearZone3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空托盘3数量：确定移除并更换托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone3.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(3, 0);//托盘状态
                cleanZoneCurrentNum(3, int.Parse(GLB.RobotId));//托盘数量
                //【*】修改对应托盘识别状态
                SetZoneCheckStatus(3, 0);
                GLB.zoneCheckStatus[3] = false;
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
            }
            else
            {
                return;
            }
        }

        private void btnClearZone4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空托盘4数量：确定移除并更换托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone4.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(4, 0);//托盘状态
                cleanZoneCurrentNum(4, int.Parse(GLB.RobotId));//托盘数量
                //【*】修改对应托盘识别状态
                SetZoneCheckStatus(4, 0);
                GLB.zoneCheckStatus[4] = false;
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
            }
            else
            {
                return;
            }
        }

        private void btnClearZone5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("清空托盘5数量：确定移除并更换托盘？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                btnClearZone5.Enabled = false;
                zoneFull = false;
                SetZoneFullStatus(5, 0);//托盘状态
                cleanZoneCurrentNum(5, int.Parse(GLB.RobotId));//托盘数量
                //【*】修改对应托盘识别状态
                SetZoneCheckStatus(5, 0);
                GLB.zoneCheckStatus[5] = false;
                //产品设置更新到界面
                GLB.objListProduct.Clear();
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //初始化每垛的数量
                for (int i = 0; i < 6; i++)
                {
                    ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                    GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
                }
            }
            else
            {
                return;
            }
        }

        private void carArriveStatusBtn_Click(object sender, EventArgs e)//初始化小车到达状态为：没到达，让空车离开
        {
            ProduceArrive = false;
            SetCarryArrive(0);
            ArrayList array = new ArrayList();//多条SQL语句数组
            string sql = "update Agv_list set isworking =0,stowerid ='',pronum =0 where agvid in(select agvid from Agvmission_list where fstatus =7 and messionType =1 and stowerid='" + GLB.RobotId + "')";//修改小车状态
            string sql1 = "update Agvmission_list set fstatus =6,actionenddate=getdate() where fstatus =7 and messionType =1 and stowerid='" + GLB.RobotId + "'";//修改任务 等待状态为完成状态                        
            array.Add(sql);
            array.Add(sql1);
            bool isok = MyDataLib.transactionOp_list(array);
        }

        private void QrcCheckBtn_Click(object sender, EventArgs e)//测试识别二维码
        {
            if (my_start == 1)
            {
                MessageBox.Show("请先停止运行！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            myQRCInfo = txtQRC.Text;
            qrcCheckFun();
        }
        #endregion

        #region 自定义方法
        void boxFallEvent(bool value)
        {
            boxFall = false;
            if (runMode == 1 || runMode == 2)
            {
                Thread.Sleep(1000);
                btnWorking_Click(null, null);//停止线程
                Thread.Sleep(2000);
                runToPos(3, 0, 0, 0, 0, 0, 0, 180, 0, false);//报警 
                Thread.Sleep(2000);
                btnWorking_Click(null, null);//启动线程
            }
            if (runMode == 3 || runMode == 4)
            {
                Thread.Sleep(1000);
                boxFallDown = true;
                btnWorking_Click(null, null);//停止线程
                Thread.Sleep(1000);
                runToPos(3, 0, 0, 0, 0, 0, 0, 180, 0, false);//报警
                SetRobotStatus(3, "物品掉落");
                MessageBox.Show("箱子掉地上，请将机器人程序重新启动");
            }

        }
        /// <summary>
        ///  接收处理[进程方法]:
        /// </summary>
        static Mutex m = new Mutex();
        private void My_Process()//监听
        {
            string message = "";
            while (isListen)
            {
                //在接收前判断socket是否断开
                if (myclient.connected == false)
                {
                    myclient.close();
                    tcpServer.Close();
                    isListen = false;
                    break;
                }
                try
                {
                    message = "";
                    message = myclient.Receivemessage(message);//接收消息                   
                    if (message != "")
                        txtTCPInfo.Text += "[收到:]" + message;//显示消息  
                    if (message.Substring(0, 8) == "box fall")//箱子掉落
                    {
                        boxFall = true;
                        this.Invoke(new boxFallCallback(boxFallEvent), new object[] { boxFall });//用委托调用外部函数
                    }
                    else if (message.Substring(0, 8) == "run over")
                    {
                        m.WaitOne();
                        runStatus = true;//运行完成
                        m.ReleaseMutex();
                    }
                    if (txtTCPInfo.Lines.Length > 12) { txtTCPInfo.Text = ""; }//超过20行
                }
                catch (Exception)
                {
                    isListen = false;
                    myclient.close();
                    tcpServer.Close();
                }
            }
        }

        /// <summary>
        /// 机器人运行到对应点
        /// </summary>
        /// <param name="bit3">1：显示红灯，2：显示绿灯，3：蜂鸣+红灯</param>
        /// <param name="bit2">0不运行 1机器人运行低速30% 2中速40%  3高速60%  4ptp 60%  4ptp 80%</param>
        /// <param name="bit1">1电磁阀通电  2电磁阀断电</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        void runToPos(int bit3, int bit2, int bit1, int x, int y, int z, float A, float B, float C, bool needWait)
        {
            string sendMsg = "";

            runStatus = false;
            sendMsg = "Image\r\n";
            sendMsg += "[STR:" + bit3 + bit2 + bit1 + ";X:" + x + ";Y:" + y + ";Z:" + z + ";A:" + A + ";B:" + B + ";C:" + C + ";]\r\n";// 工件坐标 
            sendMsg += "Done\r\n";
            myclient.sendmessage(sendMsg);
            while (runStatus == false && needWait)
            {
                if (runStatus == true) break;
                Thread.Sleep(10);
            }
            if (needWait==false)Thread.Sleep(500);            
        }
        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes) //二维码监听
        {
            myQRCInfo = codes.Result;//二维码结果
            Thread.Sleep(50);
            txtQRC.BeginInvoke(new Action(() =>
            {
                txtQRC.Text = codes.Result;//清空二维码缓存
            }));
        }
        /// <summary>
        /// 二维码识别
        /// </summary>
        /// <returns>返回识别成功与否</returns>
        bool qrcCheckFun()
        {
            bool qrcCheck = false;//是否检测成功
            int checkTimes = 0;
            Thread.Sleep(200);
            while (myQRCInfo == myQRCInfoLast)//等待识别到和上次不一样
            {
                checkTimes++;
                if (checkTimes > 50) break;
                Thread.Sleep(10);
            }
            if (myQRCInfo.Length >= 13)//太短不对比
            {
                myQRCInfoLast = myQRCInfo;
                //txtTCPInfo.Text += qrc + "\r\n";//显示细节
                globalBarcode = myQRCInfo;//存储识别到的条码
                string sql = "exec getStowInfoByBarCode  @barCode='" + myQRCInfo + "', @stowId='" + GLB.RobotId + "'";//通过机器人ID查找
                using (SqlConnection conn = new SqlConnection(MyDataLib.ConnString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())//判断有数据
                    {
                        txtMissionID.Text = reader["businessNo"].ToString();//订单号  
                        txtCustomer.Text = reader["customer"].ToString();//客户
                        txtMaterielCode.Text = reader["fnumber"].ToString();//物料代码
                        txtProZone.Text = reader["area"].ToString();//垛区
                        for (int j = 0; j < GLB.objListProduct.Count; j++)
                        {
                            if (!string.IsNullOrEmpty(txtProZone.Text) && int.Parse(txtProZone.Text)!= 0 && GLB.objListProduct[j].Zone == int.Parse(txtProZone.Text))//查找是否为设定的订单
                            {
                                qrcProduce = productMethod.GetProductByZone(int.Parse(txtProZone.Text), GLB.objListProduct);
                                qrcProduce.PerZoneNumbers = int.Parse(reader["mtsl"].ToString());//每托数量
                                qrcProduce.Direction = int.Parse(reader["direction"].ToString()); //方向    
                                txtCurrentCounts.Text = qrcProduce.CurrentCounts.ToString();//当前数量   
                               
                                if (txtTCPInfo.Lines.Length > 12) { txtTCPInfo.Text = "" + myQRCInfo + "检测成功"; } 
                                 else txtTCPInfo.Text += "\r\n" + myQRCInfo + "检测成功";//显示细节
                                NG_Type = 0;
                                qrcCheck = true;
                                break;
                            }
                          
                            else if (j == GLB.objListProduct.Count - 1)//最后一个也不是设定的任务条码
                            {                               
                                if (txtTCPInfo.Lines.Length > 12) { txtTCPInfo.Text = "" + myQRCInfo + "检测不在设定任务"; } 
                                else txtTCPInfo.Text += "\r\n" + myQRCInfo + "检测不在设定任务";//显示细节
                                qrcCheck = false;
                                NG_Type = 1;
                                break;
                            }
                        }
                    }
                    else
                    {                        
                        if (txtTCPInfo.Lines.Length > 12) { txtTCPInfo.Text = "" + myQRCInfo + "检测不到"; }
                        else txtTCPInfo.Text += "\r\n" + myQRCInfo + "检测不到";//显示细节
                        qrcCheck = false;//返回
                        NG_Type = 2;
                    }
                }
            }
            else
            {
                NG_Type = 2;
                qrcCheck = false;
            }
            return qrcCheck;
        }

        /// <summary>
        /// 跨越
        /// </summary>
        /// <param name="basePos">基础位置</param>
        /// <param name="upPos">跨越位置</param>
        /// <param name="box_num">箱子数量</param>
        P6D passUp(P6D basePos, P6D upPos, int box_num, int height)
        {
            upPos = basePos.copyto();
            upPos.z = -420 + height * ((box_num - 1) / 4 + 2);//最下一层基面-420
            if (upPos.z < 700)//不高于700 按700算
            {
                upPos.z = 720;
            }
            runToPos(0, 3, 0, (int)upPos.x, (int)upPos.y, (int)upPos.z, (int)upPos.a, (int)upPos.b, (int)upPos.c, true);
            return upPos;
        }
        /// <summary>
        ///  ptp跨越
        /// </summary>
        /// <param name="basePos">基础位置</param>
        /// <param name="upPos">跨越位置</param>
        /// <param name="box_num">箱子数量</param>
        P6D passUpByPtp(P6D basePos, P6D upPos, int box_num, int height)
        {
            upPos = basePos.copyto();
            upPos.z = -420 + height * ((box_num - 1) / 4 + 2);//最下一层基面-420
            if (upPos.z < 700)//不高于700 按700算
            {
                upPos.z = 720;
            }
            runToPos(0, 4, 0, (int)upPos.x, (int)upPos.y, (int)upPos.z, (int)upPos.a, (int)upPos.b, (int)upPos.c, true);
            return upPos;
        }
        /// <summary>
        /// 到对应垛区放置箱子
        /// </summary>
        /// <param name="whereZone">对应垛</param>
        /// <param name="box_num">当前数量</param>
        /// <param name="boxSize">箱子尺寸</param>    
        void putDowmBox(P6D whereZone, int box_num, BOX boxSize)
        {
            putPos = whereZone.copyto();
            putPos.z = whereZone.z + boxSize.H * ((box_num - 1) / 4 + 1);
            int diff_temp = 22;

            //1111111111111111111111111先到中间 向外移动
            if (box_num % 4 == 1)//位置1
            {
                //putPos.x = whereZone.x + (boxSize.L / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0) * Math.Sin(whereZone.c * toR);
                //putPos.y = whereZone.y + (boxSize.W / 2.0) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                putPos.x = whereZone.x + (boxSize.L / 2.0) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y + (boxSize.W / 2.0) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0) * Math.Sin(whereZone.c * toR);

                if (box_num == 49 || box_num == 53)
                {
                    putPos.x = whereZone.x + (boxSize.W / 2.0) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y + (boxSize.W / 2.0) * Math.Cos(whereZone.c * toR);
                }
            }
            else if (box_num % 4 == 2)//位置2
            {
                putPos.x = whereZone.x + (boxSize.L / 2.0) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y - (boxSize.W / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0) * Math.Sin(whereZone.c * toR);
                if (box_num == 50 || box_num == 54)
                {
                    putPos.x = whereZone.x - (boxSize.W / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y - (boxSize.W / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR);
                }
            }

            else if (box_num % 4 == 3)//位置3
            {
                putPos.x = whereZone.x - (boxSize.L / 2.0) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y + (boxSize.W / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0) * Math.Sin(whereZone.c * toR);
                if (box_num == 55)//第十四层靠里
                {
                    putPos.x = whereZone.x - (boxSize.L / 2.0 + 80) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y + (boxSize.W / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 + 80) * Math.Sin(whereZone.c * toR);

                }
            }
            else if (box_num % 4 == 0)//位置4
            {
                putPos.x = whereZone.x - (boxSize.L / 2.0 + diff_temp * 2) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y - (boxSize.W / 2.0) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 + diff_temp * 2) * Math.Sin(whereZone.c * toR);
                if (box_num == 56)//第十四层靠里
                {
                    putPos.x = whereZone.x - (boxSize.L / 2.0 + diff_temp * 2 + 80) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y - (boxSize.W / 2.0) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 + diff_temp * 2 + 80) * Math.Sin(whereZone.c * toR);
                }
            }


            if (qrcProduce.Direction == 4)//朝四个方向
            {
                if (box_num % 4 == 1)
                {
                    putPos.c = putPos.c - 90;
                }
                else if (box_num % 4 == 2)
                {
                    putPos.c = putPos.c - 90;
                }
                else if (box_num % 4 == 3)
                {
                    putPos.c = putPos.c + 90;
                }
                else if (box_num % 4 == 0)
                {
                }
            }
            else if (qrcProduce.Direction == 2)//朝二个方向
            {
                if (box_num % 4 == 1)
                {
                    putPos.c = putPos.c - 90;
                }
                else if (box_num % 4 == 2)
                {
                    putPos.c = putPos.c - 90;
                }
                else if (box_num % 4 == 3)
                {
                    putPos.c = putPos.c + 90;
                }
                else if (box_num % 4 == 0)
                {
                    putPos.c = putPos.c + 90;
                }
            }

            if (box_num == 53 || box_num == 54) putPos.z = putPos.z + 30;//在前一层上方旋转
            else putPos.z = putPos.z + 60;

            //22222222222222222222222222先旋转 
            runToPos(0, 2, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);
            if (qrcProduce.Direction == 4 && (box_num % 4 == 1))//朝四个方向 转180的情况
            {
                runToPos(0, 3, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)(putPos.c - 90), true);
            }


            //3333333333333333333333333333 靠外下移箱子  保持旋转角度
            if (box_num == 53 || box_num == 54) putPos.z = putPos.z - 50;
            else if (box_num > 30) putPos.z = putPos.z - 70;
            else putPos.z = putPos.z - 90;
            if (qrcProduce.Direction == 4 && (box_num % 4 == 1))//朝四个方向 转180的情况 
            {
                runToPos(0, 1, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)(putPos.c - 90), true);

            }
            else runToPos(0, 1, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);

            //44444444444444444444444444靠里
            int OutTemp = 8, InTemp = 16;
            if (box_num % 4 == 1)//位置1
            {
                putPos.x = whereZone.x + (boxSize.L / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y + (boxSize.W / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                if (box_num == 53)
                {
                    putPos.x = whereZone.x + (boxSize.L / 2.0 + OutTemp - 80) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y + (boxSize.W / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0 + OutTemp - 80) * Math.Sin(whereZone.c * toR);

                }
            }
            else if (box_num % 4 == 2)//位置2
            {
                putPos.x = whereZone.x + (boxSize.L / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y - (boxSize.W / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                if (box_num == 54)
                {
                    putPos.x = whereZone.x + (boxSize.L / 2.0 - InTemp - 80) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y - (boxSize.W / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) - (boxSize.L / 2.0 - InTemp - 80) * Math.Sin(whereZone.c * toR);
                }

            }
            else if (box_num % 4 == 3)//位置3
            {
                putPos.x = whereZone.x - (boxSize.L / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y + (boxSize.W / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                if (box_num == 55)
                {
                    putPos.x = whereZone.x - (boxSize.L / 2.0 - InTemp + 80) * Math.Cos(whereZone.c * toR) + (boxSize.W / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y + (boxSize.W / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 - InTemp + 80) * Math.Sin(whereZone.c * toR);

                }
            }
            else if (box_num % 4 == 0)//位置4
            {
                putPos.x = whereZone.x - (boxSize.L / 2.0 + OutTemp) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                putPos.y = whereZone.y - (boxSize.W / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 + OutTemp) * Math.Sin(whereZone.c * toR);
                if (box_num == 56)
                {
                    putPos.x = whereZone.x - (boxSize.L / 2.0 + OutTemp + 80) * Math.Cos(whereZone.c * toR) - (boxSize.W / 2.0 - InTemp) * Math.Sin(whereZone.c * toR);
                    putPos.y = whereZone.y - (boxSize.W / 2.0 - InTemp) * Math.Cos(whereZone.c * toR) + (boxSize.L / 2.0 + OutTemp + 80) * Math.Sin(whereZone.c * toR);

                }
            }

            if (box_num > 40) putPos.z = putPos.z - 0;//高处少下一点
            else if (box_num > 30) putPos.z = putPos.z - 10;//高处少下一点
            else if (box_num > 20) putPos.z = putPos.z - 20;//高处少下一点
            else if (box_num > 10) putPos.z = putPos.z - 30;//高处少下一点
            else putPos.z = putPos.z - 40;

            //5555555555555555555555555靠里放下下移箱子 保持角度
            if (qrcProduce.Direction == 4 && (box_num % 4 == 1))//朝四个方向 转180的情况
            {
                runToPos(0, 1, 2, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)(putPos.c - 90), true);

            }
            else runToPos(0, 1, 2, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);



            //66666666666666666666666666666666666上升 转回  
            if (box_num > 52) putPos.z = putPos.z + 50;
            else putPos.z = putPos.z + 70;
            if (qrcProduce.Direction == 4 && (box_num % 4 == 1))//朝四个方向 转180的情况
            {
                runToPos(0, 2, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)(putPos.c - 90), true);
            }
            runToPos(0, 2, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);
        }
         
      
        /// <summary>
        /// 返回拍照区
        /// </summary>
        /// <param name="whereZone">哪垛</param>
        void goBackToCameraPos(int whereZone)
        {
            //上升
            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //跨越
            runToPos(0, 5, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
                    
            //switch (whereZone)
            //{
            //    case 1:
            //        //【1】跨越第一垛
            //        runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //        //【2】跨越拍照区                         
            //        runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
            //        break;
            //    case 2:
            //        //【1】跨越第二垛
            //        runToPos(0, 3, 0, (int)passTempPos3.x, (int)passTempPos3.y, (int)passTempPos3.z, (int)passTempPos3.a, (int)passTempPos3.b, (int)passTempPos3.c, true);
            //        //【2】跨越第一垛
            //        runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //        //【3】跨越拍照区                         
            //        runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
            //        break;
            //    case 3:
            //        //【1】跨越第三垛
            //        runToPos(0, 3, 0, (int)passTempPos4.x, (int)passTempPos4.y, (int)passTempPos4.z, (int)passTempPos4.a, (int)passTempPos4.b, (int)passTempPos4.c, true);
            //        //【2】跨越第四垛
            //        runToPos(0, 3, 0, (int)passTempPos3.x, (int)passTempPos3.y, (int)passTempPos3.z, (int)passTempPos3.a, (int)passTempPos3.b, (int)passTempPos3.c, true);
            //        //【3】跨越第五垛
            //        runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //        //【4】跨越拍照区                         
            //        runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
            //        break;
            //    case 4:
            //        //【1】跨越第四垛
            //        runToPos(0, 3, 0, (int)passTempPos3.x, (int)passTempPos3.y, (int)passTempPos3.z, (int)passTempPos3.a, (int)passTempPos3.b, (int)passTempPos3.c, true);
            //        //【2】跨越第五垛
            //        runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //        //【3】跨越拍照区                         
            //        runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
            //        break;
            //    case 5:
            //        //【1】跨越第五垛
            //        runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
            //        //【2】跨越拍照区                         
            //        runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
            //        break;
            //    default:
            //        break;
            //}
            //初始拍照位置
            runToPos(0, 2, 0, (int)cameraPos.x, (int)cameraPos.y, (int)cameraPos.z, (int)cameraPos.a, (int)cameraPos.b, (int)cameraPos.c, true);

        }
        /// <summary>
        /// 机器人运行
        /// </summary>
        private void robot_run()
        {
            int qcNgTimes = 0;//二维码识别不过的次数
            int box_num = 0;
            int diff_x, diff_y;//相机与tcp偏移
            int[,] myDxDz = new int[9, 2] { { 0, 0 }, { -5, 0 }, { 0, 5 }, { 5, 0 }, { 0, -5 }, { -10, 0 }, { 0, 10 }, { 10, 0 }, { 0, -10 } };//识别二维码偏移
            //初始拍照位置 亮绿灯
            runToPos(2, 2, 0, (int)cameraPos.x, (int)cameraPos.y, (int)cameraPos.z, (int)cameraPos.a, (int)cameraPos.b, (int)cameraPos.c, true);
            Thread.Sleep(500);
            runMode = 1;
            while (my_start == 1)
            {
                if (zoneFull == true)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        btnWorking_Click(null, null);//没有清空托盘，停止线程
                        MessageBox.Show(" 请移除并更换托盘，并清空托盘！");
                    }));
                    break;
                }

                //【1】匹配成功-抓取箱子
                if (runMode == 1 && GLB.Match_success == true)
                {
                    bool ProduceArrivetemp = true;
                    GLB.Match_success = false;
                    double ang = 35 * toR;//角度转弧度
                    if (GLB.camera_device_point.Z > 1150)//上两层
                    {
                       diff_x = 351;
                        diff_y = -23;
                    }
                    else
                    {  
                        diff_x = 351;
                        diff_y = -21;                       
                    }
                    double dx = diff_y * Math.Sin(ang) + diff_x * Math.Cos(ang) + 1250 * Math.Cos(ang);//(diff_x,diff_y)相机与tcp偏移  1250半径
                    double dy = diff_y * Math.Cos(ang) - diff_x * Math.Sin(ang) - 1250 * Math.Sin(ang);
                    GLB.robot_device_point.X = (float)(dx - (GLB.camera_device_point.X * Math.Sin(ang) + GLB.camera_device_point.Y * Math.Cos(ang)));//前相机坐标-》机器人坐标
                    GLB.robot_device_point.Y = (float)(dy - (GLB.camera_device_point.X * Math.Cos(ang) - GLB.camera_device_point.Y * Math.Sin(ang)));
                    GLB.robot_device_point.Z = (float)(1110 - GLB.camera_device_point.Z);

                    if (double.IsNaN(GLB.robot_device_point.X) || double.IsNaN(GLB.robot_device_point.Y) || double.IsNaN(GLB.robot_device_point.Z)) { runMode = 1; continue; }; //空值返回
                    if (GLB.robot_device_point.X > 2000 || GLB.robot_device_point.X < 600 || GLB.robot_device_point.Y < -1200 || GLB.robot_device_point.Y > -500
                        || GLB.robot_device_point.Z > 500 || GLB.robot_device_point.Z < -300) { runMode = 1; continue; }//限制位置

                    if (CarryMode == 0 && GLB.robot_device_point.Z < -200) ProduceArrivetemp = false;//只剩一卷，修改小车到达状态
                    runMode = 2;//停止拍照
                    GLB.device_angl += 35+0.5f;
                    float myangle = GLB.device_angl;//防止改变
                    runToPos(0, 2, 0, (int)(GLB.robot_device_point.X), (int)(GLB.robot_device_point.Y), 600, 0, 180, myangle, true);//防止刮屏
                    //抓取箱子，电磁阀开
                    runToPos(0, 1,1, (int)(GLB.robot_device_point.X), (int)(GLB.robot_device_point.Y), (int)(GLB.robot_device_point.Z), 0, 180, myangle, true);
                    runToPos(0, 1, 0, (int)(GLB.robot_device_point.X), (int)(GLB.robot_device_point.Y), (int)(GLB.robot_device_point.Z + 100), 0, 180, myangle, true);//上升到+100
                    GLB.robot_device_point.X = 1024;//清除坐标
                    GLB.robot_device_point.Y = -717;
                    GLB.robot_device_point.Z = 1100;

                    if (CarryMode == 0 && ProduceArrivetemp == false)
                    {
                        ProduceArrive = false;
                        SetCarryArrive(0);//修改产品没送到
                        ArrayList array = new ArrayList();//多条SQL语句数组
                        string sql = "update Agv_list set isworking =0,stowerid ='',pronum =0 where agvid in(select agvid from Agvmission_list where fstatus =7 and messionType =1 and stowerid='" + GLB.RobotId + "')";//修改小车状态
                        string sql1 = "update Agvmission_list set fstatus =6 ,actionenddate=getdate() where fstatus =7 and messionType =1  and stowerid='" + GLB.RobotId + "'";//修改任务 等待状态为完成状态                        
                        array.Add(sql);
                        array.Add(sql1);
                        bool isok = MyDataLib.transactionOp_list(array);
                        SetRobotStatus(2, "等待送货");//修改码垛机器人状态
                    }
                    txtQRC.BeginInvoke(new Action(() =>
                    {
                        myQRCInfo = "";
                        txtQRC.Text = "";//清空二维码缓存
                        txtQRC.Enabled = true; txtQRC.Focus();//聚焦 防止enter到其他键 
                    }));
                    Thread.Sleep(200);
                    runMode = 2;
                }
                //【2】识别二维码
                else if (runMode == 2)
                {
                    //【*】到识别二维码位置
                    if (qcNgTimes == 0)
                    {
                        runToPos(0, 2, 0, (int)qrcPos.x, (int)qrcPos.y, (int)qrcPos.z, (int)qrcPos.a, (int)qrcPos.b, (int)qrcPos.c, true);
                    }
                    //【*】识别
                    bool qrcCheckResult = qrcCheckFun();
                    //【*】识别到二维码//上传MES 匹配 获取垛区                        
                    if (qrcCheckResult == true)
                    {
                        qrcCheckResult = false;
                        qcNgTimes = 0;
                        runMode = 3;
                    }
                    //【*】没识别到
                    else
                    {
                        qcNgTimes += 1;
                        P6D qrcPosTemp = qrcPos.copyto();
                        if (qcNgTimes <= 9 && NG_Type == 2)//识别9次加相应位移
                        {
                            qrcPosTemp.x = qrcPosTemp.x + myDxDz[qcNgTimes - 1, 0];
                            qrcPosTemp.z = qrcPosTemp.z + myDxDz[qcNgTimes - 1, 1];
                            runToPos(0, 1, 0, (int)qrcPosTemp.x, (int)qrcPosTemp.y, (int)qrcPosTemp.z, (int)qrcPosTemp.a, (int)qrcPosTemp.b, (int)qrcPosTemp.c, true);
                        }
                        else
                        {
                            GLB.zoneCrrentNumTemp[0] = GLB.zoneCrrentNumTemp[0] + 1;//不良品区数量
                            qcNgTimes = 0;

                            putPos = qrcNGPos.copyto();
                            putPos.z = 700;
                            //经过不良区上方
                            runToPos(0, 2, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);

                            putPos.z = qrcNGPos.z + box1.H * (GLB.zoneCrrentNumTemp[0]);
                            //放下箱子
                            runToPos(0, 1, 2, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);
                            this.BeginInvoke(new Action(() =>
                            {
                                ProductInfo product = productMethod.GetProductByZone(0, GLB.objListProduct);//通过垛区查找
                                product.CurrentCounts = GLB.zoneCrrentNumTemp[0];
                                product.ComepleteNum += 1;
                                productMethod.ChangeProduct(product, GLB.objListProduct);//数组更新
                                //btnCancel_Click(null, null); //更新界面数据
                                productMethod.addProNumToLib(product.MissionID, product.Zone, product.RobotID);//更新数据库
                            }));
                            //插入 不良品类型
                            if (NG_Type == 1)
                            {
                                string sql = "insert into StowNgStatus(StowNo,Floor,Reason)values('" + GLB.RobotId + "','" + GLB.zoneCrrentNumTemp[0] + "','" + "该订单不在任务中" + "') ";
                                MyDataLib.ExecNoneQueryBySql(sql);
                            }
                            else if (NG_Type == 2)
                            {
                                string sql = "insert into StowNgStatus(StowNo,Floor,Reason)values('" + GLB.RobotId + "','" + GLB.zoneCrrentNumTemp[0] + "','" + "识别不出条形码" + "') ";
                                MyDataLib.ExecNoneQueryBySql(sql);
                            }

                            putPos.z = 700;
                            //回到上方
                            runToPos(0, 2, 0, (int)putPos.x, (int)putPos.y, (int)putPos.z, (int)putPos.a, (int)putPos.b, (int)putPos.c, true);

                            //初始拍照位置
                            runToPos(0, 2, 0, (int)cameraPos.x, (int)cameraPos.y, (int)cameraPos.z, (int)cameraPos.a, (int)cameraPos.b, (int)cameraPos.c, true);

                            /////////////////////////////////////////更新数据
                            this.BeginInvoke(new Action(() =>
                            {
                                ProductInfo product = productMethod.GetProductByZone(0, GLB.objListProduct);//通过垛区查找                              
                                if (product.ComepleteNum == product.Total || GLB.zoneCrrentNumTemp[0] >= 10)
                                {
                                    //不良区托盘状态 1:满了
                                    SetZoneFullStatus(0, 1);
                                    zoneFull = true;
                                    //清空不良区托盘按键使能                          
                                    btnClearZone0.Enabled = true;
                                    runMode = 0;
                                    btnWorking_Click(null, null);//数量超过10个，停止线程
                                    //报警
                                    runToPos(3, 0, 0, 0, 0, 0, 0, 0, 0, false);
                                    MessageBox.Show("不良品垛已经堆满！请移除并更换托盘");
                                    SetRobotStatus(4, "不良区满了");
                                    return;
                                }
                            }));
                            Thread.Sleep(500);
                            runMode = 1;
                        }
                    }
                }
                //【3】跨越垛区
                else if (runMode == 3)
                {
                    //左侧过渡区
                    switch (qrcProduce.Zone)
                    {
                        case 1:
                            box_num = GLB.zoneCrrentNumTemp[1];//个数
                            //【1】跨越拍照区                         
                            passTempPos1 = passUp(cameraPos, passTempPos1, box_num, box1.H);
                            //【2】到达第一垛上方
                            passTempPos2 = passUpByPtp(zoneCheckCenter[1], passTempPos2, box_num, box1.H);
                            break;
                        case 2:
                            //获取最高的
                            if (GLB.zoneCrrentNumTemp[1] < GLB.zoneCrrentNumTemp[2])
                                box_num = GLB.zoneCrrentNumTemp[2];//个数
                            else
                                box_num = GLB.zoneCrrentNumTemp[1];//个数
                            //【1】跨越拍照区
                            passTempPos1 = passUp(cameraPos, passTempPos1, box_num, box1.H);
                            //【2】到达第二垛上方
                            passTempPos2 = passUpByPtp(zoneCheckCenter[2], passTempPos2, box_num, box1.H);
                            break;
                        case 3:
                            //获取最高的
                            if (GLB.zoneCrrentNumTemp[3] < GLB.zoneCrrentNumTemp[4])
                                box_num = GLB.zoneCrrentNumTemp[4];
                            else
                                box_num = GLB.zoneCrrentNumTemp[3];
                            if (box_num < GLB.zoneCrrentNumTemp[5])
                                box_num = GLB.zoneCrrentNumTemp[5];
                            //【1】跨越拍照区
                            passTempPos1 = passUp(cameraPos, passTempPos1, box_num, box1.H);
                            //【2】到达第三垛上方
                            passTempPos2 = passUpByPtp(zoneCheckCenter[3], passTempPos2, box_num, box1.H);
                            break;
                        case 4:
                            //获取最高的
                            if (GLB.zoneCrrentNumTemp[5] < GLB.zoneCrrentNumTemp[4])
                                box_num = GLB.zoneCrrentNumTemp[4];
                            else
                                box_num = GLB.zoneCrrentNumTemp[5];
                            //【1】跨越拍照区
                            passTempPos1 = passUp(cameraPos, passTempPos1, box_num, box1.H);
                            //【2】到达第四垛上方
                            passTempPos2 = passUpByPtp(zoneCheckCenter[4], passTempPos2, box_num, box1.H);
                            break;
                        case 5:
                            box_num = GLB.zoneCrrentNumTemp[5];
                            //【1】跨越拍照区
                            passTempPos1 = passUp(cameraPos, passTempPos1, box_num, box1.H);
                            //【2】到达第五垛上方
                            passTempPos2 = passUpByPtp(zoneCheckCenter[5], passTempPos2, box_num, box1.H);
                            break;
                        default:
                            break;
                    }
                    runMode = 4;
                }
                //【4】放置区
                else if (runMode == 4)
                {
                    zoneTemp = qrcProduce.Zone;//放置哪个垛
                    GLB.zoneCrrentNumTemp[zoneTemp] = qrcProduce.CurrentCounts;//当前个数
                    GLB.zoneCrrentNumTemp[zoneTemp] = GLB.zoneCrrentNumTemp[zoneTemp] + 1;//当前垛数量加一
                    if (qrcProduce.ComepleteNum == qrcProduce.Total || GLB.zoneCrrentNumTemp[zoneTemp] > qrcProduce.PerZoneNumbers)//放之前检查
                    {
                        goBackToCameraPos(zoneTemp);//回到拍照区
                        //更新对应托盘状态1:满了
                        SetZoneFullStatus(zoneTemp, 1);
                        zoneFull = true;
                        //清空托盘按键使能
                        switch (zoneTemp)
                        {
                            case 1:
                                btnClearZone1.Enabled = true;
                                break;
                            case 2:
                                btnClearZone2.Enabled = true;
                                break;
                            case 3:
                                btnClearZone3.Enabled = true;
                                break;
                            case 4:
                                btnClearZone4.Enabled = true;
                                break;
                            case 5:
                                btnClearZone5.Enabled = true;
                                break;
                            default:
                                break;
                        }
                        runMode = 0;
                        this.BeginInvoke(new Action(() =>
                        {
                            btnWorking_Click(null, null);//数量超过，停止线程
                        }));
                        //报警
                        runToPos(3, 0, 0, 0, 0, 0, 0, 0, 0, false);
                        SetRobotStatus(4, zoneTemp + "垛满了");
                        MessageBox.Show("第" + zoneTemp + "垛已经堆满！请移除并更换托盘");
                        return;
                    }

                    box_num = GLB.zoneCrrentNumTemp[zoneTemp];//个数
                    //【*】放置到对应的垛
                    putDowmBox(zoneCenter[zoneTemp], box_num, box1);
                    this.BeginInvoke(new Action(() =>
                    {
                        ProductInfo product;                      
                        product = productMethod.GetProductByZone(qrcProduce.Zone, GLB.objListProduct);//通过垛区查找
                      
                        product.CurrentCounts = GLB.zoneCrrentNumTemp[zoneTemp];
                        product.ComepleteNum += 1;
                        productMethod.ChangeProduct(product, GLB.objListProduct);  //数组更新                   
                        //btnCancel_Click(null, null); //更新界面数据
                        productMethod.addProNumToLib(product.MissionID, product.Zone, product.RobotID);//更新数据库

                        productMethod.saveStowCode(product,globalBarcode);//保存条码到数据库
                        txtMissionID.Text = "";//订单号
                        txtCustomer.Text = "";//客户
                        txtProZone.Text = "";//垛区
                        txtMaterielCode.Text = "";//物料代码
                        txtCurrentCounts.Text = "";//当前数量
                    }));
                    //【*】回到拍照位置
                    goBackToCameraPos(zoneTemp);
                    //【*】更新数据
                    this.BeginInvoke(new Action(() =>
                    {
                        qrcProduce.ComepleteNum += 1;
                        if (qrcProduce.ComepleteNum == qrcProduce.Total || GLB.zoneCrrentNumTemp[zoneTemp] >= qrcProduce.PerZoneNumbers)
                        {
                            //更新对应托盘状态1:满了
                            SetZoneFullStatus(zoneTemp, 1);
                            zoneFull = true;
                            //清空托盘按键使能
                            switch (zoneTemp)
                            {
                                case 1:
                                    btnClearZone1.Enabled = true;
                                    break;
                                case 2:
                                    btnClearZone2.Enabled = true;
                                    break;
                                case 3:
                                    btnClearZone3.Enabled = true;
                                    break;
                                case 4:
                                    btnClearZone4.Enabled = true;
                                    break;
                                case 5:
                                    btnClearZone5.Enabled = true;
                                    break;
                                default:
                                    break;
                            }
                            runMode = 0;
                            btnWorking_Click(null, null);//数量超过，停止线程
                            //报警
                            runToPos(3, 0, 0, 0, 0, 0, 0, 0, 0, false);
                            SetRobotStatus(4, zoneTemp + "垛满了");
                            MessageBox.Show("第" + zoneTemp + "垛已经堆满！请移除并更换托盘");
                            return;
                        }
                    }));
                    Thread.Sleep(500);
                    runMode = 1;
                }
                else Thread.Sleep(10);
            }
        }
        /// <summary>
        /// 检测托盘位置
        /// </summary>
        private void checkZone()
        {
            int[,] checkZoneDxDz = new int[5, 2] { { 360, -26 }, { 362, -22 }, { 356, -27 }, { 353, -26 }, { 359, -26 }, };//tcp与相机偏移
            double[] diff_angle = new double[5] { 0.5, 1, 0, 0, 0};
            int check_time = 0;
            while (my_checkZone == 1)
            {
                //运行到对应托盘上方
                if (runMode == 5)
                {
                    int high = 1150;//上升高度
                    switch (zoneTemp)
                    {
                        case 1:
                            //【1】跨越拍照区                         
                            passTempPos1 = cameraPos.copyto();
                            passTempPos1.z = high;
                            runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);

                            //【2】到达第一垛上方
                            passTempPos2 = zoneCheckCenter[1].copyto();
                            passTempPos2.z = high;
                            runToPos(0, 4, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, 950, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            
                            break;
                        case 2:
                            //【1】跨越拍照区                         
                            passTempPos1 = cameraPos.copyto();
                            passTempPos1.z = high;
                            runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);

                            //【2】到达第二垛上方
                            passTempPos2 = zoneCheckCenter[2].copyto();
                            passTempPos2.z = high;
                            runToPos(0, 4, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, 950, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            
                            break;
                        case 3:
                            //【1】跨越拍照区
                            passTempPos1 = cameraPos.copyto();
                            passTempPos1.z = high;
                            runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
                            //【2】到达第三垛上方
                            passTempPos2 = zoneCheckCenter[3].copyto();
                            passTempPos2.z = high;
                            runToPos(0, 4, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, 950, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            
                            break;
                        case 4:
                            //【1】跨越拍照区
                            passTempPos1 = cameraPos.copyto();
                            passTempPos1.z = high;
                            runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
                            //【2】到达第四垛上方
                            passTempPos2 = zoneCheckCenter[4].copyto();
                            passTempPos2.z = high;
                            runToPos(0, 4, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, 950, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            
                            break;
                        case 5:
                            //【1】跨越拍照区
                            passTempPos1 = cameraPos.copyto();
                            passTempPos1.z = high;
                            runToPos(0, 3, 0, (int)passTempPos1.x, (int)passTempPos1.y, (int)passTempPos1.z, (int)passTempPos1.a, (int)passTempPos1.b, (int)passTempPos1.c, true);
                            //【2】到达第五垛上方
                            passTempPos2 = zoneCheckCenter[5].copyto();
                            passTempPos2.z = high;
                            runToPos(0, 4, 0, (int)passTempPos2.x, (int)passTempPos2.y, (int)passTempPos2.z, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            runToPos(0, 3, 0, (int)passTempPos2.x, (int)passTempPos2.y, 950, (int)passTempPos2.a, (int)passTempPos2.b, (int)passTempPos2.c, true);
                            
                            break;
                        default:
                            break;
                    }
                    Thread.Sleep(1200);
                    runMode = 6;
                    Thread.Sleep(200);
                }
                if (runMode == 6 && GLB.Match_success == true)
                {
                    GLB.Match_success = false;
                    double myAngle = 0;
                    //【*】旋转角选择
                    switch (zoneTemp)
                    {
                        case 1: myAngle = 90; break;
                        case 2: myAngle = 150; break;
                        case 3: myAngle = -150; break;
                        case 4: myAngle = -90; break;
                        case 5: myAngle = -30; break;
                        default:
                            break;
                    }
                    //【*】坐标换算
                    double ang = myAngle * toR;
                    //double dx = -30 * Math.Sin(ang) + 355 * Math.Cos(ang) + 1250 * Math.Cos(ang);//(355,-30)相机与tcp偏移  1250半径
                    //double dy = -30 * Math.Cos(ang) - 355 * Math.Sin(ang) - 1250 * Math.Sin(ang);
                    double dx = checkZoneDxDz[zoneTemp - 1, 1] * Math.Sin(ang) + checkZoneDxDz[zoneTemp - 1, 0] * Math.Cos(ang) + 1250 * Math.Cos(ang);//(checkZoneDxDz[zoneTemp - 1, 0],checkZoneDxDz[zoneTemp - 1, 1])相机与tcp偏移  1250半径
                    double dy = checkZoneDxDz[zoneTemp - 1, 1] * Math.Cos(ang) - checkZoneDxDz[zoneTemp - 1, 0] * Math.Sin(ang) - 1250 * Math.Sin(ang);
            
                    GLB.robot_device_point.X = (float)(dx - (GLB.camera_device_point.X * Math.Sin(ang) + GLB.camera_device_point.Y * Math.Cos(ang)));//前相机坐标-》机器人坐标
                    GLB.robot_device_point.Y = (float)(dy - (GLB.camera_device_point.X * Math.Cos(ang) - GLB.camera_device_point.Y * Math.Sin(ang)));
                    GLB.robot_device_point.Z = (float)(1043 - GLB.camera_device_point.Z);

                    if (double.IsNaN(GLB.robot_device_point.X) || double.IsNaN(GLB.robot_device_point.Y) || double.IsNaN(GLB.robot_device_point.Z))
                    { runMode = 6; GLB.Match_success = false; continue; }; //空值返回
                    if (GLB.robot_device_point.X > (1680 * Math.Cos(ang) + 50) || GLB.robot_device_point.X < (1680 * Math.Cos(ang) - 50)//限制位置
                        || GLB.robot_device_point.Y < (-1680 * Math.Sin(ang) - 50) || GLB.robot_device_point.Y > (-1680 * Math.Sin(ang) + 50)
                        || GLB.robot_device_point.Z > -400 || GLB.robot_device_point.Z < -550)
                    { runMode = 6; GLB.Match_success = false; MessageBox.Show("托盘摆放太出了或是托盘没有清空！"); continue; }

                    GLB.device_angl += (float)myAngle + (float)diff_angle[zoneTemp - 1];
                    //【*】更新托盘中心位置
                    zoneCenter[zoneTemp] = new P6D(GLB.robot_device_point.X, GLB.robot_device_point.Y, zoneCheckCenter[zoneTemp].z, 0, 180, GLB.device_angl);
                    //【*】修改坐标到内存
                    string mytext = zoneTemp + "#," + GLB.robot_device_point.X + "," + GLB.robot_device_point.Y + "," + zoneCheckCenter[zoneTemp].z + "," + 0 + "," + 180 + "," + GLB.device_angl;
                    fileOperation.ChangeLineTxtFile("zoneCenterPos.txt", zoneTemp, mytext);
                    //【*】修改对应托盘识别状态
                    SetZoneCheckStatus(zoneTemp, 1);//数据库
                    GLB.zoneCheckStatus[zoneTemp] = true;//本地

                    runMode = 0;
                    GLB.robot_device_point.X = 1024;//清除
                    GLB.robot_device_point.Y = -717;
                    GLB.robot_device_point.Z = 1000;
                    //【*】返回拍照区
                    goBackToCameraPos(zoneTemp);
                    MessageBox.Show("成功识别托盘位置！");
                    //【*】各按键启用
                    this.BeginInvoke(new Action(() =>
                    {
                        btnWorking.Enabled = true;
                        btnInitPos.Enabled = true;
                        btnClearAlarm.Enabled = true;
                        zone1Btn.Enabled = true;
                        zone2Btn.Enabled = true;
                        zone3Btn.Enabled = true;
                        zone4Btn.Enabled = true;
                        zone5Btn.Enabled = true;
                    }));
                    //【*】终止线程
                    my_checkZone = 0;
                    checkZone_Thread.Abort();
                }
                if (runMode == 6 && GLB.Match_success == false)
                {
                    check_time += 1;
                    if (check_time > 6)
                    {
                        check_time = 0;
                        runMode = 0;
                        //【*】返回拍照区
                        goBackToCameraPos(zoneTemp);
                        MessageBox.Show("识别不到托盘位置！ 请检查托盘");
                        Thread.Sleep(100);
                        //【*】各按键启用
                        this.BeginInvoke(new Action(() =>
                        {
                            btnWorking.Enabled = true;
                            btnInitPos.Enabled = true;
                            btnClearAlarm.Enabled = true;
                            zone1Btn.Enabled = true;
                            zone2Btn.Enabled = true;
                            zone3Btn.Enabled = true;
                            zone4Btn.Enabled = true;
                            zone5Btn.Enabled = true;
                        }));
                        //【*】终止线程
                        my_checkZone = 0;
                        runMode = 0;
                        checkZone_Thread.Abort();
                    }
                    Thread.Sleep(150);
                }
                else Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 产品送到状态
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="StatusInfo"></param>
        public static void SetCarryArrive(int value)
        {
            string sql = "select * from dbo.StowRobotStatus where  StowNo='" + GLB.RobotId + "'";//通过机器人ID查找
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "update  dbo.StowRobotStatus set carryArrive='" + value + "' where StowNo ='" + GLB.RobotId + "'";
                int i = MyDataLib.ExecNoneQueryBySql(sql);
            }
        }

        /// <summary>
        /// 设置机器人工作状态
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="StatusInfo"></param>
        public static void SetRobotStatus(int Status, string StatusInfo)
        {
            string sql = "select * from dbo.StowRobotStatus where  StowNo='" + GLB.RobotId + "'";//通过机器人ID查找
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "update  dbo.StowRobotStatus set Status='" + Status + "',StatusInfo= '" + StatusInfo + "' where StowNo ='" + GLB.RobotId + "'";
                int i = MyDataLib.ExecNoneQueryBySql(sql);
            }
        }
        /// <summary>
        /// 修改对应托盘：0未满  1：满了
        /// </summary>
        /// <param name="Zone">托盘号</param>
        /// <param name="value"></param>
        private void SetZoneFullStatus(int Zone, int value)
        {
            string sql = "select * from dbo.StowRobotStatus where  StowNo='" + GLB.RobotId + "'";//通过机器人ID查找
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "update  dbo.StowRobotStatus set area" + Zone + "status='" + value + "' where StowNo ='" + GLB.RobotId + "'";
                int i = MyDataLib.ExecNoneQueryBySql(sql);
            }
        }

        /// <summary>
        /// 查找对应托盘堆满状态
        /// </summary>
        /// <param name="Zone"></param>
        private bool SearchZoneFullStatus(int Zone)
        {
            string sql = "select * from StowRobotStatus where area" + Zone + "status =1 and StowNo ='" + GLB.RobotId + "'";
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                return true;//对应托盘满了
            }
            return false;
        }
        /// <summary>
        /// 修改对应托盘识别状态：0未识别  1：识别成功
        /// </summary>
        /// <param name="Zone">托盘号</param>
        /// <param name="value"></param>
        private void SetZoneCheckStatus(int Zone, int value)
        {
            string sql = "select * from dbo.StowAreaCheckStatus where  StowNo='" + GLB.RobotId + "'";//通过机器人ID查找
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                sql = "update  dbo.StowAreaCheckStatus set area" + Zone + "CheckStatus='" + value + "' where StowNo ='" + GLB.RobotId + "'";
                int i = MyDataLib.ExecNoneQueryBySql(sql);
            }
        }
        /// <summary>
        /// 查找对应托盘识别状态
        /// </summary>
        /// <param name="Zone"></param>
        private bool SearchZoneCheckStatus(int Zone)
        {
            string sql = "select * from StowAreaCheckStatus where area" + Zone + "Checkstatus =1 and StowNo ='" + GLB.RobotId + "'";
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                return true;//对应托盘满了
            }
            return false;
        }
        /// <summary>
        /// 清空托盘数量
        /// </summary>
        /// <param name="Zone"> 区域，</param>
        /// <param name="robotID">机器人编号</param>
        private void cleanZoneCurrentNum(int Zone, int robotID)
        {
            string sql = "exec CleanStowTuoQty'" + Zone + "','" + robotID + "'";
            MyDataLib.ExecNoneQueryBySql(sql);
        }
        #endregion
    }
}
