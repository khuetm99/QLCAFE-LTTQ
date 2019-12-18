using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> listFood = new List<Food>();
            string query = "select * from food where idcategory = "+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);

            }
           return listFood;
        }

        public bool InsertFood(string name , int idcategory, float price)
        {
            string query = string.Format("insert dbo.food (name , idcategory , price) values (N'{0}' , {1}, {2})",name , idcategory, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateFood(string name, int idcategory, float price, int id)
        {
            string query = string.Format("update dbo.food set name = N'{0}' , idcategory = {1} , price = {2} where id = {3}", name, idcategory, price, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteFood( int idfood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idfood);
            string query = string.Format("delete food where id = {0}", idfood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public List<Food> GetListFood()
        {
            List<Food> listfood = new List<Food>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from dbo.food ");
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listfood.Add(food);
            }
            return listfood;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = new List<Food>();
            string query = string.Format("select * from dbo.food where dbo.GetUnsignString(name) like N'%' + dbo.GetUnsignString(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        public void DeleteFoodByFoodCategory(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete dbo.food where idcategory =" + id);
        }
    }
}
