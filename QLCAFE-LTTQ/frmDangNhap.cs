using QLCAFE_LTTQ.DAO;
using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCAFE_LTTQ
{
    public partial class frmDangNhap : Form
    {
      
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Text;
            if (Login(username,password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserNames(username);
                frmQuanLyCaPhe f = new frmQuanLyCaPhe(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
               
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu !");
            }
        }
        bool Login(string username, string password)
        {
           
            return AccountDAO.Instance.login(username, password);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
