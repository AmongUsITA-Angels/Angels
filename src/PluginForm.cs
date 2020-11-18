using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using Wrl = IWshRuntimeLibrary;
using static System.Environment;

namespace Angels
{
    public partial class PluginForm: Form
    {
        public PluginForm()
        {
            InitializeComponent();
            Text = Constants.Name;
            label.Text = $"{Constants.Name} for Among Us";
            info.Text = $"v{Constants.Ver} by {Constants.Dev}";

            // Plugin events.
            Plugin.Downloading += delegate
            {
                check.Visible = false;
                progress.Visible = true;
            };

            // Plugin.Client events.
            Plugin.Client.DownloadProgressChanged += (_, e) =>
                progress.Value = e.ProgressPercentage;
            Plugin.Client.DownloadFileCompleted += (_, e) =>
            {
                if (e.Error == null)
                {
                    progress.Style = ProgressBarStyle.Marquee;
                    unzipThread.RunWorkerAsync(e.UserState);
                }
                else
                {
                    progress.Visible = false;
                    check.Visible = true;
                    MessageBox.Show("An error occurred.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // unzipThread events.
            unzipThread.DoWork += (_, e) =>
            {
                try
                {
                    var processes = Process.GetProcessesByName("Among Us");
                    foreach (var process in processes)
                        process.Kill();
                    var destination = $"\\Among Us ({Constants.Name})";
                    try
                    {
                        Directory.Delete(destination, true);
                    }
                    catch { }
                    var zip = $"{Plugin.Dir}\\package{e.Argument}.zip";
                    var dir = Directory.CreateDirectory(destination);
                    ZipFile.ExtractToDirectory(zip, destination);
                    File.WriteAllText($"{Plugin.Dir}\\uint.txt", Plugin.LastUint.ToString());
                    File.Delete(zip);
                    var exes = dir.GetFiles("Among Us.exe", SearchOption.AllDirectories);
                    if (exes.Length >= 0)
                    {
                        var exe = exes[0].FullName;
                        var exedir = exe.Remove(exe.LastIndexOf("\\"));
                        var shell = new Wrl.WshShell();
                        var steam = GetFolderPath(SpecialFolder.Programs) + "\\Steam";
                        Directory.CreateDirectory(steam);
                        Wrl.WshShortcut start = shell.CreateShortcut(steam + "\\Among Us.lnk");
                        start.Arguments = string.Empty;
                        start.Description = string.Empty;
                        start.Hotkey = string.Empty;
                        start.IconLocation = exe;
                        start.TargetPath = exe;
                        start.WindowStyle = 0;
                        start.WorkingDirectory = exedir;
                        start.Save();
                    }
                    e.Result = true;
                }
                catch
                {
                    e.Result = false;
                }
            };
            unzipThread.RunWorkerCompleted += (_, e) =>
            {
                progress.Style = ProgressBarStyle.Blocks;
                progress.Visible = false;
                check.Visible = true;
                if ((bool)e.Result)
                    MessageBox.Show("Done.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("An error occurred.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            // check events.
            check.Click += delegate
            {
                if (!Plugin.CheckForUpdates())
                    MessageBox.Show("No update available.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            // Black title bar.
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void ShowWindow(Color back)
        {
            BackColor = back;
            ShowDialog();
        }
    }
}
