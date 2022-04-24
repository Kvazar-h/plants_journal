using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Npgsql;
using yt_DesignUI;

namespace WindowsFormsApp19
{
    public partial class main : Form
    {
        private Button currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;

        int IDuser;
        NpgsqlConnection based;
        public main(int IDU, NpgsqlConnection nc)
        {
            InitializeComponent();
            IDuser = IDU;
            based = nc;
            Animator.Start();

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

        }
        private void Form2_Load(object sender, EventArgs e)
        {
            using (var command = new NpgsqlCommand($"SELECT name, color, data_sled_poliva, chastota_poliva FROM plants where userid = {IDuser};", based))
            {
                int w = 0;
                int h = 10;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (w < 530)
                    {
                        w += 10;
                        EgoldsCard btn = new EgoldsCard();
                        btn.TextHeader = reader.GetString(0);
                        btn.Text = "Полить";
                        btn.TextDescrition = "Следующий полив:" + string.Join(null, reader.GetString(2).Split()) + " Поливать каждые " + string.Join(null, reader.GetString(3).Split()) + " дня";
                        btn.Name = reader.GetString(0);
                        btn.Size = new Size(150, 200);
                        btn.Location = new Point(w, h);
                        btn.Click += new EventHandler(btn_Click);
                         /*Button btn = new Button();
                         btn.Text = reader.GetString(0);
                         btn.Name = reader.GetString(0);
                         btn.Size = new Size(70, 70);
                         btn.Location = new Point(w, h);
                         btn.Click += new EventHandler(btn_Click);
                         panel1.Controls.Add(btn);*/
                         w += 150;
                    }
                    else
                    {
                        w = 10;
                        h += 210;
                        EgoldsCard btn = new EgoldsCard();
                        btn.TextHeader = reader.GetString(0);
                        btn.TextDescrition = "Следующий полив:" + string.Join(null, reader.GetString(2).Split()) + " Поливать каждые " + string.Join(null, reader.GetString(3).Split()) + " дня";
                        btn.Text = "Полить";
                        btn.Name = reader.GetString(0);
                        btn.Size = new Size(150, 200);
                        btn.Location = new Point(w, h);
                        btn.Click += new EventHandler(btn_Click);
                        w += 150;
                    }
                }
                reader.Close();
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            //MessageBox.Show((sender as EgoldsCard).Name);
            
            DateTime thisDay = DateTime.Today;
            string[] dat = thisDay.ToShortDateString().Split('.');
            int k = Convert.ToInt32(dat[0]);
            using (var command = new NpgsqlCommand($"SELECT data_sled_poliva, chastota_poliva FROM plants WHERE name = '{(sender as EgoldsCard).Name}' and userid = {IDuser};", based))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    k += Convert.ToInt32(reader.GetString(1));
                    /*if (thisDay.ToShortDateString() != reader.GetString(0))
                    {
                        MessageBox.Show("вы полили не вовремя");
                    }*/
                }
                reader.Close();
            }
            dat[0] = Convert.ToString(k);
            string newdata = string.Join('.', dat);
            using (var command = new NpgsqlCommand($"UPDATE plants SET data_sled_poliva='{newdata}' where name = '{(sender as EgoldsCard).Name}'", based))
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Расстение усппешно полито");
            }
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (Button)senderBtn;
                currentBtn.BackColor = Color.FromArgb(255, 255, 255);
                currentBtn.ForeColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
            }
        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(78, 194, 6);
            public static Color color2 = Color.FromArgb(234, 37, 37);
            public static Color color3 = Color.FromArgb(255, 102, 0);
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(255, 255, 255);
                currentBtn.ForeColor = Color.FromArgb(0, 0, 0);
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            //open only form
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            //End
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Forms.poliv(IDuser, based));
            panel2.BackColor = RGBColors.color1;
            button4.ForeColor = RGBColors.color1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            OpenChildForm(new Forms.udobr(IDuser, based));
            panel2.BackColor = RGBColors.color2;
            button4.ForeColor = RGBColors.color2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            OpenChildForm(new Forms.achiv(IDuser, based));
            panel2.BackColor = RGBColors.color3;
            button4.ForeColor = RGBColors.color3;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panelLogo_Click(object sender, EventArgs e)
        {
           
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            Reset();
            panel2.BackColor = RGBColors.color1;
            button1.ForeColor = RGBColors.color1;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }
        private void bntMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
