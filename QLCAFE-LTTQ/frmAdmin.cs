using QLCAFE_LTTQ.DAO;
using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCAFE_LTTQ
{
    public partial class tcAdmin : Form
    {
        public Account loginAccount;
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public tcAdmin()
        {
            InitializeComponent();
            Loadmethod();
        }

        #region method
        void Loadmethod()
        {
            dgvThucAn.DataSource = foodList;
            dgvDanhMuc.DataSource = categoryList;
            dgvBan.DataSource = tableList;
            dgvTaiKhoan.DataSource = accountList;
            LoadDateTimePicker();
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
            LoadListFood();
            LoadListCategory();
            LoadListAccout();
            LoadListTable();
            AddFoodBinding();
            AddTableBinding();
            AddAccountBinding();
            AddCategoryBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
            LoadTableIntoCombobox(cbTableStatus);
            LoadAccountTypeIntoCombobox(cbLoaiTK);
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void LoadDateTimePicker()
        {          
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
           dgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dgvThucAn.DataSource, "name",true,DataSourceUpdateMode.Never));
            txtIDFood.DataBindings.Add(new Binding("Text", dgvThucAn.DataSource, "id",true, DataSourceUpdateMode.Never));
            nmPriceFood.DataBindings.Add(new Binding("Value", dgvThucAn.DataSource, "price", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBinding()
        {
            txtIDDanhMuc.DataBindings.Add(new Binding("Text", dgvDanhMuc.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtTenDanhMuc.DataBindings.Add(new Binding("Text", dgvDanhMuc.DataSource, "name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            txtIDBan.DataBindings.Add(new Binding("Text", dgvBan.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtTenBan.DataBindings.Add(new Binding("Text", dgvBan.DataSource, "name", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text",dgvBan.DataSource, "status", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txtTenTK.DataBindings.Add(new Binding("Text", dgvTaiKhoan.DataSource, "username", true, DataSourceUpdateMode.Never));
            txtTenHienThi.DataBindings.Add(new Binding("Text", dgvTaiKhoan.DataSource, "displayname", true, DataSourceUpdateMode.Never));
            cbLoaiTK.DataBindings.Add(new Binding("Text", dgvTaiKhoan.DataSource, "type", true, DataSourceUpdateMode.Never));
        }
        void LoadListCategory()
        {
            categoryList.DataSource = FoodCategoryDAO.Instance.GetListCategory();
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }

        void LoadListAccout()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = FoodCategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }

        void LoadTableIntoCombobox(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "status";
        }

        void LoadAccountTypeIntoCombobox(ComboBox cb)
        {
            cb.DataSource = AccountDAO.Instance.GetListAccount();
            cb.DisplayMember = "type";
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
               
        #endregion

        #region events
        private void btnThongKe_Click(object sender, EventArgs e)
        {           
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
        }
        private void btnXemFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txtIDFood_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvThucAn.SelectedCells.Count > 0)
                {
                    int id = (int)dgvThucAn.SelectedCells[0].OwningRow.Cells["categoryid"].Value;

                    FoodCategory foodCategory = FoodCategoryDAO.Instance.GetListCategoryById(id);

                    cbFoodCategory.SelectedItem = foodCategory;

                    int index = -1;
                    int i = 0;
                    foreach (FoodCategory item in cbFoodCategory.Items)
                    {
                        if (item.Id == foodCategory.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnThemFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int idcategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmPriceFood.Value;
            if (FoodDAO.Instance.InsertFood(name, idcategory, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn !");
            }
        }
        private void btnSuaFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDFood.Text);
            string name = txtFoodName.Text;
            int idcategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmPriceFood.Value;
            if (FoodDAO.Instance.UpdateFood(name,idcategory,price,id))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn !");
            }
        }

        private void btnXoaFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDFood.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn !");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler insertFoodCategory;
        public event EventHandler InsertFoodCategory
        {
            add { insertFoodCategory += value; }
            remove { insertFoodCategory -= value; }
        }

        private event EventHandler deleteFoodCategory;
        public event EventHandler DeleteFoodCategory
        {
            add { deleteFoodCategory += value; }
            remove { deleteFoodCategory -= value; }
        }

        private event EventHandler updateFoodCategory;
        public event EventHandler UpdateFoodCategory
        {
            add { updateFoodCategory += value; }
            remove { updateFoodCategory -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private void btnTimFood_Click(object sender, EventArgs e)
        {
           
            foodList.DataSource = SearchFoodByName(txtTimFood.Text);
        }

        private void btnThemDanhMuc_Click(object sender, EventArgs e)
        {
            string name = txtTenDanhMuc.Text;
            if(FoodCategoryDAO.Instance.InsertFoodCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công ");
                LoadListCategory();
                LoadCategoryIntoCombobox(cbFoodCategory);
                if (insertFoodCategory != null)
                    insertFoodCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục");
            }
        }

        private void btnSuaDanhMuc_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDDanhMuc.Text);
            string name = txtTenDanhMuc.Text;
            if (FoodCategoryDAO.Instance.UpdateFoodCategory(name,id))
            {
                MessageBox.Show("Sửa danh mục thành công ");
                LoadListCategory();
                LoadCategoryIntoCombobox(cbFoodCategory);
                if (updateFoodCategory != null)
                    updateFoodCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục");
            }
        }

        private void btnXoaDanhMuc_Click(object sender, EventArgs e)
        {
            int idCategory = Convert.ToInt32(txtIDDanhMuc.Text);
            if (FoodCategoryDAO.Instance.DeleteFoodCategory(idCategory)) 
            {
                MessageBox.Show("Xóa danh mục thành công ");
                LoadListCategory();
                LoadCategoryIntoCombobox(cbFoodCategory);
                LoadListFood();
                if (deleteFoodCategory != null)
                    deleteFoodCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục");
            }
        }

        private void btnXemDanhMuc_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnXoaBan_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDBan.Text);
            if(TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadListTable();
                if (deleteTable != null)
                    deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn");
            }
        }
     
        private void btnXemBan_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnSuaBan_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDBan.Text);
            string name = txtTenBan.Text;
            string status = cbTableStatus.Text;
            if (TableDAO.Instance.UpdateTable(name, status, id))
            {
                MessageBox.Show(" Sửa bàn thành công");
                LoadListTable();
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            string name = txtTenBan.Text;
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show(" Thêm bàn thành công");
                LoadListTable();
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }

        private void btnXemTK_Click(object sender, EventArgs e)
        {
            LoadListAccout();
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            string username = txtTenTK.Text;
            string displayname = txtTenHienThi.Text;
            int type = Convert.ToInt32(cbLoaiTK.Text);
            if(AccountDAO.Instance.UpdateAccount(username,displayname,type))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadListAccout();
                if (updateAccount != null)
                    updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserNames(username)));
            }
            else
            {
                MessageBox.Show("Lỗi khi sửa tài khoản ");
            }
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            string username = txtTenTK.Text;

            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Đừng xóa account hiện tại ~~ !");
                return;
            }           
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadListAccout();
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa tài khoản ");
            }
        }

        private void btnThemTK_Click(object sender, EventArgs e)
        {
            string username = txtTenTK.Text;
            string displayname = txtTenHienThi.Text;
            int type = Convert.ToInt32(cbLoaiTK.Text);
            if (AccountDAO.Instance.InsertAccount(username, displayname, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                LoadListAccout();
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm tài khoản ");
            }
        }

        private void btnDatLaiMK_Click(object sender, EventArgs e)
        {
            string username = txtTenTK.Text;
            if (AccountDAO.Instance.ResetPassword(username))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Lỗi khi đặt lại mật khẩu  ");
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        #endregion


    }
}
