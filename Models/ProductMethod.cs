using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Models
{
    public class ProductMethod
    {

        /// <summary>
        /// 保存条码
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="barcode">条码</param>
        public int saveStowCode(ProductInfo product,string barcode)
        {
            string sql = "insert into saveStowCode (barcode,stowid,completeStowCode,num,stowdatetime)values('"
                + barcode + "','" + product.RobotID + "','" + product.completeStowCode + "','" + product.CurrentCounts + "'," + "GETDATE())";
            int i = MyDataLib.ExecNoneQueryBySql(sql);
            return i;
        }
        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="objProduct">要添加的产品</param>
        /// <param name="objListProduct">产品列表</param>
        public void AddProduct(ProductInfo objProduct,List<ProductInfo> objListProduct)
        {
            try
            {
                for (int i = 0; i < objListProduct.Count; i++)
                {
                    if (objListProduct[i].Zone.Equals(objProduct.Zone))
                    {
                        //存在删除旧的
                        objListProduct.RemoveAt(i);
                    }
                }
                objListProduct.Add(objProduct);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 堆垛完成进行存储
        /// </summary>
        /// <param name="missionID">批号，</param>
        /// <param name="zone">区域，</param>
        /// <param name="robotID">堆垛机器人编号</param>
        /// <returns></returns>
        public int addProNumToLib(string missionID,int zone,string robotID)
        {

            string sql = "exec SetStowInfoByBarCode '" + missionID + "','" + zone + "','" + robotID + "'";
            int i = MyDataLib.ExecNoneQueryBySql(sql);
            return i;
        }
        /// <summary>
        ///添加新产品-----从界面更新-》数据库
        /// </summary>
        /// <param name="myRobotID"></param>
        /// <param name="objListProduct"></param>
        public int updateMyDataLib(ProductInfo product)
        {
            string sql = "exec insertStowMissionn_list '" + product.MissionID + "','" + product.MaterielCode + "','" + product.Customer
                + "','" + product.Spec + "','" + product.Total.ToString() + "','" + product.Zone.ToString() + "','" + product.RobotID + "','" + product.completeStowCode + "'";

            int i = MyDataLib.ExecNoneQueryBySql(sql);
            return i;
        }
        //修改产品
        public int changeProInfo(ProductInfo product)
        {
            string sql = "update StowMissionn_list set Fupqty='" + product.ComepleteNum.ToString() + "',Fqty='" + product.Total.ToString() 
                + "',fTuoQty='" + product.CurrentCounts.ToString() + "',completeStowCode='" + product.completeStowCode + "' where area='" + product.Zone.ToString() + "'";
                           
            int i = MyDataLib.ExecNoneQueryBySql(sql);
            return i;
        }
        /// <summary>
        /// 从数据库更新-》界面
        /// </summary>
        /// <param name="objListProduct"></param>
        public List<ProductInfo> UpdateProductFromDataLib(List<ProductInfo> objListProduct, string RobotId)
        {
            string sql = "select * from dbo.StowMissionn_list where  StowNo='" + RobotId + "' and yxbz='Y'";//通过机器人ID查找
            DataTable dt = MyDataLib.SearchItem(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objListProduct.Add(new ProductInfo()
                    {
                        RobotID = dt.Rows[i]["StowNo"].ToString(),//机器人ID 
                        completeStowCode = dt.Rows[i]["completeStowCode"].ToString(),//完成堆垛托号
                        MissionID = dt.Rows[i]["TaskNo"].ToString(),//任务单号
                        Customer = dt.Rows[i]["Customer"].ToString(),//客户
                        Zone = int.Parse(dt.Rows[i]["area"].ToString()),//垛区                            
                        Total = int.Parse(dt.Rows[i]["Fqty"].ToString()),//总数
                        ComepleteNum = int.Parse(dt.Rows[i]["Fupqty"].ToString()),//完成数量
                        CurrentCounts = int.Parse(dt.Rows[i]["fTuoQty"].ToString()),//已码数量                       
                        //PerZoneNumbers = int.Parse(dt.Rows[i]["PerZoneNum"].ToString()),//每垛数量
                        //Direction = int.Parse(dt.Rows[i]["Direction"].ToString()),//方向
                        MaterielCode = dt.Rows[i]["fnumber"].ToString(),//物料代码
                        Spec = dt.Rows[i]["spec"].ToString(),//规格                         
                    });
                }              
            }
            return objListProduct;
        }
      
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="objProduct">要修改的产品</param>
        /// <param name="objListProduct">产品列表</param>
        public void ChangeProduct(ProductInfo Product, List<ProductInfo> objListProduct)
        {
            try
            {
                for (int i = 0; i < objListProduct.Count; i++)
                {
                    if (objListProduct[i].Zone.Equals(Product.Zone))//对应垛
                    {

                        //删除旧的
                        objListProduct.RemoveAt(i);
                        //添加修改的
                        objListProduct.Insert(i, Product);
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="objProduct">要删除的产品</param>
        /// <param name="objListProduct">产品列表</param>
        public void DeleteProduct(ProductInfo Product, List<ProductInfo> objListProduct)
        {
            try
            {
                for (int i = 0; i < objListProduct.Count; i++)
                {
                    if (objListProduct[i].Zone.Equals(Product.Zone))//对应垛
                    {
                        objListProduct.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 查找对应垛区的数量
        /// </summary>
        /// <param name="Zone">垛区</param>
        /// <param name="objList">产品列表</param>
        /// <returns></returns>
        public ProductInfo GetProductByZone(int Zone, List<ProductInfo> objList)
        {
            ProductInfo objProduct = new ProductInfo();
            //遍历List
            foreach (ProductInfo item in objList)
            {
                if (item.Zone.Equals(Zone))//对应垛
                {
                    objProduct = new ProductInfo
                    {
                        RobotID = item.RobotID,//机器人ID
                        completeStowCode =item.completeStowCode,//完成堆垛托号
                        MissionID = item.MissionID,//任务单号
                        Zone = item.Zone,//垛区
                        Customer = item.Customer,//客户
                        Total = item.Total,//总数 
                        ComepleteNum = item.ComepleteNum,//完成数量
                        CurrentCounts = item.CurrentCounts,//已码数量
                        PerZoneNumbers = item.PerZoneNumbers,//每垛数量
                        Direction = item.Direction,//方向   
                        Spec = item.Spec,//规格                        
                        MaterielCode = item.MaterielCode,//物料代码
                    };
                    break;
                }
            }
            return objProduct;
        }
        /// <summary>
        /// 根据id获取一条产品信息
        /// </summary>
        /// <param name="id">产品编号</param>
        /// <param name="objList">产品列表</param>
        /// <returns></returns>
        public ProductInfo GetProductByMissionID(string id, List<ProductInfo> objList)
        {
            ProductInfo objProduct = new ProductInfo();
            //遍历List
            foreach (ProductInfo item in objList)
            {
                if (item.MissionID.Equals(id))
                {
                    objProduct = new ProductInfo
                    {
                        RobotID = item.RobotID,//机器人ID
                        completeStowCode = item.completeStowCode,//完成堆垛托号
                        MissionID = item.MissionID,//任务单号
                        Zone = item.Zone,//垛区
                        Customer = item.Customer,//客户
                        Total = item.Total,//总数 
                        ComepleteNum = item.ComepleteNum,//完成数量
                        CurrentCounts = item.CurrentCounts,//已码数量
                        PerZoneNumbers = item.PerZoneNumbers,//每垛数量
                        Direction = item.Direction,//方向   
                        Spec = item.Spec,//规格                        
                        MaterielCode = item.MaterielCode,//物料代码
                    };
                    break;
                }
            }
            return objProduct;
        }
        /// <summary>
        /// 根据MaterielCode获取一条产品信息
        /// </summary>
        /// <param name="id">产品编号</param>
        /// <param name="objList">产品列表</param>
        /// <returns></returns>
        public ProductInfo GetProductByMaterielCode(string MaterielCode, List<ProductInfo> objList)
        {
            ProductInfo objProduct = new ProductInfo();
            //遍历List
            foreach (ProductInfo item in objList)
            {
                if (item.MaterielCode.Equals(MaterielCode))
                {
                    objProduct = new ProductInfo
                    {
                        RobotID = item.RobotID,//机器人ID
                        completeStowCode = item.completeStowCode,//完成堆垛托号
                        MissionID = item.MissionID,//任务单号
                        Zone = item.Zone,//垛区
                        Customer = item.Customer,//客户
                        Total = item.Total,//总数 
                        ComepleteNum = item.ComepleteNum,//完成数量
                        CurrentCounts = item.CurrentCounts,//已码数量
                        PerZoneNumbers = item.PerZoneNumbers,//每垛数量
                        Direction = item.Direction,//方向   
                        Spec = item.Spec,//规格                        
                        MaterielCode = item.MaterielCode,//物料代码
                    };
                    break;
                }
            }
            return objProduct;
        }
    }
}
