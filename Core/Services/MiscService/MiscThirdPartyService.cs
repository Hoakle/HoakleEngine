namespace HoakleEngine.Core.Services.MiscService
{
    public class MiscThirdPartyService : ThirdPartyService
    {
        private MiscThirdPartyActor _Actor;

        public override void Initialize()
        {
            _Actor = new AndroidMiscTPA();
            _Actor.Init();
        }

        public void OpenEmail()
        {
            _Actor.OpenEmail();
        }
    }
}
