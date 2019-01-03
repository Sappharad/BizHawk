using System;
using System.IO;
using BizHawk.Common;
using System.Reflection;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System.Diagnostics;
using System.Threading;

namespace BizHawk.Client.EmuHawkMacApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Bizware.BizwareGL.Drivers.OpenTK.IGL_TK.NativeViewportScale = (int)AppKit.NSScreen.MainScreen.BackingScaleFactor;
			//Note: If you have multiple monitors with different scale factors, this won't work. Known limitation.

			//BizHawk.Common.TempFileCleaner.Start();


			HawkFile.ArchiveHandlerFactory = new SevenZipSharpArchiveHandler();

			string iniPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.ini");

			try
			{
				Global.Config = ConfigService.Load<Config>(iniPath);
				Global.Config.SoundOutputMethod = Config.ESoundOutputMethod.OpenAL;
			}
			catch (Exception e)
			{
				new ExceptionBox(e).ShowDialog();
				new ExceptionBox("Since your config file is corrupted, we're going to recreate it. Back it up before proceeding if you want to investigate further.").ShowDialog();
				File.Delete(iniPath);
				Global.Config = ConfigService.Load<Config>(iniPath);
			}

			Global.Config.ResolveDefaults();

			BizHawk.Client.Common.StringLogUtil.DefaultToDisk = Global.Config.MoviesOnDisk;
			BizHawk.Client.Common.StringLogUtil.DefaultToAWE = Global.Config.MoviesInAWE;

			// create IGL context. we do this whether or not the user has selected OpenGL, so that we can run opengl-based emulator cores
			GlobalWin.IGL_GL = new Bizware.BizwareGL.Drivers.OpenTK.IGL_TK(2, 0, false);

			// setup the GL context manager, needed for coping with multiple opengl cores vs opengl display method
			GLManager.CreateInstance(GlobalWin.IGL_GL);
			GlobalWin.GLManager = GLManager.Instance;

			//now create the "GL" context for the display method. we can reuse the IGL_TK context if opengl display method is chosen
			GlobalWin.GL = GlobalWin.IGL_GL;
			//GlobalWin.GL = new Bizware.BizwareGL.Drivers.GdiPlus.IGL_GdiPlus();

			// try creating a GUI Renderer. If that doesn't succeed. we fail
			try
			{
				using (GlobalWin.GL.CreateRenderer()) { }
			}
			catch (Exception ex)
			{
				var e2 = new Exception("Initialization of Display Method failed; Failing completely because macOS is supposed to have OpenGL.", ex);
				new ExceptionBox(e2).ShowDialog();
				return;// 1; //Failed
			}

			try
			{
				var mf = new MainForm(args);
				var title = mf.Text;
				//mf.Show();
				mf.Text = title;
				Application.Run(mf);


				try
				{
					//GlobalWin.ExitCode = mf.ProgramRunLoop();
				}
				catch (Exception e) when (!Debugger.IsAttached && Global.MovieSession.Movie.IsActive)
				{
					var result = MessageBox.Show(
						"EmuHawk has thrown a fatal exception and is about to close.\nA movie has been detected. Would you like to try to save?\n(Note: Depending on what caused this error, this may or may not succeed)",
						"Fatal error: " + e.GetType().Name,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Exclamation
						);
					if (result == DialogResult.Yes)
					{
						Global.MovieSession.Movie.Save();
					}
				}

			}
			catch (Exception e) when (!Debugger.IsAttached)
			{
				new ExceptionBox(e).ShowDialog();
			}
			finally
			{
				if (GlobalWin.Sound != null)
				{
					GlobalWin.Sound.Dispose();
					GlobalWin.Sound = null;
				}
				GlobalWin.GL.Dispose();
				Input.Cleanup();
			}

			//return 0 assuming things have gone well, non-zero values could be used as error codes or for scripting purposes
			return;
		}
	}
}
