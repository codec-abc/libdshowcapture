using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class CameraReaderWindows : MonoBehaviour
{
    private IntPtr _devicePtr;
    private bool _hasCaptureStartedCorrectly;
    private bool _wasMaterialSet;
    private Texture2D _YTex;
    private Texture2D _UTex;
    private Texture2D _VTex;

    // Use this for initialization
    void Start()
    {
        IntPtr size = new IntPtr(0);
        IntPtr pointer = new IntPtr(0);
        var isOk = CWrapperAPI.getDevices(ref size, ref pointer);

        if (pointer.ToInt64() == 0 || size.ToInt64() == 0)
        {
            return;
        }

        byte[] array = new byte[size.ToInt32()];
        Marshal.Copy(pointer, array, 0, size.ToInt32());
        var str = Encoding.ASCII.GetString(array);

        CWrapperAPI.freeByteArray(pointer);

        Google.Protobuf.JsonParser parser =
            new Google.Protobuf.JsonParser
            (
                new Google.Protobuf.JsonParser.Settings(10000)
            );

        var camList = parser.Parse<Camera.CameraList>(str);

        Camera.StartCaptureArguments args = GetStartCaptureArgument(camList);

        Google.Protobuf.JsonFormatter formatter =
            new Google.Protobuf.JsonFormatter
            (
                new Google.Protobuf.JsonFormatter.Settings(true)
            );

        var msg = formatter.Format(args);

        var byteArray = Encoding.UTF8.GetBytes(msg);
        GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
        var Inpointer = pinnedArray.AddrOfPinnedObject();

        pointer = new IntPtr(0);
        size = new IntPtr(0);

        CWrapperAPI.startCapture(Inpointer, ref size, ref pointer);
        pinnedArray.Free();

        array = new byte[size.ToInt32()];
        Marshal.Copy(pointer, array, 0, size.ToInt32());
        str = Encoding.ASCII.GetString(array);
        var startCaptureResult = parser.Parse<Camera.StartCaptureResult>(str);

        if (pointer.ToInt64() == 0 || size.ToInt64() == 0)
        {
            return;
        }

        CWrapperAPI.freeByteArray(pointer);
        pointer = new IntPtr(0);

        _devicePtr = new IntPtr((long)startCaptureResult.DevicePointer);
        _hasCaptureStartedCorrectly =
            startCaptureResult.CanConnectFilters &&
            startCaptureResult.CanResetGraph &&
            startCaptureResult.CanSetAudioConfig &&
            startCaptureResult.CanSetVideoConfig &&
            startCaptureResult.DevicePointer != 0 &&
            startCaptureResult.Result == Camera.StartResult.Success;

    }

    private static Camera.StartCaptureArguments GetStartCaptureArgument(Camera.CameraList camList)
    {
        Camera.StartCaptureArguments args = new Camera.StartCaptureArguments();

        var convertedCameras = ProtobufConverter.FromProtoBuff(camList);
        var firstCamera = convertedCameras[0];

        var decentFramerateFormat = firstCamera.Formats.Where(format => format.FrameInterval <= 333333);
        var sortedByHigherPixelCount = decentFramerateFormat.OrderBy(format => 0 - (format.Width * format.Height)).ToList();
        var formatToUse = sortedByHigherPixelCount[0];

        args.CameraName = firstCamera.CameraName;
        args.CameraPath = firstCamera.CameraPath;
        args.Encoding = formatToUse.Encoding;
        args.Frameinterval = formatToUse.FrameInterval;
        args.Width = formatToUse.Width;
        args.Height = formatToUse.Height;
        return args;
    }

    public static class ProtobufConverter
    {
        public static List<CameraProps> FromProtoBuff(Camera.CameraList camerasProtoBuf)
        {
            var cameras = new List<CameraProps>();
            foreach (var camProtoBuf in camerasProtoBuf.Cameras)
            {
                var formats = new List<Format>();
                foreach (var format in camProtoBuf.Formats)
                {
                    formats.Add(new Format()
                    {
                        Encoding = format.Encoding,
                        FrameInterval = format.Frameinterval,
                        Width = format.Width,
                        Height = format.Height
                    });
                }
                cameras.Add(new CameraProps()
                {
                    CameraName = camProtoBuf.CameraName,
                    CameraPath = camProtoBuf.CameraPath,
                    Formats = formats
                });
            }
            return cameras;
        }
    }

    public class CameraProps
    {
        public string CameraName;
        public string CameraPath;
        public List<Format> Formats;
    }

    public class Format
    {
        public Camera.CaptureEncoding Encoding;
        public ulong FrameInterval;
        public uint Width;
        public uint Height;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasCaptureStartedCorrectly)
        {
            uint width = 0;
            uint height = 0;
            IntPtr pointer = new IntPtr(0);
            CWrapperAPI.tryGetBuffer(ref pointer, _devicePtr, ref width, ref height);

            if (pointer.ToInt64() != 0)
            {
                if (!_wasMaterialSet)
                {
                    var mat = new Material(Shader.Find("Unlit/YUV_Vijeo"));
                    _YTex = new Texture2D((int)width, (int)height, TextureFormat.Alpha8, false);
                    _UTex = new Texture2D((int)width, (int)height, TextureFormat.Alpha8, false);
                    _VTex = new Texture2D((int)width, (int)height, TextureFormat.Alpha8, false);
                    mat.SetTexture("_YTex", _YTex);
                    mat.SetTexture("_UTex", _UTex);
                    mat.SetTexture("_VTex", _VTex);
                    _wasMaterialSet = true;
                    var renderer = GetComponent<Renderer>();
                    renderer.material = mat;
                }

                var size = (int)width * (int)height;

                _YTex.LoadRawTextureData(pointer, size);
                _UTex.LoadRawTextureData(new IntPtr(pointer.ToInt64() + 2 * size), size);
                _VTex.LoadRawTextureData(new IntPtr(pointer.ToInt64() + 1 * size), size);

                _YTex.Apply();
                _UTex.Apply();
                _VTex.Apply();

                CWrapperAPI.recycleUsedBuffer(pointer, _devicePtr);
            }
        }
    }

    private void OnDestroy()
    {
        if (_devicePtr.ToInt64() != 0)
        {
            CWrapperAPI.shutDownAndFreeDevice(_devicePtr);
        }
    }
}

public static class CWrapperAPI
{
    private const string dllPath = @"Assets/Plugins/CWrapper.dll";

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void startCapture(IntPtr message, ref IntPtr arraySize, ref IntPtr arrayPtr);

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte getDevices(ref IntPtr arraySize, ref IntPtr arrayPtr);

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void freeByteArray(IntPtr array);

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void shutDownAndFreeDevice(IntPtr devicePtr);

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void tryGetBuffer(ref IntPtr arrayPtr, IntPtr devicePtr, ref uint width, ref uint height);

    [DllImport(dllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern void recycleUsedBuffer(IntPtr arrayPtr, IntPtr devicePtr);

}

