using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace シナリオA
{
	class Program
	{
		static void Main(string[] args)
		{
			//センサーを取得（何も考えずに最初のセンサーを取得している）
			var kinect = Microsoft.Kinect.KinectSensor.KinectSensors[0];

			//センサーの現在の状態
			Console.WriteLine("Status:" + kinect.Status);

			//センサーの利用を開始
			kinect.Start();

			//現在の角度と可動範囲を取得
			Console.WriteLine("Current Angle:" + kinect.ElevationAngle);
			Console.WriteLine("Max Angle:" + kinect.MaxElevationAngle);
			Console.WriteLine("Min Angle:" + kinect.MinElevationAngle);

			Console.Write("New Angle?:");
			var angle = int.Parse(Console.ReadLine());

			//新しい角度を指定（=チルトモーターが作動して角度が変わる）
			kinect.ElevationAngle = angle;

			Console.WriteLine("Current Angle:" + kinect.ElevationAngle);

			//センサーの利用を停止＋後始末
			kinect.Stop();
			kinect.Dispose();

			Console.ReadKey();
		}
	}
}
