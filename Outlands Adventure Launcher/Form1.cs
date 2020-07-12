using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outlands_Adventure_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string sqlQuery = "SELECT user_name, user_password FROM user_information WHERE user_email LIKE 'thenapo212@gmail.com2'";
            //List<string> recoveredData = SQLManager.CheckCurrentUsers(sqlQuery);

            SQLManager.InsertNewUser("INSERT INTO user_information VALUES ('email De no napo5333', 'Napo5333', 5333, null)");
        }
    }
}
