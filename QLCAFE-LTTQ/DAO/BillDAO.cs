using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        public int GetUncheckBillIDByTableID(int id )
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.bill where idtable = " + id +"and status = 0");

            if(data.Rows.Count >0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id; // lấy id thành công
            }
            return -1; // id =-1 nếu thất bại
        }

        public void InsertBill(int id) //id của table
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idtable", new object[] { id });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from dbo.bill ");
            }
            catch
            {
                return 1;
            }
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkin , @checkout ", new object[] { checkIn, checkOut });
        }

        public void CheckOut(int id, int discount , float totalPrice) // id của bill
        {
            string query = "update dbo.bill set datecheckout = GETDATE(), status = 1, "+" discount = "+ discount + ", totalprice = "+ totalPrice + " where id =" + id;
            DataProvider.Instance.ExecuteQuery(query);
        }  

        public void DeleteBillByTableID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete bill where idtable =" + id);
        }
    }
}
