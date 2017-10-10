using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

            byte[] array = new byte[size.ToInt32()];
            Marshal.Copy(pointer, array, 0, size.ToInt32());
            var str = Encoding.ASCII.GetString(array);

            Google.Protobuf.JsonParser parser =
                new Google.Protobuf.JsonParser
                (
                    new Google.Protobuf.JsonParser.Settings(10000)
                );

            var msg = parser.Parse<Camera.CameraList>(str);

            //Console.WriteLine(msg);

            Camera.StartCaptureArguments args = new Camera.StartCaptureArguments();
            args.CameraName = msg.Cameras[0].CameraName;
            args.CameraPath = msg.Cameras[0].CameraPath;
            args.Encoding = Camera.CaptureEncoding.Mjpeg;
            args.Framerate = 30;
            args.Width = 1280;
            args.Height = 720;

            Google.Protobuf.JsonFormatter formatter = new Google.Protobuf.JsonFormatter(new Google.Protobuf.JsonFormatter.Settings(true));

            var msg2 = formatter.Format(args);

            var byteArray = Encoding.UTF8.GetBytes(msg2);
            GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            IntPtr pointer2 = pinnedArray.AddrOfPinnedObject();
            var byteResult = CWrapperAPI.startCapture(pointer2);
            pinnedArray.Free();
            Console.WriteLine("result is " + byteResult);
        }
    }
}
