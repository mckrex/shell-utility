using System.Diagnostics;

namespace ShellUtility
{
    public static class InfoCopier
    {
        private static readonly List<byte[]> _fileSignatures = new()
        {
            //Wikipedia File Signatures - https://en.wikipedia.org/wiki/List_of_file_signatures
            new byte[] { 0xEF, 0xBB, 0xBF },        //utf-8
            new byte[] { 0xFF, 0xFE },              //utf-16LE
            new byte[] { 0xFE, 0xFF },              //utf-16BE
            new byte[] { 0xFF, 0xFE, 0x00, 0x00 },  //utf-32LE
            new byte[] { 0x00, 0x00, 0xFE, 0xFF },  //utf-32BE
            new byte[] { 0x2B, 0x2F, 0x76, 0x38 },  //utf-7
            new byte[] { 0x2B, 0x2F, 0x76, 0x39 },  //utf-7
            new byte[] { 0x2B, 0x2F, 0x76, 0x2B },  //utf-7
            new byte[] { 0x2B, 0x2F, 0x76, 0x2F },  //utf-7
            new byte[] { 0x0E, 0xFE, 0xFF},         //scsu
            new byte[] { 0xDD, 0x73, 0x66, 0x73},   //utf-ebcdic

        };
        private static readonly int _headerReadLength = _fileSignatures.Max(m => m.Length);

        public static int CopyInfo(string path, string action, bool doBinaryCheck = false)
        {
            if (!Enum.TryParse<CopyAction>(action.ToUpper(), out var copyAction)) { return 1; };
            if (!File.Exists(path) && !Directory.Exists(path)) { return 1; }

            if (TryGetFileInfo(path, out FileInfo file) && copyAction == CopyAction.CONTENT) 
            { 
                return SetClipboardWithContent(file, copyAction, doBinaryCheck);
            }
            else if (copyAction != CopyAction.CONTENT) 
            { 
                return SetClipboard(path, copyAction);
            }
            else
            {
                return 1; 
            }
        }
        public static int SetClipboardWithContent(FileInfo file, CopyAction copyAction, bool doBinaryCheck)
        {
            try
            {
                using (var byteReader = new BinaryReader(file.OpenRead()))
                {
                    var allBytes = byteReader.ReadBytes((int)byteReader.BaseStream.Length);

                    if (doBinaryCheck && !IsProbablyText(allBytes))
                    {
                        TextCopy.ClipboardService.SetText(file.FullName);
                        return 1;
                    }
                    using (var reader = new StreamReader(new MemoryStream(allBytes)))
                    {
                        TextCopy.ClipboardService.SetText(reader.ReadToEnd());
                        return 0;
                    }
                }

            }
            catch (Exception ex)
            {
                TextCopy.ClipboardService.SetText($"{file.FullName}: {ex.Message}");
                return 1;
            }
        }

        public static bool IsProbablyText(byte[] allBytes)
        {
#if DEBUG
            var bytesAsDecimal = new string[100];
            var bytesAsHex = new string[100];
            for (int i = 0; i < 100; i++)
            {
                bytesAsDecimal[i] = $"{allBytes[i]}";
                bytesAsHex[i] = $"{(int)allBytes[i]:X2}";
            }
            Debug.WriteLine(string.Join('|', bytesAsDecimal));
            Debug.WriteLine(string.Join('|', bytesAsHex));
#endif
            if (_fileSignatures.Any(signature => allBytes.Take(signature.Length).SequenceEqual(signature)))
            {
                return true;
            }

            return !allBytes.Any(c => c == 0);
        }

        public static int SetClipboard(string path, CopyAction copyAction)
        {
            switch (copyAction)
            {
                case CopyAction.PATH:
                {
                    TextCopy.ClipboardService.SetText(path);
                    break;
                }
                case CopyAction.UNIX:
                {
                    var drive = Path.GetPathRoot(path);
                    var relPath = Path.GetRelativePath(drive, path).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    TextCopy.ClipboardService.SetText($"/{drive[0]}/{relPath}");
                    break;
                }
                case CopyAction.NONE:
                case CopyAction.NAME:
                default:
                {
                    TextCopy.ClipboardService.SetText(Path.GetFileName(path) ?? Path.GetDirectoryName(path));
                    break;
                }

            }
            return 0;

        }
        private static bool TryGetFileInfo(string path, out FileInfo file)
        {
            file = new FileInfo(path);
            return file.Exists;
        }


    }

    public enum CopyAction
    {
        NONE,
        PATH,
        UNIX,
        NAME,
        CONTENT
    }

}
