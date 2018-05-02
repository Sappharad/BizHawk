using System;
using Eto;
using Eto.Forms;
using Eto.Gl;

namespace BizHawk.Client.EtoHawk.WinForms
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var platform = new Eto.WinForms.Platform();
#if WINDOWS
            platform.Add<GLSurface.IHandler>(() => new Eto.Gl.Windows.WinGLSurfaceHandler());
#endif

            new Application(platform).Run(new MainForm());
        }
    }
}
