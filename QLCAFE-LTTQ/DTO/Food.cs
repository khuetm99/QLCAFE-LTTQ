﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCAFE_LTTQ.DTO
{
    class Food
    {
        public Food(int id, string name, int categoryID, float price)
        {
            this.Id = id;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;
        }
        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.CategoryID = (int)row["idcategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }

        private int id;
        private string name;
        private int categoryID;
        private float price;
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public float Price { get => price; set => price = value; }
    }
}
