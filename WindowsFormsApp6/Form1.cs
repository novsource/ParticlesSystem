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

            var rnd = new Random();
            for (var i = 0; i < 10; ++i)
            {
                emiters.Add(new DirectionColorfulEmiter
                {
                    ParticlesCount = 4,
                    Position = new Point(rnd.Next(picBox.Width / 2), rnd.Next(picBox.Height))
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


