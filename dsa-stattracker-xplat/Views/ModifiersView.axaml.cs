using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using dsa_battle_tracker.ViewModels;

namespace dsa_battle_tracker.Views;

public partial class ModifiersView : UserControl
{
    public ModifiersView()
    {
        this.DataContext = new ModifiersViewModel();
        InitializeComponent();
    }
}