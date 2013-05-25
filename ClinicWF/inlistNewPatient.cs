using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicWF
{
    public partial class inlistNewPatient : Form
    {
        public Patients parentForm;
        public int currentPatientIndex;

        public inlistNewPatient()
        {
            InitializeComponent();
        }
        public inlistNewPatient(Patients parent)
        {
            InitializeComponent();
            parentForm = parent;
            currentPatientIndex = -1;
        }
        public inlistNewPatient(Patients parent, int currentPatientId)
        {
            InitializeComponent();

            parentForm = parent;

            currentPatientIndex = parentForm.parent.patientList.FindIndex(x => x.idNumber == currentPatientId);

            this.textBoxFName.Text = parentForm.parent.patientList[currentPatientIndex].firstName;
            this.textBoxLName.Text = parentForm.parent.patientList[currentPatientIndex].lastName;
            this.textBoxAge.Text = parentForm.parent.patientList[currentPatientIndex].age.ToString();
            this.comboBoxGender.SelectedIndex = parentForm.parent.patientList[currentPatientIndex].sex == "male" ? 0 : 1;
            this.textBoxCountry.Text = parentForm.parent.patientList[currentPatientIndex].contacts.country;
            this.textBoxCity.Text = parentForm.parent.patientList[currentPatientIndex].contacts.city;
            this.textBoxStreet.Text = parentForm.parent.patientList[currentPatientIndex].contacts.street;
            this.textBoxHouseNum.Text = parentForm.parent.patientList[currentPatientIndex].contacts.houseNumber.ToString();
            this.textBoxFlatNum.Text = parentForm.parent.patientList[currentPatientIndex].contacts.flatNumber.ToString();
            this.textBoxPhoneNum.Text = parentForm.parent.patientList[currentPatientIndex].contacts.phoneNumber.ToString();

            int illnessHistoryIndex = parentForm.parent.illnessHistoryList.FindIndex(x => x.patientID == currentPatientId);

            this.dateTimePicker1.Value = parentForm.parent.illnessHistoryList[illnessHistoryIndex].startingDate;
            this.textBoxDesease.Text = parentForm.parent.illnessHistoryList[illnessHistoryIndex].deseaseName;
            this.textBoxSymphtoms.Text = parentForm.parent.illnessHistoryList[illnessHistoryIndex].sympthoms;
            this.textBoxDrugList.Text = parentForm.parent.illnessHistoryList[illnessHistoryIndex].drugs;
            this.textBoxStatus.Text = parentForm.parent.illnessHistoryList[illnessHistoryIndex].curingStatus;
            this.dateTimePicker2.Value = parentForm.parent.illnessHistoryList[illnessHistoryIndex].approxEndingDate;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            int patientIndex;

            if (currentPatientIndex == -1)
            {
                parentForm.parent.patientList.Add(new patient());
                patientIndex = parentForm.parent.patientList.Count - 1;
            }
            else
            {
                patientIndex = currentPatientIndex;
            }

            parentForm.parent.patientList[patientIndex].firstName = this.textBoxFName.Text;
            parentForm.parent.patientList[patientIndex].lastName = this.textBoxLName.Text;
            parentForm.parent.patientList[patientIndex].age = Convert.ToInt32(this.textBoxAge.Text);
            parentForm.parent.patientList[patientIndex].sex = this.comboBoxGender.SelectedItem.ToString();

            

            if (currentPatientIndex == -1)
            {
                int max = parentForm.parent.patientList[0].idNumber;
                for (int i = 1; i < parentForm.parent.patientList.Count; i++)
                {
                    if (max < i)
                    {
                        max = i;
                    }
                }
                parentForm.parent.patientList[patientIndex].idNumber = max + 1;
            }

            parentForm.parent.patientList[patientIndex].contacts.country = this.textBoxCountry.Text;
            parentForm.parent.patientList[patientIndex].contacts.city = this.textBoxCity.Text;
            parentForm.parent.patientList[patientIndex].contacts.street = this.textBoxStreet.Text;
            parentForm.parent.patientList[patientIndex].contacts.houseNumber = Convert.ToInt32(this.textBoxHouseNum.Text);
            parentForm.parent.patientList[patientIndex].contacts.flatNumber = Convert.ToInt32(this.textBoxFlatNum.Text);
            parentForm.parent.patientList[patientIndex].contacts.phoneNumber = Convert.ToInt32(this.textBoxPhoneNum.Text);


            int illnessHistoryIndex;

            if (currentPatientIndex == -1)
            {
                parentForm.parent.illnessHistoryList.Add(new IllnessHistory());
                illnessHistoryIndex = parentForm.parent.illnessHistoryList.Count - 1;
                parentForm.parent.illnessHistoryList[illnessHistoryIndex].patientID = parentForm.parent.patientList[patientIndex].idNumber;
                parentForm.currentPatientId = parentForm.parent.patientList[patientIndex].idNumber;
            }
            else
            {
                illnessHistoryIndex = parentForm.parent.illnessHistoryList.FindIndex(x => x.patientID == patientIndex);
            }

            parentForm.parent.illnessHistoryList[illnessHistoryIndex].startingDate = this.dateTimePicker1.Value;
            parentForm.parent.illnessHistoryList[illnessHistoryIndex].deseaseName = this.textBoxDesease.Text;
            parentForm.parent.illnessHistoryList[illnessHistoryIndex].sympthoms = this.textBoxSymphtoms.Text;
            parentForm.parent.illnessHistoryList[illnessHistoryIndex].drugs = this.textBoxDrugList.Text;
            parentForm.parent.illnessHistoryList[illnessHistoryIndex].curingStatus = this.textBoxStatus.Text;
            parentForm.parent.illnessHistoryList[illnessHistoryIndex].approxEndingDate = this.dateTimePicker2.Value;

            

            this.Close();
        }
    }
}
