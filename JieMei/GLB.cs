using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;//Point定义必须

using Models;
namespace JieMei
{
    //普通类中的常量，变量：
    public class GLB
    {
        public static string RobotId = "1";
        public static int BUFW = 1280, BUFH = 960;// BUFSIZE = 1280 * 960 * 3;
        public static Image<Emgu.CV.Structure.Bgr, Byte> frame = new Image<Bgr, byte>(BUFW, BUFH);//显示区
        public static string TitleStr = "";//显示信息
        public static int Sample_Distance = 750;//滚动条值:采样深度
        public static int img_mode = 0;//选择图像模式
        public static bool Match_success = false;//检测到工件
        public static int block_num = 0;//块数计数器

        public static Point3 camera_device_point = new Point3(0, 0, 0);//工件位置
        public static Point3 robot_device_point = new Point3(0, 0, 0);//工件位置
        public static float device_angl = 0;//工件旋转角
        //摄像头动态数据流:
        public static byte[] mycolor = new byte[1280 * 960 * 3];//彩色一维数数;
        public static int[] mydepth = new int[1280 * 960];//深度一维数组
        public static float[] myp3d = new float[1280 * 960 * 3];//真三维数据;
        public static List<Point3> device_point_list = new List<Point3>();//工件位置
        public static List<Point3> device_norm_list = new List<Point3>();//工件位置法向量
        //自定义结构的集合：
        public static Dictionary<int, OBJ> obj = new Dictionary<int, OBJ>();//全部连通域
      
        ///产品设置
        public static List<ProductInfo> objListProduct = new List<ProductInfo>();//定义接收产品信息的数组
        public static int[] zoneCrrentNumTemp = new int[6];//NG区+5个区 的堆垛数量
        public static bool[] zoneCheckStatus = new bool[6];//NG区+5个区的识别状态
    }
   public class _G
    {
        public static List<Point3> pt = new List<Point3>();
        public static List<Point3> ps = new List<Point3>();
    }
    //连通域数据结构:
    public class OBJ
    {
        public string typName;      //本连通域的产品型号及工件名称
        public string type; //工件类型

        //public double Area;         //本连通域的面积        
        //public double ArcLen;       //本连通域的轮廓周长   
        public double axisLong;         //长轴        
        public double axisShort;       //短轴  
        public int xCenter;         //中心X坐标
        public int yCenter;         //中心Y坐标

        public int Depth;        //本连通域的最小外接矩形中心处深度(距离)
        public List<Point> jd = new List<Point>();//轮廓角点
        public double Angle;        //本连通域的最小外接矩形长边倾斜角(-180 ... +180)

        public double L2S;          //本连通域的最小外接矩形长短边比值
        public double R2R;          //本连通域的最大最小半径的比值
        public double SZ;           //按距离远近进行缩放,相当于正方形的边长
        public double avgR;         //按距离远近进行缩放,本连通域的平均半径
        public double stdR;         //按距离远近进行缩放,本连通域的半径标准差(离散度)        

        public string AVUD;         //字符串表达:峰,谷,单调升,单调降
        public int sumAV;           //"峰+谷"之和
        public int sumUD;           //"单调升+单调降"之和
        public int isAverage;       //是否为平均值，是为1，否为0
        public Point3 Norm;         //质心处法向量
        public string depth_diff;  //9*9深度差
        public Dictionary<int, double> DAR = new Dictionary<int, double>();//边缘线上的(角度[-180 ... +180],半径)集合
        public List<Point> D2D = new List<Point>();        //连通域内的(二维点)集合
        public List<Point3> D3D = new List<Point3>();       //连通域内的(三维点)集合


    }
    //箱子尺寸
    public class BOX
    {
        public int W;
        public int L;
        public int H;

        //构造函数:
        public BOX()
        {

        }

        //构造函数:
        public BOX(int w, int l, int h)
        {
            this.W = w;
            this.L = l;
            this.H = h;
        }
    }
    //三维点:
    public class P3D
    {
        public double X;
        public double Y;
        public double Z;

        //构造函数:
        public P3D()
        {

        }

        //构造函数:
        public P3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    //自定义类:
   public class P6D
    {
        public double x;
        public double y;
        public double z;
        public double a;
        public double b;
        public double c;

        //构造函数:
        public P6D()
        {

        }

        //构造函数:
        public P6D(double X, double Y, double Z, double A, double B, double C)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.a = A;
            this.b = B;
            this.c = C;
        }
        public P6D copyto()
        {
            return new P6D(x, y, z, a, b, c);
        }
    }



    //六维点:
    public class Point6
    {
        public Double X1;
        public Double Y1;
        public Double Z1;
        public Double X2;
        public Double Y2;
        public Double Z2;

        //构造函数:
        public Point6(Double x1, Double y1, Double z1, Double x2, Double y2, Double z2)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
        }

