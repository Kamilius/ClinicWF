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
    public partial class loginForm : Form
    {
        public Form1 parentForm;

        public loginForm()
        {
            InitializeComponent();
        }
        public loginForm(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;
            this.MdiParent = parent;
        }
        public void loginPassCheck(object sender, EventArgs e)
        {
            bool correctUser = false;

            foreach (User user in parentForm.usersList)
            {
                if (user.username == this.textBoxLogin.Text && user.password == this.textBoxPass.Text)
                {
                    correctUser = true;
                    break;
                }
            }
            if (correctUser == false)
            {
                this.errorLabel.Visible = true;
            }
            else
            {
                parentForm.authPassed(this.textBoxLogin.Text);
                this.Close();
            }
        }

        private void loginPassCheck()
        {

        }
    }
}
