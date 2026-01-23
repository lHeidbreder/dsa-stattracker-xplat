using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace dsa_battle_tracker;

public class Msg {

    public static async Task<bool> OverwriteWarning(ContentControl source, string path)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            new MessageBoxStandardParams
            {
                ContentTitle = "Datei existiert",
                ContentMessage = $"Die Datei \"{path}\" existiert bereits.\nÜberschreiben?",
                ButtonDefinitions = ButtonEnum.YesNo,
                Icon = Icon.Warning,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });

        var result = await box.ShowAsPopupAsync(source);

        return result == ButtonResult.Yes;
    }

    public static async Task NoSaveData(ContentControl source)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(
            new MessageBoxStandardParams
            {
                ContentTitle = "Keine Daten",
                ContentMessage = $"Es existieren für diese Option keine gespeicherten Daten!",
                Icon = Icon.Warning,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });

        await box.ShowAsPopupAsync(source);
    }

    public static async Task ErrorNotice(System.Exception e, string? customMsg = null)
    {
        string msg = customMsg ?? $"<{e.ToString()}> wurde ausgelöst.";
        if (App.MainWindow is null)
        {
            Console.Error.Write("Kritisch: Fehlermeldung kann nicht geöffnet werden!");
            Console.Error.Write($"Meldung war: {msg}");
            return;
        }
        
        var box = MessageBoxManager.GetMessageBoxStandard(
            new MessageBoxStandardParams
            {
                ContentTitle = $"Fehler: {e.ToString()}",
                ContentMessage = msg,
                Icon = Icon.Warning,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }
        );
        await box.ShowAsPopupAsync(App.MainWindow);
    }
}