using System;
using CmlLib.Core.Installer.Forge;
using System.Threading;
using System.Windows.Forms;
using CmlLib.Core.Auth;
using CmlLib.Core;
using CmlLib.Core.Installer.Forge.Versions;
using System.Net.Http;
using System.Net;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace Launceher
{
    public partial class Form1 : Form
    {
        public string process = "javaw";
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

        public async void Launch()
        {
            bool Discord = true;

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
            if (comboBox1.Text == "Комфорт")
            {
                if (fileCount < 19 || fileCount > 19)
                {
                    if (Directory.Exists(@".\pseasons\mods"))
                    {
                        DirectoryInfo di = new DirectoryInfo(@".\pseasons\mods");

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
            }
            else if (comboBox1.Text == "Оптимизация")
            {
                if (fileCount < 24 || fileCount > 24)
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
                    client.DownloadFile("https://projectseasons.ru/optimized.zip", @".\pseasons\mods\optimized.zip");


                    string zipPath = @".\pseasons\mods\optimized.zip";
                    string extractPath = @".\pseasons\mods";

                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
                    File.Delete(zipPath);
                }
            }
            else
            {
                MessageBox.Show("Ошибка. Вы не выбрали сборку.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            
            

            DeSmall.Text = "Запуск...";//Start
            var LaunchOptions = new MLaunchOption
            {
                MaximumRamMb = int.Parse(textBox1.Text),
                Session = MSession.CreateOfflineSession(Login.Text),
            };
            var Process = Launcher.CreateProcess(versionName, LaunchOptions);
            Process.Start();
            if (Discord == false)
            {
                Close();
            }
            else
            {
                Hide();
                Thread.Sleep(10000);
                Process[] processes = Process.GetProcessesByName(process);

                if (processes.Length == 0)
                {
                    return;
                }

                DiscordRRPC discordRRPC = new DiscordRRPC();
                discordRRPC.Setup();

                // Создаем новый поток для отслеживания процесса Minecraft
                Thread monitoringThread = new Thread(() =>
                {
                    while (true)
                    {
                        processes = Process.GetProcessesByName(process);
                        if (processes.Length == 0)
                        {
                            Environment.Exit(0);
                        }
                        Thread.Sleep(1000); // Пауза в 1 секунду
                    }
                });

                // Запускаем поток отслеживания
                monitoringThread.Start();

                // Ожидаем завершения потока отслеживания
                monitoringThread.Join();
            }
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
            string pathRAM = @".\RAM.pseasons";
            if (File.Exists(path))
            {
                Login.Text = File.ReadAllText(path);
            }
            if (File.Exists(pathRAM))
            {
                textBox1.Text = File.ReadAllText(pathRAM);
            }
            comboBox1.SelectedIndex = 1;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void settings_Click(object sender, EventArgs e)
        {
            panel1.Location = new Point(375, 0);
        }

        private void close_settings_Click(object sender, EventArgs e)
        {
            panel1.Location = new Point(796, 0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                button1.BackColor = Color.LimeGreen;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                button1.BackColor = Color.Turquoise;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = @".\RAM.pseasons";
            File.Create(path).Close();
            File.WriteAllText(path, textBox1.Text);
            
        }
    }
}
