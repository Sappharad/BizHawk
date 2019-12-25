using System;
using System.IO;
using BizHawk.Common;
using System.Reflection;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System.Diagnostics;
using AppKit;
using OpenTK.Input;

namespace BizHawk.Client.EmuHawkMacApp
{
	public class Program
	{
		public static NSEvent KeyHandler(NSEvent vent)
		{
			if (vent.Type == NSEventType.KeyDown)
			{
				OTK_Keyboard.InjectedKeys.Add(TranslateKey(vent.KeyCode));
			}
			else if (vent.Type == NSEventType.KeyUp)
			{
				OTK_Keyboard.InjectedKeys.Remove(TranslateKey(vent.KeyCode));
			}
			return vent;
		}
		private static Key TranslateKey(uint code)
		{
			switch (code)
			{
				case 0x00: return Key.A;
				case 0x01: return Key.S;
				case 0x02: return Key.D;
				case 0x03: return Key.F;
				case 0x04: return Key.H;
				case 0x05: return Key.G;
				case 0x06: return Key.Z;
				case 0x07: return Key.X;
				case 0x08: return Key.C;
				case 0x09: return Key.V;
				case 0x0B: return Key.B;
				case 0x0C: return Key.Q;
				case 0x0D: return Key.W;
				case 0x0E: return Key.E;
				case 0x0F: return Key.R;
				case 0x10: return Key.Y;
				case 0x11: return Key.T;
				case 0x12: return Key.Number1;
				case 0x13: return Key.Number2;
				case 0x14: return Key.Number3;
				case 0x15: return Key.Number4;
				case 0x16: return Key.Number6;
				case 0x17: return Key.Number5;
				//case 0x18: return Key.Equal;             
				case 0x19: return Key.Number9;
				case 0x1A: return Key.Number7;
				case 0x1B: return Key.Minus;
				case 0x1C: return Key.Number8;
				case 0x1D: return Key.Number0;
				case 0x1E: return Key.RBracket;
				case 0x1F: return Key.O;
				case 0x20: return Key.U;
				case 0x21: return Key.LBracket;
				case 0x22: return Key.I;
				case 0x23: return Key.P;
				case 0x25: return Key.L;
				case 0x26: return Key.J;
				case 0x27: return Key.Quote;
				case 0x28: return Key.K;
				case 0x29: return Key.Semicolon;
				case 0x2A: return Key.BackSlash;
				case 0x2B: return Key.Comma;
				case 0x2C: return Key.Slash;
				case 0x2D: return Key.N;
				case 0x2E: return Key.M;
				case 0x2F: return Key.Period;
				case 0x32: return Key.Grave;
				case 0x41: return Key.KeypadDecimal;
				case 0x43: return Key.KeypadMultiply;
				case 0x45: return Key.KeypadPlus;
				case 0x47: return Key.Clear;
				case 0x4B: return Key.KeypadDivide;
				case 0x4C: return Key.KeypadEnter;
				case 0x4E: return Key.KeypadMinus;
				//case 0x51: return Key.KeypadEquals;      
				case 0x52: return Key.Keypad0;
				case 0x53: return Key.Keypad1;
				case 0x54: return Key.Keypad2;
				case 0x55: return Key.Keypad3;
				case 0x56: return Key.Keypad4;
				case 0x57: return Key.Keypad5;
				case 0x58: return Key.Keypad6;
				case 0x59: return Key.Keypad7;
				case 0x5B: return Key.Keypad8;
				case 0x5C: return Key.Keypad9;
				case 0x24: return Key.Enter;
				case 0x30: return Key.Tab;
				case 0x31: return Key.Space;
				case 0x33: return Key.BackSpace;
				case 0x35: return Key.Escape;
				case 0x37: return Key.Command;
				case 0x38: return Key.ShiftLeft;
				case 0x39: return Key.CapsLock;
				//case 0x3A: return Key.Option;          
				case 0x3B: return Key.LControl;
				case 0x3C: return Key.RShift;
				//case 0x3D: return Key.ROption;     
				case 0x3E: return Key.RControl;
				//case 0x3F: return Key.Function;
				case 0x40: return Key.F17;
				//case 0x48: return Key.VolumeUp;       
				//case 0x49: return Key.VolumeDown;
				//case 0x4A: return Key.Mute;              
				case 0x4F: return Key.F18;
				case 0x50: return Key.F19;
				case 0x5A: return Key.F20;
				case 0x60: return Key.F5;
				case 0x61: return Key.F6;
				case 0x62: return Key.F7;
				case 0x63: return Key.F3;
				case 0x64: return Key.F8;
				case 0x65: return Key.F9;
				case 0x67: return Key.F11;
				case 0x69: return Key.F13;
				case 0x6A: return Key.F16;
				case 0x6B: return Key.F14;
				case 0x6D: return Key.F10;
				case 0x6F: return Key.F12;
				case 0x71: return Key.F15;
				//case 0x72: return Key.Help;
				case 0x73: return Key.Home;
				case 0x74: return Key.PageUp;
				case 0x75: return Key.Delete;
				case 0x76: return Key.F4;
				case 0x77: return Key.End;
				case 0x78: return Key.F2;
				case 0x79: return Key.PageDown;
				case 0x7A: return Key.F1;
				case 0x7B: return Key.Left;
				case 0x7C: return Key.Right;
				case 0x7D: return Key.Down;
				case 0x7E: return Key.Up;
			}
			return Key.Unknown;
		}

		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Bizware.BizwareGL.Drivers.OpenTK.IGL_TK.ViewportScale = (int)NSScreen.MainScreen.BackingScaleFactor;
			NSEvent.AddLocalMonitorForEventsMatchingMask(NSEventMask.KeyDown | NSEventMask.KeyUp, KeyHandler); //Workaround for broken input on Catalina
			//Note: If you have multiple monitors with different scale factors, this won't work. Known limitation.

