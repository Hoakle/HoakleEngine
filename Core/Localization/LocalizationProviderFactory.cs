using Zenject;

namespace HoakleEngine.Core.Localization
{
    public class LocalizationProviderFactory : IFactory<LocalizationDataBase, ILocalizationProvider>
    {
        private DiContainer parentContainer;

        public LocalizationProviderFactory(DiContainer parentContainer)
        {
            this.parentContainer = parentContainer;
        }
        
        public ILocalizationProvider Create(LocalizationDataBase dataBase)
        {
            var localizationContainer = parentContainer.CreateSubContainer();
            localizationContainer.Install<LocalizationInstaller>();
            localizationContainer.BindInstance(dataBase).AsSingle();
            return localizationContainer.Instantiate<LocalizationProvider>();
        }
    }
}
