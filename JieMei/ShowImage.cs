using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
namespace JieMei
{
    public class ShowImage
    {
        DealWithImage dealWithImage = new DealWithImage();
        /// <summary>
        /// 0-彩色方式显示 : -------------------------------------
        /// </summary>
        /// <param name="mycolor">3D坐标图像</param>
        /// <param name="ptb">图像容器</param>
        public void DisplayColorImg(byte[] mycolor, PictureBox ptb)
        {
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    GLB.frame.Data[i, j, 2] = mycolor[(i * GLB.BUFW + j) * 3 + 2];
                    GLB.frame.Data[i, j, 1] = mycolor[(i * GLB.BUFW + j) * 3 + 1];
                    GLB.frame.Data[i, j, 0] = mycolor[(i * GLB.BUFW + j) * 3 + 0];
                }
            }
            if (GLB.frame.Data != null) ptb.Image = GLB.frame.ToBitmap();
        }

        /// <summary>
        ///  1-深度方式显示  :-------------------------------------
        /// </summary>
        /// <param name="myp3d">3D坐标图像</param>
        /// <param name="ptb">图像容器</param>
        public void DisplayDepthImg(float[] myp3d, PictureBox ptb)
        {
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    //伪彩色效果1:
                    //float xx = myp3d[3 * (i * BUFW + j) + 0];//取得realX
                    //float yy = myp3d[3 * (i * BUFW + j) + 1];//取得realY
                    //float zz = myp3d[3 * (i * BUFW + j) + 2];//取得realZ                    
                    //frame.Data[i, j, 0] = (byte)(128 + 127 * Math.Sin(xx / 1.7f));  //B
                    //frame.Data[i, j, 1] = (byte)(128 + 127 * Math.Sin(yy / 1.7f));  //G
                    //frame.Data[i, j, 2] = (byte)(128 + 127 * Math.Sin(zz / 3f));  //R

                    //伪彩色效果2:
                    byte v = (byte)(myp3d[3 * (i * GLB.BUFW + j) + 2] % 255);
                    GLB.frame.Data[i, j, 0] = v;  //B
                    GLB.frame.Data[i, j, 1] = v;  //G
                    GLB.frame.Data[i, j, 2] = v;  //R
                }
            }
            if (GLB.frame.Data != null) ptb.Image = GLB.frame.ToBitmap();   //pictureBox1.Image = frame.Flip(FlipType.Vertical).ToBitmap();           
        }

        /// <summary>
        /// 2-单样品学习:-------------------------------------
        /// </summary>
        /// <param name="myp3d">3D图像</param>
        /// <param name="ptb">图像容器</param>
        public void DisplayDepthImg2(float[] myp3d, PictureBox ptb,TextBox txtTypeName)
        {
            float black_area_rate = 0;//黑区统计
            float dis;//距离
            int pix_sum = 0;//记录像素个数
            GLB.frame.SetValue(new Bgr(255, 255, 255).MCvScalar);//清除显示区           
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 2] = myp3d[(i * GLB.BUFW + j) * 3 + 2];
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 1] = myp3d[(i * GLB.BUFW + j) * 3 + 1];
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 0] = myp3d[(i * GLB.BUFW + j) * 3 + 0];

                    dis = GLB.myp3d[(i * GLB.BUFW + j) * 3 + 2];

                    if (dis > 500 && dis < GLB.Sample_Distance) //dis: 约离镜头50cm.
                    {
                        GLB.frame.Data[i, j, 0] = (byte)(GLB.Sample_Distance % 255);  //B
                        GLB.frame.Data[i, j, 1] = (byte)(GLB.Sample_Distance % 155);  //G
                        GLB.frame.Data[i, j, 2] = (byte)(GLB.Sample_Distance % 55);  //R  
                        pix_sum++;//记录像素个数
                    }

                }
            }
            black_area_rate = (float)(pix_sum / (float)(GLB.BUFW * GLB.BUFH));
            GLB.TitleStr = "当前采样深度:" + GLB.Sample_Distance + ",黑区占有:" + (10000 * black_area_rate / 100) + "%";

            //限制面积获取1区块的轮廓:
            if (black_area_rate > 1 / 50f && black_area_rate < 1 / 2f)
            {
                dealWithImage.getContours(txtTypeName,ptb);
            }
            if (GLB.frame.Data != null) ptb.Image = GLB.frame.ToBitmap();
        }

        /// <summary>
        /// 匹配显示
        /// </summary>
        /// <param name="myp3d">3D坐标图像</param>
        /// <param name="ptb">图像容器</param>
        public void DisplayDepthImg3(float[] myp3d, PictureBox ptb,TextBox txtTypeName)
        {
            GLB.TitleStr = "";
            float dis_temp;
            //frame.SetValue(new Bgr(255, 255, 255).MCvScalar);//清除显示区  
            for (int i = 0; i < GLB.BUFH; i++)
            {
                for (int j = 0; j < GLB.BUFW; j++)
                {
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 2] = myp3d[(i * GLB.BUFW + j) * 3 + 2];//深度数据
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 1] = myp3d[(i * GLB.BUFW + j) * 3 + 1];
                    GLB.myp3d[(i * GLB.BUFW + j) * 3 + 0] = myp3d[(i * GLB.BUFW + j) * 3 + 0];
                }
            }

            int d = 2;
            int depth_temp;//检测最远的距离
            if (Mainform.runMode == 1) depth_temp = 1450;
            else  depth_temp = 1550;
            for (int i = d; i < GLB.BUFH - d; i += d)
            {
                for (int j = d; j < GLB.BUFW - d; j += d)
                {
                    float z_center = myp3d[(i * GLB.BUFW + j) * 3 + 2];
                    if (z_center > 0 && z_center < depth_temp)//小于托盘的深度
                    {

                        float v1 = myp3d[3 * ((i - d) * GLB.BUFW + j - d) + 2];
                        float v2 = myp3d[3 * ((i - d) * GLB.BUFW + j) + 2];
                        float v3 = myp3d[3 * ((i - d) * GLB.BUFW + j + d) + 2];
                        float v4 = myp3d[3 * (i * GLB.BUFW + j - d) + 2];

                        float v6 = myp3d[3 * (i * GLB.BUFW + j + d) + 2];
                        float v7 = myp3d[3 * ((i + d) * GLB.BUFW + j - d) + 2];
                        float v8 = myp3d[3 * ((i + d) * GLB.BUFW + j) + 2];
                        float v9 = myp3d[3 * ((i + d) * GLB.BUFW + j + d) + 2];

                        if (double.IsNaN(v1) || double.IsNaN(v2) || double.IsNaN(v3) || double.IsNaN(v4)
                           || double.IsNaN(v6) || double.IsNaN(v7) || double.IsNaN(v8) || double.IsNaN(v9)) dis_temp = 0;


                        dis_temp = Math.Abs(v1 - z_center) + Math.Abs(v2 - z_center) + Math.Abs(v3 - z_center) + Math.Abs(v4 - z_center)
                           + Math.Abs(v6 - z_center) + Math.Abs(v7 - z_center) + Math.Abs(v8 - z_center) + Math.Abs(v9 - z_center);//双线斜率绝对值之和
                        dis_temp = dis_temp <50 ? 255 : 0;
                    }
                    else
                    {
                        dis_temp = 0;
                    }
                    //伪彩色效果: 
                    for (int dy = -d / 2; dy < d / 2; dy++)
                    {
                        for (int dx = -d / 2; dx < d / 2; dx++)
                        {
                            GLB.frame.Data[i + dx, j + dy, 0] = (byte)(dis_temp);   //B
                            GLB.frame.Data[i + dx, j + dy, 1] = (byte)(dis_temp);   //G
                            GLB.frame.Data[i + dx, j + dy, 2] = (byte)(dis_temp);   //R
                        }
                    }
                }
            }

            //获取各区块的轮廓:
            dealWithImage.getContours(txtTypeName, ptb);
            //RotatedRect myrect = new RotatedRect(new PointF((float)GLB.BUFW / 2, (float)GLB.BUFH / 2), new Size(8, 8), 0);
            //CvInvoke.Ellipse(GLB.frame, myrect, new MCvScalar(255, 0, 0), 5);//在角上一个小圆
           if (GLB.frame.Data != null) ptb.Image = GLB.frame.ToBitmap();
        }
    }
}
