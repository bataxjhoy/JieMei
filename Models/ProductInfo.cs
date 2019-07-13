using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ProductInfo
    {
        public string RobotID { get; set; }//机器人ID
        public string MissionID { get; set; }//任务单号
        public int Zone { get; set; }//垛区      
        public string Customer { get; set; }//客户        
        public int Total { get; set; }//总数
        public int ComepleteNum { get; set; }//完成数量
        public int CurrentCounts { get; set; }//已码数量
        public int PerZoneNumbers { get; set; }//每垛数量
        public int Direction { get; set; }//方向
        public string Spec { get; set; }//规格
        public string MaterielCode { get; set; }//物料代码
        public string completeStowCode { get; set; }//完成的拖号
    }
}
