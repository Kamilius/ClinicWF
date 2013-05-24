using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace ClinicWF
{
    public partial class Doctors : Form
    {
        public int currentDoctorId;
        public Form1 parent;

        public Doctors()
        {
            InitializeComponent();
        }
        public Doctors(Form1 parent)
        {
            InitializeComponent();

            this.MdiParent = parent;
            this.parent = parent;

            refreshListBox();
        }

        public void refreshListBox()
        {
            this.listBox1.Items.Clear();
            this.listBox1.Refresh();
            foreach (doctor d in parent.doctorList)
            {
                currentDoctorId = d.idNumber;
                this.listBox1.Items.Add(d.firstName + " " + d.lastName);
            }
        }

        public void saveDoctorInfo()
        {
            foreach (doctor d in parent.doctorList)
            {
                XmlSerializer SerializerDoc = new XmlSerializer(typeof(doctor));

                TextWriter WriteFileStream = new StreamWriter("doctors/" + d.firstName + d.lastName + ".xml");
                SerializerDoc.Serialize(WriteFileStream, d);

                WriteFileStream.Close();
            }
            Console.WriteLine("Doctors info is saved!");
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Clear();
                foreach (doctor d in parent.doctorList)
                {
                    if ((d.firstName + " " + d.lastName) == this.listBox1.SelectedItem.ToString())
                    {
                        currentDoctorId = d.idNumber;
                        this.textBox1.Text = d.ToString();
                        break;
                    }
                }
            }
            catch
            {
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Clear();
                foreach (doctor d in parent.doctorList)
                {
                    if ((d.firstName + " " + d.lastName) == this.listBox1.SelectedItem.ToString())
                    {
                        parent.doctorList.Remove(d);
                        break;
                    }
                }
                refreshListBox();
            }
            catch
            {
                MessageBox.Show("No item selected!!!");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            hireDoctor hireForm = new hireDoctor(this);
            hireForm.ShowDialog();
            saveDoctorInfo();
            refreshListBox();
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            hireDoctor hireForm = new hireDoctor(this, currentDoctorId);
            hireForm.ShowDialog();
            saveDoctorInfo();
            refreshListBox();
        }
    }
}