        public Point3 getVector()
        {
            return new Point3(X2 - X1, Y2 - Y1, Z2 - Z1);
        }
       


    }



    //三维点:
   public class Point3
    {
        public Double X;
        public Double Y;
        public Double Z;

        //构造函数:
        public Point3(Double x, Double y, Double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //长度,模:
        public Double Length()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
        }

        //单位化:
        public void Normalize(Double ScaleFactor)
        {
            Double Length = this.Length();
            X /= Length / ScaleFactor;
            Y /= Length / ScaleFactor;
            Z /= Length / ScaleFactor;
        }

        //点积:
        public Double Dot(Point3 Vector3)
        {
            return (X * Vector3.X + Y * Vector3.Y + Z * Vector3.Z);
        }

        //叉积:
        public Point3 Cross(Point3 Vector3)
        {
            return new Point3(Y * Vector3.Z - Z * Vector3.Y, Z * Vector3.X - X * Vector3.Z, X * Vector3.Y - Y * Vector3.X);
        }

        //法向量获取欧拉角(返回弧度)--简化计算方案之四(ea.X=pitch, ea.Y=yaw, ea.Z=roll,常取:ea.Z=0):
        public Point3 getPYR()
        {
            Point3 ea = new Point3(0, 0, 0);

            //-------简易计算欧拉角--------
            ea.X = (float)(-Math.Atan2(Y, Math.Sqrt(X * X + Z * Z)));//tan(sqrt(X^2+Z^2)/Y)
            ea.Y = (float)(Math.Atan2(X, Z));

            //以下只为计算roll:
            float p = (float)(Math.Sqrt(X * X + Y * Y));
            float s = (float)(Math.Sqrt((1 - Z) / 2f));
            ea.Z = 0;

            if (p > 0)
            {
                X *= s / p; Y *= s / p;
                ea.Z = (float)(Math.Atan2(-2 * X * Y, 1 - 2 * Y * Y));
            }
            else if (Z > 0)
            {
                ea.Z = 0;
            }
            else if (Z < 0)
            {
                //ea.Z = 0;//默认是绕X轴旋转而来的
                ea.Z = Math.PI;//默认是绕Y轴旋转而来的
            }

            return ea;
        }

        //姿态矩阵转换成角度制的ABC:
        public Point3 Pose2ABC(double[] prof)
        {
            double a = 0, b = 0, c = 0;
            double b1 = Math.Atan2((0 - prof[6]), Math.Sqrt(prof[0] * prof[0] + prof[3] * prof[3]));
            b = b1 * 180 / 3.14159;

            if (b1 == 90 || b1 == -90)
            {
                c = 0.0;
                if (b == 90)
                {
                    a = Math.Atan2(prof[1], prof[4]) * 180 / 3.14159;
                }
                else
                {
                    a = (0 - Math.Atan2(prof[1], prof[4])) * 180 / 3.14159;
                }
            }

            c = Math.Atan2(prof[3] / Math.Cos(b1), prof[0] / Math.Cos(b1)) * 180 / 3.14159;
            a = Math.Atan2(prof[7] / Math.Cos(b1), prof[8] / Math.Cos(b1)) * 180 / 3.14159;

            return new Point3(a, b, c);
        }

        //角度制的ABC转换为姿态矩阵:
        public double[] ABC2Pose(double a, double b, double c)
        {
            double[] prof = new double[9];

            a = a / (180 / 3.14159);//转换为弧度
            b = b / (180 / 3.14159);
            c = c / (180 / 3.14159);

            prof[0] = Math.Cos(c) * Math.Cos(b);
            prof[1] = Math.Cos(c) * Math.Sin(b) * Math.Sin(a) - Math.Sin(c) * Math.Cos(a);
            prof[2] = Math.Cos(c) * Math.Sin(b) * Math.Cos(a) + Math.Sin(c) * Math.Sin(a);
            prof[3] = Math.Sin(c) * Math.Cos(b);
            prof[4] = Math.Sin(c) * Math.Sin(b) * Math.Sin(a) + Math.Cos(c) * Math.Cos(a);
            prof[5] = Math.Sin(c) * Math.Sin(b) * Math.Cos(a) - Math.Cos(c) * Math.Sin(a);
            prof[6] = -Math.Sin(b);
            prof[7] = Math.Cos(b) * Math.Sin(a);
            prof[8] = Math.Cos(b) * Math.Cos(a);

            return prof;
        }

        //通过向量获取旋转矩阵(第六轴常常越界):
        public double[] Vector2Pose(Point3 V)
        {
            double[] prof = new double[9];//旋转矩阵

            Point3 Z = new Point3(0, 0, 1);//起始向量             
            V.Normalize(1);
            Point3 n = Z.Cross(V);//获取旋转轴            

            double q = Math.Acos(Z.Dot(V));//获取旋转角

            if (Math.Abs(Z.Dot(V) + 1) < 0.01)//将成一直线时
            {
                n = new Point3(0, 1, 0);
                V = new Point3(0.001, 0, -0.999);
                V.Normalize(1);
                q = Math.Acos(Z.Dot(V));//获取旋转角
            }

            double x = n.X; double y = n.Y; double z = n.Z;

            double cq = Math.Cos(q);
            double sq = Math.Sin(q);
            double dq = 1 - Math.Cos(q);

            prof[0] = cq + x * x * dq;
            prof[1] = -z * sq + x * y * dq;
            prof[2] = y * sq + x * z * dq;
            prof[3] = z * sq + x * y * dq;
            prof[4] = cq + y * y * dq;
            prof[5] = -x * sq + y * z * dq;
            prof[6] = -y * sq + x * z * dq;
            prof[7] = x * sq + y * z * dq;
            prof[8] = cq + z * z * dq;

            return prof;
        }

        //法兰头向量转四元数,再转姿态矩阵:
        public double[] V2Q2P(Point3 V)
        {
            double[] prof = new double[9];//旋转矩阵

            Point3 Z = new Point3(0, 0, 1);//起始向量             
            V.Normalize(1);

            Point3 n = Z.Cross(V);//获取旋转轴            
            n.Normalize(1);

            double q = Math.Acos(Z.Dot(V));//获取旋转角(dot=1,同方向:q=0;  dot=-1,反方向:q=PI)

            if (Math.Abs(Z.Dot(V) - 1) < 0.01)//将成一直线时,与Z+同方向
            {
                prof = new double[]{
                       1,  0,  0,
                       0,  1,  0,
                       0,  0,  1 
               };
                //return prof;
            }

            else if (Math.Abs(Z.Dot(V) + 1) < 0.01)//将成一直线时,与Z-同方向
            {
                prof = new double[]{
                      -1,  0,  0,
                       0,  1,  0,
                       0,  0, -1 
               };
                //return prof;
            }
            else
            {

                //生成四元数:
                double w = Math.Cos(0.5 * q);
                double x = n.X * Math.Sin(0.5 * q);
                double y = n.Y * Math.Sin(0.5 * q);
                double z = n.Z * Math.Sin(0.5 * q);


                prof = Q2P(w, x, y, z);//四元数转矩阵

            }

            return prof;

        }

        //法兰头向量转四元数:
        public double[] V2Q(Point3 V)
        {
            double[] Q = new double[4];//四元数

            if (V.Length() > 0.001) //V不可以是接近(0,0,0)的向量
            {
                V.Normalize(1);
                Point3 Z = new Point3(0, 0, 1);//起始向量
                double q = Math.Acos(Z.Dot(V));//获取旋转角(dot=1,同方向:q=0;  dot=-1,反方向:q=PI)
                Point3 n = Z.Cross(V);//获取旋转轴

                if (Math.Abs(Z.Dot(V) - 1) < 0.001)
                { //dot=1,同方向:q=0; 指定只绕Y轴旋转
                    n = new Point3(0, 1, 0); q = 0;
                }
                if (Math.Abs(Z.Dot(V) + 1) < 0.001)//dot=-1,反方向:q=PI;指定只绕Y轴旋转
                {
                    n = new Point3(0, 1, 0); q = Math.PI;
                }

                n.Normalize(1);

                //生成四元数:
                double w = Math.Cos(0.5 * q);
                double x = n.X * Math.Sin(0.5 * q);
                double y = n.Y * Math.Sin(0.5 * q);
                double z = n.Z * Math.Sin(0.5 * q);
                Q = new double[] { w, x, y, z };
            }


            return Q;
        }

        //四元数转欧拉角(弧度制):
        public Point3 Q2ABC(double w, double x, double y, double z)
        {
            Point3 ea = new Point3(0, 0, 0); double v = 0;
            ea.Z = Math.Atan2(2 * (w * z + x * y), 1 - 2 * (z * z + x * x));//Roll
            //ea.X = Math.Asin(CLAMP(2 * (w * x - y * z), -1.0f, 1.0f));//Pitch
            v = 2 * (w * x - y * z);
            if (v < -1) { v = -1; }
            if (v > 1) { v = 1; }
            ea.X = Math.Asin(v);//Pitch
            ea.Y = Math.Atan2(2 * (w * y + z * x), 1 - 2 * (x * x + y * y));//Yaw
            return ea;
        }

        //四元数转姿态矩阵:
        public double[] Q2P(double w, double x, double y, double z)
        {

            double[] prof = new double[9];//旋转矩阵

            //四元数转旋转矩阵(姿态)-方案1:
            prof[0] = 1 - 2 * (y * y + z * z);
            prof[1] = 2 * (x * y + w * z);
            prof[2] = 2 * (x * z - w * y);

            prof[3] = 2 * (x * y - w * z);
            prof[4] = 1 - 2 * (x * x + z * z);
            prof[5] = 2 * (y * z + w * x);

            prof[6] = 2 * (x * z + w * y);
            prof[7] = 2 * (y * z - w * x);
            prof[8] = 1 - 2 * (x * x + y * y);


            ////四元数转旋转矩阵(姿态)-方案2:
            //prof[0] = 2 * (x * x + w * w) - 1;
            //prof[3] = 2 * (x * y + w * z);
            //prof[6] = 2 * (x * z - w * y);

            //prof[1] = 2 * (x * y - w * z);
            //prof[4] = 2 * (y * y + w * w) - 1;
            //prof[7] = 2 * (y * z + w * x);

            //prof[2] = 2 * (x * z + w * y);
            //prof[5] = 2 * (y * z - w * x);
            //prof[8] = 2 * (z * z + w * w) - 1;

            return prof;


        }
    }
}