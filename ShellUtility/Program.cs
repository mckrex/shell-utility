namespace ShellUtility
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(args[0])) { return 0; }

            var retVal = 0;
            var thread = new Thread(() => {
                retVal = InfoCopier.CopyInfo(args[0], args[1]);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return retVal;
        }
    }

}



/*
 
HKEY_CLASSES_ROOT\*\shell\MK Copy Content     C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" content
HKEY_CLASSES_ROOT\*\shell\MK Copy File Name   C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" name
HKEY_CLASSES_ROOT\*\shell\MK Copy Full Path   C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" path
HKEY_CLASSES_ROOT\*\shell\MK Copy Unix Path   C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" unix
 
HKEY_CLASSES_ROOT\Directory\shell\MK Copy Folder Path   C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" path
HKEY_CLASSES_ROOT\Directory\shell\MK Copy Unix Path     C:\_prj\ShellUtility\publish\ShellUtility.exe "%1" unix
 
*/
