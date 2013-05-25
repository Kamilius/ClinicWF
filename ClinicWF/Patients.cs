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
        public int currentPatientId;
        public Form1 parent;

        public Patients()
        {
            InitializeComponent();
        }
        public Patients(Form1 parent)
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
            foreach (patient p in parent.patientList)
            {
                currentPatientId = p.idNumber;
                this.listBox1.Items.Add(p.firstName + " " + p.lastName);
            }
        }

        public void showIllnessHistory(object sender, EventArgs e)
        {
            try
            {
                currentPatientId = parent.patientList.Find(x => (x.firstName + " " + x.lastName) == this.listBox1.SelectedItem.ToString()).idNumber;
                string choice = this.listBox1.SelectedItem.ToString();
                if (parent.patientList.Contains(parent.patientList.Find(patient => patient.firstName + " " + patient.lastName == choice)))
                {
                    int index = parent.patientList.FindIndex(patient => patient.firstName + " " + patient.lastName == choice);
                    int illnessHistoryIndex = parent.illnessHistoryList.FindIndex(IllnessHistory => IllnessHistory.patientID == parent.patientList[index].idNumber);
                    int doctorId = parent.illnessHistoryList[illnessHistoryIndex].currentDoctor;
                    int doctorIndex = parent.doctorList.FindIndex(doctor => doctor.idNumber == doctorId);

                    this.textBox1.Clear();
                    this.textBox2.Clear();
                    foreach (patient p in parent.patientList)
                    {
                        if ((p.firstName + " " + p.lastName) == this.listBox1.SelectedItem.ToString())
                        {
                            this.textBox1.Text = p.ToString();

                            foreach (IllnessHistory iH in parent.illnessHistoryList)
                            {
                                if (p.idNumber == iH.patientID)
                                {
                                    string currentDoctor;
                                    if (doctorIndex >= 0)
                                    {
                                        currentDoctor = parent.doctorList[doctorIndex].firstName + " " + parent.doctorList[doctorIndex].lastName;
                                    }
                                    else
                                    {
                                        currentDoctor = "No doctor assigned";
                                    }
                                    this.textBox2.Text = iH.ToString() + Environment.NewLine + "Current doctor: " + currentDoctor;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                
            }
            catch
            {
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            inlistNewPatient inlistForm = new inlistNewPatient(this);
            inlistForm.ShowDialog();
            saveHistory(parent.patientList);
            refreshListBox();
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            inlistNewPatient inlistForm = new inlistNewPatient(this, currentPatientId);
            inlistForm.ShowDialog();
            saveHistory(parent.patientList);
            refreshListBox();
        }
        private void checkOutPatient(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Are you sure, you want to checkout a patient?", "Checkout patient", MessageBoxButtons.YesNo);

                if (DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    patient removePatient = this.parent.patientList.Find(x => (x.firstName + " " + x.lastName) == this.listBox1.SelectedItem.ToString());
                    File.Delete("history/" + removePatient.firstName + removePatient.lastName + ".xml");
                    int remIllnessIndex = this.parent.illnessHistoryList.FindIndex(x => x.patientID == removePatient.idNumber);
                    this.parent.illnessHistoryList.RemoveAt(remIllnessIndex);
                    this.parent.patientList.Remove(removePatient);
                    refreshListBox();
                }
            }
            catch
            {
                MessageBox.Show("No patient selected!!!");
            }
        }
        public void saveHistory(List<patient> pL)
        {
            foreach (patient p in pL)
            {
                if (currentPatientId == p.idNumber)
                {
                    int illnessHistoryIndex = this.parent.illnessHistoryList.FindIndex(x => x.patientID == p.idNumber);
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
                        writer.WriteElementString("patient", parent.illnessHistoryList[illnessHistoryIndex].patientID.ToString());
                        writer.WriteStartElement("startDate");
                        writer.WriteElementString("day", parent.illnessHistoryList[illnessHistoryIndex].startingDate.Day.ToString());
                        writer.WriteElementString("month", parent.illnessHistoryList[illnessHistoryIndex].startingDate.Month.ToString());
                        writer.WriteElementString("year", parent.illnessHistoryList[illnessHistoryIndex].startingDate.Year.ToString());
                        writer.WriteEndElement();
                        writer.WriteElementString("desease", parent.illnessHistoryList[illnessHistoryIndex].deseaseName);
                        writer.WriteElementString("sympthoms", parent.illnessHistoryList[illnessHistoryIndex].sympthoms);
                        writer.WriteElementString("drugs", parent.illnessHistoryList[illnessHistoryIndex].drugs);
                        writer.WriteElementString("status", parent.illnessHistoryList[illnessHistoryIndex].curingStatus);
                        writer.WriteElementString("doctor", parent.illnessHistoryList[illnessHistoryIndex].currentDoctor.ToString());
                        writer.WriteStartElement("endDate");
                        writer.WriteElementString("day", parent.illnessHistoryList[illnessHistoryIndex].approxEndingDate.Day.ToString());
                        writer.WriteElementString("month", parent.illnessHistoryList[illnessHistoryIndex].approxEndingDate.Month.ToString());
                        writer.WriteElementString("year", parent.illnessHistoryList[illnessHistoryIndex].approxEndingDate.Year.ToString());
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
    }
}
