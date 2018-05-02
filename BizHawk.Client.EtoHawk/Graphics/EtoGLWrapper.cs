using System;
using BizHawk.Bizware.BizwareGL;
using BizHawk.Bizware.BizwareGL.Drivers.OpenTK;

namespace BizHawk.Client.EtoHawk.Graphics
{
    public class EtoGLWrapper : Eto.Gl.GLSurface, IGraphicsControl
    {
        public EtoGLWrapper(Bizware.BizwareGL.Drivers.OpenTK.IGL_TK owner)
            : base(OpenTK.Graphics.GraphicsMode.Default, 2, 0, OpenTK.Graphics.GraphicsContextFlags.Default)
        {
            Owner = owner;
            //GLControl = this;
            //this.
        }

        //global::OpenTK.GLControl GLControl;
        IGL_TK Owner;

        //public Control Control { get { return this; } }


        public void SetVsync(bool state)
        {
            //IGraphicsContext curr = global::OpenTK.Graphics.GraphicsContext.CurrentContext;
            this.MakeCurrent();
            //GLControl.MakeCurrent();
            //GLControl.VSync = state;
            //Owner.MakeContextCurrent(curr, Owner.NativeWindowsForContexts[curr]);
        }

        public void Begin()
        {
            /*if (!GLControl.Context.IsCurrent)
                Owner.MakeContextCurrent(GLControl.Context, GLControl.WindowInfo);*/
            MakeCurrent();
        }

        public void End()
        {
            Owner.MakeDefaultCurrent();
        }

        public new void SwapBuffers()
        {
            //if (!GLControl.Context.IsCurrent)
                MakeCurrent();
            base.SwapBuffers();
        }
    }
}
