using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FileSpector.ViewModels;

namespace FileSpector.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Connect folder picker to ViewModel
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FolderPickerAsync = async () =>
            {
                var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Select Folder to Analyze",
                    AllowMultiple = false
                });
                return folders.Count > 0 ? folders[0] : null;
            };
        }

        // Handle DataContext changes
        DataContextChanged += (s, e) =>
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.FolderPickerAsync = async () =>
                {
                    var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        Title = "Select Folder to Analyze",
                        AllowMultiple = false
                    });
                    return folders.Count > 0 ? folders[0] : null;
                };
            }
        };
    }
}