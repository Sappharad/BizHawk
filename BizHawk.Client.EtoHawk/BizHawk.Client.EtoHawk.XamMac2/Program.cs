using System;
using Eto;
using Eto.Forms;
using Eto.Gl;
using Eto.Gl.XamMac;

namespace BizHawk.Client.EtoHawk.XamMac2
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var gen = new Eto.Mac.Platform();

            gen.Add<GLSurface.IHandler>(() => new MacGLSurfaceHandler());

            new Application(gen).Run(new MainForm());
        }
    }
}
