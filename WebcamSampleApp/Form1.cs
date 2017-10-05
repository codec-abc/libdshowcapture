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
            Console.WriteLine("current dir " + currentDir);

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

            Console.WriteLine(msg);


            var byteResult = CWrapperAPI.enumDevices();
            Console.WriteLine("result is " + byteResult);
        }
    }
}
