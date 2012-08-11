using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel.Activation;
using System.IO;
using System.Web;

namespace GCB_1.Web
{
    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceGCB" in code, svc and config file together.
    public class ServiceGCB : IServiceGCB
    {
     //functions

        private static string con = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        //sql connection
        private SqlConnection conn = new SqlConnection(con);
        //check if connection is open or not
        public void checkconn()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        //select xmlfile return as string
        public string selectxml(string value,string username)
        {
            checkconn();
            string xml=null;
            SqlCommand sqlcmd = new SqlCommand("select xaml from xmlfiles where username='"+username+"' and name='"+value+"'", conn);
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
            while (sqlrdr.Read())
            {
                xml = sqlrdr.GetString(0);
            }

            conn.Close();
            return xml;
        }

        //send an array of xml file names associated with the user that are either editable or complted(specified in string val)
        List<string> ob=new List<string>();
        public List<string> fill(string user,string val)
        {
            checkconn();
            if (val == "true" || val == "false")
            {
                SqlCommand sqlcmd = new SqlCommand("select name from xmlfiles where username='" + user + "' and editable='" + val + "' ", conn);
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                while (sqlrdr.Read())
                {
                    ob.Add(sqlrdr.GetString(0));
                }
                return ob;
            }
            else if (val == "image")
            {
                SqlCommand sqlcmd = new SqlCommand("select relativename,imagename from ImageList where username='" + user + "'", conn);
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
                while (sqlrdr.Read())
                {
                    ob.Add(sqlrdr.GetString(0));
                    //ob.Add(sqlrdr.GetString(1))
                }
                return ob;
            }
            else
            return null;
        }
       //insert into database query is specified in str
        public bool InsertOpe(string str)
        {
            
            checkconn();
            SqlCommand sqlcmd = new SqlCommand(str, conn);
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch
            {
                return (false);
            }
            conn.Close();
            return (true);

        }

        //check if the id is already exists in database and return a true or false
        public bool IsAvail(string str, string id)
        {
            checkconn();
            bool avail = false;
            SqlCommand sqlcmd = new SqlCommand(str, conn);
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
            while (sqlrdr.Read())
            {
                if (id.ToUpper() == sqlrdr.GetString(0).ToUpper())
                {
                    avail = true;
                }
            }

            conn.Close();

            return (avail);
        }
        


        //Image download

        
        public byte[] downloadfile(string value)
        {
            
            Stream imagefile = null;
            BinaryReader reader=null;
           byte[] imageBytes=null;
          

        //   string imagePath = Current.Server.MapPath("~"+"/Image_Files" + filename + ".jpg");
         //string imagePath=  Path.Combine( "/Image_Files/" , value + ".jpg");
         //imagePath =HttpContext.Current.Server.MapPath(imagePath);
          string imagePath = HttpContext.Current.Server.MapPath(".") + "/Image_Files/" + value ;
            if (File.Exists(imagePath))
                {
                    imagefile = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    reader = new BinaryReader(imagefile);

                    imageBytes = reader.ReadBytes((int)imagefile.Length);

                    
                }
            return imageBytes;

        }

        //image download and loaded in drawing area above is used everwhere other than the canvasdraw area load
        public byte[] downloadfileonload(string value)

        {

            Stream imagefile = null;
            BinaryReader reader = null;
            byte[] imageBytes = null;


            
            string imagePath = HttpContext.Current.Server.MapPath(".") + "/Image_Files/" + value;
            if (File.Exists(imagePath))
            {
                imagefile = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                reader = new BinaryReader(imagefile);

                imageBytes = reader.ReadBytes((int)imagefile.Length);


            }
            return imageBytes;

        }
    




        
        //the select operation here check if the username and password match or exists in DB
        public bool selectop(string str,string  username,string password)
        {
            checkconn();
               
            bool auth2 = false;
            SqlCommand sqlcmd = new SqlCommand(str, conn);
            
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();
            while (sqlrdr.Read())
            {
                if (username == sqlrdr.GetString(0))
                {
                    if (password == sqlrdr.GetString(1))
                    {
                        //         Session.Add("username", username);
                        auth2 = true;

                    }
                    else
                    {
                        auth2 = false;
                    }
                }
                else
                {
                    auth2 = false;
                }



            }
            conn.Close();
            return (auth2);
        }

    }
}
