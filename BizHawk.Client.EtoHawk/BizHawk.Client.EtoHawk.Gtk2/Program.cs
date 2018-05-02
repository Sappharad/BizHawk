using System;
using Eto;
using Eto.Forms;
using Eto.Gl;

namespace BizHawk.Client.EtoHawk.Gtk2
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var platform = new Eto.GtkSharp.Platform();
            //TODO: The preprocessor definitions can be removed once I compile a build that includes all platforms
#if LINUX
            platform.Add<GLSurface.IHandler>(() => new Eto.Gl.Gtk.GtkGlSurfaceHandler());
#endif

            new Application(platform).Run(new MainForm());
        }
    }
}
