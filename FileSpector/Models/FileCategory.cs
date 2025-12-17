using System.Collections.Generic;

namespace FileSpector.Models;

/// <summary>
/// Represents a file category with its associated extensions and icon
/// </summary>
public class FileCategory
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "❓";
    public string Color { get; set; } = "#808080";
    public string[] Extensions { get; set; } = [];
    public long TotalSize { get; set; }
    public int FileCount { get; set; }

    public string TotalSizeFormatted => FormatSize(TotalSize);

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

    public static List<FileCategory> DefaultCategories =>
    [
        new() { Name = "Images", Icon = "📷", Color = "#E91E63", Extensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg", ".bmp", ".ico", ".tiff", ".raw", ".heic"] },
        new() { Name = "Videos", Icon = "🎬", Color = "#9C27B0", Extensions = [".mp4", ".mov", ".avi", ".mkv", ".webm", ".wmv", ".flv", ".m4v"] },
        new() { Name = "Audio", Icon = "🎵", Color = "#673AB7", Extensions = [".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a"] },
        new() { Name = "Documents", Icon = "📄", Color = "#2196F3", Extensions = [".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".rtf", ".odt"] },
        new() { Name = "Archives", Icon = "📦", Color = "#FF9800", Extensions = [".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz", ".dmg", ".iso"] },
        new() { Name = "Code", Icon = "💻", Color = "#4CAF50", Extensions = [".cs", ".js", ".ts", ".py", ".java", ".html", ".css", ".json", ".xml", ".yaml", ".md", ".sh", ".sql"] },
        new() { Name = "Others", Icon = "❓", Color = "#607D8B", Extensions = [] }
    ];
}
