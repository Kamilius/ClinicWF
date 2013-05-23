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
    public partial class hireDoctor : Form
    {
        public Patients parentForm;
        public int currentDoctorIndex;
        public hireDoctor()
        {
            InitializeComponent();
        }
        public hireDoctor(Patients parent)
        {
            InitializeComponent();
            parentForm = parent;
            currentDoctorIndex = -1;
        }
        public hireDoctor(Patients parent, int currentDoctorId)
        {
            InitializeComponent();

            parentForm = parent;

            currentDoctorIndex = parentForm.doctorList.FindIndex(x => x.idNumber == currentDoctorId);

            this.textBoxFName.Text = parentForm.doctorList[currentDoctorIndex].firstName;
            this.textBoxLName.Text = parentForm.doctorList[currentDoctorIndex].lastName;
            this.textBoxAge.Text = parentForm.doctorList[currentDoctorIndex].age.ToString();
            this.comboBoxGender.SelectedIndex = parentForm.doctorList[currentDoctorIndex].sex == "male"? 0 : 1;
            this.textBoxCountry.Text = parentForm.doctorList[currentDoctorIndex].contacts.country;
            this.textBoxCity.Text = parentForm.doctorList[currentDoctorIndex].contacts.city;
            this.textBoxStreet.Text = parentForm.doctorList[currentDoctorIndex].contacts.street;
            this.textBoxHouseNum.Text = parentForm.doctorList[currentDoctorIndex].contacts.houseNumber.ToString();
            this.textBoxFlatNum.Text = parentForm.doctorList[currentDoctorIndex].contacts.flatNumber.ToString();
            this.textBoxPhoneNum.Text = parentForm.doctorList[currentDoctorIndex].contacts.phoneNumber.ToString();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            int docIndex;

            if (currentDoctorIndex == -1)
            {
                parentForm.doctorList.Add(new doctor());
                docIndex = parentForm.doctorList.Count - 1;
            }
            else
            {
                docIndex = currentDoctorIndex;
            }

            parentForm.doctorList[docIndex].firstName = this.textBoxFName.Text;
            parentForm.doctorList[docIndex].lastName = this.textBoxLName.Text;
            parentForm.doctorList[docIndex].age = Convert.ToInt32(this.textBoxAge.Text);
            parentForm.doctorList[docIndex].sex = this.comboBoxGender.SelectedItem.ToString();

            int max = parentForm.doctorList[0].idNumber;
            for (int i = 1; i < parentForm.doctorList.Count; i++)
            {
                if (max < i)
                {
                    max = i;
                }
            }
            parentForm.doctorList[docIndex].idNumber = max + 1;
            parentForm.doctorList[docIndex].contacts.country = this.textBoxCountry.Text;
            parentForm.doctorList[docIndex].contacts.city = this.textBoxCity.Text;
            parentForm.doctorList[docIndex].contacts.street = this.textBoxStreet.Text;
            parentForm.doctorList[docIndex].contacts.houseNumber = Convert.ToInt32(this.textBoxHouseNum.Text);
            parentForm.doctorList[docIndex].contacts.flatNumber = Convert.ToInt32(this.textBoxFlatNum.Text);
            parentForm.doctorList[docIndex].contacts.phoneNumber = Convert.ToInt32(this.textBoxPhoneNum.Text);
            parentForm.doctorList[docIndex].contractExpirationDate = DateTime.Today;
            parentForm.doctorList[docIndex].contractExpirationDate = new DateTime(parentForm.doctorList[docIndex].contractExpirationDate.Year + 1, parentForm.doctorList[docIndex].contractExpirationDate.Month, parentForm.doctorList[docIndex].contractExpirationDate.Day);

            this.Close();
        }
    }
}
