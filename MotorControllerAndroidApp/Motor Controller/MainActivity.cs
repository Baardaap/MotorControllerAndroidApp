using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Motor_Controller
{
    [Activity(Label = "Motor Controller", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        SeekBar seekMotor1;
        TextView textMotorPercentMotor1;
        int PercentMotor1;

        SeekBar seekMotor2;
        TextView textMotorPercentMotor2;
        int PercentMotor2;

        SeekBar seekMotor3;
        TextView textMotorPercentMotor3;
        int PercentMotor3;

        Socket sock;
        static IPAddress serverAddr;
        IPEndPoint endPoint;
        string message;
        byte[] send_buffer;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            seekMotor1 = FindViewById<SeekBar>(Resource.Id.seekMotor1);
            textMotorPercentMotor1 = FindViewById<TextView>(Resource.Id.textPercentMotor1);
            PercentMotor1 = 0;

            seekMotor2 = FindViewById<SeekBar>(Resource.Id.seekMotor2);
            textMotorPercentMotor2 = FindViewById<TextView>(Resource.Id.textPercentMotor2);
            PercentMotor2 = 0;

            seekMotor3 = FindViewById<SeekBar>(Resource.Id.seekMotor3);
            textMotorPercentMotor3 = FindViewById<TextView>(Resource.Id.textPercentMotor3);
            PercentMotor3 = 0;

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverAddr = IPAddress.Parse("192.168.2.255");
            endPoint = new IPEndPoint(serverAddr, 11000);
            message = "";
            send_buffer = Encoding.ASCII.GetBytes(message);

            seekMotor1.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                textMotorPercentMotor1.Text = string.Format("{0}%", e.Progress);
                PercentMotor1 = e.Progress;
            };

            seekMotor2.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                textMotorPercentMotor2.Text = string.Format("{0}%", e.Progress);
                PercentMotor2 = e.Progress;
            };

            seekMotor3.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                textMotorPercentMotor3.Text = string.Format("{0}%", e.Progress);
                PercentMotor2 = e.Progress;
            };

            seekMotor1.StopTrackingTouch += (object sender, SeekBar.StopTrackingTouchEventArgs e) =>
            {
                SendNewValue(1, PercentMotor1);
            };

            seekMotor2.StopTrackingTouch += (object sender, SeekBar.StopTrackingTouchEventArgs e) =>
            {
                SendNewValue(2, PercentMotor2);
            };

            seekMotor3.StopTrackingTouch += (object sender, SeekBar.StopTrackingTouchEventArgs e) =>
            {
                SendNewValue(3, PercentMotor3);
            };
        }

        public void SendNewValue(int motor, int value)
        {
            message = "%" + motor.ToString() + "," + value.ToString() + "#"; 
            sock.SendTo(send_buffer, endPoint);
        }
    }
}

