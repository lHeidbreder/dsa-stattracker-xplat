using System;
using System.IO;
using System.Text.Json.Serialization;

namespace dsa_battle_tracker.Models;

public class Config
{
    private static Lazy<Config> _instance = new(() => Load());
    public static Config Instance { get; } = _instance.Value;

    #region Paths
    public static string AppDataDirPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/DSAStattracker/";
    public static string ConfigFilePath => AppDataDirPath + "config.json";
    public string CharSaveLoadStartpath { set; get; } = AppDataDirPath;
    public string CharListSavePath { get; set; } = AppDataDirPath;
    public string PlayerListSavePath { get; set; } = AppDataDirPath + "dsastattrack-playerlist.json";
    #endregion

    [JsonConstructor]
    private Config()
    {
        Directory.CreateDirectory(AppDataDirPath);
        Directory.CreateDirectory(CharSaveLoadStartpath);

        Save();
    }

    public void Save()
    {
        var o = System.Text.Json.JsonSerializer.Serialize(this);
        File.WriteAllText(ConfigFilePath, o);
    }

    private static Config Load()
    {
        if (!File.Exists(ConfigFilePath))
            return new();

        return System.Text.Json.JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFilePath)) ?? throw new FileLoadException();
    }
}