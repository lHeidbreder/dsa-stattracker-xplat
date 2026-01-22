using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using dsa_battle_tracker.Models;

namespace dsa_battle_tracker.Views;

public partial class DesktopHostWindow : Window
{
    public DesktopHostWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        Config.Instance.Save();
    }
}
