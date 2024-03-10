global using Microsoft.Extensions.Logging;

global using System.Diagnostics;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Text.Json;
global using System.Text;

global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Views;
global using CommunityToolkit.Maui.Storage;
global using CommunityToolkit.Maui.Core.Primitives;

global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;

global using MauiMusicPlayer.ViewModels;
global using MauiMusicPlayer.Views;
global using MauiMusicPlayer.Models;
global using MauiMusicPlayer.Common;

global using MauiMusicPlayer.Services.AudioService;
global using MauiMusicPlayer.Services.JSONService;
global using MauiMusicPlayer.Services.Repository;
global using MauiMusicPlayer.Services.MessageService;

namespace MauiMusicPlayer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Shell
        builder.Services.AddSingleton<AppShell>();

        // Views & ViewModels
        builder.Services.AddSingleton<HomeView>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<LibraryView>();
        builder.Services.AddSingleton<LibraryViewModel>();

        builder.Services.AddSingleton<PlaylistsView>();
        builder.Services.AddSingleton<PlaylistsViewModel>();

        builder.Services.AddSingleton<SettingsView>();
        builder.Services.AddSingleton<SettingsViewModel>();

        builder.Services.AddSingleton<EditorView>();
        builder.Services.AddSingleton<EditorViewModel>();

        // Services
        builder.Services.AddSingleton<IAudioService, AudioService>();
        builder.Services.AddTransient<IJSONService, JSONService>();
        builder.Services.AddTransient<IMessageService, MessageService>();

        builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();
        builder.Services.AddTransient<IPlaylistsRepository, PlaylistsRepository>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        Initialize();

        return builder.Build();
    }

    private static void Initialize()
    {
        Directory.CreateDirectory(AppFolder.MainPath);
        Directory.CreateDirectory(AppFolder.DataPath);
        Directory.CreateDirectory(AppFolder.PlaylistsPath);
    }
}
