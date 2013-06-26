using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace KinectSocket.Hubs
{
	public class JointPosition
	{
		public float x { get; set; }
		public float y { get; set; }
	}

	[HubName("kinect")]
	public class Kinect : Hub
	{
		public void Send(List<JointPosition> message)
		{
			if (message == null || !message.Any()) return;
			Clients.All.Received(message);
		}
	}
}