using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using GDPSMaker.Controls;
using GDPSMaker.Models;
using GDPSMaker.Services;

namespace GDPSMaker
{
    public class mf : Form
    {
        private readonly dt tb = new() { Dock = DockStyle.Top };
        private readonly ft ft = new() { Dock = DockStyle.Bottom };

        private readonly mp fs = new() { fc = at.sf };
        private readonly sl fl = new() { Text = "SOURCE FILE" };
        private readonly tx fp = new();
        private readonly mb fb = new() { Text = "Browse", Width = 100, Height = 38 };
        

        private readonly mp ss = new() { fc = at.sf };
        private readonly sl sl = new() { Text = "GDPS URL ADDRESS" };
        private readonly tx ub = new();
        

        private readonly mb bb = new() { Text = "BUILD GDPS CLIENT", Width = 240, Height = 40 };

        private readonly mp ls = new() { fc = at.lg };
        private readonly sl ll = new() { Text = "BUILD LOG" };
        private readonly lv lg = new();

        private readonly Panel fsRow = new();
        private readonly Panel bbRow = new();

        private string pw = null!;
        private string ka = null!;
        private bool _dg;
        private Color _ob = at.sf;

        public mf()
        {
            ix();
            lf();
            we();
            ol();
        }

        private void ix()
        {
            Text = "GDPS Maker";
            BackColor = at.bg;
            ForeColor = at.tx;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(720, 540);
            Size = new Size(920, 680);
            DoubleBuffered = true;
            Font = at.f4;
            AllowDrop = true;
            KeyPreview = true;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)!; } catch { }
        }

        private void lf()
        {
            SuspendLayout();

            tb.tt = "GDPS Maker";
            Controls.Add(tb);

            sl.Height = 18;
            ub.ph = "https://your-gdps-server.com";
            ss.BackColor = at.sf;
            ss.Controls.Clear();
            ss.Controls.Add(sl);
            ss.Controls.Add(ub);

            fl.Text = "SOURCE FILE";
            fl.Height = 18;
            fp.ph = "Drag and drop or click Browse...";
            fb.Text = "Browse";
            fb.Width = 90;
            fb.Height = 34;
            fs.BackColor = at.sf;
            fs.Controls.Clear();
            fs.Controls.Add(fl);
            fs.Controls.Add(fp);
            fs.Controls.Add(fb);

            bb.Text = "BUILD GDPS CLIENT";
            bb.Width = 200;
            bb.Height = 36;
            bbRow.BackColor = at.bg;
            bbRow.Height = 50;
            bbRow.Controls.Clear();
            bbRow.Controls.Add(bb);

            ll.Text = "BUILD LOG";
            ll.Height = 18;
            lg.Margin = Padding.Empty;
            ls.BackColor = at.lg;
            ls.Controls.Clear();
            ls.Controls.Add(lg);
            ls.Controls.Add(ll);

            Controls.Add(ss);
            Controls.Add(fs);
            Controls.Add(bbRow);
            Controls.Add(ls);
            Controls.Add(ft);

            Shown += (_, _) => LayoutBody();
            Resize += (_, _) => LayoutBody();

            ResumeLayout(false);
            PerformLayout();
            LayoutBody();
        }

        private void LayoutBody()
        {
            if (ClientSize.Width < 100 || ClientSize.Height < 100) return;

            const int gap = 10;
            const int urlH = 76;
            const int fileH = 72;
            const int bbH = 50;

            int padX = 16;
            int padTop = 12;

            int contentW = ClientSize.Width - padX * 2;
            if (contentW < 400) contentW = 400;

            int y = tb.Height + padTop;

            ss.SetBounds(padX, y, contentW, urlH);
            LayoutSS();
            y += urlH + gap;

            fs.SetBounds(padX, y, contentW, fileH);
            LayoutFS();
            y += fileH + gap;

            bbRow.SetBounds(padX, y, contentW, bbH);
            bb.SetBounds((bbRow.Width - bb.Width) / 2, (bbH - bb.Height) / 2, bb.Width, bb.Height);
            y += bbH + gap;

            int logH = ClientSize.Height - ft.Height - y - padTop;
            if (logH < 80) logH = 80;
            ls.SetBounds(padX, y, contentW, logH);
            LayoutLS();
        }

        private void LayoutSS()
        {
            if (ss.ClientSize.Width <= 0 || ss.ClientSize.Height <= 0) return;
            int innerW = ss.ClientSize.Width;
            int innerH = ss.ClientSize.Height;
            int y = (innerH - 18 - 6 - 34) / 2;
            if (y < 0) y = 0;
            sl.SetBounds(14, y, innerW - 28, 18);
            ub.SetBounds(14, y + 18 + 6, innerW - 28, 34);
        }

        private void LayoutFS()
        {
            if (fs.ClientSize.Width <= 0 || fs.ClientSize.Height <= 0) return;
            int innerW = fs.ClientSize.Width;
            int innerH = fs.ClientSize.Height;
            int y = (innerH - 18 - 6 - 34) / 2;
            if (y < 0) y = 0;
            fl.SetBounds(14, y, innerW - 28, 18);
            int rowY = y + 18 + 6;
            int browseW = 90;
            int sp = 8;
            fb.SetBounds(14 + innerW - 28 - browseW, rowY, browseW, 34);
            fp.SetBounds(14, rowY, innerW - 28 - browseW - sp, 34);
        }

        private void LayoutLS()
        {
            if (ls.ClientSize.Width <= 0 || ls.ClientSize.Height <= 0) return;
            int innerW = ls.ClientSize.Width;
            int innerH = ls.ClientSize.Height;
            int padT = 14;
            int padL = 14;
            int gap = 10;
            int labelH = 18;
            ll.SetBounds(padL, padT, innerW - padL * 2, labelH);
            int lgY = padT + labelH + gap;
            int lgH = innerH - padT - labelH - gap - padT;
            if (lgH < 40) lgH = 40;
            lg.SetBounds(padL, lgY, innerW - padL * 2, lgH);
        }


        private void we()
        {
            fb.Click += (_, _) => bk();
            bb.Click += (_, _) => bd();
        }

        private void ol()
        {
            ld();
            lg.ai("GDPS Maker ready.");
            lg.ag("Tip: drag a Geometry Dash binary into the source field to get started.");
            lg.ag("Android builds require Java + the apk folder shipped next to this exe.");
            jc();
        }

        private void jc()
        {
            try
            {
                int r = rn.df("where", "java");
                if (r == 0)
                    lg.ai("Java detected. Android builds will work.");
                else
                    lg.aw("Java not found. Android builds will fail until you add it to PATH.");
            }
            catch
            {
                lg.aw("Java not found. Android builds will fail until you add it to PATH.");
            }
        }

        private void bk()
        {
            using var d = new OpenFileDialog
            {
                Title = "Select a Geometry Dash binary",
                Filter = "Geometry Dash binaries (*.exe;*.ipa;*.apk)|*.exe;*.ipa;*.apk|All files (*.*)|*.*"
            };

            if (d.ShowDialog() == DialogResult.OK)
            {
                fp.Text = d.FileName;
                bs(d.FileName);
            }
        }

        private void bs(string p)
        {
            var pf = pd.df(p);
            fp.Text = p;
            lg.ai("Loaded: " + Path.GetFileName(p) + " (" + pf + ")");
        }

        private void bd()
        {
            if (string.IsNullOrWhiteSpace(fp.Text) || !File.Exists(fp.Text))
            {
                lg.ae("Source file missing. Pick a Geometry Dash binary first.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ub.Text))
            {
                lg.ae("GDPS URL is empty. Please enter your server URL.");
                return;
            }

            bb.Enabled = false;
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                rb();
            }
            catch (Exception ex)
            {
                lg.ae("Build failed: " + ex.Message);
            }
            finally
            {
                bb.Enabled = true;
                Cursor = Cursors.Default;
                sv();
            }
        }

        private void rb()
        {
            string fp2 = fp.Text;

            lg.ai("Asking for GDPS name");
            string tn = md.Input(this, "GDPS Name", "Enter your GDPS name:");
            if (string.IsNullOrEmpty(tn)) { lg.aw("Build cancelled."); return; }

            if (ub.Text.Length > 33)
            {
                lg.ae("URL is too long! Please use a shorter one.");
                return;
            }

            string pu = ub.Text;
            while (pu.Length < 33)
                pu += "/";
            lg.ai("Replacing... (padded URL to " + pu.Length + " chars)");

            string bn = string.Empty;
            var pf = pd.df(fp2);

            if (pf > pt.w)
            {
                lg.ai("Bundle ID length is 24");
                double ml = 24;

                bn = md.Input(this, "Bundle ID", "Enter the Bundle ID (e.g., com.example.mygdps). It must be exactly 24 characters long.");
                if (string.IsNullOrEmpty(bn)) { lg.aw("Build cancelled."); return; }
                if (bn.Length > ml)
                {
                    lg.ae("Bundle ID too long! Please make it shorter.");
                    return;
                }
                while (bn.Length < ml)
                    bn += "0";
            }

            bool ok;
            switch (pf)
            {
                case pt.i:
                    ok = bi(fp2, pu, bn, tn);
                    break;
                case pt.w:
                    ok = bw(fp2, pu, tn);
                    break;
                case pt.a:
                    ok = ba(fp2, pu, bn, tn);
                    break;
                default:
                    lg.ae("No platform detected.");
                    return;
            }

            if (ok)
                lg.as2("GDPS client created!");
        }

        private bool bi(string si, string pu, string bn, string tn)
        {
            string wd = Path.Combine(Environment.CurrentDirectory, "dindetemp_ios");
            lg.ai("Extracting IPA");
            if (Directory.Exists(wd)) Directory.Delete(wd, true);
            ZipFile.ExtractToDirectory(si, wd);

            string bpp = Path.Combine(wd, "Payload", "GeometryJump.app", "GeometryJump");
            string plp = Path.Combine(wd, "Payload", "GeometryJump.app", "Info.plist");
            bp.gf(bpp, pu, bn, tn);
            bp.gf(plp, pu, bn, tn);

            lg.ai("Exporting IPA");
            string oi2 = Path.Combine(Environment.CurrentDirectory, tn + ".ipa");
            if (File.Exists(oi2)) File.Delete(oi2);
            ZipFile.CreateFromDirectory(wd, oi2, CompressionLevel.NoCompression, false);
            Directory.Delete(wd, true);
            if (!File.Exists(oi2))
                throw new FileNotFoundException("Output IPA was not created.", oi2);
            lg.ai("Saved: " + oi2);
            return true;
        }

        private bool bw(string se, string pu, string tn)
        {
            string pp2 = Path.GetDirectoryName(se) ?? AppDir();
            string te = Path.Combine(pp2, SafeFileName(tn) + ".exe");
            bp.gf(se, pu, string.Empty, tn, te);
            if (!File.Exists(te))
                throw new FileNotFoundException("Output EXE was not created.", te);
            lg.ai("Saved: " + te);
            return true;
        }

        private static string AppDir()
        {
            return Path.GetDirectoryName(Application.ExecutablePath)
                ?? AppContext.BaseDirectory
                ?? Environment.CurrentDirectory;
        }

        private static string SafeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return string.IsNullOrWhiteSpace(name) ? "GDPS" : name.Trim();
        }

        private static string Q(string value)
        {
            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        private bool ba(string sa, string pu, string bn, string tn)
        {
            string appDir = AppDir();
            string kp = Path.Combine(appDir, "gdps.keystore");
            if (!File.Exists(kp))
            {
                lg.ai("No keystore found. Generating a new one...");
                string np = md.Input(this, "Keystore Password", "Enter a password for the new keystore:");
                if (string.IsNullOrEmpty(np)) { lg.aw("Build cancelled."); return false; }
                string na = md.Input(this, "Key Alias", "Enter a key alias (e.g., myalias):", "myalias");
                if (string.IsNullOrEmpty(na)) na = "myalias";
                try
                {
                    cs.df(kp, np, na);
                    if (!File.Exists(kp))
                    {
                        lg.ae("Auto keystore generation failed.");
                        return false;
                    }
                    lg.as2("Keystore generated: gdps.keystore");
                    pw = np;
                    ka = na;
                }
                catch (Exception ex)
                {
                    lg.ae("Auto keystore generation failed: " + ex.Message);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(pw))
            {
                pw = md.Input(this, "Keystore Password", "Enter your keystore password:");
                if (string.IsNullOrEmpty(pw)) { lg.aw("Build cancelled."); return false; }
            }
            if (string.IsNullOrEmpty(ka))
            {
                ka = md.Input(this, "Key Alias", "Enter your key alias (e.g., myalias):", "myalias");
                if (string.IsNullOrEmpty(ka)) ka = "myalias";
            }

            try
            {
                int jc2 = rn.df("where", "java");
                if (jc2 != 0)
                {
                    lg.ae("Java not detected! Add it to PATH to build Android APKs.");
                    return false;
                }
            }
            catch
            {
                lg.ae("Java not detected! Add it to PATH to build Android APKs.");
                return false;
            }

            string ap2 = Path.Combine(appDir, "apk");
            string toolJar = Path.Combine(ap2, "tool.jar");
            string signerJar = Path.Combine(ap2, "signer.jar");
            lg.ai("Extracting APK");
            if (!Directory.Exists(ap2))
            {
                lg.ae("apk folder missing. Copy it next to this exe: " + ap2);
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://files.141412.xyz/r/apktool.zip",
                        UseShellExecute = true
                    });
                }
                catch { }
                return false;
            }
            if (!File.Exists(toolJar))
            {
                lg.ae("apktool jar missing: " + toolJar);
                return false;
            }
            if (!File.Exists(signerJar))
            {
                lg.ae("signer jar missing: " + signerJar);
                return false;
            }

            string outDir = Path.GetDirectoryName(sa) ?? appDir;
            string workRoot = Path.Combine(Path.GetTempPath(), "GDPSMaker");
            Directory.CreateDirectory(workRoot);
            string dd = Path.Combine(workRoot, "apkedit");
            if (Directory.Exists(dd)) Directory.Delete(dd, true);
            lg.ai("Decompiling APK with apktool");
            int decompileCode = rn.df("java", "-jar " + Q(toolJar) + " d " + Q(sa) + " -o " + Q(dd) + " -f", workRoot);
            if (decompileCode != 0 || !Directory.Exists(dd))
                throw new Exception("apktool decompile failed with exit code " + decompileCode);

            string sf = Path.Combine(dd, "res", "values", "strings.xml");
            string yf = Path.Combine(dd, "apktool.yml");
            string ld2 = Path.Combine(dd, "lib");

            if (Directory.Exists(ld2))
            {
                foreach (string dr in Directory.GetDirectories(ld2))
                {
                    string s1 = Path.Combine(dr, "libcocos2dcpp.so");
                    string s2 = Path.Combine(dr, "libgame.so");
                    if (File.Exists(s1))
                        bp.gf(s1, pu, bn, tn);
                    else if (File.Exists(s2))
                        bp.gf(s2, pu, bn, tn);
                }
            }

            if (File.Exists(sf))
            {
                File.WriteAllText(sf, File.ReadAllText(sf).Replace("Geometry Dash", tn));
            }
            if (File.Exists(yf))
            {
                File.WriteAllText(yf,
                    File.ReadAllText(yf).Replace("renameManifestPackage: null", "renameManifestPackage: " + bn));
            }

            lg.ai("Rebuilding APK");
            string bt = Path.Combine(workRoot, "build_temp.apk");
            if (File.Exists(bt)) File.Delete(bt);
            int buildCode = rn.df("java", "-jar " + Q(toolJar) + " b " + Q(dd) + " -o " + Q(bt), workRoot);
            if (buildCode != 0 || !File.Exists(bt))
                throw new Exception("apktool build failed with exit code " + buildCode);

            string oa = Path.Combine(outDir, SafeFileName(tn) + ".apk");
            if (File.Exists(oa)) File.Delete(oa);

            lg.ai("Aligning and signing APK");
            int signCode = rn.df("java",
                "-jar " + Q(signerJar) +
                " --apks " + Q(bt) +
                " --ks " + Q(kp) +
                " --ksPass pass:" + pw +
                " --ksKeyAlias " + Q(ka) +
                " --ksKeyPass pass:" + pw +
                " --out " + Q(workRoot),
                workRoot);
            if (signCode != 0)
                throw new Exception("APK signing failed with exit code " + signCode);

            string[] candidates =
            {
                Path.Combine(workRoot, "build_temp-aligned-signed.apk"),
                Path.Combine(workRoot, "build_temp_signed.apk"),
                Path.Combine(workRoot, "build_temp-aligned-debugSigned.apk"),
                Path.Combine(workRoot, "build_temp-debugSigned.apk")
            };

            string? signedApk = null;
            foreach (string c in candidates)
            {
                if (File.Exists(c))
                {
                    signedApk = c;
                    break;
                }
            }

            if (signedApk == null)
            {
                string[] produced = Directory.GetFiles(workRoot, "*.apk");
                foreach (string c in produced)
                {
                    if (!string.Equals(c, bt, StringComparison.OrdinalIgnoreCase))
                    {
                        signedApk = c;
                        break;
                    }
                }
            }

            if (signedApk == null)
                throw new FileNotFoundException("Signed APK was not created by signer.jar.");

            File.Move(signedApk, oa);

            if (File.Exists(bt)) File.Delete(bt);
            if (Directory.Exists(dd)) Directory.Delete(dd, true);

            if (!File.Exists(oa))
                throw new FileNotFoundException("Final APK was not created.", oa);

            lg.ai("Saved: " + oa);
            return true;
        }


        private void sv()
        {
            try
            {
                string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GDPSMaker");
                Directory.CreateDirectory(dir);
                string f2 = Path.Combine(dir, "settings.txt");
                File.WriteAllText(f2,
                    "url=" + ub.Text + Environment.NewLine +
                    "lastfile=" + (fp.Text ?? string.Empty) + Environment.NewLine);
            }
            catch { }
        }

        private void ld()
        {
            try
            {
                string f = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "GDPSMaker", "settings.txt");
                if (File.Exists(f))
                {
                    foreach (var ln in File.ReadAllLines(f))
                    {
                        if (ln.StartsWith("url="))
                            ub.Text = ln.Substring(4);
                        else if (ln.StartsWith("lastfile=") && File.Exists(ln.Substring(9)))
                        {
                            fp.Text = ln.Substring(9);
                            bs(ln.Substring(9));
                        }
                    }
                }
            }
            catch { }
        }

        protected override void OnDragEnter(DragEventArgs de)
        {
            base.OnDragEnter(de);
            de.Effect = de.Data?.GetDataPresent(DataFormats.FileDrop) == true
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            _dg = de.Effect == DragDropEffects.Copy;
            if (_dg)
            {
                _ob = fs.fc;
                fs.fc = Color.FromArgb(35, 45, 60);
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
            if (_dg)
            {
                fs.fc = _ob;
                _dg = false;
            }
        }

        protected override void OnDragOver(DragEventArgs de)
        {
            base.OnDragOver(de);
            if (de.Data?.GetDataPresent(DataFormats.FileDrop) == true)
                de.Effect = DragDropEffects.Copy;
        }

        protected override void OnDragDrop(DragEventArgs de)
        {
            base.OnDragDrop(de);
            if (_dg)
            {
                fs.fc = _ob;
                _dg = false;
            }
            if (de.Data?.GetData(DataFormats.FileDrop) is string[] fs2 && fs2.Length > 0)
            {
                fp.Text = fs2[0];
                bs(fs2[0]);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            sv();
            base.OnFormClosing(e);
        }
    }
}


