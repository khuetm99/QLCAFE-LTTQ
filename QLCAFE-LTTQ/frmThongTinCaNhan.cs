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
    public partial class frmThongTinCaNhan : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; }
        }
        public frmThongTinCaNhan(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
            ChangeAccount(LoginAccount);
        }

        void UpdateAccount()
        {
            string displayName = txtTenHienThi.Text;
            string password = txtMatKhau.Text;
            string newpass = txtMatKhauMoi.Text;
            string reenterpass = txtNhapLai.Text;
            string username = txtTenDangNhap.Text;

            if(!newpass.Equals(reenterpass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới !");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(username,displayName,password,newpass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserNames(username)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }

        }

        void ChangeAccount(Account acc)
        {
            txtTenDangNhap.Text = LoginAccount.UserName;
            txtTenHienThi.Text = LoginAccount.DisplayName;          
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        private EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdatingAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
