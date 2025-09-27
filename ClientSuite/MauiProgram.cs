using ClientSuite.Data.Core;
using ClientSuite.Data.Repositories;
using ClientSuite.Data.Services;
using ClientSuite.Model.Contracts.Core;
using ClientSuite.Model.Contracts.Repositories;
using ClientSuite.Model.Contracts.Services;
using ClientSuite.View.Pages;
using ClientSuite.View.Popups;
using ClientSuite.ViewModel.Pages;
using ClientSuite.ViewModel.Popups;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ClientSuite
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterRepositories()
                .RegisterServices()
                .RegisterViewModels()
                .RegisterPopupViewModels()
                .RegisterViews()
                .RegisterPopupViews()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        public static MauiAppBuilder RegisterRepositories(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<IClientRepository, ClientRepository>();
            mauiAppBuilder.Services.AddTransient<IClientAddressRepository, ClientAddressRepository>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
            mauiAppBuilder.Services.AddSingleton<ISqliteProvider, SqliteProvider>();
            mauiAppBuilder.Services.AddTransient<IClientsService, ClientsService>();
            mauiAppBuilder.Services.AddHttpClient<IIbgeService, IbgeService>();
            mauiAppBuilder.Services.AddHttpClient<ICepService, CepService>();
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<SplashViewModel>();
            mauiAppBuilder.Services.AddSingleton<HomeViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterPopupViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<ClientEditPopupViewModel>();
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<SplashPage>();
            mauiAppBuilder.Services.AddSingleton<HomePage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterPopupViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<ClientEditPopup>();
            return mauiAppBuilder;
        }

    }
}
