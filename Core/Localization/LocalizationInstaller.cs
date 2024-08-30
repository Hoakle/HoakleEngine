using HoakleEngine.Core.Localization;
using Zenject;

namespace HoakleEngine
{
    public class LocalizationInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<LocalizationProvider>().AsSingle();
        }
    }
}
