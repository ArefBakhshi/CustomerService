using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;
using Business_Entity;
using HandyControl.Tools.Extension;
using DevComponents.DotNetBar.Controls;
using HandyControl.Tools;

namespace Customer_Service
{
    public partial class CustomersForm : Form //adds blur effect in background when opened.
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
        
        
        public CustomersForm() // Here i Implemented the code that will make form corners rounded.
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }


        CustomerBll customerBll = new CustomerBll();
        void FillDataGrid() //As the name suggests.
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = customerBll.GetActiveCustomers();
                dataGridView1.Columns["id"].Visible = false;
            }
            catch 
            {

                MessageBox.Show("مشکلی پیش آمده است لطفا فرم را باز و بسته کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ClearTextBoxes() //To clear customer creation text boxes
        {
            textBoxCustomerName.Text = String.Empty;
            textBoxCustomerPhone.Text = String.Empty;
            textBoxSearch.Text = String.Empty;
        }
        public int GetId() // Gets id from the Grid For Update or Deletion
        {                        
                int rowIndex = dataGridView1.CurrentRow.Index;              
                int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["Id"].Value);
                return id;             
        }


        private void CustomersForm_Load(object sender, EventArgs e) //Fills data grid on start
        {
            FillDataGrid();
            dataGridView1.ClearSelection();
            
        }
        private void pictureBox2_Click(object sender, EventArgs e) //To close Form
        {
            this.Close();
        }
        private void label10_Click(object sender, EventArgs e) //To close Form
        {
            this.Close();
        }



        private void label1_Click(object sender, EventArgs e) //Checks and executes either create or update
        {
            try
            {
                if (label1.Text == "ثبت اطلاعات")
                {
                    Customer customer = new Customer();
                    customer.Name = textBoxCustomerName.Text;
                    customer.Phone = textBoxCustomerPhone.Text;
                    customer.RegDate = DateTime.Now;
                    MessageBox.Show(customerBll.CreateCustomer(customer));
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
                else if (label1.Text == "ویرایش اطلاعات")
                {
                    Customer customer = new Customer();
                    customer.Name = textBoxCustomerName.Text;
                    customer.Phone = textBoxCustomerPhone.Text;
                    MessageBox.Show(customerBll.UpdateCustomer(customer, GetId()));
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
        private void pictureBox1_Click(object sender, EventArgs e) //Checks and executes either create or update 
        {
            try 
            {
                if (label1.Text == "ثبت اطلاعات")
                {
                    Customer customer = new Customer();
                    customer.Name = textBoxCustomerName.Text;
                    customer.Phone = textBoxCustomerPhone.Text;
                    customer.RegDate = DateTime.Now;
                    MessageBox.Show(customerBll.CreateCustomer(customer));
                    FillDataGrid();
                    ClearTextBoxes();
                    dataGridView1.ClearSelection();
                }
                else if (label1.Text == "ویرایش اطلاعات")
                {
                    Customer customer = new Customer();
                    customer.Name = textBoxCustomerName.Text;
                    customer.Phone = textBoxCustomerPhone.Text;
                    MessageBox.Show(customerBll.UpdateCustomer(customer, GetId()));
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



        int index; // A flag for search, to determine which stored procedure should be used.
        private void textBoxSearch_TextChanged(object sender, EventArgs e) // For Search
        {
            try
            {
                if (checkBox1.Checked && checkBox2.Checked || (!checkBox1.Checked && !checkBox2.Checked))
                {
                    index = 0;
                }
                else if (checkBox1.Checked)
                {
                    index = 1;
                }
                else if (checkBox2.Checked)
                {
                    index = 2;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = customerBll.SearchCustomers(textBoxSearch.Text, index);
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
        private void ویرایشToolStripMenuItem_Click(object sender, EventArgs e) //edition in menustrip
        {
            try
            {
                ClearTextBoxes();
                Customer customer = new Customer();
                customer.Id = GetId();
                customer = customerBll.GetCustomerById(customer.Id);
                textBoxCustomerName.Text = customer.Name;
                textBoxCustomerPhone.Text = customer.Phone;
                label1.Text = "ویرایش اطلاعات";
                dataGridView1.ReadOnly = true;
                
                
            }
            catch
            {
                MessageBox.Show("مشکلی در ویرایش پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            



        }
        private void حذفToolStripMenuItem_Click(object sender, EventArgs e) //Deletion in menustrip
        {
            try
            {
                
                Customer customer = new Customer();
                customer.Id = GetId();
                customer = customerBll.GetCustomerById(customer.Id);
                DialogResult result = MessageBox.Show("آیا از حذف مشتری اطمینان دارید ؟", "! هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) { label1.Text = "ثبت اطلاعات"; ClearTextBoxes(); customerBll.DeleteCustomer(GetId()); FillDataGrid(); dataGridView1.ClearSelection(); }
                else if (result == DialogResult.No) { }

                


            }
            catch
            {
                MessageBox.Show("مشکلی در حذف پیش آمده است لطفا دوباره تلاش کنید و یا با توسعه دهنده تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


    }
}
