using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp19
{
    public partial class auth : Form
    {
        public auth()
        {
            InitializeComponent();

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        NpgsqlConnection nc;
        //private void Form1_Load(object sender, EventArgs e)
        //{
        //   
        //}

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string text;
                string path = "base.txt";
                FileInfo fil = new FileInfo(path);
                if (!fil.Exists)
                    File.AppendAllText(path, "Host=80.78.201.6;Username=postgres;Password=2022;Database=Plantb");
                using (StreamReader rea = new StreamReader(path))
                {
                    text = rea.ReadLine();
                }
                string connString = text; //"Host=localhost;Username=postgres;Password=2022;Database=Plantb";
                nc = new NpgsqlConnection(connString);
                nc.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var command = new NpgsqlCommand($"SELECT * FROM users where login = '{textBox1.Text}';", nc))
            {
                if(nc.State == ConnectionState.Closed)
                nc.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if(reader.GetString(2) == textBox2.Text)
                    {
                        int tmp = reader.GetInt32(0);
                        reader.Close();
                        main f2 = new main(tmp, nc);
                        f2.Show(this);
                        this.Hide();
                        return;
                    }
                }
                MessageBox.Show("Не правильный логин или пароль");
                reader.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var command = new NpgsqlCommand($"insert into users (login, password, counter_achiv) values ('{textBox1.Text}', '{textBox2.Text}',0);", nc))
            {
                int nRows = command.ExecuteNonQuery();
                MessageBox.Show("Регистрация успешна!!");
                nc.Close();
            }
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
