using System;
using System.IO;
using System.Text;

namespace GDPSMaker.Services
{
    public static class bp
    {
        public static void df(byte[] dt, byte[] fd, byte[] rp)
        {
            if (rp.Length != fd.Length)
                throw new Exception("find/replace length mismatch");

            int i = 0;
            while (i <= dt.Length - fd.Length)
            {
                bool mt = true;
                for (int j = 0; j < fd.Length; j++)
                {
                    if (dt[i + j] != fd[j])
                    {
                        mt = false;
                        break;
                    }
                }

                if (mt)
                {
                    Array.Copy(rp, 0, dt, i, rp.Length);
                    i += rp.Length;
                }
                else
                {
                    i += 1;
                }
            }
        }

        public static void gf(string bp, string pu, string bn, string pn, string ob = "")
        {
            byte[] dt = File.ReadAllBytes(bp);

            df(
                dt,
                Encoding.ASCII.GetBytes("http://www.boomlings.com/database"),
                Encoding.ASCII.GetBytes(pu));

            df(
                dt,
                Encoding.ASCII.GetBytes("https://www.boomlings.com/database"),
                Encoding.ASCII.GetBytes(pu + "/"));

            df(
                dt,
                Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes("http://www.boomlings.com/database"))),
                Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.ASCII.GetBytes(pu))));

            if (!string.IsNullOrEmpty(bn))
            {
                string obn = "com.robtopx.geometryjump";

                if (bn.Length == 23)
                {
                    obn = "com.robtop.geometryjump";

                    if (bp.EndsWith(".plist", StringComparison.OrdinalIgnoreCase))
                    {
                        df(dt, Encoding.ASCII.GetBytes("Geometry"), Encoding.ASCII.GetBytes(pn));
                        df(dt, Encoding.ASCII.GetBytes(pn + "Jump"), Encoding.ASCII.GetBytes("GeometryJump"));
                    }
                }

                df(dt, Encoding.ASCII.GetBytes(obn), Encoding.ASCII.GetBytes(bn));
            }

            string op = string.IsNullOrEmpty(ob) ? bp : ob;
            File.WriteAllBytes(op, dt);
        }
    }
}
