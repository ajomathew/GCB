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
using System.Data;



namespace GCB_1.Sub_Elements
{
    public partial class Loginpage : Page
    {
        public bool auth;
        ServiceReference1.ServiceGCBClient client = new ServiceReference1.ServiceGCBClient();
        
        public Loginpage()
        {
            InitializeComponent();
			formload.BeginTime = TimeSpan.FromSeconds(0);
            formload.Begin();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        //to check the user name and password match
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string username, password;
            client.selectopCompleted += new EventHandler<ServiceReference1.selectopCompletedEventArgs>(client_selectopCompleted);
            username = t_username.Text;
            password = p_password.Password;
            string str = "select UserName,PassWord from userinfo where UserName='" + username + "'";
            //Connectivity LogCon = new Connectivity();

            // id = tbusername.Value.ToString().Replace("'", " ");
            //password = txtUserPass.Value.ToString().Replace("'", " ");
            //  LogCon.checkconn();

            client.selectopAsync(str, username, password);
            button1.Visibility = Visibility.Collapsed;

              //auth= LogCon.selectop("select id,password from userinfo where id='"+username+"'",username,password);
          
        }

        void client_selectopCompleted(object sender, ServiceReference1.selectopCompletedEventArgs e)
        {
            bool auth = (bool)e.Result;
            //if (tr)
            //    MessageBox.Show("Logged");
            //    this.Content = new signup();
            if (auth == true)
            {


                this.Content = new Page_OpenDialogue(t_username.Text);

            }
            else
            {
                button1.Visibility = Visibility.Visible;
                MessageBox.Show("Enter correct username or password");
                p_password.Password = "";

            }


            

            
        }
    }
}
    

        

        

   
