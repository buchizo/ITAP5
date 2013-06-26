using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Kinect;
using Newtonsoft.Json;

namespace Player
{
	class Program
	{
		public class JointPosition
		{
			public float x { get; set; }
			public float y { get; set; }
		}

		static void Main(string[] args)
		{
			var server = new HubConnection("http://localhost:8001/");
			var method = server.CreateHubProxy("kinect");
			var sensor = Microsoft.Kinect.KinectSensor.KinectSensors.First();

			try
			{
				server.Start().Wait();
				sensor.SkeletonFrameReady += (sender, eventArgs) =>
				                             {
					                             using (var frame = eventArgs.OpenSkeletonFrame())
					                             {
						                             if (frame == null) return;
						                             var skeletons = new Skeleton[frame.SkeletonArrayLength];
						                             frame.CopySkeletonDataTo(skeletons);
						                             var user = skeletons.FirstOrDefault(x => x.TrackingState == SkeletonTrackingState.Tracked);
													 if (user == null) return;
													 var m = user.Joints.
														 Where(x => x.TrackingState == JointTrackingState.Tracked)
														 .Select(s =>
														         {
															         var c = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(s.Position, ColorImageFormat.RgbResolution640x480Fps30);
															         return new JointPosition
															                {
																                x = c.X,
																                y = c.Y
															                };
														         }).ToList();
													 Console.Write(".");
						                             method.Invoke("Send",m);
					                             }
				                             };

				sensor.SkeletonStream.Enable(new TransformSmoothParameters()
				                             {
					                             Correction = 0.7f,
					                             JitterRadius = 0.1f,
					                             MaxDeviationRadius = 0.1f,
					                             Prediction = 0.7f,
					                             Smoothing = 0.8f
				                             });

				sensor.Start();
				Console.WriteLine("Started.");
				Console.ReadLine();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.ReadLine();
			}
			finally
			{
				sensor.Stop();
				sensor.Dispose();
				server.Stop();
			}
		}
	}
}
