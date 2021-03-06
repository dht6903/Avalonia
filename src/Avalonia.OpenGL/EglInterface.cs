﻿using System;
using Avalonia.Platform;
using Avalonia.Platform.Interop;

namespace Avalonia.OpenGL
{
    public class EglInterface : GlInterfaceBase
    {
        public EglInterface() : base(Load())
        {
            
        }
        
        public EglInterface(string library) : base(Load(library))
        {
        }

        static Func<string, bool, IntPtr> Load()
        {
            var os = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            if(os == OperatingSystemType.Linux || os == OperatingSystemType.Android)
                return Load("libEGL.so.1");
            if (os == OperatingSystemType.WinNT)
                return Load(@"libegl.dll");
            throw new PlatformNotSupportedException();
        }

        static Func<string, bool, IntPtr> Load(string library)
        {
            var dyn = AvaloniaLocator.Current.GetService<IDynamicLibraryLoader>();
            var lib = dyn.LoadLibrary(library);
            return (s, o) => dyn.GetProcAddress(lib, s, o);
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public delegate int EglGetError();
        [GlEntryPoint("eglGetError")]
        public EglGetError GetError { get; }

        public delegate IntPtr EglGetDisplay(IntPtr nativeDisplay);
        [GlEntryPoint("eglGetDisplay")]
        public EglGetDisplay GetDisplay { get; }
        
        public delegate IntPtr EglGetPlatformDisplayEXT(int platform, IntPtr nativeDisplay, int[] attrs);
        [GlEntryPoint("eglGetPlatformDisplayEXT", true)]
        public EglGetPlatformDisplayEXT GetPlatformDisplayEXT { get; }

        public delegate bool EglInitialize(IntPtr display, out int major, out int minor);
        [GlEntryPoint("eglInitialize")]
        public EglInitialize Initialize { get; }        
        
        public delegate IntPtr EglGetProcAddress(Utf8Buffer proc);
        [GlEntryPoint("eglGetProcAddress")]
        public EglGetProcAddress GetProcAddress { get; }

        public delegate bool EglBindApi(int api);
        [GlEntryPoint("eglBindAPI")]
        public EglBindApi BindApi { get; }

        public delegate bool EglChooseConfig(IntPtr display, int[] attribs,
            out IntPtr surfaceConfig, int numConfigs, out int choosenConfig);
        [GlEntryPoint("eglChooseConfig")]
        public EglChooseConfig ChooseConfig { get; }

        public delegate IntPtr EglCreateContext(IntPtr display, IntPtr config,
            IntPtr share, int[] attrs);
        [GlEntryPoint("eglCreateContext")]
        public EglCreateContext CreateContext { get; }

        public delegate IntPtr EglCreatePBufferSurface(IntPtr display, IntPtr config, int[] attrs);
        [GlEntryPoint("eglCreatePbufferSurface")]
        public EglCreatePBufferSurface CreatePBufferSurface { get; }

        public delegate bool EglMakeCurrent(IntPtr display, IntPtr draw, IntPtr read, IntPtr context);
        [GlEntryPoint("eglMakeCurrent")]
        public EglMakeCurrent MakeCurrent { get; }

        public delegate void EglDisplaySurfaceVoidDelegate(IntPtr display, IntPtr surface);
        [GlEntryPoint("eglDestroySurface")]
        public EglDisplaySurfaceVoidDelegate DestroySurface { get; }
        
        [GlEntryPoint("eglSwapBuffers")]
        public EglDisplaySurfaceVoidDelegate SwapBuffers { get; }
        
        public delegate IntPtr
            EglCreateWindowSurface(IntPtr display, IntPtr config, IntPtr window, int[] attrs);
        [GlEntryPoint("eglCreateWindowSurface")]
        public EglCreateWindowSurface CreateWindowSurface { get; }

        public delegate bool EglGetConfigAttrib(IntPtr display, IntPtr config, int attr, out int rv);
        [GlEntryPoint("eglGetConfigAttrib")]
        public EglGetConfigAttrib GetConfigAttrib { get; }

        // ReSharper restore UnassignedGetOnlyAutoProperty
    }
}
