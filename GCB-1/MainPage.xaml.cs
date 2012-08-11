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
using projectsilverlight.blogspot.com.XamlWriter;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;
namespace GCB_1
{
    public partial class MainPage : UserControl
    {
        public Image image = new Image();
        int imagesincanvas=0;
        public string[] imagelist;
        public double imgposx = 0,imgposy=0;
        Sub_Elements.xmlop xamlop = new Sub_Elements.xmlop();
        ServiceReference1.ServiceGCBClient client = new ServiceReference1.ServiceGCBClient();
        public string sizeC, colorC;
        public TextBlock tb;
        public FontFamily fo;
        bool isDragDropInEffect = false;
        Point pos;
        string username;
        string xmlfile;
        string editable;
        public int i, j;
        public bool gotimag;
        List<Image> limage = new List<Image>();
        Image loadimage=new Image();
        /// <summary>
        /// Picture file class holds the picture name and a stream variable
        /// </summary>
        public class PictureFile
        {
            public string PictureName { get; set; }
            public byte[] PictureStream { get; set; }
        }

        public MainPage(string size,string color,string uname,string xmlname,string editabl)
        {
            
            
            InitializeComponent();
            formload.BeginTime = TimeSpan.FromSeconds(0);
            formload.Begin();
            hide(); 
            sizeC = size;
            colorC = color;
            editable = editabl;
            username = uname;
            //check if xml file comes from database
            if (xmlname == null)
            {
                Canvasdrawarea.MouseLeftButtonDown += new MouseButtonEventHandler(Canvasdrawarea_MouseLeftButtonDown);
                Canvasdrawarea.MouseEnter += new MouseEventHandler(Canvasdrawarea_MouseEnter);
                setcanvas();
            }
            else
            {
                client.selectxmlCompleted += new EventHandler<ServiceReference1.selectxmlCompletedEventArgs>(client_selectxmlCompleted);
                client.selectxmlAsync(xmlname, username);
            }
            
        }
        /// <summary>
        /// 
        /// here the layout root loaded also get the list of images associated with the username
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            client.fillCompleted += new EventHandler<ServiceReference1.fillCompletedEventArgs>(client_fillCompleted);
            client.fillAsync(username, "image");
            canvas_imaglist.Children.Add(image);
        }
        void client_fillCompleted(object sender, ServiceReference1.fillCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                imagelist = e.Result.ToArray();
                i = imagelist.Length;
                gotimag = false;
            }
            else
            {
                
            }
        }
        //to Load the xaml file
        void client_selectxmlCompleted(object sender, ServiceReference1.selectxmlCompletedEventArgs e)
        {
            xmlfile = e.Result.ToString();
            Loadxaml(xmlfile);
        }

        // Function to load form xmlfile


        private void Loadxaml(string xmlfile)
        {
            
            Image img = new Image();         
            UIElement tree = (UIElement)XamlReader.Load(xmlfile);
            canvas1.Children.Remove(Canvasdrawarea);
            Canvasdrawarea = (Canvas)tree;         
            canvas1.Children.Add(Canvasdrawarea);
            Canvasdrawarea.MouseEnter+=new MouseEventHandler(Canvasdrawarea_MouseEnter);
            Canvasdrawarea.MouseLeftButtonDown+=new MouseButtonEventHandler(Canvasdrawarea_MouseLeftButtonDown);
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(0, 0, Canvasdrawarea.Width, Canvasdrawarea.Height);
            Canvasdrawarea.Clip = rg;
            foreach (UIElement uiEle in Canvasdrawarea.Children)
            {
                try
                {
                    
                  loadimage= (Image)uiEle;
                  imagesincanvas++;
                  
                }
                catch
                {
 
                }
            }
            UpdateLayout();
            if (editable == "true")
            {

                bool temp=false;
                foreach (UIElement uiEle in Canvasdrawarea.Children)
                {
                    try
                    {
                        TextBlock ob = (TextBlock)uiEle;
                        temp = true;
                    }
                    catch
                    {
 
                    }
                    if (temp == true)
                    {
                        uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
                        uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                        uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
                        temp = false;
                    }
                }
                foreach (UIElement uiEle in Canvasdrawarea.Children)
                {
                    try
                    {
                        Image ob = (Image)uiEle;
                        temp = true;
                    }
                    catch
                    {

                    }
                    if (temp == true)
                    {

                        uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
                        uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                        uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
                        temp = false;
                    }
                }

            }
            else
            {
                G_button.Visibility = Visibility.Collapsed;
                canvasSavedlg.Visibility = Visibility.Collapsed;
                LayoutRoot.Children.Remove(canvasDspEleprop);
            }
          
            CanvasdrawareaLoaded();
        }

       
        

     //set the canvas properties width height cliping etc
        private void setcanvas()
        {
            Canvasdrawarea.Width = Int32.Parse(sizeC.Substring(0,3));
            Canvasdrawarea.Height = Int32.Parse(sizeC.Substring(4,3));
            Canvasdrawarea.SetValue(Canvas.BackgroundProperty,new SolidColorBrush(ParseColor(colorC)));
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(0, 0, Canvasdrawarea.Width, Canvasdrawarea.Height);
           Canvasdrawarea.Clip = rg;
            
        }

        //parse color from hex form to RBG
        private static Color ParseColor(string colorString)
        {
            colorString = colorString.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            // do we have an alpha value?
            if (colorString.Length == 8)
            {
                a = byte.Parse(colorString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            // convert color channels
            r = byte.Parse(colorString.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(colorString.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(colorString.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }


        private void hide() //on form load hide the components
        {
            buttonUpdateText.Visibility=Visibility.Collapsed;
            buttonDeleteTextBlock.Visibility = Visibility.Collapsed;
            if (canvas_imaglist.Visibility == Visibility.Visible)
            {
                Canvas_Imageholder.Visibility = Visibility.Collapsed;
            }
            if (C_Textcontrol_Holder.Visibility == Visibility.Visible)
            {
                C_Textcontrol_Holder.Visibility = Visibility.Collapsed;
            }
        }


        //create TextBlock from the elements passed and return it
        private TextBlock CreateTextBlock(string text, Color color, FontFamily font, int size)
        {
            Brush b = new SolidColorBrush(color);
           tb = new TextBlock() { Text = text, FontSize = size, FontFamily = font,Foreground=b,MaxWidth=Canvasdrawarea.Width};
         
            return (tb);
        }



        ///////////////////////////////////////Events////////////////////////////////////


        void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                // Retrieving the item's current x and y position
                double xPos = e.GetPosition(null).X - pos.X;
                double yPos = e.GetPosition(null).Y - pos.Y;

                // Re-position Element
                currEle.SetValue(Canvas.TopProperty, yPos + (double)currEle.GetValue(Canvas.TopProperty));
                currEle.SetValue(Canvas.LeftProperty, xPos + (double)currEle.GetValue(Canvas.LeftProperty));

                // Reset the new position value
                pos = e.GetPosition(null);
            }
        }


        void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            // x and y coords of mouse pointer position
            pos = e.GetPosition(null);
            // Enable mouse capture on element
            fEle.CaptureMouse();
            // Set the cursor to 'Hand' when mouse pointer is over element
            fEle.Cursor = Cursors.Hand;
            
        }


        void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                // Removes Mouse Capture from Element being dragged
                ele.ReleaseMouseCapture();
                
            }
        }

        private void Canvasdrawarea_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                TextBlock temp = e.OriginalSource as TextBlock;
                
            }
                
        }

        private void Canvasdrawarea_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                tb = e.OriginalSource as TextBlock;
                
                B_submit.Visibility = Visibility.Collapsed;
                buttonUpdateText.Visibility = Visibility.Visible; 
                buttonDeleteTextBlock.Visibility=Visibility.Visible;
                dynamicTextBlockClicked();
            }
            else
            {
              
                B_submit.Visibility = Visibility.Visible;
                buttonUpdateText.Visibility= Visibility.Collapsed;
                buttonDeleteTextBlock.Visibility = Visibility.Collapsed;
            }
            if (e.OriginalSource is Image)
            {
                editimage = e.OriginalSource as Image;
                dynamicimage();
                // MessageBox.Show(img.Tag.ToString());
            }
            else
            {
                canvasDspEleprop.Visibility = Visibility.Collapsed;
            }
        }


        Image editimage = new Image();

        private void imageupdate_Click(object sender, RoutedEventArgs e)
        {
            
            editimage.SetValue(Image.WidthProperty, Convert.ToDouble(textBoxWidth.Text));
            editimage.SetValue(Image.HeightProperty, Convert.ToDouble(textBoxHeight.Text));
           
        }

        private void deleteimage_Click(object sender, RoutedEventArgs e)
        {
            Canvasdrawarea.Children.Remove(editimage);
        }



        //here the image's properties are passed to the canvas canvasDspEleprop's textboxes
        void dynamicimage()
        {
            canvasDspEleprop.Visibility = Visibility.Visible;
            textBoxWidth.Text = editimage.ActualWidth.ToString();
            textBoxHeight.Text = editimage.ActualHeight.ToString();
        }

        //submit button events this insert the created textblock to the drawing canvas
        private void B_submit_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem x= (ComboBoxItem) F_font.SelectedItem;
            
           //= new FontFamily("Arial");
            //fo = x.Content;
            if (F_font.SelectedIndex == -1)
                fo = new FontFamily("Times New Roman");
            else
                fo =new FontFamily(x.Content.ToString());
            string sel;
            if (selectColor.Selected.ToString() == "#00000000")
            {
                sel = "#FF000000";
            }
            else
            {
                sel = selectColor.Selected.ToString();
            }
            TextBlock tb = CreateTextBlock(userinput_Text.Text.ToString(), ParseColor(sel), fo, Convert.ToInt32(textBoxFont.Text));
            Canvasdrawarea.Children.Add(tb);
          
            textblockEventDefine(tb);
            
            
        }



        public static SolidColorBrush GetColorFromHex(string myColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    Convert.ToByte(myColor.Substring(1, 2), 16),
                    Convert.ToByte(myColor.Substring(3, 2), 16),
                    Convert.ToByte(myColor.Substring(5, 2), 16),
                    Convert.ToByte(myColor.Substring(7, 2), 16)));
        }


        private void B_text_Click(object sender, RoutedEventArgs e)
        {
            if (canvas_imaglist.Visibility == Visibility.Visible)
            {
                Canvas_Imageholder.Visibility = Visibility.Collapsed;
            }
            C_Textcontrol_Holder.Visibility = Visibility.Visible;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ImageUploadPage(username);
        }


        //to fill the textblock canvase's selected color is shown in a rectangle box
        private void selectColor_SelectionChanged(object sender, EventArgs e)
        {
            rectangleColor.Fill = GetColorFromHex(selectColor.Selected.ToString());
            //MessageBox.Show(Convert.ToString (selectColor.Selected));
        }


        private void C_fontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem x =(ComboBoxItem) C_fontSize.SelectedItem;
            textBoxFont.Text =x.Content.ToString();//.ToString();
        }


        //events on textblock
        private void textblockEventDefine(UIElement uiEle)
        {
            uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
            uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
            uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
        }

        //in the draw canvas the textblock is clicked this load the textblock's properties to the editing fields
        private void dynamicTextBlockClicked()
        {
           userinput_Text.Text= tb.Text;
           textBoxFont.Text =tb.FontSize.ToString();
           ComboBoxItem cb = new ComboBoxItem();
            for(int i=0;i<F_font.Items.Count ;i++)
            {
                cb = (ComboBoxItem)F_font.Items[i];
                string font=tb.FontFamily.ToString();
                if (cb.Content.ToString()==font )
                {
                    F_font.SelectedIndex = i;
                }
            }

            rectangleColor.Fill = tb.Foreground;
        }


        //the textblock on canvas updated 
        private void buttonUpdateText_Click(object sender, RoutedEventArgs e)
        {
            string sel;
            tb.Text = userinput_Text.Text;
            if (selectColor.Selected.ToString() == "#00000000")
            {
                sel = "#FF000000";
            }
            else
            {
                sel = selectColor.Selected.ToString();
            }
            if (Convert.ToInt32(textBoxFont.Text) < 200)
            {
                tb.Foreground = GetColorFromHex(sel);
                tb.FontSize = Convert.ToDouble(textBoxFont.Text);
                ComboBoxItem x = (ComboBoxItem)F_font.SelectedItem;
                fo = new FontFamily(x.Content.ToString());
                tb.FontFamily = fo;
                tb.SetValue(Canvas.LeftProperty, (double)0);
                tb.SetValue(Canvas.TopProperty, (double)0);
            }
            else
                MessageBox.Show("Please Choose a Font Size less than 200 Pixl");

        }

        //delete textblock

        private void buttonDeleteTextBlock_Click(object sender, RoutedEventArgs e)
        {
            Canvasdrawarea.Children.Remove(tb);
        }


        //to limit the selected font size
        private void textBoxFont_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(textBoxFont.Text) > 200)
            MessageBox.Show("Please Choose a Font Size less than 200 Pixl");
        }
        //going back to the openfile dilg
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Page_OpenDialogue(username);
        }

        //save button clicked this opens the save-option's canvas
        private void button_save_Click_1(object sender, RoutedEventArgs e)
        {
            canvasSavedlg.Visibility = Visibility.Visible;
            G_button.Visibility = Visibility.Collapsed;
        }
        //when save as draft clicked here the file is inserted into the 
        private void buttonSavedraft_Click(object sender, RoutedEventArgs e)
        {
            
            client.InsertOpeCompleted += new EventHandler<ServiceReference1.InsertOpeCompletedEventArgs>(client_InsertOpeCompleted);
            client.InsertOpeAsync("insert into xmlfiles values('"+username+"','"+textBox1.Text+"','"+xamlop.xamlsaveop(Canvasdrawarea)+"','"+true+"')");
            G_button.Visibility = Visibility.Visible;
            canvasSavedlg.Visibility = Visibility.Collapsed;
        }
        //saving completed collagteral as xaml the button click event
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            client.InsertOpeCompleted += new EventHandler<ServiceReference1.InsertOpeCompletedEventArgs>(client_InsertOpeCompleted);
            client.InsertOpeAsync("insert into xmlfiles values('" + username + "','" + textBox1.Text + "','" + xamlop.xamlsaveop(Canvasdrawarea) + "','" + false + "')");
            G_button.Visibility = Visibility.Visible;
            canvasSavedlg.Visibility = Visibility.Collapsed;
        }
        //function return the xaml correnspodant of canvas and make the Canvas Draw area to xaml
        private string xamlsave()
        {
            
            G_button.Visibility = Visibility.Visible;
            canvasSavedlg.Visibility = Visibility.Collapsed;
            return(xamlop.xamlsaveop(Canvasdrawarea));
        }
        //during insertation check if there exists the user for this account 
        void client_InsertOpeCompleted(object sender, ServiceReference1.InsertOpeCompletedEventArgs e)
        {
            bool auth = (bool)e.Result;
            if (auth == true)
            {
               
            }
            else
            {

                textBox1.Text = "File Name Already Exists";
                G_button.Visibility = Visibility.Collapsed;
                canvasSavedlg.Visibility = Visibility.Visible;
            }
        }

        //
        // Load Image to canvas
        //
        private void B_image_Click(object sender, RoutedEventArgs e)
        {
            if (C_Textcontrol_Holder.Visibility == Visibility.Visible)
            {
                C_Textcontrol_Holder.Visibility = Visibility.Collapsed;
            }
            Canvas_Imageholder.Visibility = Visibility.Visible;
            Array ob;
            ob = imagelist.ToArray();
            listBox1.ItemsSource = ob;
            
            
        }
        //download image to the drawing canvas as the insert button clicked. here the list boxes selected item is the agrument
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            draimgdownloaded = false;
            client.downloadfileCompleted += new EventHandler<ServiceReference1.downloadfileCompletedEventArgs>(client_downloadfileCompleted2);
            client.downloadfileAsync(listBox1.SelectedItem.ToString());
            
        }

        public bool draimgdownloaded;//to check it this is downloaed
        private void client_downloadfileCompleted2(object sender, ServiceReference1.downloadfileCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null && draimgdownloaded == false)
                {
                    draimgdownloaded = true;
                    BitmapImage bitmapImage = new BitmapImage();
                    PictureFile pict = new PictureFile();
                    pict.PictureStream = e.Result;
                    MemoryStream stream = new MemoryStream(pict.PictureStream);
                    bitmapImage.SetSource(stream);
                    Image image = new Image();
                    image.SetValue(Image.SourceProperty, bitmapImage);
                    image.SetValue(Image.NameProperty, DateTime.Now.TimeOfDay.ToString());
                    image.SetValue(Image.TagProperty, (listBox1.SelectedItem.ToString()));
                    //image.SetValue(Image.SourceProperty, img);
                    image.SetValue(Image.StretchProperty, System.Windows.Media.Stretch.Fill);
                    image.SetValue(Image.WidthProperty, (double)45);
                    image.SetValue(Image.HeightProperty, (double)45);
                    image.MouseMove += new MouseEventHandler(Element_MouseMove);
                    image.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                    image.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);





                    Canvasdrawarea.Children.Add(image);
                    
                }
                else
                {
                    // MessageBox.Show("Error Downloading the files");
                }
            }
        }

        void client_downloadfileCompleted(object sender, ServiceReference1.downloadfileCompletedEventArgs e)
        {


            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    
                    BitmapImage bitmapImage = new BitmapImage();
                    PictureFile pict = new PictureFile();
                    
                    pict.PictureStream = e.Result;
                    MemoryStream stream = new MemoryStream(pict.PictureStream);
                    bitmapImage.SetSource(stream);
                    loadimgcanvas(bitmapImage);
                   
                        //loadimage();
                       
                    

                        
                    }
                }
                else
                {
                    MessageBox.Show("Error Downloading the files");
                }
            }
       

        void loadimgcanvas(BitmapImage bmpval)
        {
            
                ImageSource img = bmpval;
                image.SetValue(Image.SourceProperty, img);
                image.SetValue(Image.NameProperty, listBox1.SelectedItem.ToString());

                image.SetValue(Image.WidthProperty, (Double)305);
                image.SetValue(Image.HeightProperty, (Double)221);
                image.SetValue(Canvas.LeftProperty, imgposx);
                image.SetValue(Canvas.TopProperty, imgposy);
               
               

        }
        //when the image list is selected the corresponding image is loaded on to the canvas below it
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                client.downloadfileCompleted += new EventHandler<ServiceReference1.downloadfileCompletedEventArgs>(client_downloadfileCompleted);
                client.downloadfileAsync(listBox1.SelectedItem.ToString());
            }
        }
        bool temp = false;
        List<Image> imageincanvas=new List<Image>();
        private void CanvasdrawareaLoaded()
        {
           
            if (imagesincanvas != 0)
            {

                foreach (UIElement uiEle in Canvasdrawarea.Children)
                {
                    try
                    {
                        
                        loadimage = (Image)uiEle;
                        
                        client.downloadfileonloadCompleted += new EventHandler<ServiceReference1.downloadfileonloadCompletedEventArgs>(client_downloadfileonloadCompleted);

                        client.downloadfileonloadAsync(loadimage.Tag.ToString());
                        temp = true;
                    }
                    catch
                    {
                        
                    }
                    if (temp == true)
                    {
                        temp = false;
                        imageincanvas.Add(loadimage);
                    }
                }
            }
        }

        /// <summary>
        /// the mouse event for image
        /// </summary>

        int imgincanvas = 0;
        PictureFile pict=new PictureFile();
        void client_downloadfileonloadCompleted(object sender, ServiceReference1.downloadfileonloadCompletedEventArgs e)
        {
            if (e.Error == null && imgincanvas != imagesincanvas)
            {
                if (e.Result != null)
                {

                    BitmapImage bmp = new BitmapImage();
                    
                    if (pict.PictureStream != e.Result)
                    {
                        pict.PictureStream = e.Result;
                        MemoryStream memstre = new MemoryStream(pict.PictureStream);
                        bmp.SetSource(memstre);
                        //Image image = new Image();
                        imageincanvas[imgincanvas++].SetValue(Image.SourceProperty, bmp);
                      
                    }
                    

                }





            }
        }

        
        

       
       
    }
}
