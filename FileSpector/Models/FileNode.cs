using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace FileSpector.Models;

/// <summary>
/// Represents a file or folder node in the tree
/// </summary>
public partial class FileNode : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _fullPath = string.Empty;

    [ObservableProperty]
    private bool _isDirectory;

    [ObservableProperty]
    private long _size;

    [ObservableProperty]
    private DateTime _lastModified;

    [ObservableProperty]
    private string _extension = string.Empty;

    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private FileCategory? _category;

    public ObservableCollection<FileNode> Children { get; } = [];

    public string SizeFormatted => FormatSize(Size);

    public string Icon => IsDirectory ? "📁" : (Category?.Icon ?? "📄");

    public string CategoryColor => Category?.Color ?? "#607D8B";

    private static string FormatSize(long bytes)
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
