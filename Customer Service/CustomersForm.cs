﻿using System;
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

namespace Customer_Service
{
    public partial class CustomersForm : Form
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

        void FillDataGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = customerBll.GetActiveCustomers();
        }
        void ClearTextBoxes()
        {
            textBoxCustomerName.Text = String.Empty;
            textBoxCustomerPhone.Text = String.Empty;
            textBoxSearch.Text = String.Empty;
        }
        public CustomersForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        CustomerBll customerBll = new CustomerBll();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void label1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.Name = textBoxCustomerName.Text;
            customer.Phone = textBoxCustomerPhone.Text;
            customer.RegDate = DateTime.Now;
            MessageBox.Show(customerBll.CreateCustomer(customer));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            customer.Name = textBoxCustomerName.Text;
            customer.Phone = textBoxCustomerPhone.Text;
            customer.RegDate = DateTime.Now;
            MessageBox.Show(customerBll.CreateCustomer(customer));
        }
        int index;
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
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

        private void CustomersForm_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                Point mousePosition = dataGridView1.PointToClient(Cursor.Position);
            }
        }
    }
}
