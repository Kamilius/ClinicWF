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
    public partial class Form1 : Form
    {
        public Doctors doctorsMenu;
        public Patients patientsMenu;
        public List<doctor> doctorList;
        public List<patient> patientList;
        public List<IllnessHistory> illnessHistoryList;

        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            doctorList = new List<doctor>();
            loadDoctorInfo(doctorList);

            patientList = new List<patient>();
            illnessHistoryList = new List<IllnessHistory>();
            loadPatients();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                foreach (Form child in this.MdiChildren)
                {
                    child.Close();
                }
            }

            doctorsMenu = new Doctors(this);
            doctorsMenu.Show();
            doctorsMenu.Left = 0;
            doctorsMenu.Top = 0;
            doctorsMenu.Size = this.ClientRectangle.Size;
            doctorsMenu.WindowState = FormWindowState.Maximized;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length != 0)
            {
                foreach (Form child in this.MdiChildren)
                {
                    child.Close();
                }
            }

            patientsMenu = new Patients(this);
            patientsMenu.Show();
            patientsMenu.Left = 0;
            patientsMenu.Top = 0;
            patientsMenu.Size = this.ClientRectangle.Size;
            patientsMenu.WindowState = FormWindowState.Maximized;
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
        public void loadPatients()
        {
            DirectoryInfo di = new DirectoryInfo("history/");
            FileInfo[] fi = di.GetFiles();

            if (fi.Length > 0)
            {
                for (int i = 0; i < fi.Length; i++)
                {
                    illnessHistoryList.Add(new IllnessHistory());
                    illnessHistoryList[i].loadHistory(fi[i].Name);
                    patientList.Add(new patient());
                    patientList[i].loadPatient(fi[i].Name, patientList);
                }
            }
        }
    }
    public class Contacts
    {
        public string country;
        public string city;
        public string street;
        public int houseNumber;
        public int flatNumber;
        public int phoneNumber;

        public Contacts()
        {
        }

        public Contacts(string cntry, string city, string str, int houseNum, int flatNum, int phoneNum)
        {
            country = cntry;
            this.city = city;
            street = str;
            houseNumber = houseNum;
            flatNumber = flatNum;
            phoneNumber = phoneNum;
        }

        public override string ToString()
        {
            return Environment.NewLine + "Country: " + country +
                   Environment.NewLine + "City: " + city +
                   Environment.NewLine + "Address: " + street + " " + houseNumber + "/" + flatNumber +
                   Environment.NewLine + "Phone number: " + phoneNumber;
        }
    }
    public class SimpleHuman
    {
        public string firstName;
        public string lastName;
        public int age;
        public string sex;
        public Contacts contacts;

        public SimpleHuman()
        {
            contacts = new Contacts();
        }

        public SimpleHuman(string fn, string ln, int a, string s, Contacts c)
        {
            firstName = fn;
            lastName = ln;
            age = a;
            sex = s;
            contacts = c;
        }

        public override string ToString()
        {
            return Environment.NewLine + "First name: " + firstName +
                   Environment.NewLine + "Last name: " + lastName +
                   Environment.NewLine + "Age: " + age +
                   Environment.NewLine + "Sex: " + sex +
                   contacts.ToString();
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
    public class patient : SimpleHuman
    {
        public int idNumber;

        public patient()
        {
        }

        public patient(string n, string sn, int a, string s, int id, Contacts c)
            : base(n, sn, a, s, c)
        {
            idNumber = id;
        }

        public override string ToString()
        {
            return "\nPatient ID: " + idNumber + base.ToString();
        }

        public void loadPatient(string historyName, List<patient> patientList)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("history/" + historyName);

            XmlNodeList root = doc.GetElementsByTagName("patient");
            root = root[0].ChildNodes;

            patientList.Add(new patient());

            foreach (XmlNode child in root)
            {
                switch (child.Name)
                {
                    case "first-name":
                        firstName = child.InnerText;
                        break;

                    case "last-name":
                        lastName = child.InnerText;
                        break;

                    case "age":
                        age = Convert.ToInt32(child.InnerText);
                        break;

                    case "sex":
                        sex = child.InnerText;
                        break;

                    case "id":
                        idNumber = Convert.ToInt32(child.InnerText);
                        break;

                    case "country":
                        contacts.country = child.InnerText;
                        break;

                    case "city":
                        contacts.city = child.InnerText;
                        break;

                    case "street":
                        contacts.street = child.InnerText;
                        break;

                    case "house-number":
                        contacts.houseNumber = Convert.ToInt32(child.InnerText);
                        break;

                    case "flat-number":
                        contacts.flatNumber = Convert.ToInt32(child.InnerText);
                        break;

                    case "phone-number":
                        contacts.phoneNumber = Convert.ToInt32(child.InnerText);
                        break;
                }
            }
        }
    }
    public class IllnessHistory
    {
        public int patientID;
        public DateTime startingDate;
        public string sympthoms;
        public string deseaseName;
        public string drugs;
        public int currentDoctor;
        public string curingStatus;
        public DateTime approxEndingDate;

        public IllnessHistory()
        {
            startingDate = new DateTime();
            approxEndingDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day + 3);
            currentDoctor = 0;
        }

        public void loadHistory(string historyName)
        {

            int day = 0, month = 0, year = 0;
            XmlNodeList date;
            XmlDocument doc = new XmlDocument();
            doc.Load("history/" + historyName);

            XmlNodeList root = doc.GetElementsByTagName("history");
            root = root[0].ChildNodes;

            foreach (XmlNode child in root)
            {
                switch (child.Name)
                {
                    case "patient":
                        patientID = Convert.ToInt32(child.InnerText);
                        break;

                    case "startDate":
                        date = child.ChildNodes;
                        day = Convert.ToInt32(date.Item(0).InnerText);
                        month = Convert.ToInt32(date.Item(1).InnerText);
                        year = Convert.ToInt32(date.Item(2).InnerText);
                        startingDate = new DateTime(year, month, day);
                        break;

                    case "desease":
                        deseaseName = child.InnerText;
                        break;

                    case "sympthoms":
                        sympthoms = child.InnerText;
                        break;

                    case "drugs":
                        drugs = child.InnerText;
                        break;

                    case "status":
                        curingStatus = child.InnerText;
                        break;

                    case "doctor":
                        currentDoctor = Convert.ToInt32(child.InnerText);
                        break;

                    case "endDate":
                        date = child.ChildNodes;
                        day = Convert.ToInt32(date.Item(0).InnerText);
                        month = Convert.ToInt32(date.Item(1).InnerText);
                        year = Convert.ToInt32(date.Item(2).InnerText);
                        approxEndingDate = new DateTime(year, month, day);
                        break;
                }
            }
        }
        public void saveHistory(List<patient> pL)
        {
            foreach (patient p in pL)
            {
                if (patientID == p.idNumber)
                {
                    XmlWriter writer = null;

                    try
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        writer = XmlWriter.Create("history/" + p.firstName + p.lastName + ".xml", settings);
                        writer.WriteStartDocument();
                        writer.WriteStartElement("patient-info");

                        writer.WriteStartElement("patient");
                        writer.WriteElementString("first-name", p.firstName);
                        writer.WriteElementString("last-name", p.lastName);
                        writer.WriteElementString("age", p.age.ToString());
                        writer.WriteElementString("sex", p.sex);
                        writer.WriteElementString("id", p.idNumber.ToString());
                        writer.WriteElementString("country", p.contacts.country);
                        writer.WriteElementString("city", p.contacts.city);
                        writer.WriteElementString("street", p.contacts.street);
                        writer.WriteElementString("house-number", p.contacts.houseNumber.ToString());
                        writer.WriteElementString("flat-number", p.contacts.flatNumber.ToString());
                        writer.WriteElementString("phone-number", p.contacts.phoneNumber.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("history");
                        writer.WriteElementString("patient", patientID.ToString());
                        writer.WriteStartElement("startDate");
                        writer.WriteElementString("day", startingDate.Day.ToString());
                        writer.WriteElementString("month", startingDate.Month.ToString());
                        writer.WriteElementString("year", startingDate.Year.ToString());
                        writer.WriteEndElement();
                        writer.WriteElementString("desease", deseaseName);
                        writer.WriteElementString("sympthoms", sympthoms);
                        writer.WriteElementString("drugs", drugs);
                        writer.WriteElementString("status", curingStatus);
                        writer.WriteElementString("doctor", currentDoctor.ToString());
                        writer.WriteStartElement("endDate");
                        writer.WriteElementString("day", approxEndingDate.Day.ToString());
                        writer.WriteElementString("month", approxEndingDate.Month.ToString());
                        writer.WriteElementString("year", approxEndingDate.Year.ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();


                    }
                    finally
                    {
                        if (writer != null)
                        {
                            writer.Close();
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            string str = "\nStart of curing: " + startingDate.Day + "/" + startingDate.Month + "/" + startingDate.Year +
                         "\nDesease name: " + deseaseName +
                         "\nSympthoms: " + sympthoms +
                         "\nDrugs list: " + drugs +
                         "\nStatus of desease: " + curingStatus +
                         "\nApproximate curing end date: " + approxEndingDate.Day + "/" + approxEndingDate.Month + "/" + approxEndingDate.Year;
            return str;
        }
    }
}
