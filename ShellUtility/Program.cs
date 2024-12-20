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

