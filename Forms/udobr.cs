using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using yt_DesignUI;

namespace WindowsFormsApp19.Forms
{
    public partial class udobr : Form
    {

        int IDuser;
        NpgsqlConnection based;
        bool which = false;
        public udobr(int IDU, NpgsqlConnection nc)
        {
            InitializeComponent();
            IDuser = IDU;
            based = nc;
            listBox1.Hide();
            textBox1.Hide();
            button2.Hide();
            label1.Hide();
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void btn_Click(object sender, EventArgs e)
        {
            DateTime thisDay = DateTime.Today;

            bool ach = false;
            string newdata = "";
            using (var command = new NpgsqlCommand($"SELECT data_sled_udob, chastota_udob FROM plants WHERE name = '{(sender as EgoldsCard).Name}' and userid = {IDuser};", based))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newdata = thisDay.AddDays(Convert.ToDouble(reader.GetString(1))).ToShortDateString();
                   
                    if (thisDay.ToShortDateString() != reader.GetString(0))
                    {
                        MessageBox.Show("Вы удобрили не вовремя");
                    }
                    else
                    {
                        ach = true;
                    }
                }

                reader.Close();
            }
            if (!ach)
            {
                using (var command1 = new NpgsqlCommand($"UPDATE users SET counter_achiv = counter_achiv-1 where id={IDuser} AND counter_achiv>0;", based))
                {
                    command1.ExecuteNonQuery();
                }
            }
            else
            {
                using (var command1 = new NpgsqlCommand($"UPDATE users SET counter_achiv = counter_achiv+1 where id={IDuser};", based))
                {
                    command1.ExecuteNonQuery();
                }
            }
            using (var command = new NpgsqlCommand($"UPDATE plants SET data_sled_udob='{newdata}' where name = '{(sender as EgoldsCard).Name}'", based))
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Вы удобрили");
            }
            panel3.Controls.Clear();
            upd();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {



        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void mth()
        {
            //Form2 tnn = new Form2();
            //Form2.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!which)
            {
                listBox1.Show();
                textBox1.Show();
                button2.Show();
                label1.Show();
                using (var command = new NpgsqlCommand($"SELECT Distinct type FROM plants;", based))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader.GetString(0));
                    }
                    reader.Close();
                }
                which = true;
            }
            else
            {
                using (var command = new NpgsqlCommand($"INSERT INTO plants (name, data_sled_poliva, chastota_poliva, data_sled_udob, chastota_udob, userid, type) VALUES ('{textBox1.Text}', '{DateTime.Today.ToShortDateString()}',(SELECT distinct chastota_poliva from plants where type = '{listBox1.SelectedItem.ToString()}'),'{DateTime.Today.ToShortDateString()}',(SELECT distinct chastota_udob from plants where type = '{listBox1.SelectedItem.ToString()}'),'{IDuser}','{listBox1.SelectedItem.ToString()}');", based))
                {
                    command.ExecuteNonQuery();

                }

                listBox1.Items.Clear();
                upd();
                which = false;
                listBox1.Hide();
                textBox1.Hide();
                button2.Hide();
                label1.Hide();
                listBox1.Items.Clear();
                textBox1.Text = "";
            }

        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(78, 194, 6);
            public static Color color2 = Color.FromArgb(234, 37, 37);
            public static Color color3 = Color.FromArgb(255, 102, 0);
        }

        private void upd()
        {
            using (var command = new NpgsqlCommand($"SELECT name, color, data_sled_udob, chastota_udob, type FROM plants where userid = {IDuser};", based))
            {
                int w = 0;
                int h = 10;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (w < 810)
                    {
                        w += 10;
                        EgoldsCard btn = new EgoldsCard();
                        btn.FontDescrition = new Font("Segoe UI", 9);
                        btn.Font = new Font("Segoe UI", 9);
                        btn.FontHeader = new Font("Segoe UI", 14);
                        btn.TextHeader = reader.GetString(0);
                        btn.Text = "Удобрить";
                        btn.TextDescrition = "Тип: " + reader.GetString(4) + "\nСледующее удобрение: " + reader.GetString(2) + "\nУдобрять каждые " + reader.GetString(3) + " дня";
                        btn.Name = reader.GetString(0);
                        btn.Size = new Size(150, 200);
                        btn.Location = new Point(w, h);
                        btn.Click += new EventHandler(btn_Click);
                        panel3.Controls.Add(btn);
                        w += 150;
                        btn.BackColorCurtain = RGBColors.color2;
                    }
                    else
                    {
                        w = 10;
                        h += 210;
                        EgoldsCard btn = new EgoldsCard();
                        btn.TextHeader = reader.GetString(0);
                        btn.TextDescrition = "Тип: " + reader.GetString(4) + "\nСледующее удобрение: " + reader.GetString(2) + "\nУдобрять каждые " + reader.GetString(3) + " дня";
                        btn.Text = "Удобрить";
                        btn.Name = reader.GetString(0);
                        btn.Size = new Size(150, 200);
                        btn.Location = new Point(w, h);
                        btn.Click += new EventHandler(btn_Click);
                        panel3.Controls.Add(btn);
                        w += 150;
                        btn.BackColorCurtain = RGBColors.color2;
                    }
                }
                reader.Close();
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            upd();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            message msg = new message();
            msg.Show(this);
        }
    }
}
