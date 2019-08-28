using System;
using System.IO;
using System.Runtime.InteropServices;

using BizHawk.Common;

namespace BizHawk.Common.BizInvoke
{
	public class DynamicLibraryImportResolver : IImportResolver, IDisposable
	{
		private IntPtr _p;
		private readonly OSTailoredCode.ILinkedLibManager libLoader = OSTailoredCode.LinkedLibManager;

		public DynamicLibraryImportResolver(string dllName)
		{
			if(OSTailoredCode.CurrentOS == OSTailoredCode.DistinctOS.macOS)
			{
				dllName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), dllName);
			}
			_p = libLoader.LoadPlatformSpecific(dllName);
			if (_p == IntPtr.Zero) throw new InvalidOperationException($"null pointer returned by {nameof(libLoader.LoadPlatformSpecific)}");
		}

		public IntPtr Resolve(string entryPoint)
		{
			return libLoader.GetProcAddr(_p, entryPoint);
		}

		private void Free()
		{
			if (_p == IntPtr.Zero) return;
			libLoader.FreePlatformSpecific(_p);
			_p = IntPtr.Zero;
		}

		public void Dispose()
		{
			Free();
			GC.SuppressFinalize(this);
		}

		~DynamicLibraryImportResolver()
		{
			Free();
		}
	}
}
