using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShellUtility;
using System;

namespace ShellUtilityTests
{
    [TestClass]
    public class InfoCopierTests
    {
        [TestMethod]
        public void SetClipboard_UnixDirectoryPath_Copied()
        {
            var testDirectory = new DirectoryInfo("data");
            var retVal = InfoCopier.SetClipboard(testDirectory.FullName, CopyAction.UNIX);
            var clipboardText = TextCopy.ClipboardService.GetText();
            Assert.IsTrue(retVal == 0);
            Assert.IsTrue(clipboardText.StartsWith("/D/"));
            Assert.IsTrue(clipboardText.EndsWith("/data"));
        }
        [TestMethod]
        public void SetClipboard_UnixFilePath_Copied()
        {
            var testFile = new FileInfo(@"data\text_file.txt");
            var retVal = InfoCopier.SetClipboard(testFile.FullName, CopyAction.UNIX);
            var clipboardText = TextCopy.ClipboardService.GetText();
            Assert.IsTrue(retVal == 0);
            Assert.IsTrue(clipboardText.StartsWith("/D/"));
            Assert.IsTrue(clipboardText.EndsWith("/text_file.txt"));
        }
        [DataTestMethod]
        [DataRow(@"data\SS64.com - CMD_Index.htm")]
        [DataRow(@"data\SS64.com - CMD_Index.mhtml")]
        [DataRow(@"data\brave-logo-dark.4VG56075.svg")]
        [DataRow(@"data\encoding_test_file_us-ascii.txt")]
        [DataRow(@"data\encoding_test_file_utf-8.txt")]
        [DataRow(@"data\encoding_test_file_utf-16.txt")]
        [DataRow(@"data\encoding_test_file_utf-16BE.txt")]
        [DataRow(@"data\encoding_test_file_utf-32.txt")]
        [DataRow(@"data\encoding_test_file_utf-32BE.txt")]
        [DataRow(@"data\encoding_test_file_iso-8859-1.txt")]
        [DataRow(@"data\encoding_test_file_utf-7.txt")]
        [DataRow(@"data\encoding_test_file_utf-16_null.txt")]
        public void IsProbablyText_True(string filePath)
        {
            using (var byteReader = new BinaryReader(new FileInfo(filePath).OpenRead()))
            {
                var allBytes = byteReader.ReadBytes((int)byteReader.BaseStream.Length);
                var isText = InfoCopier.IsProbablyText(allBytes);
                Assert.IsTrue(isText);
            }
        }
        [DataTestMethod]
        [DataRow(@"data\banana-cheerer.gif")]
        [DataRow(@"data\banana-cheerer.webp")]
        [DataRow(@"data\Mona_Lisa.jpg")]
        [DataRow(@"data\SS64.com - CMD_Index.pdf")]
        public void IsProbablyText_False(string filePath)
        {
            using (var byteReader = new BinaryReader(new FileInfo(filePath).OpenRead()))
            {
                var allBytes = byteReader.ReadBytes((int)byteReader.BaseStream.Length);
                var isText = InfoCopier.IsProbablyText(allBytes);
                Assert.IsFalse(isText);
            }
        }
    }
}