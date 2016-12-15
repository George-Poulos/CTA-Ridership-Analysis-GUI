using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms;


//
// CTA Ridership analysis using C# and SQL Serer.
//
// George Poulos
// U. of Illinois, Chicago
// CS341, Fall2016
// Homework 6//


namespace Proj6
{

    public partial class Form1 : Form
    {
        string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=C:\\USERS\\GEORG\\DOWNLOADS\\CTA_DB\\CTA_DB\\CTA.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Form1()
        {
            InitializeComponent();
        }

        void FillData()
        {
            
            using (SqlConnection c = new SqlConnection(connString))
            {
                c.Open();

                // use a SqlAdapter to execute the query
                using (SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM dbo.Stations ORDER BY Name", c))
                {
                    // fill a data table
                    var t = new DataTable();
                    a.Fill(t);

                    // Bind the table to the list box
                    listBox1.DisplayMember = "Name";
                    listBox1.ValueMember = "StationID";
                    listBox1.DataSource = t;
                }
                c.Close();
            }
        }

        void FillList2Data() {
            var optionSelected = listBox1.SelectedValue;
            using (SqlConnection c = new SqlConnection(connString))
            {
                c.Open();
                SqlCommand comm = new SqlCommand("SELECT SUM(DailyTotal) FROM dbo.Riderships WHERE dbo.Riderships.StationID = " + optionSelected, c);
                double newTextBox1 = Convert.ToInt32(comm.ExecuteScalar().ToString());
                string temp = newTextBox1.ToString("N0");
                textBox1.Text = string.Format("{0:n0}", temp);

                comm = new SqlCommand("SELECT SUM(DailyTotal)/COUNT(StationID) FROM dbo.Riderships WHERE dbo.Riderships.StationID = " + optionSelected, c);
                Int32 newTextBox2 = Convert.ToInt32(comm.ExecuteScalar().ToString()) ;
                temp = newTextBox2.ToString("N0");
                textBox2.Text = string.Format("{0:n0}", temp) + "/day";

                comm = new SqlCommand("SELECT SUM(convert(bigint, DailyTotal)) FROM dbo.Riderships", c);
                double newTextBox3 = Convert.ToInt64(comm.ExecuteScalar().ToString());
                double finala =(newTextBox1 / newTextBox3) * 100;
                temp = finala.ToString("0.00");
                textBox3.Text = temp +  "%";

                comm = new SqlCommand("SELECT SUM(DailyTotal) FROM dbo.Riderships WHERE dbo.Riderships.TypeOfDay = 'W' AND dbo.Riderships.StationID = " + optionSelected, c);
                Int32 newTextBox4 = Convert.ToInt32(comm.ExecuteScalar().ToString());
                temp = newTextBox4.ToString("N0");
                textBox4.Text = string.Format("{0:n0}", temp);

                comm = new SqlCommand("SELECT SUM(DailyTotal) FROM dbo.Riderships WHERE dbo.Riderships.TypeOfDay = 'A' AND dbo.Riderships.StationID = " + optionSelected, c);
                Int32 newTextBox5 = Convert.ToInt32(comm.ExecuteScalar().ToString());
                temp = newTextBox5.ToString("N0");
                textBox5.Text = string.Format("{0:n0}", temp);

                comm = new SqlCommand("SELECT SUM(DailyTotal) FROM dbo.Riderships WHERE dbo.Riderships.TypeOfDay = 'U' AND dbo.Riderships.StationID = " + optionSelected, c);
                Int32 newTextBox6 = Convert.ToInt32(comm.ExecuteScalar().ToString());
                temp = newTextBox6.ToString("N0");
                textBox6.Text = string.Format("{0:n0}", temp);

                // use a SqlAdapter to execute the query
                using (SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM dbo.Stops WHERE dbo.Stops.StationID = " + optionSelected + " ORDER BY Name", c))
                {
                    // fill a data table
                    var t = new DataTable();
                    a.Fill(t);

                    // Bind the table to the list box
                    listBox2.DisplayMember = "Name";
                    listBox2.ValueMember = "StopID";
                    listBox2.DataSource = t;
                }
                c.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillList2Data();

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SqlConnection c = new SqlConnection(connString))
            {
                c.Open();
                // use a SqlAdapter to execute the query
                using (SqlDataAdapter a = new SqlDataAdapter("SELECT TOP 10 dbo.Stations.StationID, dbo.Stations.Name, SUM(convert(bigint,dbo.Riderships.DailyTotal)) as theSum FROM dbo.Stations FULL OUTER JOIN dbo.Riderships ON dbo.Stations.StationID = dbo.Riderships.StationID GROUP BY dbo.Stations.StationID, dbo.Stations.Name ORDER BY theSum DESC ", c))
                {
                    // fill a data table
                    var t = new DataTable();   
                    a.Fill(t);
                    listBox1.DisplayMember = "Name";
                    listBox1.ValueMember = "StationID";
                    listBox1.DataSource = t;
                }
                c.Close();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var optionSelected = listBox2.SelectedValue;
            
            using (SqlConnection c = new SqlConnection(connString))
            {
                c.Open();
                SqlCommand comm = new SqlCommand("SELECT dbo.Stops.Direction FROM dbo.Stops WHERE dbo.Stops.StopID = " + optionSelected, c);
                string newTextBox8 = (comm.ExecuteScalar().ToString());
                textBox8.Text = newTextBox8;

                 comm = new SqlCommand("SELECT dbo.Stops.Latitude FROM dbo.Stops WHERE dbo.Stops.StopID = " + optionSelected, c);
                string newTextBox9 = (comm.ExecuteScalar().ToString());
                comm = new SqlCommand("SELECT dbo.Stops.Longitude FROM dbo.Stops WHERE dbo.Stops.StopID = " + optionSelected, c);
                string newTextBoxlong = (comm.ExecuteScalar().ToString());
                textBox9.Text = "(" + newTextBox9 + ", " + newTextBoxlong + ")";

                comm = new SqlCommand("SELECT dbo.Stops.ADA FROM dbo.Stops WHERE dbo.Stops.StopID = " + optionSelected , c);
                byte newTextBox7 = Convert.ToByte(comm.ExecuteScalar());
                string finala;
                if (newTextBox7.Equals(1))
                    finala = "YES";
                else
                    finala = "NO";      
                textBox7.Text = finala;

                // use a SqlAdapter to execute the query
                using (SqlDataAdapter a = new SqlDataAdapter("SELECT dbo.Lines.LineID, dbo.Lines.Color FROM dbo.StopDetails FULL OUTER JOIN dbo.Lines ON dbo.StopDetails.LineID = dbo.Lines.LineID WHERE dbo.StopDetails.StopID = " + optionSelected + " ORDER BY dbo.Lines.Color", c))
                {
                    // fill a data table
                    var t = new DataTable();
                    a.Fill(t);

                    // Bind the table to the list box
                    listBox3.DisplayMember = "Color";
                    listBox3.ValueMember = "LineID";
                    listBox3.DataSource = t;
                }
                c.Close();
            }
        }

    }
}
