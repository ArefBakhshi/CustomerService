using Business_Entity;
using BusinessLogicLayer;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Customer_Service
{
    public partial class ProductForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthRect,
            int nHeightRect
            );

        public ProductForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        ProductBll productBll = new ProductBll();

        void FillDataGrid() //As the name suggests.
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = productBll.GetActiveProducts();
                dataGridView1.Columns["id"].Visible = false;
            }
            catch
            {

                MessageBox.Show("مشکلی پیش آمده است لطفا فرم را باز و بسته کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ClearTextBoxes() //To clear product creation text boxes
        {
            textBoxName.Text = String.Empty;
            textBoxPrice.Text = String.Empty;
            textBoxStock.Text = String.Empty;
            textBoxSearch.Text = String.Empty;
        }
        public int GetId() // Gets id from the Grid For Update or Deletion
        {
            int rowIndex = dataGridView1.CurrentRow.Index;
            int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["Id"].Value);
            return id;
        }


        private void ProductForm_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            dataGridView1.ClearSelection();
        }
        private void label10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            try
            {
                if (label1.Text == "ثبت اطلاعات")
                {
                    Product product = new Product();
                    product.Name = textBoxName.Text;
                    product.Price = Convert.ToDouble(textBoxPrice.Text);
                    product.Stock = Convert.ToInt32(textBoxStock.Text);
                    MessageBox.Show(productBll.CreateProduct(product));
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
                else if (label1.Text == "ویرایش اطلاعات")
                {
                    Product product = new Product();
                    product.Name = textBoxName.Text;
                    product.Price = Convert.ToDouble(textBoxPrice.Text);
                    product.Stock = Convert.ToInt32(textBoxStock.Text);
                    MessageBox.Show(productBll.UpdateProduct(product, GetId()));
                    label1.Text = "ثبت اطلاعات";
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
            }
            catch
            {
                MessageBox.Show("مشکلی در ثبت یا ویرایش پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (label1.Text == "ثبت اطلاعات")
                {
                    Product product = new Product();
                    product.Name = textBoxName.Text;
                    product.Price = Convert.ToDouble(textBoxPrice.Text);
                    product.Stock = Convert.ToInt32(textBoxStock.Text);
                    MessageBox.Show(productBll.CreateProduct(product));
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
                else if (label1.Text == "ویرایش اطلاعات")
                {
                    Product product = new Product();
                    product.Name = textBoxName.Text;
                    product.Price = Convert.ToDouble(textBoxPrice.Text);
                    product.Stock = Convert.ToInt32(textBoxStock.Text);
                    MessageBox.Show(productBll.UpdateProduct(product, GetId()));
                    label1.Text = "ثبت اطلاعات";
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
            }
            catch
            {
                MessageBox.Show("مشکلی در ثبت یا ویرایش پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;
            dataGridView1.DataSource = productBll.SearchProducts(textBoxSearch.Text);
            dataGridView1.Columns["id"].Visible = false;
            }
            catch
            {
                MessageBox.Show("مشکلی در جستجو پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.RowCount)
                {

                    dataGridView1.ClearSelection();


                    dataGridView1.Rows[e.RowIndex].Selected = true;


                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];


                    contextMenuStrip1.Show(Control.MousePosition);
                }
            }
        }

        private void ویرایشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearTextBoxes();
                Product product = new Product();
                product.Id = GetId();
                product = productBll.GetProductById(product.Id);
                textBoxName.Text = product.Name;
                textBoxPrice.Text = Convert.ToString(product.Price);
                textBoxStock.Text = Convert.ToString(product.Stock);
                label1.Text = "ویرایش اطلاعات";
                dataGridView1.ReadOnly = true;


            }
            catch
            {
                MessageBox.Show("مشکلی در ویرایش پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void حذفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                Product product = new Product();
                product.Id = GetId();
                product = productBll.GetProductById(product.Id);
                DialogResult result = MessageBox.Show("آیا از حذف مشتری اطمینان دارید ؟", "! هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { label1.Text = "ثبت اطلاعات"; ClearTextBoxes(); productBll.DeleteProduct(GetId()); FillDataGrid(); dataGridView1.ClearSelection(); }
                else if (result == DialogResult.No) { }




            }
            catch
            {
                MessageBox.Show("مشکلی در حذف پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
