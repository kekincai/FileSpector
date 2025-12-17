# FileSpector

一个基于 Avalonia UI 的跨平台文件分析工具，帮助您可视化和分析文件夹结构、文件大小分布和存储使用情况。

## 功能特性

- 📁 **文件夹扫描**: 快速扫描指定文件夹，分析文件结构
- 📊 **文件分类统计**: 按文件类型自动分类并统计大小和数量
- 📈 **大文件识别**: 快速找出占用空间最大的文件
- 🎨 **现代化界面**: 支持深色/浅色主题切换
- 🔍 **文件选择**: 支持多选文件并查看总大小
- ⚡ **高性能**: 异步扫描，不阻塞界面操作
- 🌐 **跨平台**: 支持 Windows、macOS 和 Linux

## 技术栈

- **.NET 10.0**: 最新的 .NET 框架
- **Avalonia UI 11.3.9**: 跨平台 UI 框架
- **CommunityToolkit.Mvvm**: MVVM 模式支持
- **C# 12**: 现代 C# 语言特性

## 系统要求

- .NET 10.0 Runtime 或更高版本
- 支持的操作系统：
  - Windows 10/11
  - macOS 10.15 或更高版本
  - Linux (各主流发行版)

## 快速开始

### 1. 克隆项目

```bash
git clone <repository-url>
cd FileSpector
```

### 2. 安装 .NET SDK

确保您已安装 .NET 10.0 SDK。如果没有，请从 [Microsoft 官网](https://dotnet.microsoft.com/download) 下载安装。

验证安装：
```bash
dotnet --version
```

### 3. 还原依赖包

```bash
cd FileSpector
dotnet restore
```

### 4. 编译项目

```bash
dotnet build
```

### 5. 运行应用程序

```bash
dotnet run
```

或者使用发布版本：

```bash
# 发布为单文件可执行程序
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# 运行发布的程序
./bin/Release/net10.0/win-x64/publish/FileSpector.exe
```

## 使用说明

1. **选择文件夹**: 点击"选择文件夹"按钮，选择要分析的目录
2. **查看分析结果**: 
   - 左侧面板显示文件类型分类统计
   - 右侧面板显示最大的文件列表
3. **文件选择**: 点击文件可以选中/取消选中，底部显示选中文件的总大小
4. **主题切换**: 点击主题按钮可以在深色/浅色主题间切换

## 项目结构

```
FileSpector/
├── Assets/                 # 资源文件
├── Converters/            # 数据转换器
├── Models/                # 数据模型
│   ├── FileCategory.cs    # 文件分类模型
│   └── FileNode.cs        # 文件节点模型
├── Services/              # 业务服务
│   └── FileAnalyzerService.cs  # 文件分析服务
├── ViewModels/            # 视图模型
│   ├── MainWindowViewModel.cs  # 主窗口视图模型
│   └── ViewModelBase.cs   # 视图模型基类
├── Views/                 # 视图
│   ├── MainWindow.axaml   # 主窗口视图
│   └── MainWindow.axaml.cs
├── App.axaml             # 应用程序样式
├── App.axaml.cs          # 应用程序入口
├── Program.cs            # 程序主入口
└── FileSpector.csproj    # 项目配置文件
```

## 开发指南

### 调试模式运行

```bash
dotnet run --configuration Debug
```

### 发布不同平台版本

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained true

# macOS x64
dotnet publish -c Release -r osx-x64 --self-contained true

# macOS ARM64 (Apple Silicon)
dotnet publish -c Release -r osx-arm64 --self-contained true

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained true
```

### 代码规范

- 使用 C# 命名约定
- 遵循 MVVM 模式
- 使用 async/await 进行异步操作
- 添加适当的异常处理

## 贡献指南

1. Fork 本项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 更新日志

### v1.0.0
- 初始版本发布
- 基本文件夹扫描功能
- 文件分类统计
- 大文件识别
- 主题切换支持

## 问题反馈

如果您遇到任何问题或有功能建议，请在 [Issues](../../issues) 页面提交。

## 致谢

- [Avalonia UI](https://avaloniaui.net/) - 优秀的跨平台 UI 框架
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) - MVVM 工具包