using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class dt : Control
    {
        private readonly Label _tl = new();
        private readonly Button _mb = new();
        private readonly Button _mxb = new();
        private readonly Button _cb = new();
        private Icon? _ic;

        public string tt { get => _tl.Text; set => _tl.Text = value; }

        public dt()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            Height = 40;
            BackColor = at.tb;
            DoubleBuffered = true;

            try
            {
                _ic = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch { }
            if (_ic == null)
            {
                string ip = Path.Combine(AppContext.BaseDirectory, "icon.ico");
                if (File.Exists(ip))
                {
                    try { _ic = new Icon(ip); } catch { }
                }
            }

            _tl.AutoSize = false;
            _tl.ForeColor = at.tx;
            _tl.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold);
            _tl.BackColor = Color.Transparent;
            _tl.Text = "GDPS Maker";
            _tl.TextAlign = ContentAlignment.MiddleLeft;
            _tl.Location = new Point(44, 0);
            _tl.Size = new Size(380, 36);

            int bw = 40;
            int bh = 26;
            int by = 5;

            _mb.FlatStyle = FlatStyle.Flat;
            _mb.FlatAppearance.BorderSize = 0;
            _mb.BackColor = Color.Transparent;
            _mb.Cursor = Cursors.Hand;
            _mb.Size = new Size(bw, bh);
            _mb.Paint += DpMin;
            _mb.MouseEnter += (_, _) => _mb.Invalidate();
            _mb.MouseLeave += (_, _) => _mb.Invalidate();
            _mb.Click += (_, _) => { var f = FindForm(); if (f != null) f.WindowState = FormWindowState.Minimized; };

            _mxb.FlatStyle = FlatStyle.Flat;
            _mxb.FlatAppearance.BorderSize = 0;
            _mxb.BackColor = Color.Transparent;
            _mxb.Cursor = Cursors.Hand;
            _mxb.Size = new Size(bw, bh);
            _mxb.Paint += DpMax;
            _mxb.MouseEnter += (_, _) => _mxb.Invalidate();
            _mxb.MouseLeave += (_, _) => _mxb.Invalidate();
            _mxb.Click += (_, _) =>
            {
                var f = FindForm();
                if (f != null) f.WindowState = f.WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
            };

            _cb.FlatStyle = FlatStyle.Flat;
            _cb.FlatAppearance.BorderSize = 0;
            _cb.BackColor = Color.Transparent;
            _cb.Cursor = Cursors.Hand;
            _cb.Size = new Size(bw, bh);
            _cb.Paint += DpClose;
            _cb.MouseEnter += (_, _) => _cb.Invalidate();
            _cb.MouseLeave += (_, _) => _cb.Invalidate();
            _cb.Click += (_, _) => FindForm()?.Close();

            Controls.Add(_tl);
            Controls.Add(_mb);
            Controls.Add(_mxb);
            Controls.Add(_cb);

            _tl.MouseDown += OnTitleMouseDown;
            _mb.MouseDown += OnButtonMouseDown;
            _mxb.MouseDown += OnButtonMouseDown;
            _cb.MouseDown += OnButtonMouseDown;

            int bx = Width - bw;
            _cb.Location = new Point(bx, by);
            bx -= bw;
            _mxb.Location = new Point(bx, by);
            bx -= bw;
            _mb.Location = new Point(bx, by);
        }

        private void Hb(Graphics g, Button b, bool close = false)
        {
            bool hov = b.ClientRectangle.Contains(b.PointToClient(Cursor.Position));
            if (!hov) return;
            Color hc = close ? Color.FromArgb(38, 220, 80, 80) : Color.FromArgb(28, 255, 255, 255);
            using var bb = new SolidBrush(hc);
            g.FillRectangle(bb, 0, 0, b.Width, b.Height);
        }

        private void DpMin(object? s, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Hb(g, _mb);
            bool hov = _mb.ClientRectangle.Contains(_mb.PointToClient(Cursor.Position));
            using var pn = new Pen(hov ? Color.White : at.tx, 1.8f);
            int y = _mb.Height / 2;
            g.DrawLine(pn, _mb.Width / 2 - 6, y, _mb.Width / 2 + 6, y);
        }

        private void DpMax(object? s, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Hb(g, _mxb);
            bool hov = _mxb.ClientRectangle.Contains(_mxb.PointToClient(Cursor.Position));
            using var pn = new Pen(hov ? Color.White : at.tx, 1.8f);
            int x = _mxb.Width / 2 - 5;
            int y = _mxb.Height / 2 - 4;
            g.DrawRectangle(pn, x, y, 10, 8);
        }

        private void DpClose(object? s, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Hb(g, _cb, true);
            bool hov = _cb.ClientRectangle.Contains(_cb.PointToClient(Cursor.Position));
            using var pn = new Pen(hov ? Color.White : at.tx, 1.8f);
            int cx = _cb.Width / 2;
            int cy = _cb.Height / 2;
            g.DrawLine(pn, cx - 5, cy - 5, cx + 5, cy + 5);
            g.DrawLine(pn, cx + 5, cy - 5, cx - 5, cy + 5);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            const int HTCAPTION = 2;
            const int WM_NCLBUTTONDOWN = 0xA1;
            ReleaseCapture();
            var f = FindForm();
            if (f != null) SendMessage(f.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void OnTitleMouseDown(object? s, MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        private void OnButtonMouseDown(object? s, MouseEventArgs e)
        {
            var btn = s as Control;
            if (btn == null) return;
            var pt2 = btn.PointToScreen(new Point(e.X, e.Y));
            int rx = pt2.X - FindForm()!.Location.X;
            if (rx >= Width - 130) return;
            OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, rx, 0, e.Delta));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int bw = 40;
            int by = 5;
            int bx = Width - bw;
            _cb.Location = new Point(bx, by);
            bx -= bw;
            _mxb.Location = new Point(bx, by);
            bx -= bw;
            _mb.Location = new Point(bx, by);
            _tl.Size = new Size(Math.Min(380, Width - 200), 36);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            using (var bb = new SolidBrush(at.tb))
                g.FillRectangle(bb, 0, 0, Width, Height);
            if (_ic != null)
            {
                int iy = (Height - 22) / 2;
                g.DrawIcon(_ic, new Rectangle(14, iy, 22, 22));
            }
            using (var pn = new Pen(at.bd, 1f))
                g.DrawLine(pn, 0, Height - 1, Width, Height - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _ic != null)
            {
                _ic.Dispose();
                _ic = null;
            }
            base.Dispose(disposing);
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
    }
}