using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace TMBSupportApp
{
    public partial class Form1 : Form
    {
        private XDocument document;
        private XElement root;
        private XElement receivers;
        private List<string> duplicateReceivers;
        private List<object> duplicateUsers;
        private List<object> sfnfdcList;
        private List<string> missingXML;
        private TMBDataContext context;

        public Form1()
        {
            InitializeComponent();
            duplicateReceivers = new List<string>();
            duplicateUsers = new List<object>();
            missingXML = new List<string>();
            sfnfdcList = new List<object>();
            context = new TMBDataContext();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) 
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Load the XML config file
            if (File.Exists(textBox1.Text))
            {
                document = XDocument.Load(textBox1.Text);
                root = document.Element("REFERENCE_DATA");
                receivers = root.Element("RECEIVERS");
            }

            // Check for duplicate receivers in XML
            var q = from r in receivers.Descendants("RECEIVER")
                    group r by r.Element("GROUP_INTERNAL_ID").Value into grp
                    where grp.Count()>1
                    select grp.Key;

            foreach (var v in q)             
                duplicateReceivers.Add(v);
            
            // Check for duplicate users per receiver
            var q2 = from r in receivers.Descendants("RECEIVER")
                     select r;
            foreach (var receiver in q2) 
            { 
            // Element("USERS")
                var q3 = from usr in receiver.Element("USERS").Descendants("USER")
                         group usr by usr.Element("INTERNAL_ID").Value into grp
                         where grp.Count() > 1
                         select grp.Key;

                foreach (var u in q3)
                    duplicateUsers.Add(u);
            }

            // Check for receivers where SFN and FDC Group names are the same
            var q4 = from r in receivers.Descendants("RECEIVER")
                     where r.Element("SFN_GROUP_NAME").Value == r.Element("FDC_GROUP_NAME").Value
                     select new 
                     { 
                         ReceiverName = r.Element("GROUP_INTERNAL_ID").Value ,
                         SFNName = r.Element("SFN_GROUP_NAME").Value
                     };

            // Returns banks from DB who are not in XML
            var receiverList = (receivers.Descendants("RECEIVER").Select(r => r.Element("GROUP_INTERNAL_ID").Value)).ToList();
            var q5 = context.Banks
                .Where(r=>r.Status == 1)
                .Select(r => r.InternalID)                
                .ToList()
                .Except(receiverList);

            foreach (var rec in q5)
                missingXML.Add(rec);
                
                
                                     
                     // select r
                     

            // Check if all receivers in DB are in XML

            foreach (var rec in q4)
                sfnfdcList.Add(rec.ReceiverName + ": " + rec.SFNName);

            textBox2.Text = "DUPLICATE RECEIVERS:" + Environment.NewLine;
            foreach (var s in duplicateReceivers)
                textBox2.Text += s + Environment.NewLine;

            textBox2.Text += "DUPLICATE USERS:" + Environment.NewLine;
            foreach (var s in duplicateUsers)
                textBox2.Text += s + Environment.NewLine;

            textBox2.Text += "SFN FDC CODES:" + Environment.NewLine;
            foreach (var s in sfnfdcList)
                textBox2.Text += s + Environment.NewLine;

            textBox2.Text += "MISSING in XML:" + Environment.NewLine;
            foreach (var s in missingXML)
                textBox2.Text += s + Environment.NewLine;
        }
    }
}
