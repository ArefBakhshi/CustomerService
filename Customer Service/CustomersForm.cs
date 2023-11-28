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
        
        
        public CustomersForm()// Here i Implemented the code that will make form corners rounded.
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }


        CustomerBll customerBll = new CustomerBll();
        void FillDataGrid()//To fill DataGrid called in later events
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = customerBll.GetActiveCustomers();
            dataGridView1.Columns["id"].Visible = false;
        }
        void ClearTextBoxes()
        {
            textBoxCustomerName.Text = String.Empty;
            textBoxCustomerPhone.Text = String.Empty;
            textBoxSearch.Text = String.Empty;
        }//To clear customer creation text boxes
        public int GetId() //Gets id from the Grid For Update or Deletion
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int id = (int)dataGridView1.Rows[rowIndex].Cells["id"].Value;
            return id;
        }


        private void CustomersForm_Load(object sender, EventArgs e)
        {
            FillDataGrid();
            
        }//Fills data grid on start
        private void pictureBox2_Click(object sender, EventArgs e)//To close Form
        {
            this.Close();
        }
        private void label10_Click(object sender, EventArgs e)//To close Form
        {
            this.Close();
        }



        private void label1_Click(object sender, EventArgs e)
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
            }
            else if (label1.Text == "ویرایش اطلاعات")
            {
                Customer customer = new Customer();
                customer.Name = textBoxCustomerName.Text;
                customer.Phone = textBoxCustomerPhone.Text;
                MessageBox.Show(customerBll.UpdateCustomer(customer, GetId()));
                FillDataGrid();
                ClearTextBoxes();
            }
        }//Checks and executes either create or update 
        private void pictureBox1_Click(object sender, EventArgs e)
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
            }
            else if (label1.Text == "ویرایش اطلاعات")
            {
                Customer customer = new Customer();
                customer.Name = textBoxCustomerName.Text;
                customer.Phone = textBoxCustomerPhone.Text;
                MessageBox.Show(customerBll.UpdateCustomer(customer, GetId()));
                FillDataGrid();
                ClearTextBoxes();
            }
        }//Checks and executes either create or update 



        int index;// A flag for search, to determine which stored procedure should be used.
        private void textBoxSearch_TextChanged(object sender, EventArgs e)// For Search
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
        }

        

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)// to choose a certain row and showing menustrip when right clicking
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    Point mousePosition = dataGridView1.PointToClient(Cursor.Position);
                    contextMenuStrip1.Show(dataGridView1, mousePosition);
                }
            }
        }       
        private void ویرایشToolStripMenuItem_Click(object sender, EventArgs e)//Edition in menustrip
        {
            Customer customer = new Customer();
            customer.Id = GetId();
            customer = customerBll.GetCustomerById(customer.Id);
            textBoxCustomerName.Text = customer.Name;
            textBoxCustomerPhone.Text = customer.Phone;
            label1.Text = "ویرایش اطلاعات";

        }
        private void حذفToolStripMenuItem_Click(object sender, EventArgs e)//Deletion in menustrip
        {
            Customer customer = new Customer();
            customer.Id = GetId();
            customer = customerBll.GetCustomerById(customer.Id);
            DialogResult result = MessageBox.Show("آیا از حذف مشتری اطمینان دارید ؟", "! هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) { customerBll.DeleteCustomer(GetId()); FillDataGrid(); }
            else if (result == DialogResult.No) { }
        }

        
    }
}
