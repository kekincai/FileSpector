using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FileSpector.Models;
using FileSpector.Services;

namespace FileSpector.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly FileAnalyzerService _analyzerService;

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private string _currentScanFile = string.Empty;

    [ObservableProperty]
    private string _statusMessage = "Select a folder to analyze";

    [ObservableProperty]
    private FileNode? _rootNode;

    [ObservableProperty]
    private FileNode? _selectedFolder;

    [ObservableProperty]
    private bool _isDarkTheme = true;

    [ObservableProperty]
    private long _totalSelectedSize;

    [ObservableProperty]
    private int _selectedFileCount;

    public string TotalSelectedSizeFormatted => FormatSize(TotalSelectedSize);

    public ObservableCollection<FileCategory> Categories { get; } = [];
    public ObservableCollection<FileNode> LargestFiles { get; } = [];
    public ObservableCollection<FileNode> SelectedFiles { get; } = [];

    // For folder picker
    public Func<Task<IStorageFolder?>>? FolderPickerAsync { get; set; }

    public MainWindowViewModel()
    {
        _analyzerService = new FileAnalyzerService();
    }

    [RelayCommand]
    private async Task SelectFolderAsync()
    {
        if (FolderPickerAsync == null) return;

        var folder = await FolderPickerAsync();
        if (folder == null) return;

        await ScanFolderAsync(folder.Path.LocalPath);
    }

    private async Task ScanFolderAsync(string path)
    {
        IsScanning = true;
        StatusMessage = "Scanning...";
        Categories.Clear();
        LargestFiles.Clear();
        SelectedFiles.Clear();
        TotalSelectedSize = 0;
        SelectedFileCount = 0;

        try
        {
            var progress = new Progress<(int current, int total, string currentFile)>(p =>
            {
                CurrentScanFile = p.currentFile;
            });

            RootNode = await _analyzerService.ScanDirectoryAsync(path, progress);
            SelectedFolder = RootNode;

            // Update categories
            var categories = _analyzerService.AnalyzeCategories(RootNode);
            foreach (var cat in categories.OrderByDescending(c => c.TotalSize))
            {
                Categories.Add(cat);
            }

            // Update largest files
            var largestFiles = _analyzerService.GetFilesOrderedBySize(RootNode, 50);
            foreach (var file in largestFiles)
            {
                LargestFiles.Add(file);
            }

            StatusMessage = $"Analyzed {RootNode.Name} - {RootNode.SizeFormatted}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
            CurrentScanFile = string.Empty;
        }
    }

    [RelayCommand]
    private void ToggleFileSelection(FileNode file)
    {
        file.IsSelected = !file.IsSelected;

        if (file.IsSelected)
        {
            if (!SelectedFiles.Contains(file))
            {
                SelectedFiles.Add(file);
                TotalSelectedSize += file.Size;
                SelectedFileCount++;
            }
        }
        else
        {
            SelectedFiles.Remove(file);
            TotalSelectedSize -= file.Size;
            SelectedFileCount--;
        }
    }

    [RelayCommand]
    private void ClearSelection()
    {
        foreach (var file in SelectedFiles.ToList())
        {
            file.IsSelected = false;
        }
        SelectedFiles.Clear();
        TotalSelectedSize = 0;
        SelectedFileCount = 0;
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
    }

    public string FormatSize(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        int order = 0;
        double size = bytes;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return $"{size:0.##} {sizes[order]}";
    }
}
