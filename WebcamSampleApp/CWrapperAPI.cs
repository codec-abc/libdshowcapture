using System;
using System.Runtime.InteropServices;

namespace WebcamSampleApp
{
    public static class CWrapperAPI
    {
        private const string basePath = @"..\..\..\..\build\";

#if DEBUG && x86
        private const string dllPath = basePath + @"Win32\Debug\CWrapper.dll";
#elif DEBUG
        private const string dllPath = basePath + @"x64\Debug\CWrapper.dll";
#elif x86
        private const string dllPath = basePath + @"Win32\Release\CWrapper.dll";
#else
        private const string dllPath = basePath + @"x64\Release\CWrapper.dll";
#endif

        [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void startCapture(IntPtr message, ref IntPtr arraySize, ref IntPtr arrayPtr);

        [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte getDevices(ref IntPtr arraySize, ref IntPtr arrayPtr);

        [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void freeByteArray(IntPtr array);

        [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void shutDownAndFreeDevice(IntPtr devicePtr);

    }
}
