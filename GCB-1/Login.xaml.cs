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

namespace GCB_1
{
    public partial class Login : Page
    {
        public Login()
        {

            InitializeComponent();
            
            txtgraphic.BeginTime = TimeSpan.FromSeconds(0);
            txtgraphic.Begin();
			
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        

        private void B__Click(object sender, RoutedEventArgs e)
        {
           
			
            C_mainLog.Visibility = Visibility.Collapsed;
            this.Content = new GCB_1.Sub_Elements.Loginpage();
            
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
          
            C_mainLog.Visibility = Visibility.Collapsed;
            this.Content = new GCB_1.Sub_Elements.signup();
        }

        

       
    }
}
