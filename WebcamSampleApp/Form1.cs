using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WebcamSampleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var currentDir = Directory.GetCurrentDirectory();

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

            Console.WriteLine(camList);

            Camera.StartCaptureArguments args = new Camera.StartCaptureArguments();

            // TODO : logic to choose best devices
            args.CameraName = camList.Cameras[0].CameraName;
            args.CameraPath = camList.Cameras[0].CameraPath;
            args.Encoding = Camera.CaptureEncoding.Mjpeg;
            args.Frameinterval = 300000000;
            args.Width = 1280;
            args.Height = 720;

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
            Console.WriteLine("result is " + startCaptureResult.ToString());
            Thread.Sleep(1000);
            pointer = new IntPtr(0);
            
            var devicePtr = new IntPtr((long)startCaptureResult.DevicePointer);

            if 
            (
                startCaptureResult.CanConnectFilters &&
                startCaptureResult.CanResetGraph &&
                startCaptureResult.CanSetAudioConfig &&
                startCaptureResult.CanSetVideoConfig &&
                startCaptureResult.DevicePointer != 0 &&
                startCaptureResult.Result == Camera.StartResult.Success
            )
            {
                uint width = 0;
                uint height = 0;
                CWrapperAPI.tryGetBuffer(ref pointer, devicePtr, ref width, ref height);

                if (pointer.ToInt64() != 0)
                {
                    var image = new Bitmap((int) width, (int)height);
                    int bufferSize = (int) width * (int) height * 3;
                    var buffer = new byte[bufferSize];
                    Marshal.Copy(pointer, buffer, 0, bufferSize);
                    int byteIndex = 0;

                    for (int j = 0; j < image.Height; j++)
                    {
                        for (int i = 0; i < image.Width; i++)
                        {
                            var b = buffer[byteIndex++];
                            var g = buffer[byteIndex++];
                            var r = buffer[byteIndex++];
                            var a = (byte)255;
                            var color = Color.FromArgb(a, r, g, b);

                            image.SetPixel(i, j, color);
                        }
                    }
                    this.pictureBox1.Image = image;
                    CWrapperAPI.recycleUsedBuffer(pointer, devicePtr);
                }
            }

            if (startCaptureResult.DevicePointer != 0)
            {
                CWrapperAPI.shutDownAndFreeDevice(devicePtr);
            }
        }
    }
}
