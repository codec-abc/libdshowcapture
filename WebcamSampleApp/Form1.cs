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

            var camList = parser.Parse<CameraReaderWindows.CameraList>(str);

            Console.WriteLine(camList);

            CameraReaderWindows.StartCaptureArguments args = new CameraReaderWindows.StartCaptureArguments();

            // TODO : logic to choose best devices
            args.CameraName = camList.Cameras[0].CameraName;
            args.CameraPath = camList.Cameras[0].CameraPath;
            args.Encoding = CameraReaderWindows.CaptureEncoding.Mjpeg;
            args.Frameinterval = 300000000;
            args.Width = 640;
            args.Height = 480;
            args.FlippingMode = CameraReaderWindows.Flip.Vertically;

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
            var startCaptureResult = parser.Parse<CameraReaderWindows.StartCaptureResult>(str);

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
                startCaptureResult.Result == CameraReaderWindows.StartResult.Success
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
    
                    var imageSize = image.Width * image.Height;

                    for (int j = 0; j < image.Height; j++)
                    {
                        for (int i = 0; i < image.Width; i++)
                        {
                            var byteIndex = j * image.Width + i;
                            var y = buffer[byteIndex + imageSize * 0];
                            var v = buffer[byteIndex + imageSize * 1];
                            var u = buffer[byteIndex + imageSize * 2];
                            var a = (byte)255;
                            var r = y / 255.0 + 1.13983 * (v - 255.0 / 2.0) / 255.0;
                            var g = y / 255.0 - 0.39465 * (u - 255.0 / 2.0) / 255.0 - 0.58060 * (v - 255.0 / 2.0) / 255.0;
                            var b = y / 255.0 + 2.03211 * (u - 255.0 / 2.0) / 255.0;
                            var color = Color.FromArgb(a, (byte) (r * 255), (byte) (g * 255), (byte)(b * 255));
                            
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
