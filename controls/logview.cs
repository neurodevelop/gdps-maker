using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GDPSMaker.Controls
{
    public class lv : Control
    {
        private const int SBW = 10;
        private const int MTH = 28;

        private string[] _ln = Array.Empty<string>();
        private Color[] _cl = Array.Empty<Color>();
        private int _lh = 18;
        private int _sy;
        private bool _hv;
        private bool _dg;

        [DllImport("user32.dll")]
        private static extern bool ShowScrollBar(IntPtr hWnd, int nBar, bool bShow);

        public lv()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable,
                true);
            BackColor = at.lg;
            ForeColor = at.ts;
            Font = new Font("Cascadia Mono, Consolas, Courier New", 9f);
            DoubleBuffered = true;
            TabStop = true;
            HandleCreated += (_, _) => ShowScrollBar(Handle, 1, false);
            MouseEnter += (_, _) => { _hv = true; Invalidate(); };
            MouseLeave += (_, _) => { _hv = false; _dg = false; Invalidate(); };
        }

        public string ft
        {
            get => string.Join(Environment.NewLine, _ln);
            set
            {
                _ln = string.IsNullOrEmpty(value) ? Array.Empty<string>() : value.Split('\n');
                _cl = new Color[_ln.Length];
                for (int i = 0; i < _cl.Length; i++) _cl[i] = at.ts;
                _sy = int.MaxValue;
                Invalidate();
            }
        }

        public void ai(string t) { ag(t, at.cp); }
        public void ag(string t) { ag(t, at.ts); }
        public void aw(string t) { ag(t, at.wn); }
        public void ae(string t) { ag(t, at.er); }
        public void as2(string t) { ag(t, at.ok); }

        private void ag(string t, Color c)
        {
            int n = _ln.Length;
            Array.Resize(ref _ln, n + 1);
            Array.Resize(ref _cl, n + 1);
            _ln[n] = t.TrimEnd('\r', '\n');
            _cl[n] = c;
            _sy = int.MaxValue;
            Invalidate();
        }

        private int Vl() { return Math.Max(1, (Height - 16) / Math.Max(1, _lh)); }
        private int Mx() { return Math.Max(0, _ln.Length - Vl()); }

        private int ThH()
        {
            int mx = Mx();
            if (mx == 0) return Math.Min(Height, MTH);
            long h = (long)Height * Vl() / Math.Max(1, _ln.Length);
            if (h < MTH) h = MTH;
            if (h > Height) h = Height;
            if (h < 0) h = 0;
            return (int)h;
        }

        private int ThY()
        {
            int mx = Mx();
            int th = ThH();
            if (mx <= 0 || th <= 0 || Height <= 0) return 0;
            int tr = Height - th;
            if (tr <= 0) return 0;
            long y = (long)tr * _sy / mx;
            if (y < 0) y = 0;
            if (y > tr) y = tr;
            return (int)y;
        }

        private void SetP(int p)
        {
            int mx = Mx();
            if (p < 0) p = 0;
            if (p > mx) p = mx;
            _sy = p;
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta != 0) SetP(_sy - e.Delta / 30);
            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) { Focus(); return; }
            int sbx = Width - SBW;
            if (e.X < sbx || sbx <= 0 || Height <= 0) { Focus(); return; }
            int mx = Mx();
            if (mx == 0) return;
            int th = ThH();
            int ty = ThY();
            if (th > 0 && th < Height && e.Y >= ty && e.Y <= ty + th)
            {
                _dg = true;
                Capture = true;
            }
            else
            {
                int tr = Math.Max(1, Height - th);
                int newP = (int)((long)(e.Y - th / 2) * mx / tr);
                SetP(newP);
            }
            Focus();
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_dg) { Invalidate(); return; }
            int mx = Mx();
            if (mx == 0 || Height <= 0) return;
            int th = ThH();
            int tr = Math.Max(1, Height - th);
            int newP = (int)((long)(e.Y - th / 2) * mx / tr);
            SetP(newP);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_dg) { _dg = false; Capture = false; Invalidate(); }
        }

        protected override void OnResize(EventArgs e) { base.OnResize(e); Invalidate(); }

        protected override bool IsInputKey(Keys kd)
        {
            if (kd == Keys.Up || kd == Keys.Down || kd == Keys.PageUp ||
                kd == Keys.PageDown || kd == Keys.Home || kd == Keys.End)
                return true;
            return base.IsInputKey(kd);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int mx = Mx();
            int vl = Vl();
            switch (e.KeyCode)
            {
                case Keys.Up: SetP(_sy - 1); break;
                case Keys.Down: SetP(_sy + 1); break;
                case Keys.PageUp: SetP(_sy - vl); break;
                case Keys.PageDown: SetP(_sy + vl); break;
                case Keys.Home: SetP(0); break;
                case Keys.End: SetP(mx); break;
                default: base.OnKeyDown(e); return;
            }
            e.Handled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (Width <= 0 || Height <= 0) return;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);

            int textW = Math.Max(8, Width - SBW - 4);

            if (_ln.Length > 0)
            {
                int y = 8;
                int fl = (_sy == int.MaxValue) ? Mx() : _sy;
                if (fl < 0) fl = 0;
                if (fl >= _ln.Length) fl = _ln.Length - 1;

                for (int i = fl; i < _ln.Length; i++)
                {
                    if (y > Height - 4) break;
                    var ln = _ln[i];
                    var c = i < _cl.Length ? _cl[i] : at.ts;
                    if (ln.Length > 200) ln = ln.Substring(0, 197) + "...";
                    int rw = Math.Max(8, textW - 16);
                    var sz = TextRenderer.MeasureText(g, ln, Font, new Size(rw, 0),
                        TextFormatFlags.NoPrefix | TextFormatFlags.Left | TextFormatFlags.NoClipping);
                    if (y >= 0 && sz.Height > 0)
                    {
                        TextRenderer.DrawText(g, ln, Font, new Rectangle(12, y, rw, sz.Height), c,
                            TextFormatFlags.NoPrefix | TextFormatFlags.Left | TextFormatFlags.NoClipping);
                    }
                    y += _lh;
                }
            }

            DrawSb(g);
        }

        private void DrawSb(Graphics g)
        {
            if (Width < SBW + 4 || Height < MTH + 4) return;
            int sbx = Width - SBW;
            if (sbx < 0 || Height <= 0) return;

            using (var bb = new SolidBrush(at.bd))
            {
                g.FillRectangle(bb, sbx, 0, SBW, Height);
            }

            int mx = Mx();
            if (mx == 0) return;

            int th = ThH();
            int ty = ThY();
            if (th <= 0 || ty < 0 || (ty + th) > Height) return;

            int pad = 2;
            int tw = SBW - pad * 2;
            if (tw <= 0 || th <= 0) return;

            Color tc = _dg ? at.cp : (_hv ? at.sa : Color.FromArgb(180, at.sa));
            using (var bb2 = new SolidBrush(tc))
            {
                g.FillRectangle(bb2, sbx + pad, ty, tw, th);
            }
        }
    }
}