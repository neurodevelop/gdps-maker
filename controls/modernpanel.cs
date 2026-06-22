using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class mp : Panel
    {
        private float _cr = 10f;
        private Color _fc = at.sf;
        private Color _bc = at.bd;
        private int _bw = 1;

        public mp()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        public Color fc
        {
            get => _fc;
            set { _fc = value; Invalidate(); }
        }

        public Color bc
        {
            get => _bc;
            set { _bc = value; Invalidate(); }
        }

        public int bw
        {
            get => _bw;
            set { _bw = value; Invalidate(); }
        }

        public float cr
        {
            get => _cr;
            set { _cr = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rc = new Rectangle(0, 0, Width - 1, Height - 1);
            int rd = (int)Math.Min(_cr, Math.Min(Width, Height) / 2f);

            using (var pp = rr(rc, rd))
            {
                if (_fc.A > 0)
                    g.FillPath(new SolidBrush(_fc), pp);

                if (_bw > 0)
                {
                    using var pn = new Pen(_bc, _bw);
                    g.DrawPath(pn, pp);
                }
            }

            base.OnPaint(e);
        }

        private static GraphicsPath rr(Rectangle r, int rd)
        {
            var pp = new GraphicsPath();
            int d = rd * 2;
            if (d > r.Width) d = r.Width;
            if (d > r.Height) d = r.Height;

            pp.AddArc(r.X, r.Y, d, d, 180, 90);
            pp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            pp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            pp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            pp.CloseFigure();
            return pp;
        }
    }
}