			//BizHawk.Common.TempFileCleaner.Start();


			HawkFile.ArchiveHandlerFactory = new SharpCompressArchiveHandler();

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
				mf.Text = title;
				mf.Shown += (sender, e) => { DoMenuExtraction(mf); };
				mf.OnPauseChanged += (sender, e) => { RefreshAllMenus(mf); };
				mf.FormClosed += (sender, e) =>
				{
					Application.Exit();
					//This is supposed to account for threads not exiting cleanly on macOS, but I think mono crashes anyway.
					NSApplication.SharedApplication.Terminate(NSApplication.SharedApplication);
				};

				try
				{
					mf.Show();
					GlobalWin.ExitCode = mf.ProgramRunLoop();
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

		private static System.Collections.Generic.Dictionary<ToolStripMenuItem, NSMenuItem> _menuLookup;
		private static System.Collections.Generic.Dictionary<NSMenuItem, ToolStripMenuItem> _reverseMenuLookup;

		private static void DoMenuExtraction(Form mainForm)
		{
			_menuLookup = new System.Collections.Generic.Dictionary<ToolStripMenuItem, NSMenuItem>();
			_reverseMenuLookup = new System.Collections.Generic.Dictionary<NSMenuItem, ToolStripMenuItem>();
			ExtractMenus(mainForm.MainMenuStrip);
			mainForm.MainMenuStrip.Visible = false; //Hide original menu since we extracted it.
		}

		private static void ExtractMenus(System.Windows.Forms.MenuStrip menus)
		{
			for (int i = 0; i < menus.Items.Count; i++)
			{
				ToolStripMenuItem item = menus.Items[i] as ToolStripMenuItem;
				NSMenuItem menuOption = new NSMenuItem(CleanMenuString(item.Text));
				NSMenu dropDown = new NSMenu(CleanMenuString(item.Text));
				dropDown.AutoEnablesItems = false;
				menuOption.Submenu = dropDown;
				NSApplication.SharedApplication.MainMenu.AddItem(menuOption);
				_menuLookup.Add(item, menuOption);
				_reverseMenuLookup.Add(menuOption, item);
				menuOption.Hidden = !item.Visible;
				item.VisibleChanged += HandleItemVisibleChanged;
				menuOption.Enabled = item.Enabled;
				ExtractSubmenu(item.DropDownItems, dropDown, i == 0); //Skip last 2 options in first menu, redundant exit option
			}
		}

		private static void ExtractSubmenu(ToolStripItemCollection subItems, NSMenu destMenu, bool fileMenu)
		{
			int max = subItems.Count;
			if (fileMenu) max -= 2;
			for (int i = 0; i < max; i++)
			{
				ToolStripItem item = subItems[i];
				if (item is ToolStripMenuItem)
				{
					ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
					NSMenuItem translated = new NSMenuItem(CleanMenuString(item.Text));
					menuItem.CheckedChanged += HandleMenuItemCheckedChanged;
					menuItem.EnabledChanged += HandleMenuItemEnabledChanged;
					translated.Activated += (sender, e) => 
					{
						menuItem.PerformClick();
						//MainForm.AddMessage(menuItem.PerformClick);
					};
					translated.State = menuItem.Checked ? NSCellStateValue.On : NSCellStateValue.Off;
					translated.Enabled = item.Enabled;
					if (menuItem.Image != null) translated.Image = ImageToCocoa(menuItem.Image);
					destMenu.AddItem(translated);
					_menuLookup.Add(menuItem, translated);
					_reverseMenuLookup.Add(translated, menuItem);
					if (menuItem.DropDownItems.Count > 0)
					{
						NSMenu dropDown = new NSMenu(CleanMenuString(item.Text));
						dropDown.AutoEnablesItems = false;
						translated.Submenu = dropDown;
						ExecuteDropDownOpened(menuItem);
						ExtractSubmenu(menuItem.DropDownItems, dropDown, false);
					}
				}
				else if (item is ToolStripSeparator)
				{
					destMenu.AddItem(NSMenuItem.SeparatorItem);
				}
			}
		}

		private static void ExecuteDropDownOpened(ToolStripMenuItem item)
		{
			var dropDownOpeningKey = typeof(ToolStripDropDownItem).GetField("DropDownOpenedEvent", BindingFlags.Static | BindingFlags.NonPublic);
			var eventProp = typeof(ToolStripDropDownItem).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
			if (eventProp != null && dropDownOpeningKey != null)
			{
				var dropDownOpeningValue = dropDownOpeningKey.GetValue(item);
				var eventList = eventProp.GetValue(item, null) as System.ComponentModel.EventHandlerList;
				if (eventList != null)
				{
					Delegate ddd = eventList[dropDownOpeningValue];
					try
					{
						if (ddd != null) ddd.DynamicInvoke(null, EventArgs.Empty);
					}
					catch (Exception ex)
					{
						//throw ex;
					}
				}
			}
		}

		private static void HandleItemVisibleChanged(object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem && _menuLookup.ContainsKey((ToolStripMenuItem)sender))
			{
				NSMenuItem translated = _menuLookup[(ToolStripMenuItem)sender];
				translated.Hidden = !translated.Hidden;
				//Can't actually look at Visible property because the entire menubar is hidden.
				//Since the event only gets called when Visible is changed, we can assume it got flipped.
				if (((ToolStripMenuItem)sender).Text.Equals("&NES"))
				{
					//Hack to rebuild menu contents due to changing FDS sub-menu.
					//At some point, I might want to figure out a better way to do this.
					RemoveMenuItems(translated);
					ExtractSubmenu(((ToolStripMenuItem)sender).DropDownItems, translated.Submenu, false);
				}
			}
		}

