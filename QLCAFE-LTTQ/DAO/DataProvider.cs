using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    public class DataProvider
    {   
        //tạo đối tượng singleton
        private static DataProvider instance;
        private string connectionSTR = @"Data Source =.\sqlexpress; Initial Catalog = QLCAFE; Integrated Security = True";

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider(){}
    
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data  = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR))

            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);
                
                connection.Close();
            }

            return data;

        } //trả về những dòng kết quả trong sql
        public int ExecuteNonQuery(string query, object[] parameter = null) // trả về những dòng được thực thi (insert, update,delete)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))

            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();
             }

             return data;
          
        }  // 
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR))

            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();
            }

            return data;


        } //trả về những kết quả như count * ..

    }
}
