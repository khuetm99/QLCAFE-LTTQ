using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DTO
{
    class FoodCategory
    {
        public FoodCategory(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public FoodCategory(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
        }
        private int id;
        private string name;

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}
