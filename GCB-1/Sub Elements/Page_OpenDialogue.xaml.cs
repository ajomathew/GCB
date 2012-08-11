using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.ServiceModel;
using System.ServiceModel.DomainServices.Client;

namespace GCB_1
{
    public partial class Page_OpenDialogue : Page
    {
        string usename;
        
        ServiceReference1.ServiceGCBClient client = new ServiceReference1.ServiceGCBClient();
        public Page_OpenDialogue(string uname)
        {
            InitializeComponent();
            formload.BeginTime = TimeSpan.FromSeconds(0);
            formload.Begin();
            usename = uname;
            hide();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        
        
        
        private void hide()
        {
            Canvas_newcoll.Visibility = Visibility.Collapsed;
            Canvas_open.Visibility = Visibility.Collapsed;
            Canvas_Template.Visibility = Visibility.Collapsed;
           
        }

        private void Button_new_Click(object sender, RoutedEventArgs e)
        {
            hide();
            Canvas_newcoll.Visibility = Visibility.Visible;
            createnew.BeginTime = TimeSpan.FromSeconds(0);
            createnew.Begin();
            
            //this.Content = new MainPage();
        }
        /// <summary>
        /// here the user click the button and a list of complted and draft collatratl fill the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_open_Click(object sender, RoutedEventArgs e)
        {
            hide();

            Canvas_open.Visibility = Visibility.Visible;
            client.fillCompleted += new EventHandler<ServiceReference1.fillCompletedEventArgs>(client_fillCompleted);
            client.fillAsync(usename,"true");
            openexisting.BeginTime = TimeSpan.FromSeconds(0);
            openexisting.Begin();
            
           
        }

        void client_fillCompleted(object sender, ServiceReference1.fillCompletedEventArgs e)
        {
            
         
            Array ob;
            ob = e.Result.ToArray();
            
            listBox1.ItemsSource = ob;
                           
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_template_Click(object sender, RoutedEventArgs e)
        {
            hide();
            Canvas_Template.Visibility = Visibility.Visible;
            choosetemplate.BeginTime = TimeSpan.FromSeconds(0);
            choosetemplate.Begin();
            
        }
        /// <summary>
        /// Button Click event on the template selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectColor.Selected.ToString() != "#00000000")
                this.Content = new MainPage(SizeComboBox.SelectionBoxItem.ToString(), selectColor.Selected.ToString(),usename,null,null);
            else
                MessageBox.Show("Please Choose a Color value");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("300x200", "#FFFB0B0B",usename,null,null);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("300x200", "#FFF3A804",usename,null,null);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("300x200", "#FF7AEB20", usename, null, null);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("400x200", "#FFFB0B0B", usename, null, null);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("400x200", "#FFF3A804", usename, null, null);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("400x200", "#FF7AEB20", usename, null, null);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("500x300", "#FFFB0B0B", usename, null, null);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("500x300", "#FFF3A804", usename, null, null);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            this.Content = new MainPage("500x300", "#FF7AEB20", usename, null, null);
        }

        private void xmlfileDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
        /// <summary>
        /// the button get the complted or draf collateral that is to be filled in the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            button2.Visibility = Visibility.Visible;
            label1.Content = "Completed";
            button1.Visibility = Visibility.Collapsed;
            client.fillCompleted += new EventHandler<ServiceReference1.fillCompletedEventArgs>(client_fillCompleted2);
            client.fillAsync(usename, "false");
        }
        void client_fillCompleted2(object sender, ServiceReference1.fillCompletedEventArgs e)
        {
            Array ob;
            ob = e.Result.ToArray();

            listBox1.ItemsSource = ob;
            label1.Content = "Completed";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Drafts";
            button1.Visibility = Visibility;
            button2.Visibility = Visibility.Collapsed;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if(button1.Visibility==Visibility.Visible)
            this.Content = new MainPage(null, null, usename, listBox1.SelectedItem.ToString(),"true");
            else
            this.Content = new MainPage(null, null, usename, listBox1.SelectedItem.ToString(), "false");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ImageUploadPage(usename);
        }
        

       
        

       
    }
}
