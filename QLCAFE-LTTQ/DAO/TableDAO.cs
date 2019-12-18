using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    class TableDAO
    {
        private static TableDAO instance;

        internal static TableDAO Instance 
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 80;
        public static int TableHeight = 80;
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach  (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public Table GetListTableByTableID(int id)
        {
            Table table = null;
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.tablefood where id =" + id);
            foreach (DataRow item in data.Rows)
            {
                table = new Table(item);
                return table;
            }
            return table;
        }

        public void SwitchTable( int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idtable1 , @idtable2", new object[] { id1, id2 });
        }

        public bool DeleteTable(int id)
        {
            BillDAO.Instance.DeleteBillByTableID(id);
            string query = "delete tablefood where id =" + id;
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("insert dbo.tablefood (name) values (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateTable(string name,string status , int id)
        {
            string query = string.Format("update dbo.tablefood set name = N'{0}' , status = N'{1}' where id = {2}", name ,status ,id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
