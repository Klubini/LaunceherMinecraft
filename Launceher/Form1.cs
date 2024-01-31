using System;
using CmlLib.Core.Installer.Forge;
using CmlLib;
using System.Threading;
using System.Windows.Forms;
using CmlLib.Core.Auth;
using CmlLib.Core;
using CmlLib.Core.Installer.Forge.Versions;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.IO;
using Aspose.Zip;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace Launceher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        /*private void path()
        {
            var path = new MinecraftPath();
            var launcher = new CMLauncher(path);
            foreach (var item in launcher.GetAllVersions())
            {
                comboBox1.Items.Add(item.Name);
            }
        }*/

        private async void Launch()
        {
            DeSmall.Text = "Установка Forge и Minecraft...\n\rЭто может занять много времени...";//Download
            var Path = new MinecraftPath(@".\pseasons");
            var Launcher = new CMLauncher(Path);
            var versionLoader = new ForgeVersionLoader(new HttpClient());
            var versions = await versionLoader.GetForgeVersions("1.20.1");
            var recommendedVersion = versions.First(v => v.IsRecommendedVersion);
            if (!Directory.Exists(@".\pseasons\mods"))
            {
                Directory.CreateDirectory(@".\pseasons\mods");
            }

            var forge = new MForge(Launcher);
            await forge.Install(recommendedVersion.MinecraftVersionName, recommendedVersion.ForgeVersionName);
            var versionName = await forge.Install("1.20.1");
            DeSmall.Text = "Подготовка модов..."; //LoadMods

            string[] files = Directory.GetFiles(@".\pseasons\mods");
            int fileCount = files.Length;
            Console.WriteLine("Количество файлов: " + fileCount);

            if (fileCount < 19 || fileCount > 19)
            {
                if (Directory.Exists(@".\pseasons\mods"))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(@".\pseasons\mods");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.CreateDirectory(@".\pseasons\mods");
                }

                WebClient client = new WebClient();
                client.DownloadFile("https://projectseasons.ru/mods_comfort.zip", @".\pseasons\mods\mods_comfort.zip");
                
                
                string zipPath = @".\pseasons\mods\mods_comfort.zip";
                string extractPath = @".\pseasons\mods";

                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
                File.Delete(zipPath);
            }
            

            DeSmall.Text = "Запуск...";//Start
            var LaunchOptions = new MLaunchOption
            {
                MaximumRamMb = 2048,
                Session = MSession.CreateOfflineSession(Login.Text),
            };
            var Process = Launcher.CreateProcess(versionName, LaunchOptions);
            Process.Start();
            Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.Text != "")
            {
                string pather = @".\nick.pseasons";
                
                File.Create(pather).Close();
                File.WriteAllText(pather, Login.Text);

                DeSmall.Visible = true;
                progressBar1.Visible = true;
                button1.Enabled = false;
                Login.Enabled = false;
                comboBox1.Enabled = false;

                Thread thread = new Thread(() => Launch());
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                MessageBox.Show("Вы не ввели ник!", Name = "Ошибка!");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = @".\nick.pseasons";
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
            {
                Login.Text = File.ReadAllText(@".\nick.pseasons");
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void settings_Click(object sender, EventArgs e)
        {
            
        }
    }
}