		private static void RemoveMenuItems(NSMenuItem menu)
		{
			if (menu.HasSubmenu)
			{
				for (int i = (int)(menu.Submenu.Count - 1); i >= 0; i--)
				{
					NSMenuItem item = menu.Submenu.ItemAt(i) as NSMenuItem;
					if (item != null) //It will be null if it's a separator
					{
						RemoveMenuItems(item);
						if (_reverseMenuLookup.ContainsKey(item))
						{
							var hostMenu = _reverseMenuLookup[item];
							if (_menuLookup.ContainsKey(hostMenu))
							{
								_menuLookup.Remove(hostMenu);
								_reverseMenuLookup.Remove(item);
							}
							hostMenu.CheckedChanged -= HandleMenuItemCheckedChanged;
							hostMenu.EnabledChanged -= HandleMenuItemEnabledChanged;
						}
					}
					menu.Submenu.RemoveItemAt(i);
				}
			}
		}

		private static void HandleMenuItemEnabledChanged(object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem && _menuLookup.ContainsKey((ToolStripMenuItem)sender))
			{
				NSMenuItem translated = _menuLookup[(ToolStripMenuItem)sender];
				translated.Enabled = ((ToolStripMenuItem)sender).Enabled;
			}
		}

		private static void HandleMenuItemCheckedChanged(object sender, EventArgs e)
		{
			if (sender is ToolStripMenuItem && _menuLookup.ContainsKey((ToolStripMenuItem)sender))
			{
				NSMenuItem translated = _menuLookup[(ToolStripMenuItem)sender];
				translated.State = ((ToolStripMenuItem)sender).Checked ? NSCellStateValue.On : NSCellStateValue.Off;
			}
		}

		private static void RefreshAllMenus(Form mainForm)
		{
			for (int i = 0; i < mainForm.MainMenuStrip.Items.Count; i++)
			{
				ToolStripMenuItem item = mainForm.MainMenuStrip.Items[i] as ToolStripMenuItem;
				NSMenuItem mia = _menuLookup[item];
				if (mia != null)
				{
					RemoveMenuItems(mia);
					if (_reverseMenuLookup.ContainsKey(mia))
					{
						ExtractSubmenu(_reverseMenuLookup[mia].DropDownItems, mia.Submenu, i == 0);
					}
				}
			}
		}

		private static NSImage ImageToCocoa(System.Drawing.Image input)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			input.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			ms.Position = 0;
			NSImage img = NSImage.FromStream(ms);
			img.Size = new CoreGraphics.CGSize(16.0, 16.0); //Some of BizHawk's menu icons are larger, even though WinForms only does 16x16.
			return img;
		}

		private static string CleanMenuString(string text)
		{
			return text.Replace("&", string.Empty);
		}

		/*private class MenuItemAdapter : NSMenuItem
		{
			public MenuItemAdapter(ToolStripMenuItem host) : base(CleanMenuString(host.Text))
			{
				HostMenu = host;
			}
			public ToolStripMenuItem HostMenu { get; set; }
		}*/

		/*[Export("OnAppQuit")]
		private static void OnQuit()
		{
			_mainWinForm.Close();
		}*/
	}
}
