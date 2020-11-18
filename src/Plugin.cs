using System;
using System.Net;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using UnfallenLauncherAPI;
using UnfallenLauncherAPI.Default;

namespace Angels
{
    // Main plugin.
    public class Plugin: IPlugin
    {
        private static readonly Random randomizer = new Random();
        internal static PluginForm Form;
        internal static ulong? LastUint = null;
        internal static readonly string Dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AngelsData";
        internal static readonly WebClient Client = new WebClient();
        public IHost Host { get; } = new Host();
        public bool VisibleInMenu => true;
        public uint Version => Constants.Ver;
        public string Developer => Constants.Dev;
        public string Name => Constants.Name;
        public Image Icon => Properties.Resources.IconImage;
        internal static event EventHandler Downloading = delegate { };

        public void OnPluginMenuItemClick() =>
            Form.ShowWindow((Host as Host).GetDarkerBackColor());

        private static void TryDownloadPackage(string msg)
        {
            if (MessageBox.Show(msg, Constants.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var random = randomizer.Next(10000, 100000).ToString();
                Client.DownloadFileAsync(
                    new Uri(Client.DownloadString("https://github.com/AmongUsITA-Angels/Angels/raw/main/pkgref.txt").Trim('\r', '\n').Trim()),
                    $"{Dir}\\package{random}.zip",
                    random
                );
                Downloading(null, new EventArgs());
            }
        }

        private static ulong? ParseNullableUInt64(string text) =>
            ulong.TryParse(text, out var result) ? result as ulong? : null;

        internal static bool CheckForUpdates()
        {
            try
            {
                LastUint = ParseNullableUInt64(Client.DownloadString("https://github.com/AmongUsITA-Angels/Angels/raw/main/uint.txt"));
                var dir = Directory.CreateDirectory(Dir);
                ulong? uintTxt = null;
                foreach (var file in dir.GetFiles())
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
                else if (uintTxt > LastUint)
                {
                    TryDownloadPackage("An update of Among Us is available, do you want to download and install it?");
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                MessageBox.Show("An error occurred.", Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    // Main host.
    public class Host: CompleteHostBase
    {
        protected override void OnStart()
        {
            Plugin.Form = new PluginForm();
            Plugin.CheckForUpdates();
        }

        internal Color GetDarkerBackColor()
        {
            switch (BackColor.Value)
            {
                default:
                    return Color.FromArgb(20, 20, 20);

                case BackColorState.Dark:
                    return Color.FromArgb(10, 10, 10);

                case BackColorState.Red:
                    return Color.FromArgb(15, 0, 0);

                case BackColorState.Blue:
                    return Color.FromArgb(0, 0, 15);

                case BackColorState.Purple:
                    return Color.FromArgb(15, 0, 15);
            }
        }
    }
}
