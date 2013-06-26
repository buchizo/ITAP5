using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;

namespace シナリオD
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private KinectSensor _Kinect;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_Kinect = KinectSensor.KinectSensors[0];
			_Kinect.SkeletonStream.Enable();
			_Kinect.DepthStream.Enable();
			_Kinect.Start();

			//KinectRegionコントロールにKinectセンサーをバインドする
			kinectRegion.KinectSensor = _Kinect;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_Kinect.Stop();
			_Kinect.Dispose();
		}

		private void KinectTileButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("OK!");
		}
	}
}
