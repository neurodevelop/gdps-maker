using System;
using System.IO;
using GDPSMaker.Models;

namespace GDPSMaker.Services
{
    public static class pd
    {
        public static pt df(string p)
        {
            if (string.IsNullOrEmpty(p))
                return pt.u;
            if (p.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                return pt.w;
            if (p.EndsWith(".ipa", StringComparison.OrdinalIgnoreCase))
                return pt.i;
            if (p.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
                return pt.a;
            return pt.u;
        }
    }
}
