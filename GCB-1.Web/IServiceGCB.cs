using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using System.Web;

namespace GCB_1.Web
{
    [ServiceContract]
    public interface IServiceGCB
    {
        [OperationContract]
        bool InsertOpe(string str);
        [OperationContract]
        bool selectop(string str, string username, string password);
        [OperationContract]
        bool IsAvail(string str, string value);
        [OperationContract]
        List<string> fill(string val, string user);
        [OperationContract]
        string selectxml(string value, string username);
        [OperationContract]
        byte[] downloadfile(string value);
        [OperationContract]
        byte[] downloadfileonload(string value);
       
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceGCB" in both code and config file together.
    //[ServiceContract]
    
    //public interface IServiceGCB
    //{
        
       
    //    [OperationContract]
    //    bool InsertOpe(string str);
    //    [OperationContract]
    //    bool selectop(string str, string username, string password);
    //    [OperationContract]
    //    bool IsAvail(string str, string id);
    //    [OperationContract]
    //    List<string> fill(string val,string user);
    //    [OperationContract]
    //    string selectxml(string xmlname,string username);
    //    [OperationContract]
    //    byte[] downloadfile(string filename, HttpContext current);
        
      
        
        
    //}
}
