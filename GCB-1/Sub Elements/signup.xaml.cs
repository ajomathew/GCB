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
using System.Text.RegularExpressions;

namespace GCB_1.Sub_Elements
{
    public partial class signup : Page
    {
        ServiceReference1.ServiceGCBClient client = new ServiceReference1.ServiceGCBClient();

        public signup()
        {
			
            InitializeComponent();
            formload.BeginTime = TimeSpan.FromSeconds(0);
            formload.Begin();
            
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        //save the user to database
        private void b_submit_Click(object sender, RoutedEventArgs e)
        {
            b_submit.Visibility = Visibility.Collapsed;
           // client.OpenAsync();
            string pass, pass1, email;
            
            client.InsertOpeCompleted+=new EventHandler<ServiceReference1.InsertOpeCompletedEventArgs>(client_InsertOpeCompleted);
            //this.Content = new Loginpage();
            pass = PBpass.Password;
            pass1 = PBpass1.Password;
            email = TBemail.Text;
            bool va=isValidEmail(email);
            if(va==false)
                 {
                     MessageBox.Show("enter correct email format ");
                     b_submit.Visibility = Visibility.Visible;
                     TBemail.Text = "";
                 }
            else if (pass == pass1)
                     { 
                        string str = "insert into userinfo values('" + TBUserName.Text + "','" + PBpass.Password + "','" + TBemail.Text + "')";
                        client.InsertOpeAsync(str);
                       
                      }
                 
                    else
                    {
                       MessageBox.Show("Password mismatch.Renter Password");
                       b_submit.Visibility = Visibility.Visible;
                        PBpass.Password = "";
                         PBpass1.Password = "";
                      }
            


                
           

        
        }
        void client_InsertOpeCompleted(object sender, ServiceReference1.InsertOpeCompletedEventArgs e)
        {
            bool tr = (bool)e.Result;
            if (tr)
                this.Content = new Loginpage();
            else
                MessageBox.Show("User Name already taken", "Databaseop", MessageBoxButton.OK);
            
           // client.CloseAsync();
        }

        private void TBUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PBpass_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TBemail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public static bool isValidEmail(string inputEmail)//validating Emails
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +

                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +

                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);

            if (re.IsMatch(inputEmail))

                return (true);

            else

                return (false);
        }
    }
}
