using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using System.Data.SqlClient;
namespace JieMei
{
    public partial class ProductSetForm : Form
    {

        private ProductMethod productMethod = new ProductMethod();//实例化产品操作的类
        private int actionFlag = 0;//识别是修改还是添加操作的标志，如果是   1---》添加      2---》修改
        public ProductSetForm()
        {
            InitializeComponent();
        }

        private void ProductSetForm_Load(object sender, EventArgs e)
        {
            gpbProductSet.Enabled = false;
            //产品设置更新到界面
            btnUpdate_Click(null, null);
        }
        private void ProductSetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //初始化每垛的数量
            for (int i = 0; i < 6; i++)
            {
                ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
            }
        }
        /// <summary>
        /// 加载信息到dgv
        /// </summary>
        /// <param name="product"></param>
        private void LoadDataToDetail(ProductInfo product)
        {
            txtRobotID.Text = product.RobotID;//机器人编号
            txtcompleteStowCode.Text = product.completeStowCode;//完成的托号
            txtMissionID.Text = product.MissionID.ToString();//任务单号
            txtCustomer.Text = product.Customer;//客户
            txtProZone.Text = product.Zone.ToString();//所属垛区
            txtProTotal.Text = product.Total.ToString();//总数
            txtComepleteNum.Text = product.ComepleteNum.ToString();//完成数量
            txtProNum.Text = product.PerZoneNumbers.ToString();//每垛数量
            txtProDirection.Text = product.Direction.ToString();//方向
            txtCurrentCounts.Text = product.CurrentCounts.ToString();//每垛当前数量 
            txtMaterielCode.Text = product.MaterielCode.ToString();//物料代码
            txtSpec.Text = product.Spec.ToString();//规格
            if (!string.IsNullOrEmpty(txtMissionID.Text)) txtMissionID_TextChanged(null, null);//查询
            else txtMaterielCode_TextChanged(null, null);//查询
        }
       
        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            txtRobotID.Text = "";//机器人编号
            txtcompleteStowCode.Text = "";//完成堆垛托号
            txtMissionID.Text = "";//任务单号
            txtCustomer.Text = "";//客户
            txtProZone.Text = "";//所属垛区
            txtProTotal.Text = "";//总数
            txtComepleteNum.Text = "0";//完成数量                
            txtCurrentCounts.Text = "0";//当前数量
            txtProNum.Text = "";//每垛数量
            txtProDirection.Text = "";//方向
            txtMaterielCode.Text = "";//物料代码
            txtSpec.Text = "";//规格
            gpbProductSet.Enabled = true;
            btnUpdate.Enabled = false;
            btnChangeProduct.Enabled = false;
            btnDeleteProduct.Enabled = false;
            dgvProductSetting.Enabled = false;
            txtComepleteNum.ReadOnly=true;//完成数量                
            txtCurrentCounts.ReadOnly=true;//当前数量
            actionFlag = 1;//添加标志
        }
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeProduct_Click(object sender, EventArgs e)
        {
            gpbProductSet.Enabled = true;
            btnUpdate.Enabled = false;
            btnAddProduct.Enabled = false;
            btnDeleteProduct.Enabled = false;
            dgvProductSetting.Enabled = false;
            actionFlag = 2;//修改标志
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProductSetting.Rows.Count == 0)
            {
                MessageBox.Show("没有产品信息可以删除！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (dgvProductSetting.CurrentRow.Selected == false)
            {
                MessageBox.Show("请选择要删除的产品信息条目！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                
                ProductInfo objProduct;
                //通过夺取查找
               objProduct = productMethod.GetProductByZone(int.Parse(dgvProductSetting.CurrentRow.Cells[5].Value.ToString()), GLB.objListProduct);
                string info = "您确定要删除信息【任务单号：" + dgvProductSetting.CurrentRow.Cells[2].Value.ToString() + "垛区："
                    + dgvProductSetting.CurrentRow.Cells[5].Value.ToString() + "】信息吗？";
                DialogResult result = MessageBox.Show(info, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        //执行删除动作
                        productMethod.DeleteProduct(objProduct, GLB.objListProduct);
                        //待加数据库删除
                        string sql = "delete from dbo.StowMissionn_list where StowNo='" + objProduct.RobotID + "'and TaskNo='" + objProduct.MissionID + "'and area='" + objProduct.Zone + "' and yxbz='Y'";
                        int i = MyDataLib.ExecNoneQueryBySql(sql);
                       
                        //提示删除成功
                        MessageBox.Show("删除成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        btnCancel_Click(null, null); //更新界面数据
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败，具体原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else return;
            }
        }
        /// <summary>
        /// 从数据库更新-》界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GLB.objListProduct.Clear();
            try
            {
                productMethod.UpdateProductFromDataLib(GLB.objListProduct, GLB.RobotId);
                //把读取到的List<Student>展示在DataGridView中
                dgvProductSetting.DataSource = null;
                dgvProductSetting.AutoGenerateColumns = false;//只显示绑定的字段，不会自动填充字段
                dgvProductSetting.DataSource = GLB.objListProduct;
                //展示第一行
                ProductInfo product = productMethod.GetProductByZone(int.Parse(dgvProductSetting.CurrentRow.Cells[5].Value.ToString()), GLB.objListProduct);
                LoadDataToDetail(product);
                gpbProductSet.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据失败，具体原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtRobotID.Text))
            {
                MessageBox.Show("机器人ID不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtcompleteStowCode.Text))
            {
                MessageBox.Show("完成堆垛托号不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtMaterielCode.Text))
            {
                MessageBox.Show("物料代码不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtCustomer.Text))
            {
                MessageBox.Show("客户名称不能为空,不为零", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtProTotal.Text) || int.Parse(txtProTotal.Text)==0)
            {
                MessageBox.Show("产品总量不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtComepleteNum.Text))
            {
                MessageBox.Show("完成数量不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtProNum.Text))
            {
                MessageBox.Show("每垛可堆产品数量不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtProZone.Text))
            {
                MessageBox.Show("产品所属垛区不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtProDirection.Text))
            {
                MessageBox.Show("产品码垛方向不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtCurrentCounts.Text))
            {
                MessageBox.Show("产品已经码垛数量不能为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!string.IsNullOrEmpty(txtProZone.Text) && GLB.zoneCheckStatus[int.Parse(txtProZone.Text)] == false)
            {
                MessageBox.Show("该托盘没有识别位置成功！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
          
            ProductInfo product = new ProductInfo
            {
                RobotID = txtRobotID.Text.Trim().ToString(),//机器人编号               
                completeStowCode= txtcompleteStowCode.Text.Trim(),//完成堆垛托号
                MissionID = txtMissionID.Text.Trim().ToString(),//任务单号
                Customer = txtCustomer.Text.Trim().ToString(),//客户
                Zone = int.Parse(txtProZone.Text.Trim()),//垛区
                Total = (int)double.Parse(txtProTotal.Text.Trim()),//总数
                ComepleteNum = int.Parse(txtComepleteNum.Text.Trim()),//完成数量                
                CurrentCounts = int.Parse(txtCurrentCounts.Text.Trim()),//已码数量
                PerZoneNumbers = int.Parse(txtProNum.Text.Trim()),//每垛数量                
                Direction = int.Parse(txtProDirection.Text.Trim()),//方向              
                MaterielCode = txtMaterielCode.Text.Trim().ToString(),//物料代码
                Spec = txtSpec.Text.Trim().ToString(),//规格
            };
            DialogResult result = MessageBox.Show("是否覆盖第"+txtProZone.Text.Trim()+"垛区订单？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            if (product.Zone==0)
            {
                if (product.CurrentCounts == 0) 
                {
                    //删除不良品记录
                    string sql = "delete from StowNgStatus ";
                    MyDataLib.ExecNoneQueryBySql(sql);
                    MessageBox.Show("确保不良区已经清空？", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else if (int.Parse(txtCurrentCounts.Text) > int.Parse(txtProNum.Text))
            {
                MessageBox.Show("垛区当前产品数量大于每垛产品总数量", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            switch (actionFlag)
            {
                case 1://Add
                    try
                    {
                        productMethod.AddProduct(product, GLB.objListProduct);
                        gpbProductSet.Enabled = false;
                        MessageBox.Show("新增产品信息成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnCancel_Click(null, null); //更新界面数据
                        productMethod.updateMyDataLib(product);//添加新产品---更新数据库
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("添加失败，具体原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case 2://Modify
                    try
                    {
                        productMethod.ChangeProduct(product, GLB.objListProduct);
                        gpbProductSet.Enabled = false;
                        MessageBox.Show("修改产品信息成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnCancel_Click(null, null); //更新界面数据
                        productMethod.changeProInfo(product);//修改产品--更新数据库
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("修改失败，具体原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 取消+界面更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //更新数据
            dgvProductSetting.DataSource = null;
            dgvProductSetting.AutoGenerateColumns = false;//只显示绑定的字段，不会自动填充字段
            dgvProductSetting.DataSource = GLB.objListProduct;

            if (GLB.objListProduct.Count != 0)
            {
                //展示第一行
                ProductInfo product;
               product = productMethod.GetProductByZone(int.Parse (dgvProductSetting.CurrentRow.Cells[5].Value.ToString()), GLB.objListProduct);//第一行明细

                LoadDataToDetail(product);
            }
            for (int i = 0; i < 6; i++)//初始化每垛的数量
            {
                ProductInfo product = productMethod.GetProductByZone(i, GLB.objListProduct);
                GLB.zoneCrrentNumTemp[i] = product.CurrentCounts;
            }
            //使控件有效
            btnUpdate.Enabled = true;
            btnAddProduct.Enabled = true;
            btnDeleteProduct.Enabled = true;
            btnChangeProduct.Enabled = true;
            gpbProductSet.Enabled = false;
            dgvProductSetting.Enabled = true;
            txtComepleteNum.ReadOnly = false;//完成数量                
            txtCurrentCounts.ReadOnly = false;//当前数量
        }
        /// <summary>
        /// 选择行发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductSetting_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductSetting.CurrentRow.Selected == false)
            {
                return;
            }
            else if (dgvProductSetting.Rows.Count == 0)
            {
                return;
            }
            else
            {
                //展示选中行
                ProductInfo product;               
                product = productMethod.GetProductByZone(int.Parse(dgvProductSetting.CurrentRow.Cells[5].Value.ToString()), GLB.objListProduct);
                   
                LoadDataToDetail(product);
            }
        }
        /// <summary>
        /// 通过任务单号查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMissionID_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMissionID.Text))
            {
                string sql = " exec GetdataByTaskNo '" + txtMissionID.Text + "'";
                using (SqlConnection conn = new SqlConnection(MyDataLib.ConnString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())//判断有数据
                    {
                        txtMaterielCode.Text = reader["fnumber"].ToString();//物料单号
                        txtCustomer.Text = reader["f_102"].ToString();//客户
                        txtSpec.Text = reader["fmodel"].ToString();//规格
                        txtProTotal.Text = reader["FAuxQty"].ToString();//数量
                        txtProNum.Text = reader["mtsl"].ToString();//每托数量
                        txtProDirection.Text = reader["direction"].ToString();//方向
                    }
                    else
                    {
                        return;//返回
                    }
                }
            }
        }

        /// <summary>
        /// 通过物料代码查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaterielCode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaterielCode.Text))
            {
                string sql = " exec GetdataByTaskNo '','" + txtMaterielCode.Text + "'";
                using (SqlConnection conn = new SqlConnection(MyDataLib.ConnString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())//判断有数据
                    {
                        txtMaterielCode.Text = reader["fnumber"].ToString();//物料单号
                        txtCustomer.Text = reader["f_102"].ToString();//客户
                        txtSpec.Text = reader["fmodel"].ToString();//规格
                        txtProTotal.Text = reader["FAuxQty"].ToString();//数量
                        txtProNum.Text = reader["mtsl"].ToString();//每托数量
                        txtProDirection.Text = reader["direction"].ToString();//方向
                    }
                    else
                    {
                        return;//返回
                    }
                }
            }
        }

        private void txtProZone_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProZone.Text)&&GLB.zoneCheckStatus[int.Parse(txtProZone.Text)] == false)
            {
                MessageBox.Show("该托盘没有识别位置成功！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
