Native placeholder

Place C/C++ sources for native interop here. Build a shared library exposing the following symbol:

- `const char* native_get_version()` - returns a null-terminated version string.

On Linux you can build `libnative.so` and ship it as `native_lib` for the runtime to load.
