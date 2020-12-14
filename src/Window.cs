using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Net;
using Wrl = IWshRuntimeLibrary;
using static System.Environment;

namespace Angels
{
    internal partial class Window: Form
    {
        private static ulong? lastUint = null;
        private static readonly string dir = GetFolderPath(SpecialFolder.MyDocuments) + "\\AngelsData";
        private static readonly Random randomizer = new Random();
        private static readonly WebClient client = new WebClient();

        internal Window()
        {
            InitializeComponent();
            Text = Constants.Name;
            label.Text = $"{Constants.Name} for Among Us";
            info.Text = $"v{Constants.Ver} by {Constants.Dev}";

            // client events.
            client.DownloadProgressChanged += (_, e) =>
                progress.Value = e.ProgressPercentage;
            client.DownloadFileCompleted += (_, e) =>
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
                    Error();
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
                    var zip = $"{dir}\\package{e.Argument}.zip";
                    var d = Directory.CreateDirectory(destination);
                    ZipFile.ExtractToDirectory(zip, destination);
                    File.WriteAllText($"{dir}\\uint.txt", lastUint.ToString());
                    File.Delete(zip);
                    var exes = d.GetFiles("Among Us.exe", SearchOption.AllDirectories);
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
                    Error();
            };

            // check events.
            check.Click += delegate
            {
                if (!CheckForUpdates())
                    MessageBox.Show("No update available.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            
            CheckForUpdates();
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            // Black title bar.
            if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
        }

        private static ulong? ParseNullableUInt64(string text) =>
            ulong.TryParse(text, out var result) ? result as ulong? : null;

        private static void Error()
        {
            try
            {
                File.Delete($"{dir}\\uint.txt");
            }
            finally
            {
                MessageBox.Show("An error occurred.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TryDownloadPackage(string msg)
        {
            if (MessageBox.Show(msg, Constants.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var random = randomizer.Next(10000, 100000).ToString();
                client.DownloadFileAsync(
                    new Uri(client.DownloadString("https://github.com/LorenzoLotti/Angels/raw/main/pkgref.txt").Trim('\r', '\n').Trim()),
                    $"{dir}\\package{random}.zip",
                    random
                );
                check.Visible = false;
                progress.Visible = true;
            }
        }

        private bool CheckForUpdates()
        {
            try
            {
                lastUint = ParseNullableUInt64(client.DownloadString("https://github.com/LorenzoLotti/Angels/raw/main/uint.txt"));
                var d = Directory.CreateDirectory(dir);
                ulong? uintTxt = null;
                foreach (var file in d.GetFiles())
                    if (file.Name == "uint.txt")
                        uintTxt = ParseNullableUInt64(File.ReadAllText(file.FullName));
                    else
                        try
                        {
                            file.Delete();
                        }
                        catch { }
                if (uintTxt == null)
                {
                    TryDownloadPackage("Do you want to download and install Among Us?");
                    return true;
                }
                else if (uintTxt > lastUint)
                {
                    TryDownloadPackage("An update of Among Us is available, do you want to download and install it?");
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                Error();
                return false;
            }
        }
    }
}
