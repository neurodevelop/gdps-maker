using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class md : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 2;

        private string _rs = string.Empty;

        private md(string t, string p, string d, bool ip)
        {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = at.sf;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            KeyPreview = true;
            DoubleBuffered = true;
            Size = new Size(480, ip ? 230 : 190);

            var lb = new Label
            {
                Text = t,
                ForeColor = at.tx,
                Font = at.f2,
                Location = new Point(20, 18),
                AutoSize = true
            };
            lb.MouseDown += dg;
            Controls.Add(lb);

            var pl = new Label
            {
                Text = p,
                ForeColor = at.ts,
                Font = at.f4,
                Location = new Point(20, 50),
                AutoSize = true,
                MaximumSize = new Size(440, 0)
            };
            pl.MouseDown += dg;
            Controls.Add(pl);

            tx tbx = null!;
            if (ip)
            {
                tbx = new tx
                {
                    Location = new Point(20, 115),
                    Width = 440
                };
                tbx.Text = d;
                Controls.Add(tbx);
            }

            var ok = new mb
            {
                Text = "OK",
                Width = 110,
                Height = 36
            };
            ok.Click += (_, _) =>
            {
                if (ip) _rs = tbx.Text;
                DialogResult = DialogResult.OK;
            };

            var cl = new mb
            {
                Text = "Cancel",
                Width = 110,
                Height = 36,
                bc = at.sa
            };
            cl.Click += (_, _) => DialogResult = DialogResult.Cancel;

            int by = ip ? 175 : 135;
            cl.Location = new Point(240, by);
            ok.Location = new Point(356, by);
            Controls.Add(cl);
            Controls.Add(ok);

            MouseDown += dg;

            KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (ip) _rs = tbx.Text;
                    DialogResult = DialogResult.OK;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.Cancel;
                    e.SuppressKeyPress = true;
                }
            };

            Shown += (_, _) => { if (ip) tbx.Focus(); };
        }

        private void dg(object? s, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var pn = new Pen(at.bd, 1);
            e.Graphics.DrawRectangle(pn, 0, 0, Width - 1, Height - 1);
        }

        public static string Input(IWin32Window? ow, string title, string prompt, string def = "")
        {
            using var d = new md(title, prompt, def, true);
            return d.ShowDialog(ow) == DialogResult.OK ? d._rs : string.Empty;
        }

        public static void Info(IWin32Window? ow, string title, string prompt)
        {
            using var d = new md(title, prompt, string.Empty, false);
            d.ShowDialog(ow);
        }
    }
}