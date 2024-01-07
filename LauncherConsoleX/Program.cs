using CmlLib.Core.Installer.Forge;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installer.Forge.Versions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace LauncherX
{
    class ProgrammX
    {
        static async Task Main()
        {
            var path = new MinecraftPath();
            var versionLoader = new ForgeVersionLoader(new HttpClient());
            var versions = await versionLoader.GetForgeVersions("1.20.1");
            var recommendedVersion = versions.First(v => v.IsRecommendedVersion);
            var launcher = new CMLauncher(path);
                
            var forge = new MForge(launcher);
            // ~~~ event handlers ~~~
            await forge.Install(recommendedVersion.MinecraftVersionName, recommendedVersion.ForgeVersionName);
            // ~~~ launch codes ~~~
            // Install the best forge for specific minecraft version
            var versionName = await forge.Install("1.20.1");

            // Install with specific forge version
            // var versionName = await forge.Install("1.20.1", "47.0.35");

            //Start Minecraft
            var launchOption = new MLaunchOption
            {
                MaximumRamMb = 1024,
                Session = MSession.GetOfflineSession("TaiogStudio"),
                ServerIp = "mc.projectseasons.ru",
            };

            var process = await launcher.CreateProcessAsync(versionName, launchOption);
            process.Start();
        }
    }
}
