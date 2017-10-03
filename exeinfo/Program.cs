using System;
using System.IO;
using System.Runtime.InteropServices;

namespace exeinfo
{
    class MainClass
    {
        static MZ.Header mzHdr;

        public static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("exeinfo version 0.1 © 2017 Natalia Portillo");
                Console.WriteLine("Usage: exeinfo file.exe");
                return;
            }

            FileStream exeFs = File.Open(args[0], FileMode.Open, FileAccess.Read);

            bool recognized = false;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(MZ.Header))];

            exeFs.Read(buffer, 0, buffer.Length);
			IntPtr hdrPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, hdrPtr, buffer.Length);
			mzHdr = (MZ.Header)Marshal.PtrToStructure(hdrPtr, typeof(MZ.Header));
			Marshal.FreeHGlobal(hdrPtr);

            if(mzHdr.signature == MZ.Signature)
            {
                recognized = true;
                MZ.PrintInfo(mzHdr);
            }

            if (!recognized)
                Console.WriteLine("Executalbe format not recognized");
		}
    }
}
