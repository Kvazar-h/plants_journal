using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp19.Forms
{
    public partial class achiv : Form
    {
        int IDuser;
        NpgsqlConnection based;
        public achiv(int _IDuser, NpgsqlConnection _based)
        {
            InitializeComponent();
            string ach = "";
            IDuser = _IDuser;
            based = _based;
            egoldsCard1.BackColorCurtain = Color.FromArgb(186, 186, 186);
            egoldsCard2.BackColorCurtain = Color.FromArgb(186, 186, 186);
            egoldsCard3.BackColorCurtain = Color.FromArgb(186, 186, 186);
            using (var command = new NpgsqlCommand($"SELECT counter_achiv FROM users WHERE id = {IDuser};", based))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ach += Convert.ToString(reader.GetInt32(0));
                }
                reader.Close();
            }
            egoldsCard1.TextDescrition = ach + "/5";
            egoldsCard2.TextDescrition = ach + "/10";
            egoldsCard3.TextDescrition = ach + "/15";
            
            upda();
        }

        private void egoldsCard1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }
        void upda()
        {
            double curr = 0;
            using (var command = new NpgsqlCommand($"SELECT counter_achiv FROM users WHERE id = {IDuser};", based))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    curr = reader.GetInt32(0);
                }
                reader.Close();
            }
            int a = 5;
            int b = 10;
            int c = 15;
            if (curr < a)
            {
                progressBar1.Value = (int)(curr / a * 100);
            }
            else
            {
                progressBar1.Value = 100;
                egoldsCard1.BackColorCurtain = Color.FromArgb(78, 194, 6);
            }

            if (curr < b)
            {
                progressBar2.Value = (int)(curr / b * 100);

            }
            else
            {
                progressBar2.Value = 100;
                egoldsCard2.BackColorCurtain = Color.FromArgb(78, 194, 6);
            }

            if (curr < c)
            {
                progressBar3.Value = (int)(curr / c * 100);
            }
            else
            {
                progressBar3.Value = 100;
                egoldsCard3.BackColorCurtain = Color.FromArgb(78, 194, 6);
            }
        }
    }
}
