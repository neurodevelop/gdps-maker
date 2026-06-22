using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class tx : UserControl
    {
        private readonly TextBox _in = new();
        private readonly Label _pl = new();
        private string _ph = string.Empty;
        private bool _fc;

        public tx()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            BackColor = at.ip;
            Height = 36;
            DoubleBuffered = true;

            _in.BorderStyle = BorderStyle.None;
            _in.BackColor = at.ip;
            _in.ForeColor = at.tx;
            _in.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            _in.AutoSize = false;
            _in.Location = new Point(12, 7);
            _in.Size = new Size(100, 22);
            _in.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(_in);

            _pl.AutoSize = false;
            _pl.BackColor = Color.Transparent;
            _pl.ForeColor = at.tm;
            _pl.Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            _pl.Location = new Point(12, 7);
            _pl.Size = new Size(100, 22);
            _pl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _pl.TextAlign = ContentAlignment.MiddleLeft;
            _pl.Padding = new Padding(3, 0, 0, 0);
            _pl.Cursor = Cursors.IBeam;
            _pl.MouseDown += (_, _) => _in.Focus();
            Controls.Add(_pl);
            _pl.BringToFront();

            _in.TextChanged += (_, _) => up();
            _in.GotFocus += (_, _) => { _fc = true; up(); Invalidate(); };
            _in.LostFocus += (_, _) => { _fc = false; up(); Invalidate(); };
            Resize += (_, _) =>
            {
                _in.Width = Width - 24;
                _pl.Width = Width - 24;
            };
        }

        public string ph
        {
            get => _ph;
            set { _ph = value; _pl.Text = value; up(); }
        }

        private void up()
        {
            _pl.Visible = !_fc && string.IsNullOrEmpty(_in.Text);
        }

        public override string Text
        {
            get => _in.Text;
            set { _in.Text = value; up(); }
        }

        public new event EventHandler? TextChanged
        {
            add => _in.TextChanged += value;
            remove => _in.TextChanged -= value;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;

            Color bc = at.bd;
            int bw = 1;
            if (_fc) { bc = at.ca; bw = 2; }

            int off = bw / 2;
            using (var pn = new Pen(bc, bw))
            {
                g.DrawLine(pn, off, off, Width - bw, off);
                g.DrawLine(pn, off, Height - bw, Width - bw, Height - bw);
                g.DrawLine(pn, off, off, off, Height - bw);
                g.DrawLine(pn, Width - bw, off, Width - bw, Height - bw);
            }
        }
    }
}