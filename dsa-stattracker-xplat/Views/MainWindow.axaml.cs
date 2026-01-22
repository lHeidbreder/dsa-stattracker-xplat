using System.Threading.Tasks;
using Avalonia.Controls;
using dsa_battle_tracker.Models;

namespace dsa_battle_tracker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        Config.Instance.Save();
    }
}