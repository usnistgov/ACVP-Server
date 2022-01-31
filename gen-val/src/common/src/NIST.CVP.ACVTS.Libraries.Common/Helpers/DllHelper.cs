using System;
using System.Runtime.InteropServices;

namespace NIST.CVP.ACVTS.Libraries.Common.Helpers
{
    public static class DllHelper
    {
        private static IntPtr BytesToIntPtr(byte[] bytes)
        {
            return Marshal.AllocCoTaskMem(bytes.Length);
        }

        private static byte[] IntPtrToBytes(IntPtr ptr, int sizeOfBytes)
        {
            var result = new byte[sizeOfBytes];
            Marshal.Copy(ptr, result, 0, result.Length);
            Marshal.FreeCoTaskMem(ptr);
            return result;
        }

        public static byte[] PickOS(Action<IntPtr> osxAction, Action<IntPtr> winAction, Action<IntPtr> linuxAction, int resultByteSize)
        {
            var result = new byte[resultByteSize];
            var result_ptr = BytesToIntPtr(result);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                osxAction(result_ptr);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                winAction(result_ptr);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                linuxAction(result_ptr);
            }
            else
            {
                throw new Exception("No dll available for OS");
            }
            result = IntPtrToBytes(result_ptr, result.Length);
            return result;
        }

        public static void PickOS(Action osxAction, Action winAction, Action linuxAction = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                osxAction();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                winAction();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                linuxAction();
            }
            else
            {
                throw new Exception("No dll available for OS");
            }
        }
    }
}
