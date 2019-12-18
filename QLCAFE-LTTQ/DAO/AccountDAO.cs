using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    public class AccountDAO
    {
        public static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        public bool login(string username, string password)
        {
            string query = "USP_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });

            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserNames(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.account where username = '" + userName + "'");

            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public bool UpdateAccount(string username, string displayname ,string pass, string newpass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @username , @displayname , @password , @newpassword ", new object[] { username, displayname, pass, newpass });
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select username, displayname, type from dbo.account");
        }


        public bool InsertAccount(string username, string displayname, int type)
        {
            string query = string.Format("insert dbo.account (username, displayname, type) values( N'{0}' , N'{1}' , {2})", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateAccount(string username, string displayname, int type)
        {
            string query = string.Format("update dbo.account set displayname = N'{1}' , type ={2} where username = N'{0}' ", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteAccount(string username)
        {
            string query = string.Format("delete dbo.account  where username = N'{0}' ",  username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool ResetPassword(string username)
        {
            string query = string.Format("update dbo.account set password = N'0' where username = N'{0}' ", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
