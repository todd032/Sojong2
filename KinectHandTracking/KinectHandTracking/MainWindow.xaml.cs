using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace KinectHandTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        float x_variation = 0;
        float y_variation = 0;
        float prev_x = 0;
        float prev_y = 0;
        int frameNum = 0;
        bool handIsPrevTracked = false;
        //string state = "-";
        //float threshold = 0.02f;

        TextBoxOutputter outputter;

        static Socket sck;
        IPEndPoint localEndPoint;

        class Message
        {
            public int Frame;
            public float X;
            public float Y;
            public bool isClicked;

            public Message(int frameNum, float x, float y, bool click)
            {
                this.Frame = frameNum;
                this.X = x;
                this.Y = y;
                this.isClicked = click;
            }
        }

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //localEndPoint = new IPEndPoint(IPAddress.Parse("58.121.61.90"), 1234);
            localEndPoint = new IPEndPoint(IPAddress.Parse("58.232.166.114"), 1234);
            outputter = new TextBoxOutputter(TestBox);
            Console.SetOut(outputter);
            Console.WriteLine("Started");

        }


        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
            try
            {
                sck.Connect(localEndPoint);
                Console.Write("SUCCESS CONNECTING");
            }
            catch
            {
                Console.Write("Unable to connect to remote end point!\r\n");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }

            sck.Close();
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            Message msg;
                            if (body.IsTracked)
                            {
                                // Find the joints
                                Joint handRight = body.Joints[JointType.HandRight];
                                Joint thumbRight = body.Joints[JointType.ThumbRight];

                                Joint handLeft = body.Joints[JointType.HandLeft];
                                Joint thumbLeft = body.Joints[JointType.ThumbLeft];

                                Joint elbowLeft = body.Joints[JointType.ElbowLeft];
                                Joint elbowRight = body.Joints[JointType.ElbowRight];


                                CameraSpacePoint handRightPosition;
                                string hRight;
                                if (handRight.TrackingState == TrackingState.Tracked)
                                {
                                    handRightPosition = handRight.Position;
                                    hRight = "(" + handRightPosition.X + ",\n" + handRightPosition.Y + ",\n" + handRightPosition.Z + ")";
                                    tblRightHand.Text = hRight;
                                }

                                //Event triggered 되기까지 누적된 변화량
                                x_variation += (handRight.Position.X - prev_x);
                                y_variation += (handRight.Position.Y - prev_y);


                                if (handRight.TrackingState == TrackingState.Tracked)
                                {
                                    if (handIsPrevTracked)
                                    {
                                        if (body.HandRightState == HandState.Closed)
                                            msg = new Message(frameNum, handRight.Position.X - prev_x, handRight.Position.Y - prev_y, true);
                                        else
                                            msg = new Message(frameNum, handRight.Position.X - prev_x, handRight.Position.Y - prev_y, false);
                                    }
                                    else
                                    {
                                        msg = new Message(frameNum, 0, 0, false);
                                        handIsPrevTracked = true;
                                    }

                                    prev_x = handRight.Position.X;
                                    prev_y = handRight.Position.Y;
                                }
                                else
                                {
                                    msg = new Message(frameNum, 0, 0, false);
                                    handIsPrevTracked = false;
                                }

                                canvas.DrawThumb(elbowRight, _sensor.CoordinateMapper);
                                canvas.DrawThumb(handRight, _sensor.CoordinateMapper);
                            }
                            else
                            {
                                msg = new Message(frameNum, 0, 0, false);
                                handIsPrevTracked = false;
                            }

                            string json = JsonConvert.SerializeObject(msg);


                            /* Scocket Communication Part*/
                            try
                            {
                                if (sck != null)
                                {
                                    sck.Send(Encoding.Unicode.GetBytes(json));
                                    Console.Write("Data Sent!\r\n");
                                }
                            }
                            catch
                            {
                                Console.Write("Unable to send data!\r\n");
                            }

                            frameNum++;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
