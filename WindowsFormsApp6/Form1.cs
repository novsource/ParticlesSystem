using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        List<DirectionColorfulEmiter> emiters = new List<DirectionColorfulEmiter>();

        private void UpdateState()
        {
            foreach (var emiter in emiters)
            {
                emiter.UpdateState();
            } 
        }

        private void UpdatePosition()
        {
            int y0 = 0;
            int x0 = 0;
            string side = "";
            foreach (var emiter in emiters)
            {
                int constant = -450;
                int direct = 0;
                if (tbDirection.Value < 90) //левая сторона
                {
                    x0 = constant;
                    y0 = Particle.rand.Next(picBox.Height);
                    direct = 0 + 15 - Particle.rand.Next(30);
                    side = "Левая сторона";
                }
                else if (tbDirection.Value < 180) //верхняя сторона
                {
                    x0 = Particle.rand.Next(picBox.Width);
                    y0 = constant;
                    direct = -90 + 15 - Particle.rand.Next(30);
                    side = "Верхняя сторона";
                }
                else if (tbDirection.Value < 270) //правая сторона
                {
                    x0 = picBox.Width - constant;
                    y0 = Particle.rand.Next(picBox.Height);
                    direct = -180 + 15 - Particle.rand.Next(30);
                    side = "Правая сторона";
                }
                else if (tbDirection.Value < 360) //нижняя сторона
                {
                    x0 = Particle.rand.Next(picBox.Width);
                    y0 = picBox.Height - constant;
                    direct = -270 + 15 - Particle.rand.Next(30);
                    side = "Нижняя сторона";
                }
                emiter.Position = new Point(x0, y0);
                emiter.Direction = direct;
            }
            
            txtBox1.Text = "Сторона: " + side ;
            /* int x0 = picBox.Width / 2;
             int y0 = picBox.Height / 2;

             var r =  Math.Sqrt(Math.Pow(picBox.Width, 2) + Math.Pow(picBox.Height, 2));

             var t = r * Math.Cos(tbDirection.Value) + x0;
             var q = r * Math.Sin(tbDirection.Value) + y0;
             emiters.Clear();
             for (var i = 0; i < 10; ++i)
             {
                 t = Math.Min(picBox.Width / 2, Math.Max(-picBox.Width / 2, t));
                 q = Math.Min(picBox.Height / 2, Math.Max(-picBox.Height / 2, t));
                 emiters.Add(new DirectionColorfulEmiter
                 {
                     ParticlesCount = 4,
                     Position = new Point ((int) t, (int) q)
                 });
                 //txtBox1.Text = "x': " + t + " \n" + "y': " + q;

             }
             txtBox1.Text = "Диагональ: " + r.ToString() + ";   "+ "X: " + t + ";   " + "Y: " + q + ";   " + "Angle: " + (tbDirection.Value -180); */

        } 

        private void Render(Graphics g)
        {
            foreach (var emiter in emiters)
            {
                emiter.Render(g);
            }
        }

        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();

            for (var i = 0; i < 10; ++i)
            {
                    emiters.Add(new DirectionColorfulEmiter
                    {
                        Position = new Point(-400, Particle.rand.Next(picBox.Height))
                    });
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            picBox.Image = new Bitmap(Properties.Resources.anypics_ru_33473); //переместил сюда чтобы при изменении формы все работало корректно
            UpdateState();
            using (var g = Graphics.FromImage(picBox.Image))
            {
                //g.Clear();
                Render(g);
            }
            picBox.Invalidate();
        }

        private int x = 0; private int y = 0;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));
            }
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void picBox_Click(object sender, EventArgs e)
        {

        }

        private void picBox_Resize(object sender, EventArgs e)
        {
            picBox.Invalidate();
        }

        private void CloseFormButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CloseFormButton_Hover(object sender, EventArgs e)
        {
            CloseFormButton.BackColor = Color.Red;
        }

        private void CloseFormButton_Leave(object sender, EventArgs e)
        {
            CloseFormButton.BackColor = Color.FromArgb(33, 33, 33);
        }
       

        //попытка воплотить возможность масштабировать форму


        protected const int cGrip = 16;
        protected const int cCaption = 32;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);

                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;

                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            foreach (var emiter in emiters)
            {
                emiter.Direction = tbDirection.Value;
            }
            UpdatePosition();
        }

        private void tbSpread_Scroll(object sender, EventArgs e)
        {
            foreach (var emiter in emiters)
            {
                emiter.Spread = tbSpread.Value;
            }
        }

        private void btnFromColor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var emiter in emiters)
                {
                    emiter.FromColor = dialog.Color;
                }
                btnFromColor.BackColor = dialog.Color;
            }
        }

        private void btnToColor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var emiter in emiters)
                {
                    emiter.ToColor = dialog.Color;
                }
                btnToColor.BackColor = dialog.Color;
            }
        }
    }
}


