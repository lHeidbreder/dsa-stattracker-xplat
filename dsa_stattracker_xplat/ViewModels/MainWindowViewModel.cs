namespace dsa_battle_tracker.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using dsa_battle_tracker.Models;

public partial class MainWindowViewModel : ViewModelBase
{
    Lazy<Window?> window = new(() => App.MainWindow);

    public string LoadButtonContent { get; } = "Charakter laden...";
    public string NewButtonContent { get; } = "Neuen Charakter eingeben";
    public string CounterTab { get; } = "Einsen/Zwanzigen";

    public async Task LoadCharacter()
    {
        if (window.Value is null)
            throw new Exception("Kein Fenster gefunden");
            //FIXME: show messagebox instead

        Uri _filepath = new("file://" + Config.Instance.CharSaveLoadStartpath);
        IStorageFolder? startpath = await App.MainWindow!.StorageProvider.TryGetFolderFromPathAsync(_filepath);
        var path = await window.Value.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                SuggestedStartLocation = startpath,
                FileTypeFilter = DSACharacter.SAVE_FILE_FILTER,
                AllowMultiple = true,
            }
        );

        if (path.Count <= 0)
            return;

        foreach (var p in path)
        {
            var c = DSACharacter.Load(p.Path.AbsolutePath);
            if (c is not null)
                Chars.Add(c);
        }
    }
    public async Task LoadList()
    {
        if (window.Value is null)
            throw new Exception("Kein Fenster gefunden");

        Uri _filepath = new("file://" + Config.Instance.CharListSavePath);
        IStorageFolder? startpath = await App.MainWindow!.StorageProvider.TryGetFolderFromPathAsync(_filepath);
        var path = await window.Value.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                SuggestedStartLocation = startpath,
                FileTypeFilter = DSACharacter.LIST_SAVE_FILE_FILTER,
                AllowMultiple = false,
            }
        );

        if (path.Count <= 0)
            return;

        var c = DSACharacter.LoadList(path[0].Path.AbsolutePath);
        if (c is not null)
            Chars = new(c);
    }
    public void NewCharacter()
    {
        Chars.Add(new());
    }
    public void RemoveChar(DSACharacter c)
    {
        Chars.Remove(c);
    }
    public void QuickSaveChar(DSACharacter c)
    {
        if (c.LoadPath is not null)
        {
            c.Save(c.LoadPath);
        }
        else
        {
            _ = SaveChar(c);
        }
    }
    public async Task SaveChar(DSACharacter c)
    {
        if (window.Value is null)
            throw new Exception("Kein Fenster gefunden");

        Uri _filepath = new("file://" + Config.Instance.CharSaveLoadStartpath);
        IStorageFolder? startpath = await App.MainWindow!.StorageProvider.TryGetFolderFromPathAsync(_filepath);
        var path = await window.Value.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                SuggestedStartLocation = startpath, //FIXME
                FileTypeChoices = DSACharacter.SAVE_FILE_FILTER,
                SuggestedFileName = c.Name + ".json", //FIXME: replace space with hyphen
                DefaultExtension = "json",
                ShowOverwritePrompt = false,
            }
        );

        if (path is null)
            return;

        if (File.Exists(path.Path.AbsolutePath)
            && !await Msg.OverwriteWarning(window.Value, path.Path.AbsolutePath))
            return;

        c.Save(path.Path.AbsolutePath);
    }
    public async Task SaveList()
    {
        if (window.Value is null)
            throw new Exception("Kein Fenster gefunden");

        Uri _filepath = new("file://" + Config.Instance.CharListSavePath);
        IStorageFolder? startpath = await App.MainWindow!.StorageProvider.TryGetFolderFromPathAsync(_filepath);
        var path = await window.Value.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions
            {
                SuggestedStartLocation = startpath,
                FileTypeChoices = DSACharacter.LIST_SAVE_FILE_FILTER,
                SuggestedFileName = "Kampf",
                DefaultExtension = "json",
                ShowOverwritePrompt = false,
            }
        );

        if (path is null)
            return;

        if (File.Exists(path.Path.AbsolutePath)
            && !await Msg.OverwriteWarning(window.Value, path.Path.AbsolutePath))
            return;

        DSACharacter.Save(Chars, path.Path.AbsolutePath);
    }

    public ObservableCollection<DSACharacter> _chars = [];
    public ObservableCollection<DSACharacter> Chars
    {
        get { return _chars; }
        set { SetProperty(ref _chars, value); }
    }

    public void INISort()
    {
        Chars = new ObservableCollection<DSACharacter>(
            Chars
            .OrderByDescending(c => c.IniBase)
            .OrderByDescending(c => c.Ini)
        );
    }
    public void AlphaSort()
    {
        Chars = new ObservableCollection<DSACharacter>(
            Chars
            .OrderBy(c => c.Name)
        );
    }

    public ObservableCollection<DSAPlayer> _players = [];
    public ObservableCollection<DSAPlayer> Players
    {
        get => _players;
        set { SetProperty(ref _players, value); }
    }
    public void AddPlayer()
    {
        Players.Add(new());
    }
    public void SavePlayers()
    {
        var o = System.Text.Json.JsonSerializer.Serialize(_players);
        File.WriteAllText(Config.Instance.PlayerListSavePath, o);
    }
    public async Task LoadPlayers()
    {
        //FIXME: if not exists, show error toast, exit early
        if (!File.Exists(Config.Instance.PlayerListSavePath))
        {
            await Msg.NoSaveData(window.Value!);
            return;
        }

        var s = File.ReadAllText(Config.Instance.PlayerListSavePath);
        var c = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<DSAPlayer>>(s);
        if (c is not null)
            Players = c;
    }

    public string ModifiersTab { get; } = "Modifikatoren";
}
