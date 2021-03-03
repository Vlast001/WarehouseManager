using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using WarehouseManager.Models;

namespace WarehouseManager
{
    public partial class Form1 : Form
    {
        private string connStr { get; set; }
        private SqlConnection conn { get; set; }
        private List<Product> products; //= new List<Product>();

        /// <summary>
        /// Метод заменющий создание еще одной формы ради окна подключения так как мне лень)
        /// </summary>
        void StartBtn()
        {
            this.Width = 275;
            this.Height = 200;
            this.StartPosition = FormStartPosition.CenterScreen;
            Button btn = new Button();
            btn.Text = "Установить подключение к бд склада";

            btn.Location = new Point(30, 75);
            btn.Size = new Size(200, 50);
            btn.Margin = new Padding(5);
            btn.Click += (s, e) =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                connStr = connectionString;
                conn = new SqlConnection(connStr);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    MessageBox.Show("Подключение установлено");
                    this.Location = new Point(350, 200);
                    Controls.Clear();


                    InitializeComponent();
                    //this.Size = new Size(1036, 593);
                    dataGridView1.Size = new Size(679, 265);
                    this.dataGridView2.Size = new System.Drawing.Size(679, 146);

                    products = new List<Product>();
                    QueryFunc();
                    comboBox1.SelectedIndex = 0;
                    


                    comboBox2.DataSource = products.Select(u => u.ProductType).Distinct().ToList();
                    comboBox2.DisplayMember = "ProductType";

                    comboBox3.DataSource = products.Select(u => u.Supplier).Distinct().ToList();
                    comboBox3.DisplayMember = "Supplier";

                }
                else
                {
                    MessageBox.Show("Ошибка подключения", "Предупрежение");
                }
            };

            Controls.Add(btn);
        }

        public Form1()
        {
            StartBtn();

            #region Start without StartBtn()
            //var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            //connStr = connectionString;
            //conn = new SqlConnection(connStr);
            //conn.Open();
            //InitializeComponent();
            //QueryFunc();
            //comboBox1.SelectedIndex = 0;


            //comboBox2.DataSource = products.Select(s=>s.ProductType).Distinct().ToList();
            //comboBox2.DisplayMember = "ProductType";

            //comboBox3.DataSource = products.Select(s => s.Supplier).Distinct().ToList();
            //comboBox3.DisplayMember = "Supplier";

            #endregion
        }

        void QueryFunc()
        {
            string query = "SELECT * FROM Products";
            SqlCommand cmd = new SqlCommand(query, conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            DeliveryDate = (DateTime)(reader["DeliveryDate"]),
                            Price = (decimal)reader["Price"],
                            ProductType = (string)reader["ProductType"],
                            Quantity = (int)reader["Quantity"],
                            Supplier = (string)reader["Supplier"]
                        });
                    }
            }

            conn.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(conn.State == ConnectionState.Open)
            {
                conn.Close();
                //MessageBox.Show("db conncetion closed");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "Вся информация о товарах")
            {
                dataGridView1.DataSource = null;
                //dataGridView1.DataSource = products;
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).ToList();

            }

            if(comboBox1.Text == "Все типы товаров")
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = products.Select(s => new { ProductType = s.ProductType}).Distinct().ToList();
            }

            if (comboBox1.Text == "Все поставщики")
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = products.Select(s => new { Supplier = s.Supplier }).Distinct().ToList();
            }

            if (comboBox1.Text == "Товар с минимальным количеством")
            {
                dataGridView1.DataSource = null;
                var min = products.Min(s => s.Quantity);
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s=> s.Quantity==min).ToList();

            }

            if (comboBox1.Text == "Товар с максимальным количеством")
            {
                var max = products.Max(s => s.Quantity);
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s => s.Quantity == max).ToList();
            }

            if (comboBox1.Text == "Товар с минимальной себестоимостью")
            {
                var min = products.Min(s => s.Price);
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s => s.Price == min).ToList();
            }

            if (comboBox1.Text == "Товар с максимальной себестоимостью")
            {
                var max = products.Max(s => s.Price);
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s => s.Price == max).ToList();
            }

            if (comboBox1.Text == "Самый старый товар")
            {
                var min = products.Min(s => s.DeliveryDate);
                dataGridView1.DataSource = products.Select(u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s => s.DeliveryDate == min).ToList();
            }

            if (comboBox1.Text == "Среденее по категориям")
            {

                //string query1 = "SELECT c.ProductType as [Product Category], AVG(c.Quantity) as [Average quantity of goods]" +
                //                "from Products c group by c.ProductType";
                //conn.Open();
                //SqlDataAdapter da = new SqlDataAdapter(query1, conn);
                //DataSet dataSet = new DataSet();
                //da.Fill(dataSet);
                //dataGridView1.DataSource = null;
                //dataGridView1.DataSource = dataSet.Tables[0];
                //conn.Close();

                var ave = products.GroupBy(c => c.ProductType).Select(
                    g => new
                    {
                        ProductCategory = g.Key,
                        AverageQuantityOfGoods = g.Average(p => p.Quantity)
                    }).ToList();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = ave;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var que = products.Select(
                u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).Where(s => s.ProductType == (string)comboBox2.SelectedItem).ToList();
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = que;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            var que = products.Where(s => s.Supplier == (string)comboBox3.SelectedItem).Select(
                u => new
                {
                    Name = u.Name,
                    ProductType = u.ProductType,
                    Supplier = u.Supplier,
                    Quantity = u.Quantity,
                    Price = u.Price,
                    DeliveryDate = u.DeliveryDate,
                }).ToList();
            dataGridView2.DataSource = que;
        }   
    }
}
