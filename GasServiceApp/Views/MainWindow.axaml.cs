using Avalonia.Controls;
using GasServiceApp.Services;
using GasServiceApp.ViewModels;

namespace GasServiceApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new GasService(new GasDbContext()));
    }
}
