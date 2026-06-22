using System;
using System.Drawing;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class ft : Control
    {
        public Label br { get; } = new();

        public ft()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            DoubleBuffered = true;
            Height = 32;

            br.AutoSize = true;
            br.Text = "by NeuroDevelop";
            br.ForeColor = at.tm;
            br.BackColor = Color.Transparent;
            br.Font = at.f7;

            Controls.Add(br);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (br != null)
                br.Location = new Point(Width - br.Width - 18, 9);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            using (var pn = new Pen(at.bd, 1f))
                g.DrawLine(pn, 0, 0, Width, 0);
            using var bb = new SolidBrush(Color.FromArgb(10, 13, 19));
            g.FillRectangle(bb, 0, 1, Width, Height - 1);
        }
    }
}