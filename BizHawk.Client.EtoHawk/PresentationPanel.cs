using System;
using System.IO;
using System.Threading;
using Eto.Forms;
using Eto.Drawing;
using BizHawk.Client.Common;
using BizHawk.Bizware.BizwareGL;
using OpenTK.Graphics.OpenGL;
using BizHawk.Client.EtoHawk.Graphics;

namespace BizHawk.Client.EtoHawk
{
    public class PresentationPanel
    {
        public PresentationPanel()
        {
            GL = GlobalWin.GL;

            GraphicsControl = new EtoGraphicsControl(GL);
            //GraphicsControl.Dock = DockStyle.Fill;
            //GraphicsControl.BackColor = Color.Black;

            //pass through these events to the form. we might need a more scalable solution for mousedown etc. for zapper and whatnot.
            //http://stackoverflow.com/questions/547172/pass-through-mouse-events-to-parent-control (HTTRANSPARENT)
            GraphicsControl.MouseDoubleClick += (o, e) => HandleFullscreenToggle(o, e);
            //GraphicsControl.MouseClick += (o, e) => GlobalWin.MainForm.MainForm_MouseClick(o, e);
            //GraphicsControl.MouseMove += (o, e) => GlobalWin.MainForm.MainForm_MouseMove(o, e);
            //GraphicsControl.MouseWheel += (o, e) => GlobalWin.MainForm.MainForm_MouseWheel(o, e);
        }

        bool IsDisposed = false;
        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            GraphicsControl.Dispose();
        }

        //graphics resources
        IGL GL;
        public EtoGraphicsControl GraphicsControl;

        public Control Control { get { return GraphicsControl; } }
        public static implicit operator Control(PresentationPanel self) { return self.GraphicsControl; }

        private void HandleFullscreenToggle(object sender, MouseEventArgs e)
        {
            if ((e.Buttons & MouseButtons.Primary) == MouseButtons.Primary)
            {
                //allow suppression of the toggle.. but if shift is pressed, always do the toggle
                /*bool allowSuppress = Control.ModifierKeys != Keys.Shift;
                if (Global.Config.DispChrome_AllowDoubleClickFullscreen || !allowSuppress)
                {
                    GlobalWin.MainForm.ToggleFullscreen(allowSuppress);
                }*/
            }
        }

        public bool Resized { get; set; }

        public Size NativeSize { get { return GraphicsControl.ClientSize; } }

    }

    public interface IBlitterFont { }
}
