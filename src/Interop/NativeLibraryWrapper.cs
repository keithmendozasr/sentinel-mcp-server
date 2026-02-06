using System.Runtime.InteropServices;

namespace McpServer.Interop;

public static class NativeLibraryWrapper
{
    // Placeholder P/Invoke declarations. These should be implemented
    // in the native/ directory as a shared library (libnative.so / native.dll)

    [DllImport("native_lib", EntryPoint = "native_get_version", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr NativeGetVersion();

    public static string GetVersionString()
    {
        var ptr = NativeGetVersion();
        if (ptr == IntPtr.Zero) return string.Empty;
        return Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
    }
}
