using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Models;
namespace JieMei
{
    public class DealWithImage
    {
        FileOperation fileOperation = new FileOperation();//实例化处理文本的类

        /// <summary>
        /// 获取各区块的轮廓
        /// </summary>
        public void getContours(TextBox txtTypeName, PictureBox ptb)//找最近的轮廓
        {
            GLB.Match_success = false;//重新检测赋值
            Image<Gray, byte> dnc = new Image<Gray, byte>(GLB.BUFW, GLB.BUFH);
            Image<Gray, byte> threshImage = new Image<Gray, byte>(GLB.BUFW, GLB.BUFH);

            CvInvoke.CvtColor(GLB.frame, threshImage, ColorConversion.Bgra2Gray);//灰度化
            //CvInvoke.BilateralFilter(threshImage, threshImage, 10, 10, 4);//双边滤波
            //CvInvoke.GaussianBlur(threshImage, threshImage, new Size(3, 3), 4);//高斯滤波
            CvInvoke.BoxFilter(threshImage, threshImage, Emgu.CV.CvEnum.DepthType.Cv8U, new Size(3, 3), new Point(-1, -1));//方框滤波
            #region
            //var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            //CvInvoke.Dilate(threshImage, threshImage, kernal1, new Point(-1, -1), 2, BorderType.Default, new MCvScalar());//膨胀
            //var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            //CvInvoke.Erode(threshImage, threshImage, kernal1, new Point(-1, -1), 2, BorderType.Default, new MCvScalar());//腐蚀
                      
            //方式1
            //CvInvoke.Threshold(threshImage, threshImage, 100, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);//二值化
            //if (Mainform.runMode == 6)//匹配托盘
            //{
            //    var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));
            //    CvInvoke.Erode(threshImage, threshImage, kernal1, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());//腐蚀

            //}
            //else//匹配箱子
            //{
            //    var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            //    CvInvoke.Erode(threshImage, threshImage, kernal1, new Point(-1, -1), 2, BorderType.Default, new MCvScalar());//腐蚀
            //}           

            //方式2
            //if (Mainform.runMode == 6)//匹配托盘
            //{
            //    var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(9, 9), new Point(-1, -1));
            //    CvInvoke.Dilate(threshImage, threshImage, kernal1, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());//膨胀
            //}
            //else //加了膨胀跳动更大
            //{
            //    var kernal1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
            //    CvInvoke.Dilate(threshImage, threshImage, kernal1, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());//膨胀
            //}
            //ptb.Image = threshImage.ToBitmap();
            #endregion
            //检测连通域，每一个连通域以一系列的点表示，FindContours方法只能得到第一个域:
            try
            {
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(2000);//区块集合
                CvInvoke.FindContours(threshImage, contours, dnc, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);//轮廓集合
                GLB.block_num = 0;

                Dictionary<int, VectorOfPoint> mycontours = new Dictionary<int, VectorOfPoint>(100);//序号，轮廓
                mycontours.Clear();
                for (int k = 0; k < contours.Size; k++)
                {
                    double area = CvInvoke.ContourArea(contours[k]);//获取各连通域的面积 
                    if (area > 100000 && area < 800000)//根据面积作筛选(指定最小面积,最大面积):
                    {
                        if (!mycontours.ContainsKey(k)) mycontours.Add(k, contours[k]);
                    }
                }
                float my_depth_temp = GLB.myp3d[(GLB.BUFH / 2 * GLB.BUFW + GLB.BUFW / 2) * 3 + 2];
                if (mycontours.Count == 0 && Mainform.ProduceArrive == true && Mainform.CarryMode == 0 && Mainform.runMode == 1 && (my_depth_temp > 1400 || double.IsNaN(my_depth_temp)))//空车来，小车自动离开
                {
                    Mainform.ProduceArrive = false;
                    Mainform.SetCarryArrive(0);//修改产品没送到
                    ArrayList array = new ArrayList();//多条SQL语句数组
                    string sql = "update Agv_list set isworking =0,stowerid ='',pronum =0 where agvid in(select agvid from Agvmission_list where fstatus =7 and messionType =1 and stowerid='" + GLB.RobotId + "')";//修改小车状态
                    string sql1 = "update Agvmission_list set fstatus =6 ,actionenddate=getdate() where fstatus =7 and messionType =1  and stowerid='" + GLB.RobotId + "'";//修改任务 等待状态为完成状态                        
                    array.Add(sql);
                    array.Add(sql1);
                    bool isok = MyDataLib.transactionOp_list(array);
                    Mainform.SetRobotStatus(2, "等待送货");//修改码垛机器人状态
                }
                //按面积最大排序 生成新的字典
                Dictionary<int, VectorOfPoint> mycontours_SortedByKey = new Dictionary<int, VectorOfPoint>(100);//序号，轮廓;
                mycontours_SortedByKey.Clear();
                mycontours_SortedByKey = mycontours.OrderByDescending(o => CvInvoke.ContourArea(o.Value)).ToDictionary(p => p.Key, o => o.Value);
                GLB.obj.Clear();
                foreach (int k in mycontours_SortedByKey.Keys)
                {
                    OBJ obj = new OBJ();
                    {
                        if (!GLB.obj.ContainsKey(GLB.block_num)) GLB.obj.Add(GLB.block_num, obj);//不含这个，就添加
                        GLB.obj[GLB.block_num].typName = txtTypeName.Text.Replace(" ", "");// 对象名称

                        if (getMinAreaRect(mycontours_SortedByKey[k], GLB.block_num) == true)//获取最小外接矩形并处理相关参数
                        {

                            if (GLB.img_mode == 0)//匹配模式
                            {
                                if (Device_Macth(GLB.block_num) == true)//与库对比，生成工件位置，法向量,旋转角
                                {
                                    Thread.Sleep(400);
                                    break;
                                }
                            }
                        }

                        GLB.TitleStr += "block_num=" + GLB.block_num;
                        GLB.block_num++;//区块计数器 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误： " + ex.Message );
                throw;
            }
        }
        /// <summary>
        /// 获取最小外接矩形并处理相关参数
        /// </summary>
        /// <param name="contour">对应轮廓</param>
        /// <param name="block_num">编号</param>
        public bool getMinAreaRect(VectorOfPoint contour, int block_num)
        {
            //本区块颜色定义:
            MCvScalar color = new MCvScalar(53 * block_num % 255, 255 - 73 * block_num % 255, 93 * block_num % 255);//B,G,R
            try
            {  
                //【1】,获取最小外接矩形中心点下标:
                RotatedRect box = CvInvoke.MinAreaRect(contour); //最小外接矩形
                PointF[] pt = CvInvoke.BoxPoints(box);//最小外接矩形四个角点
                PointF po = box.Center;//最小外接矩形中心
                int xc = (int)po.X;//最小外接矩形中心X坐标
                int yc = (int)po.Y;//最小外接矩形中心Y坐标
 
                MCvMoments moments = CvInvoke.Moments(contour, false);//计算当前轮廓的矩
                if (Mainform.runMode == 1)
                {    
                    int gravity_x = Convert.ToInt32(moments.M10 / moments.M00);//计算当前轮廓中心点坐标
                    int gravity_y = Convert.ToInt32(moments.M01 / moments.M00);

                    if (double.IsNaN(GLB.myp3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 2])) return false;//空值不执行 
                    GLB.obj[block_num].Depth = (int)GLB.myp3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 2];
                    GLB.obj[block_num].yCenter = (int)GLB.myp3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 1];
                    GLB.obj[block_num].xCenter = (int)GLB.myp3d[(gravity_y * GLB.BUFW + gravity_x) * 3 + 0];
                    //double Angl = Math.Sign(moments.Mu11) * Math.Atan2(moments.Mu20, moments.Mu02) * 57.3;//倾斜角
                }
                else
                {
                    if (double.IsNaN(GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 2])) return false;//空值不执行 
                    GLB.obj[block_num].Depth = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 2];
                    GLB.obj[block_num].yCenter = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 1];
                    GLB.obj[block_num].xCenter = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 0];
                }
                //【2】多点求中心
                //List<Point3> half_contour_pos = new List<Point3>();
                //int step = 10;
                //for (int k = step; k < contour.Size; k += step)
                //{
                //    Point3 Ang_temp = new Point3(0, 0, 0);//0.8视场角点坐标
                //    Ang_temp.X = xc + 0.5 * (contour[k].X - xc);
                //    Ang_temp.Y = yc + 0.5 * (contour[k].Y - yc);


                //    Point p1 = new Point((int)(xc + 0.5 * (contour[k - step].X - xc)), (int)(yc + 0.5 * (contour[k - step].Y - yc)));
                //    Point p2 = new Point((int)(xc + 0.5 * (contour[k].X - xc)), (int)(yc + 0.5 * (contour[k].Y - yc))); ;
                //    MCvScalar mc = new MCvScalar(255, k % 255, 255 - k % 255, 0);
                //    CvInvoke.Line(GLB.frame, p1, p2, mc, 2);//红为头，绿为尾
                //    CvInvoke.Circle(GLB.frame, p1, 3, mc, 4);
                //    CvInvoke.Line(GLB.frame, new Point((int)xc, (int)yc), p2, mc, 2);//半径
                //    if (Ang_temp.Y < 0 || Ang_temp.Y > 960 - 2 || Ang_temp.X < 0 || Ang_temp.X > 1280 - 2)return false;//无效角点
                        
                //    Point3 Ang_point = new Point3(0, 0, 0);//0.8角点坐标               
                //    Ang_point.X = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 0];
                //    Ang_point.Y = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 1];
                //    Ang_point.Z = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 2];
                //    if (double.IsNaN(Ang_point.Z)) continue;//无效角点
                   
                //    half_contour_pos.Add(Ang_point);
                //}
                //GLB.obj[block_num].xCenter = (int)half_contour_pos.Average(o => o.X);
                //GLB.obj[block_num].yCenter = (int)half_contour_pos.Average(o => o.Y);
                //GLB.obj[block_num].Depth = (int)half_contour_pos.Average(o => o.Z);//中心点的深度

                //【3】显示本区块中心[画圆]:
                RotatedRect boxCenter = new RotatedRect(new PointF(xc, yc), new Size(8, 8), 0);
                CvInvoke.Ellipse(GLB.frame, boxCenter, new MCvScalar(0, 255 ,0), 4);//在中心画一个小圆
                CvInvoke.PutText(GLB.frame,"x:"+xc+"y:"+yc+ "Depth=" + GLB.obj[block_num].Depth + "XC=" + GLB.obj[block_num].xCenter + "YC=" + GLB.obj[block_num].yCenter, new System.Drawing.Point(xc - 76, yc + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, 1, new MCvScalar(0, 0, 255), 3);//深度显示
               

                //【4】,绘制外接最小矩形(紧贴连通域):
                for (int i = 0; i < 4; ++i)
                {
                    Point p1 = new Point((int)pt[i].X, (int)pt[i].Y);
                    GLB.obj[block_num].jd.Add(p1);//角点存下备画轨迹用
                    Point p2 = new Point((int)pt[(i + 1) % 4].X, (int)pt[(i + 1) % 4].Y);
                    CvInvoke.Line(GLB.frame, p1, p2, color, 2);
                }
                //【5】真实角点
                List<Point3> pr = new List<Point3>();
                for (int i = 0; i < 4; ++i)
                {
                    Point3 Ang_temp = new Point3(0, 0, 0);//0.8视场角点坐标
                    Ang_temp.X = xc + 0.8 * (pt[i].X - xc);
                    Ang_temp.Y = yc + 0.8 * (pt[i].Y - yc);

                    if (Ang_temp.Y < 0 || Ang_temp.Y > 960 - 2 || Ang_temp.X < 0 || Ang_temp.X > 1280 - 2)
                        return false;//无效角点
                    Point3 Ang_point = new Point3(0, 0, 0);//0.8角点坐标               
                    Ang_point.X = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 0];
                    Ang_point.Y = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 1];
                    Ang_point.Z = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 2];
                    if (double.IsNaN(Ang_point.Z))
                        return false;//无效角点
                    pr.Add(Ang_point);
                    RotatedRect myrect = new RotatedRect(new PointF((float)Ang_temp.X, (float)Ang_temp.Y), new Size(8, 8), 0);
                    CvInvoke.Ellipse(GLB.frame, myrect, new MCvScalar(255, 0, 0), 2);//在角上一个小圆               
                }



                //【6】真实的长轴,短轴,倾角计算:
                double axisLong = 1.25 * Math.Sqrt(Math.Pow(pr[1].X - pr[0].X, 2) + Math.Pow(pr[1].Y - pr[0].Y, 2) + Math.Pow(pr[1].Z - pr[0].Z, 2));
                double axisShort = 1.25 * Math.Sqrt(Math.Pow(pr[2].X - pr[1].X, 2) + Math.Pow(pr[2].Y - pr[1].Y, 2) + Math.Pow(pr[2].Z - pr[1].Z, 2));
                //double Angltemp = box.Angle;//矩形框角度 
                double Angl = 0;
                if (pr[1].X - pr[0].X > 0)
                {
                    double Angl1 = Math.Atan2(pr[1].Y - pr[0].Y, pr[1].X - pr[0].X);
                    double Angl2 = Math.Atan2(pr[2].Y - pr[1].Y, pr[2].X - pr[1].X) + Math.PI / 2.0;
                    double Angl3 = Math.Atan2(pr[3].Y - pr[2].Y, pr[3].X - pr[2].X);
                    double Angl4 = Math.Atan2(pr[0].Y - pr[3].Y, pr[0].X - pr[3].X) + Math.PI / 2.0;
                    Angl = (Angl1 + Angl2 + Angl3 + Angl4) / 4.0;
                }
                else//为正值
                {

                    double Angl1 = Math.Atan2(pr[0].Y - pr[1].Y, pr[0].X - pr[1].X) + Math.PI / 2.0;
                    double Angl2 = Math.Atan2(pr[1].Y - pr[2].Y, pr[1].X - pr[2].X);
                    double Angl3 = Math.Atan2(pr[2].Y - pr[3].Y, pr[2].X - pr[3].X) + Math.PI / 2.0;
                    double Angl4 = Math.Atan2(pr[3].Y - pr[0].Y, pr[3].X - pr[0].X);
                    Angl = (Angl1 + Angl2 + Angl3 + Angl4) / 4.0;
                }
                Angl *= 180d / Math.PI; //换算成角度制
                if (axisShort > axisLong)
                {
                    double temp = axisLong;
                    axisLong = axisShort;
                    axisShort = temp;
                }
                //img_Angle = Angl;//显示区夹角
                if (Math.Abs(Angl) > 45) Angl = Angl + 90;

                if (GLB.img_mode == 0)//样品匹配时用//转换到机器人坐标
                {
                    if (Angl >= 90)//控制旋转范围
                    {
                        Angl -= 180;
                    }
                    if (Angl <= -90)
                    {
                        Angl += 180;
                    }
                }
                GLB.obj[block_num].Angle = Angl;//旋转角
                GLB.obj[block_num].axisLong = axisLong;//长轴
                GLB.obj[block_num].axisShort = axisShort;//短轴
                GLB.obj[block_num].L2S = axisLong / axisShort;//长短轴之比;
                GLB.obj[block_num].SZ = axisLong * axisShort; //估算的物件尺寸

                //像尺寸显示:
                CvInvoke.PutText(GLB.frame, "Lr=" + (int)axisLong + ",Sr=" + (int)axisShort, new System.Drawing.Point((int)pt[2].X, (int)pt[2].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, color, 2);
                CvInvoke.PutText(GLB.frame, "Angl=" + Angl, new System.Drawing.Point((int)pt[3].X, (int)pt[3].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, color, 2);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取最小外接矩形并处理相关参数
        /// </summary>
        /// <param name="contour">对应轮廓</param>
        /// <param name="block_num">编号</param>
        public bool getMinAreaRect11(VectorOfPoint contour, int block_num)
        {
            //本区块颜色定义:
            MCvScalar color = new MCvScalar(53 * block_num % 255, 255 - 73 * block_num % 255, 93 * block_num % 255);//B,G,R

            //1,获取最小外接矩形中心点的深度:
            try
            {
            RotatedRect box = CvInvoke.MinAreaRect(contour); //最小外接矩形
            PointF[] pt = CvInvoke.BoxPoints(box);//最小外接矩形四个角点           
         
            PointF po = box.Center;//最小外接矩形中心
            int xc = (int)po.X;//最小外接矩形中心X坐标
            int yc = (int)po.Y;//最小外接矩形中心Y坐标            

            if (double.IsNaN(GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 2])) return false;//空值不执行
                
            GLB.obj[block_num].Depth = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 2];
            GLB.obj[block_num].yCenter = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 1];
            GLB.obj[block_num].xCenter = (int)GLB.myp3d[(yc * GLB.BUFW + xc) * 3 + 0];

            //1b,绘制外接最小矩形(紧贴连通域):
            for (int i = 0; i < 4; ++i)
            {
                Point p1 = new Point((int)pt[i].X, (int)pt[i].Y);
                GLB.obj[block_num].jd.Add(p1);//角点存下备画轨迹用
                Point p2 = new Point((int)pt[(i + 1) % 4].X, (int)pt[(i + 1) % 4].Y);
                CvInvoke.Line(GLB.frame, p1, p2, color, 2);
            }

            List<Point3> pr = new List<Point3>();//真实角点
            for (int i = 0; i < 4; ++i)
            {
                Point3 Ang_temp = new Point3(0, 0, 0);//0.8视场角点坐标
                Ang_temp.X = xc + 0.8 * (pt[i].X - xc);
                Ang_temp.Y = yc + 0.8 * (pt[i].Y - yc);

                if (Ang_temp.Y < 0 || Ang_temp.Y > 960-2 || Ang_temp.X < 0 || Ang_temp.X >1280-2)
                    return false;//无效角点
                Point3 Ang_point = new Point3(0, 0, 0);//0.8角点坐标               
                Ang_point.X = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 0];
                Ang_point.Y = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 1];
                Ang_point.Z = GLB.myp3d[((int)Ang_temp.Y * GLB.BUFW + (int)Ang_temp.X) * 3 + 2];
                if (double.IsNaN(Ang_point.Z))
                    return false;//无效角点
                pr.Add(Ang_point);
                RotatedRect myrect = new RotatedRect(new PointF((float)Ang_temp.X, (float)Ang_temp.Y), new Size(8, 8), 0);
                CvInvoke.Ellipse(GLB.frame, myrect, new MCvScalar(255, 0, 0), 2);//在角上一个小圆               
            }

            //GLB.obj[block_num].xCenter = (int)pr.Average(o => o.X);
            //GLB.obj[block_num].yCenter = (int)pr.Average(o => o.Y);
            //GLB.obj[block_num].Depth = (int)pr.Average(o => o.Z);//中心点的深度
            //1a,显示本区块中心[画圆]:
            RotatedRect boxCenter = new RotatedRect(new PointF(xc, yc), new Size(8, 8), 0);
            CvInvoke.Ellipse(GLB.frame, boxCenter, new MCvScalar(0, 255, 255), 4);//在中心画一个小圆
            CvInvoke.PutText(GLB.frame, "Depth=" + GLB.obj[block_num].Depth + "XC=" + GLB.obj[block_num].xCenter + "YC=" + GLB.obj[block_num].yCenter, new System.Drawing.Point(xc - 76, yc + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(0, 0, 255), 2);//深度显示
                                                                                                                                                                                                                                                                        //CvInvoke.PutText(frame, "Depth=" + GLB.obj[block_num].Depth , new System.Drawing.Point(xc - 76, yc + 25), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, new MCvScalar(0, 0, 255), 2);//深度显示


            //真实的长轴,短轴,倾角计算:
            double axisLong = 1.25 * Math.Sqrt(Math.Pow(pr[1].X - pr[0].X, 2) + Math.Pow(pr[1].Y - pr[0].Y, 2) + Math.Pow(pr[1].Z - pr[0].Z, 2));
            double axisShort = 1.25 * Math.Sqrt(Math.Pow(pr[2].X - pr[1].X, 2) + Math.Pow(pr[2].Y - pr[1].Y, 2) + Math.Pow(pr[2].Z - pr[1].Z, 2));
            double Angl=box.Angle;//矩形框角度
           

            if (axisShort > axisLong)
            {
                double temp = axisLong;
                axisLong = axisShort;
                axisShort = temp;
            }
            //img_Angle = Angl;//显示区夹角
            if (Math.Abs(Angl) > 45) Angl = Angl + 90;

            if (GLB.img_mode == 0)//样品匹配时用//转换到机器人坐标
            {
                if (Angl >= 90)//控制旋转范围
                {
                    Angl -= 180;
                }
                if (Angl <= -90)
                {
                    Angl += 180;
                }
            }
            GLB.obj[block_num].Angle = Angl;//旋转角
            GLB.obj[block_num].axisLong = axisLong;//长轴
            GLB.obj[block_num].axisShort = axisShort;//短轴
            GLB.obj[block_num].L2S = axisLong / axisShort;//长短轴之比;
            GLB.obj[block_num].SZ = axisLong * axisShort; //估算的物件尺寸

            //像尺寸显示:
            CvInvoke.PutText(GLB.frame, "Lr=" + (int)axisLong + ",Sr=" + (int)axisShort, new System.Drawing.Point((int)pt[2].X, (int)pt[2].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, color, 2);
            CvInvoke.PutText(GLB.frame, "Angl=" + Angl, new System.Drawing.Point((int)pt[3].X, (int)pt[3].Y), Emgu.CV.CvEnum.FontFace.HersheyDuplex, .75, color, 2);
            return true;
            }
            catch
            {
               // MessageBox.Show("有异常");
                return false;
            }
        }

        /// <summary>
        /// 多工件识别:
        /// </summary>
        /// <param name="block_num"></param>
        /// <returns></returns>
        public bool Device_Macth(int block_num)
        {
            string filename = "";
            if (Mainform.runMode == 1 && Mainform.ProduceArrive == true) filename = "obj_info.txt";//工件信息
            else if (Mainform.runMode == 6) filename = "zone_info.txt";//托盘信息
           
            string[] text = fileOperation.ReadTxtFile(filename);
            //MessageBox.Show("样品数目="+text.Length );
            if (text.Length >= 1)
            {
                string[] sa1 = new string[5];
                double deviation = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    sa1 = text[i].Split(' ');//分离参数
                }
                if (sa1.Length == 5)
                {
                    deviation += Math.Abs(Double.Parse(sa1[1]) - GLB.obj[block_num].axisLong) / GLB.obj[block_num].axisLong;
                    deviation += Math.Abs(Double.Parse(sa1[2]) - GLB.obj[block_num].axisShort) / GLB.obj[block_num].axisShort;
                    deviation += Math.Abs(Double.Parse(sa1[3]) - GLB.obj[block_num].L2S) / GLB.obj[block_num].L2S;//长短边比值
                    deviation += Math.Abs(Double.Parse(sa1[4]) - GLB.obj[block_num].SZ) / GLB.obj[block_num].SZ;//面积
                }

                if (deviation <= 0 || deviation > 0.2 || double.IsNaN(deviation))
                {
                    GLB.Match_success = false; ;//偏差过大不执行   
                 
                }
                else
                {
                    
                    if (GLB.img_mode == 0)
                    {
                        ////记录工件的三维点，旋转角,法向量
                        GLB.camera_device_point.X = GLB.obj[block_num].xCenter;
                        GLB.camera_device_point.Y = GLB.obj[block_num].yCenter;
                        GLB.camera_device_point.Z = GLB.obj[block_num].Depth;
                        GLB.device_angl = (float)GLB.obj[block_num].Angle;
                        //device_norm = GLB.obj[block_num].Norm;               
                  
                        GLB.TitleStr += "匹配：" + deviation + sa1[0] + "匹配成功";
                        GLB.Match_success = true;
                    }
                    else {
                        GLB.Match_success = false;
                       
                    }
                }
            }
            else
            {
                //MessageBox.Show("样品数目太少:" + text.Length);
                GLB.Match_success = false;
            }
            return GLB.Match_success;
        }
    }
}
