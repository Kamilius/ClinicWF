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
        public List<patient> patientList;
        public List<IllnessHistory> illnessHistoryList;
        public int currentPatientId;

        public Patients()
        {
            InitializeComponent();
        }
        public Patients(Form1 parent)
        {
            InitializeComponent();

            this.MdiParent = parent;

            patientList = new List<patient>();
            illnessHistoryList = new List<IllnessHistory>();
            loadPatients();

            refreshListBox();
        }

        public void refreshListBox()
        {
            this.listBox1.Items.Clear();
            this.listBox1.Refresh();
            foreach (patient p in patientList)
            {
                currentPatientId = p.idNumber;
                this.listBox1.Items.Add(p.firstName + " " + p.lastName);
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
            else
            {
                Console.Clear();
                Console.WriteLine("Illness histories directory is empty!");
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
}
