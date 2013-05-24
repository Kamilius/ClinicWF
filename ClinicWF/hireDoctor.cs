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
        public Doctors parentForm;
        public int currentDoctorIndex;
        public hireDoctor()
        {
            InitializeComponent();
        }
        public hireDoctor(Doctors parent)
        {
            InitializeComponent();
            parentForm = parent;
            currentDoctorIndex = -1;
        }
        public hireDoctor(Doctors parent, int currentDoctorId)
        {
            InitializeComponent();

            parentForm = parent;

            currentDoctorIndex = parentForm.parent.doctorList.FindIndex(x => x.idNumber == currentDoctorId);

            this.textBoxFName.Text = parentForm.parent.doctorList[currentDoctorIndex].firstName;
            this.textBoxLName.Text = parentForm.parent.doctorList[currentDoctorIndex].lastName;
            this.textBoxAge.Text = parentForm.parent.doctorList[currentDoctorIndex].age.ToString();
            this.comboBoxGender.SelectedIndex = parentForm.parent.doctorList[currentDoctorIndex].sex == "male" ? 0 : 1;
            this.textBoxCountry.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.country;
            this.textBoxCity.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.city;
            this.textBoxStreet.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.street;
            this.textBoxHouseNum.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.houseNumber.ToString();
            this.textBoxFlatNum.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.flatNumber.ToString();
            this.textBoxPhoneNum.Text = parentForm.parent.doctorList[currentDoctorIndex].contacts.phoneNumber.ToString();
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
                parentForm.parent.doctorList.Add(new doctor());
                docIndex = parentForm.parent.doctorList.Count - 1;
            }
            else
            {
                docIndex = currentDoctorIndex;
            }

            parentForm.parent.doctorList[docIndex].firstName = this.textBoxFName.Text;
            parentForm.parent.doctorList[docIndex].lastName = this.textBoxLName.Text;
            parentForm.parent.doctorList[docIndex].age = Convert.ToInt32(this.textBoxAge.Text);
            parentForm.parent.doctorList[docIndex].sex = this.comboBoxGender.SelectedItem.ToString();

            int max = parentForm.parent.doctorList[0].idNumber;
            for (int i = 1; i < parentForm.parent.doctorList.Count; i++)
            {
                if (max < i)
                {
                    max = i;
                }
            }
            parentForm.parent.doctorList[docIndex].idNumber = max + 1;
            parentForm.parent.doctorList[docIndex].contacts.country = this.textBoxCountry.Text;
            parentForm.parent.doctorList[docIndex].contacts.city = this.textBoxCity.Text;
            parentForm.parent.doctorList[docIndex].contacts.street = this.textBoxStreet.Text;
            parentForm.parent.doctorList[docIndex].contacts.houseNumber = Convert.ToInt32(this.textBoxHouseNum.Text);
            parentForm.parent.doctorList[docIndex].contacts.flatNumber = Convert.ToInt32(this.textBoxFlatNum.Text);
            parentForm.parent.doctorList[docIndex].contacts.phoneNumber = Convert.ToInt32(this.textBoxPhoneNum.Text);
            parentForm.parent.doctorList[docIndex].contractExpirationDate = DateTime.Today;
            parentForm.parent.doctorList[docIndex].contractExpirationDate = new DateTime(parentForm.parent.doctorList[docIndex].contractExpirationDate.Year + 1, parentForm.parent.doctorList[docIndex].contractExpirationDate.Month, parentForm.parent.doctorList[docIndex].contractExpirationDate.Day);

            this.Close();
        }
    }
}
