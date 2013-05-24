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
                                    if (doctorIndex > 0)
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

        private void showIllnessHistory()
        {

        }
    }
}
