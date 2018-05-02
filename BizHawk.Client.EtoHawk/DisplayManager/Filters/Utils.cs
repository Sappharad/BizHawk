using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Eto.Drawing;

using BizHawk.Common;
using BizHawk.Client.Common;
using BizHawk.Client.EtoHawk;
using BizHawk.Client.EtoHawk.FilterManager;

using BizHawk.Bizware.BizwareGL;
using BizHawk.Bizware.BizwareGL.Drivers.OpenTK;

using OpenTK;
using OpenTK.Graphics;

namespace BizHawk.Client.EtoHawk.Filters
{
	public class SourceImage : BaseFilter
	{
		public SourceImage(Size size)
		{
			this.Size = size;
		}

		Size Size;

		public Texture2d Texture;

		public override void Run()
		{
			YieldOutput(Texture);
		}

		public override void Initialize()
		{
			DeclareOutput(new SurfaceState(new SurfaceFormat(Size), SurfaceDisposition.Texture));
		}

		public override void SetInputFormat(string channel, SurfaceState format)
		{
			DeclareOutput(SurfaceDisposition.Texture);
		}
	}

	/// <summary>
	/// transforms an input texture to an output render target (by rendering it)
	/// </summary>
	class Render : BaseFilter
	{
		public override void Initialize()
		{
			DeclareInput(SurfaceDisposition.Texture);
		}

		public override void SetInputFormat(string channel, SurfaceState state)
		{
			DeclareOutput(new SurfaceState(state.SurfaceFormat, SurfaceDisposition.RenderTarget));
		}

		public override void Run()
		{
			var renderer = FilterProgram.GuiRenderer;
            var renderSize = FindOutput().SurfaceFormat.Size;
            renderer.Begin(renderSize.Width, renderSize.Height);
			renderer.SetBlendState(FilterProgram.GL.BlendNoneCopy);
			renderer.Draw(InputTexture);
			renderer.End();
		}
	}

	class Resolve : BaseFilter
	{
		public override void Initialize()
		{
			DeclareInput(SurfaceDisposition.RenderTarget);
		}

		public override void SetInputFormat(string channel, SurfaceState state)
		{
			DeclareOutput(new SurfaceState(state.SurfaceFormat, SurfaceDisposition.Texture));
		}

		public override void Run()
		{
			YieldOutput(FilterProgram.GetRenderTarget().Texture2d);
		}
	}
}