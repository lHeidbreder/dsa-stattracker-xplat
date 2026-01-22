using CommunityToolkit.Mvvm.ComponentModel;

namespace dsa_battle_tracker.Models;

public class DSAPlayer : ObservableObject
{
    public string Name { get; set; }
    public int _einsen = 0;
    public int Einsen
    {
        get => _einsen;
        set { SetProperty(ref _einsen, value); }
    }
    public int _zwanzigen = 0;
    public int Zwanzigen
    {
        get => _zwanzigen;
        set { SetProperty(ref _zwanzigen, value); }
    }

    public DSAPlayer(string Name = "Neuer Spieler")
    {
        this.Name = Name;
    }
}
