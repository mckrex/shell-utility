
namespace ShellUtility
{
    public static class InfoCopier
    {
        public static int CopyInfo(string path, string action)
        {
            if (action == "path")
            {
                TextCopy.ClipboardService.SetText(path);
                return 0;
            }

            if (action == "unix")
            {
                var drive = Path.GetPathRoot(path);
                var relPath = Path.GetFullPath(path).Replace(drive, string.Empty).Replace('\\', '/');
                TextCopy.ClipboardService.SetText($"/{drive[0]}/{relPath}");
                return 0;
            }

            var file = new FileInfo(path);

            if (!file.Exists) { return 1; }

            TextCopy.ClipboardService.SetText(file.Name);
            return 0;

        }

    }
}
