using System;
using BizHawk.Bizware.BizwareGL;
using Eto.Forms;

namespace BizHawk.Client.EtoHawk.Graphics
{
    public class EtoGraphicsControl : Panel
    {
        public EtoGraphicsControl(IGL owner)
        {
            IGL = owner;
            IGC = owner.Internal_CreateGraphicsControl();
            Managed = IGC as Control;
            this.Content = Managed;

            Managed.MouseDoubleClick += (object sender, MouseEventArgs e) => OnMouseDoubleClick(e);
            //Managed.MouseClick += (object sender, MouseEventArgs e) => OnMouseClick(e);
            Managed.MouseEnter += (object sender, MouseEventArgs e) => OnMouseEnter(e);
            Managed.MouseLeave += (object sender, MouseEventArgs e) => OnMouseLeave(e);
            Managed.MouseMove += (object sender, MouseEventArgs e) => OnMouseMove(e);
        }

        /// <summary>
        /// If this is the main window, things may be special
        /// </summary>
        public bool MainWindow;
        public readonly IGL IGL;
        IGraphicsControl IGC;
        Control Managed;

        //public virtual Control Control { get { return Managed; } } //do we need this anymore?
        public virtual void SetVsync(bool state) { IGC.SetVsync(state); }
        public virtual void SwapBuffers() { IGC.SwapBuffers(); }
        public virtual void Begin() { IGC.Begin(); }
        public virtual void End() { IGC.End(); }
    }
}
