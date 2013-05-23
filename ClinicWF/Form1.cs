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
    public partial class Form1 : Form
    {
        public Patients doctorsMenu;
        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length == 0)
            {
                doctorsMenu = new Patients(this);
                doctorsMenu.Show();
                doctorsMenu.Left = 0;
                doctorsMenu.Top = 0;
                doctorsMenu.Size = this.ClientRectangle.Size;
                doctorsMenu.WindowState = FormWindowState.Maximized;
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
}
