using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class mb : Button
    {
        private bool _h;
        private bool _p;
        private bool _d;

        public mb()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable,
                true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            ForeColor = at.tx;
            Font = at.f6;
            Cursor = Cursors.Hand;
            Size = new Size(140, 38);
            SetStyle(ControlStyles.StandardClick, true);
            DoubleBuffered = true;
        }

        public Color bc { get; set; } = at.cs;
        public Color hov { get; set; } = at.cp;

        protected override void OnMouseEnter(EventArgs e) { _h = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _h = false; _p = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _p = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _p = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { _d = !Enabled; Invalidate(); base.OnEnabledChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Color fill;
            if (_d) fill = at.tm;
            else if (_p) fill = hov;
            else if (_h) fill = hov;
            else fill = bc;

            using (var bb = new SolidBrush(fill))
                g.FillRectangle(bb, 0, 0, Width, Height);

            var tc = _d ? at.ts : at.tx;
            TextRenderer.DrawText(g, Text, Font, new Rectangle(0, 0, Width, Height), tc,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPrefix |
                TextFormatFlags.EndEllipsis);
        }
    }
}