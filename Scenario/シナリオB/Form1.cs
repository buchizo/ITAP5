using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace シナリオB
{
	public partial class Form1 : Form
	{
		private KinectSensor _Kinect;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_Kinect = KinectSensor.KinectSensors[0];

			_Kinect.DepthFrameReady += KinectOnDepthFrameReady;
			_Kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
					
			_Kinect.Start();

		}

		private void KinectOnDepthFrameReady(object sender, DepthImageFrameReadyEventArgs depthImageFrameReadyEventArgs)
		{
			using (DepthImageFrame temp = depthImageFrameReadyEventArgs.OpenDepthImageFrame())
			{
				if (temp == null) return;

				short[] depthData = new short[640 * 480];
				byte[] depthColorData = new byte[640 * 480 * 4];
				
				temp.CopyPixelDataTo(depthData);

				for (int i = 0, i32 = 0; i < depthData.Length && i32 < depthColorData.Length; i++, i32 += 4)
				{
					//深度情報のみ取得
					int realDepth = depthData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

					//得られた深度を256段階のグレースケールに
					byte intensity = (byte)(255 - (255 * realDepth / 4095));

					depthColorData[i32] = intensity;
					depthColorData[i32+1] = intensity;
					depthColorData[i32+2] = intensity;
				}

				//深度情報の配列をBitmapにして表示
				this.pictureBox1.Image = ConvertToBitmap(depthColorData, 640, 480);
			}
		}

		public Bitmap ConvertToBitmap(byte[] pixels, int width, int height)
		{
			if (pixels == null) return null;
			var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
			bitmap.UnlockBits(data);
			return bitmap;
		}
		
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			_Kinect.Stop();
			_Kinect.Dispose();
		}

	}
}
