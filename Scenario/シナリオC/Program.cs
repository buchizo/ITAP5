using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace シナリオC
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

			kinect.SkeletonFrameReady += KinectOnSkeletonFrameReady;
			kinect.SkeletonStream.Enable(new TransformSmoothParameters()
			                             {
											 Correction = 0.4f,
											 JitterRadius = 0.1f,
											 MaxDeviationRadius = 0.1f,
											 Prediction = 0.6f,
											 Smoothing = 0.8f
			                             });

			Console.ReadLine();

			//センサーの利用を停止＋後始末
			kinect.Stop();
			kinect.Dispose();
		}

		private static void KinectOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
		{
			using (var frame = skeletonFrameReadyEventArgs.OpenSkeletonFrame())
			{
				if (frame == null) return;
				
				//フレームに含まれる骨格情報を取得する
				var skeletonData = new Skeleton[frame.SkeletonArrayLength];
				frame.CopySkeletonDataTo(skeletonData);

				foreach (var skeleton in skeletonData)
				{
					if (skeleton == null) continue;

					//トラッキングできている骨格だけ取得
					if (skeleton.TrackingState != SkeletonTrackingState.Tracked) continue;
					foreach (Joint joint in skeleton.Joints)
					{
						//トラッキングできている関節だけ取得
						if (joint.TrackingState != JointTrackingState.Tracked) continue;
						Console.WriteLine("JointType: {0}\t X: {1} \t Y: {2} \t Z: {3}", joint.JointType, joint.Position.X, joint.Position.Y, joint.Position.Z);

						//座標系の変換
						var cp = (sender as KinectSensor).CoordinateMapper.MapSkeletonPointToColorPoint(joint.Position, ColorImageFormat.RawBayerResolution640x480Fps30);
						Console.WriteLine("RGB X: {0} \t RGB Y: {1}", cp.X, cp.Y);
					}
				}
			}
		}
	}
}
