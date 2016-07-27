using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using VKDriveSettings.Properties;

namespace VKDriveSettings
{
	internal partial class Form1 : Form
    {
        const int TYPE_TEXT = 1;
        string errorMessage = "Что-то пошло не так(";
        string skipMessage = "Что-то пошло не так(";

        public Form1(string[] args)
        {

            int type;
            string name, actionName;

			if (args.Length > 0)
			{
				switch (args[0])
				{
					case "--GC":
						Directory.CreateDirectory("\\_SYSTEM\\GC");
						MessageBox.Show(Resources.Form1_Form1_Операция_выполнена);
						Environment.Exit(0);
						break;
				}
			}

			try
            {
                var document = new XmlDocument();
                document.Load("VKDirvePathData.xml");

                type = Convert.ToInt32(readElement(document, "type"));
                name = readElement(document, "name");
                actionName = readElement(document, "actionName");
                errorMessage = readElement(document, "errorMessage");
                skipMessage = readElement(document, "skipMessage");
            }
            catch (Exception) {
                MessageBox.Show(Resources.Form1_Form1_nor_vkdrive_drive);
                Environment.Exit(0);
                return;
            }

	        InitializeComponent();

            if (type == TYPE_TEXT)
            {
                Text += name;
                labelActionName.Text = actionName;
            }
            

            toolStripStatusLabel1.Text = AppDomain.CurrentDomain.BaseDirectory;
        }
        
        string readElement(XmlDocument document, string name)
        {
            XmlNode node = document.GetElementsByTagName(name)[0];
            return node.InnerText;
        }


        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] fileListOld = Directory.GetDirectories(Directory.GetCurrentDirectory());

                string filename = Regex.Replace(textBox1.Text, @"http(s|)://.*/", "");

                Directory.CreateDirectory(Directory.GetCurrentDirectory()+"\\"+filename);
                string[] fileListNew = Directory.GetDirectories(Directory.GetCurrentDirectory());

                if (fileListOld.Length == fileListNew.Length - 1)
                {
                    string diff = null;
                    for (int i = 0; i < fileListNew.Length; i++)
                    {
                        if (fileListNew.Length - 1 == i)
                        {
                            diff = fileListNew[i];
                            break;
                        }
						if (fileListOld[i] != fileListNew[i])
                        {
                            bool isNew = true;
                            for (int j = 0; j < fileListOld.Length; j++)
                            {
                                if (fileListOld[j] == fileListNew[i])
                                {
                                    isNew = false;
                                }
                            }
                            if (isNew)
                            {
                                diff = fileListNew[i];
                                break;
                            }
                        }
                    }
                    MessageBox.Show("Теперь появился новый каталог: "+diff.Substring(Directory.GetCurrentDirectory().Length+1));
                }
                else
                {
                    MessageBox.Show(skipMessage, "Что-то пошло не так");
                }
                Close();
                
            }catch(Exception){
                MessageBox.Show(errorMessage, "Что-то пошло не так");
            }
        }

    }
}
