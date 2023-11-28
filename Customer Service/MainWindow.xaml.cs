using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Media.Effects;

namespace Customer_Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void OpenWinForm(Form form)
        {
            Window g = this.FindName("Main") as Window;
            BlurBitmapEffect blurBitmapEffect = new BlurBitmapEffect();
            blurBitmapEffect.Radius = 20;
            g.BitmapEffect = blurBitmapEffect;
            form.ShowDialog();
            blurBitmapEffect.Radius = 0;
            g.BitmapEffect = blurBitmapEffect;
        }
        


        private void WrapPanel_MouseDown_Customer(object sender, MouseButtonEventArgs e)
        {
            CustomersForm form = new CustomersForm();
            OpenWinForm(form);
        }

        private void WrapPanel_MouseDown_Product(object sender, MouseButtonEventArgs e)
        {
            ProductForm form = new ProductForm(); OpenWinForm(form);

        }

        private void WrapPanel_MouseDown_Invoice(object sender, MouseButtonEventArgs e)
        {
            InvoiceForm form = new InvoiceForm(); OpenWinForm(form);
        }

        private void WrapPanel_MouseDown_Activity(object sender, MouseButtonEventArgs e)
        {
            ActivityForm form = new ActivityForm(); OpenWinForm(form);
        }
    

        private void WrapPanel_MouseDown_Reminder(object sender, MouseButtonEventArgs e)
        {
            ReminderForm form = new ReminderForm();
            OpenWinForm(form);
        }

        private void WrapPanel_MouseDown_SMS(object sender, MouseButtonEventArgs e)
        {
            SmsForm form = new SmsForm();
            OpenWinForm(form);
        }

        private void WrapPanel_MouseDown_Report(object sender, MouseButtonEventArgs e)
        {
            ReportForm form = new ReportForm(); OpenWinForm(form);
        }

        private void WrapPanel_MouseDown_Setting(object sender, MouseButtonEventArgs e)
        {
            SettingForm form = new SettingForm(); OpenWinForm(form);
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
