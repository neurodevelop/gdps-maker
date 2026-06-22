using System;
using System.Diagnostics;

namespace GDPSMaker.Services
{
    public static class rn
    {
        public static int df(string ap, string ar, string? wd = null)
        {
            var si = new ProcessStartInfo
            {
                FileName = ap,
                Arguments = ar,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = wd ?? Environment.CurrentDirectory
            };

            using var pc = Process.Start(si);
            if (pc is null)
                throw new InvalidOperationException("Failed to start: " + ap);

            pc.WaitForExit();
            return pc.ExitCode;
        }
    }
}
