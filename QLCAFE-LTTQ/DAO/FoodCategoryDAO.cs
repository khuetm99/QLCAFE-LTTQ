using QLCAFE_LTTQ.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DAO
{
    class FoodCategoryDAO
    {
        private static FoodCategoryDAO instance;

        public static FoodCategoryDAO Instance
        {
            get { if (instance == null) instance = new FoodCategoryDAO(); return FoodCategoryDAO.instance; }
            private set { FoodCategoryDAO.instance = value; }
        }

        public List<FoodCategory> GetListCategory()
        {
            List<FoodCategory> listCategory = new List<FoodCategory>();
            string query= "select * from foodcategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                FoodCategory category = new FoodCategory(item);
                listCategory.Add(category);
            }
            return listCategory;
        }

        public FoodCategory GetListCategoryById(int id)
        {
            FoodCategory category = null;
            string query = "select * from foodcategory where id ="+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new FoodCategory(item);
                return category;              
            }
            return category;
        }

        public bool InsertFoodCategory(string name)
        {
            string query = string.Format("insert dbo.foodcategory (name) values ( N'{0}' )",name);
           int result= DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateFoodCategory(string name, int id)
        {
            string query = string.Format("update dbo.foodcategory set name = N'{0}' where id = {1}", name,id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteFoodCategory( int idCategory )
        { 
            //FoodDAO.Instance.DeleteFoodByFoodCategory(idCategory);           
            string query = string.Format("delete foodcategory where id = "+ idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

    }
}
