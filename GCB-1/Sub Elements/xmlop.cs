using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using projectsilverlight.blogspot.com.XamlWriter;
using System.Xml;
using System.Windows.Markup;

namespace GCB_1.Sub_Elements
{
    public class xmlop
    {
        public string xamlsaveop(Canvas canvas1)
        {
            XamlWriter xwritr = new XamlWriter();
            //textBox2.Text = xwritr.WriteXaml(canvas1, XamlWriterSettings.LogicalTree).ToString();
           string xml = xwritr.WriteXaml(canvas1, XamlWriterSettings.LogicalTree).ToString();


            xml = xml.Replace(Environment.NewLine, "");

            return xml;
        }
        public UIElement xamlreadop(string xml)
        {
            UIElement tree = (UIElement)XamlReader.Load(xml);
            return tree;
        }
    }
}
