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
    public partial class Patients : Form
    {
        public List<doctor> doctorList;
        public int currentDoctorId;

        public Patients()
        {
            InitializeComponent();
        }
        public Patients(Form1 parent)
        {
            InitializeComponent();

            this.MdiParent = parent;

            doctorList = new List<doctor>();
            loadDoctorInfo(doctorList);

            refreshListBox();
        }

        public void refreshListBox()
        {
            this.listBox1.Items.Clear();
            this.listBox1.Refresh();
            foreach (doctor d in doctorList)
            {
                currentDoctorId = d.idNumber;
                this.listBox1.Items.Add(d.firstName + " " + d.lastName);
            }
        }

        public void saveDoctorInfo(List<doctor> doctorList)
        {
            foreach (doctor d in doctorList)
            {
                XmlSerializer SerializerDoc = new XmlSerializer(typeof(doctor));

                TextWriter WriteFileStream = new StreamWriter("doctors/" + d.firstName + d.lastName + ".xml");
                SerializerDoc.Serialize(WriteFileStream, d);

                WriteFileStream.Close();
            }
            Console.WriteLine("Doctors info is saved!");
        }
        public void saveDoctorInfo()
        {
            foreach (doctor d in doctorList)
            {
                XmlSerializer SerializerDoc = new XmlSerializer(typeof(doctor));

                TextWriter WriteFileStream = new StreamWriter("doctors/" + d.firstName + d.lastName + ".xml");
                SerializerDoc.Serialize(WriteFileStream, d);

                WriteFileStream.Close();
            }
            Console.WriteLine("Doctors info is saved!");
        }
        public void loadDoctorInfo(List<doctor> doctorList)
        {
            DirectoryInfo di = new DirectoryInfo("doctors/");
            FileInfo[] fi = di.GetFiles();

            if (fi.Length > 0)
            {
                doctorList.Clear();
                XmlSerializer SerializerDoc = new XmlSerializer(typeof(doctor));
                FileStream ReadFileStream;

                for (int i = 0; i < fi.Length; i++)
                {
                    ReadFileStream = new FileStream("doctors/" + fi[i].Name, FileMode.Open, FileAccess.Read, FileShare.Read);
                    doctorList.Add(new doctor());
                    doctorList[i] = (doctor)SerializerDoc.Deserialize(ReadFileStream);

                    ReadFileStream.Close();
                }
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Clear();
                foreach (doctor d in doctorList)
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
                foreach (doctor d in doctorList)
                {
                    if ((d.firstName + " " + d.lastName) == this.listBox1.SelectedItem.ToString())
                    {
                        doctorList.Remove(d);
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
    
    public class doctor : SimpleHuman
    {
        public DateTime contractExpirationDate;
        public int idNumber;
        [NonSerialized()]
        public List<int> doctorPatients;

        public doctor()
            : base()
        {
            doctorPatients = new List<int>();
            contractExpirationDate = new DateTime();
        }

        public doctor(string name, string surname, int age, string sex, int doctorID, Contacts c)
            : base(name, surname, age, sex, c)
        {
            contractExpirationDate = new DateTime(9999);
            idNumber = doctorID;
            doctorPatients = new List<int>();
        }

        public override string ToString()
        {
            return "Doctor patient's count: " + doctorPatients.Count +
                   base.ToString() +
                   Environment.NewLine + "Contract expiration date: " + contractExpirationDate.Day + "/" + contractExpirationDate.Month + "/" + contractExpirationDate.Year;
        }
    }
}
