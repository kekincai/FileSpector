using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSpector.Models;

namespace FileSpector.Services;

/// <summary>
/// Service for analyzing folder structure and file composition
/// </summary>
public class FileAnalyzerService
{
    private readonly List<FileCategory> _categories;

    public FileAnalyzerService()
    {
        _categories = FileCategory.DefaultCategories;
    }

    /// <summary>
    /// Scans a directory and returns a tree structure of files
    /// </summary>
    public async Task<FileNode> ScanDirectoryAsync(string path, IProgress<(int current, int total, string currentFile)>? progress = null)
    {
        return await Task.Run(() => ScanDirectory(path, progress));
    }

    private FileNode ScanDirectory(string path, IProgress<(int current, int total, string currentFile)>? progress)
    {
        var dirInfo = new DirectoryInfo(path);
        var node = new FileNode
        {
            Name = dirInfo.Name,
            FullPath = dirInfo.FullName,
            IsDirectory = true,
            LastModified = dirInfo.LastWriteTime
        };

        try
        {
            // Scan files
            foreach (var file in dirInfo.EnumerateFiles())
            {
                try
                {
                    var fileNode = new FileNode
                    {
                        Name = file.Name,
                        FullPath = file.FullName,
                        IsDirectory = false,
                        Size = file.Length,
                        LastModified = file.LastWriteTime,
                        Extension = file.Extension.ToLowerInvariant(),
                        Category = GetCategory(file.Extension)
                    };
                    node.Children.Add(fileNode);
                    progress?.Report((0, 0, file.Name));
                }
                catch (UnauthorizedAccessException)
                {
                    // Skip files we can't access
                }
            }

            // Scan subdirectories
            foreach (var dir in dirInfo.EnumerateDirectories())
            {
                try
                {
                    var childNode = ScanDirectory(dir.FullName, progress);
                    node.Children.Add(childNode);
                }
                catch (UnauthorizedAccessException)
                {
                    // Skip directories we can't access
                }
            }

            // Calculate directory size
            node.Size = CalculateSize(node);
        }
        catch (UnauthorizedAccessException)
        {
            // Skip if we can't access the directory
        }

        return node;
    }

    private FileCategory GetCategory(string extension)
    {
        extension = extension.ToLowerInvariant();
        foreach (var category in _categories)
        {
            if (category.Extensions.Contains(extension))
            {
                return category;
            }
        }
        return _categories.Last(); // "Others" category
    }

    private static long CalculateSize(FileNode node)
    {
        if (!node.IsDirectory)
            return node.Size;

        long total = 0;
        foreach (var child in node.Children)
        {
            total += child.IsDirectory ? CalculateSize(child) : child.Size;
        }
        return total;
    }

    /// <summary>
    /// Analyzes files and returns category statistics
    /// </summary>
    public List<FileCategory> AnalyzeCategories(FileNode rootNode)
    {
        var stats = _categories.Select(c => new FileCategory
        {
            Name = c.Name,
            Icon = c.Icon,
            Color = c.Color,
            Extensions = c.Extensions,
            TotalSize = 0,
            FileCount = 0
        }).ToList();

        CollectStats(rootNode, stats);
        return stats.Where(s => s.FileCount > 0 || s.Name == "Others").ToList();
    }

    private void CollectStats(FileNode node, List<FileCategory> stats)
    {
        if (!node.IsDirectory)
        {
            var category = stats.FirstOrDefault(c =>
                c.Extensions.Contains(node.Extension)) ?? stats.Last();
            category.TotalSize += node.Size;
            category.FileCount++;
        }
        else
        {
            foreach (var child in node.Children)
            {
                CollectStats(child, stats);
            }
        }
    }

    /// <summary>
    /// Gets all files sorted by size (largest first)
    /// </summary>
    public List<FileNode> GetFilesOrderedBySize(FileNode rootNode, int limit = 100)
    {
        var files = new List<FileNode>();
        CollectFiles(rootNode, files);
        return files.OrderByDescending(f => f.Size).Take(limit).ToList();
    }

    private void CollectFiles(FileNode node, List<FileNode> files)
    {
        if (!node.IsDirectory)
        {
            files.Add(node);
        }
        else
        {
            foreach (var child in node.Children)
            {
                CollectFiles(child, files);
            }
        }
    }
}
