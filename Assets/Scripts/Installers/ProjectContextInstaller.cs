using Services;
using Zenject;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindEventService();
        BindUserDataService();
    }

    private void BindEventService()
    {
        Container.Bind<EventService>().AsSingle().NonLazy();
    }

    private void BindUserDataService()
    {
        Container.Bind<UserDataService>().AsSingle().NonLazy();
    }
}