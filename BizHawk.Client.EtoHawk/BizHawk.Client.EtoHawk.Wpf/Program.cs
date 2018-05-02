using System;
using Eto;
using Eto.Forms;
using Eto.Gl;

namespace BizHawk.Client.EtoHawk.Wpf
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var platform = new Eto.Wpf.Platform();
#if WINDOWS
            platform.Add<GLSurface.IHandler>(() => new Eto.Gl.WPF_WFControl.WPFWFGLSurfaceHandler());
#endif

            new Application(platform).Run(new MainForm());
        }
    }
}
