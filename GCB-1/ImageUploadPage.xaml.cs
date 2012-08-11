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
using System.IO;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;

namespace GCB_1
{
    public partial class ImageUploadPage : Page
    {
        bool msgshowf=false;
        BitmapImage imageSource = new BitmapImage();
        ServiceReference1.ServiceGCBClient client=new ServiceReference1.ServiceGCBClient();
        string username;
        OpenFileDialog dlg = new OpenFileDialog();
        bool? retval;
        public ImageUploadPage(string uname)
        {
            username = uname;
            InitializeComponent();
            formload.BeginTime = TimeSpan.FromSeconds(0);
            formload.Begin();
        }
        
        // Executes when the user navigates to this page and click the label
        private void label1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            imageSelected.Visibility = Visibility.Collapsed;
            dlg.Multiselect = false;
            dlg.Filter = "JPGE Images (*.jpg)|*.jpg";
            retval = dlg.ShowDialog();
            if (retval != null && retval == true)
            {
                imageloaded.BeginTime = TimeSpan.FromSeconds(0);
                imageloaded.Begin();
                
                imageSelected.Visibility = Visibility.Visible;
                imageSource.SetSource(dlg.File.OpenRead());
                imageSelected.Source = imageSource;
            }
            
            
        }
        //load the image to server
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (retval != null && retval == true)
            {
                client.IsAvailCompleted += new EventHandler<ServiceReference1.IsAvailCompletedEventArgs>(client_IsAvailCompleted);
                client.IsAvailAsync("select imagename from ImageList where UserName='"+username+"'", dlg.File.Name);

            }
            else
            {
                labelUpload.Content = "No File selected Click Here to Select the file."; // .Text = "No file selected...";
            }
            

        }
        public string flname;
        void client_IsAvailCompleted(object sender, ServiceReference1.IsAvailCompletedEventArgs e)
        {
            bool avail = (bool)e.Result;
            if (avail == false)
            {
                
                client.InsertOpeCompleted+=new EventHandler<ServiceReference1.InsertOpeCompletedEventArgs>(client_InsertOpeCompleted);
                flname = username + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ".jpg";
                client.InsertOpeAsync("insert into ImageList values('" + username + "','" + dlg.File.Name + "','" +flname+ "')");
            }
            
           else if (!msgshowf)
                {
                    
                    MessageBox.Show("File name already exists in your name");
                    msgshowf = true;
                    this.Content = new ImageUploadPage(username);
            }
                    msgshowf = false;
            
        }
        void client_InsertOpeCompleted(object sender, ServiceReference1.InsertOpeCompletedEventArgs e)
        {
 bool auth = (bool)e.Result;
 if (auth == true)
 {
     //MessageBox.Show("inserted");
     UploadFile(flname, dlg.File.OpenRead());
 }
 else
 {
    
 }
 this.Content = new Page_OpenDialogue(username);
        }
        //uploadint the file
        private void UploadFile(string fileName, Stream data)
        {

            UriBuilder ub = new UriBuilder("http://localhost:4278/receiver.ashx");
            ub.Query = string.Format("filename={0}", fileName);
            WebClient c = new WebClient();
            c.OpenWriteCompleted += (sender, e) =>
            {
                PushData(data, e.Result);
                e.Result.Close();
                data.Close();
            };
            c.OpenWriteAsync(ub.Uri);
            
        }
        //uploadfile associaated function
        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        private void labelUpload_MouseEnter(object sender, MouseEventArgs e)
        {
            labelUpload.FontSize = 12;
        }

        private void labelUpload_MouseLeave(object sender, MouseEventArgs e)
        {
            labelUpload.FontSize = 10;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Page_OpenDialogue(username);
        }
    }
}
