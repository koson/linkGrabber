using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linkGrabber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ImageList = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebBrowser wb = new WebBrowser();
            wb.Url = new Uri(textBox1.Text);
            wb.DocumentCompleted += Wb_DocumentCompleted; ;
        }

        private void Wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument source = ((WebBrowser)sender).Document;

            extractLink(source);
        }

        private void extractLink(HtmlDocument source)
        {
            //HtmlElementCollection anchorList = source.GetElementsByTagName("a");
            HtmlElementCollection anchorList = source.Links;


            foreach (var item in anchorList)

            {

                textBox2.AppendText(((HtmlElement)item).GetAttribute("href"));

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetAllImages();
        }
        public List<string> ImageList;
        private void GetAllImages()
        {
            // Declaring 'x' as a new WebClient() method
            WebClient x = new WebClient();

            // Setting the URL, then downloading the data from the URL.
            string source = x.DownloadString(textBox1.Text);

            // Declaring 'document' as new HtmlAgilityPack() method
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();

            // Loading document's source via HtmlAgilityPack
            document.LoadHtml(source);
            textBox2.Clear();
            // For every tag in the HTML containing the node img.
            foreach (var link in document.DocumentNode.Descendants("a")
                                        .Select(i => i.Attributes["href"]))
            {
                // Storing all links found in an array.
                // You can declare this however you want.
                //ImageList.Add(link.Attribute["src"].Value.ToString());
                if (link != null && link.Value.Contains("https"))
                {
                    ImageList.Add(link.Value);
                    textBox2.AppendText(link.Value + Environment.NewLine);
                }
            }

            Clipboard.SetText(string.Join(Environment.NewLine, ImageList.Cast<object>().Select(o => o.ToString()).ToArray()));
        }
    }
}
