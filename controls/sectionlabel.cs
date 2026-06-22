using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class sl : Control
    {
        public sl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            BackColor = Color.Transparent;
            Font = at.f1;
            ForeColor = at.ts;
            Height = 18;
            Size = new Size(200, 18);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int bw = 3;
            int bh = 10;
            int by = (Height - bh) / 2;
            using (var bb = new SolidBrush(at.ca))
                g.FillRectangle(bb, 0, by, bw, bh);

            var tr = new Rectangle(bw + 8, 0, Width - bw - 8, Height);
            TextRenderer.DrawText(g, Text.ToUpper(), Font, tr, ForeColor,
                TextFormatFlags.NoPrefix | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
