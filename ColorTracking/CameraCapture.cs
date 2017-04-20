using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace ColorTracking
{
    public partial class CameraCapture : Form
    {
        private Capture capture;
        private bool captureInProgress;
        enum Color { Blue, Green, Yellow };
        Color filter = Color.Blue;

        public CameraCapture()
        {
            InitializeComponent();
        }

        private void ProcessFrame(object senter, EventArgs arg)
        {
            Image<Bgr, byte> ImageFrame = capture.QueryFrame().ToImage<Bgr,Byte>();
            CvInvoke.MedianBlur(ImageFrame, ImageFrame, 3);
            Image<Hsv, byte> hsvImage = ImageFrame.Convert<Hsv, Byte>();
            
            // red circle
            //Image<Gray, byte> redImage = ThresholdImage(hsvImage, new Point[] { new Point(0, 10), new Point(163, 179) });

            // blue circle
            Image<Gray, byte> blueImage = ThresholdImage(hsvImage, new Point[] { new Point(100, 120)});

            // green circle
            Image<Gray, byte> greenImage = ThresholdImage(hsvImage, new Point[] { new Point(50, 85) });

            // yellow circle
            Image<Gray, byte> yellowImage = ThresholdImage(hsvImage, new Point[] { new Point(20, 40) });

            switch(filter)
            {
                case Color.Blue: outImgBox.Image = blueImage;
                    break;
                case Color.Green: outImgBox.Image = greenImage;
                    break;
                case Color.Yellow: outImgBox.Image = yellowImage;
                    break;
                default:
                    break;

            }

            //CamImgBox.Image = DetectAndDrawCircles(redImage, ImageFrame, new MCvScalar(50, 50, 150));
            var temp = DetectAndDrawCircles(blueImage, ImageFrame, new MCvScalar(150, 50, 0));
            var temp2 = DetectAndDrawCircles(yellowImage, temp, new MCvScalar(0, 200, 200));
            CamImgBox.Image = DetectAndDrawCircles(greenImage, temp2, new MCvScalar(50, 150, 0));
        }

        private Image<Gray,byte> ThresholdImage(Image<Hsv,byte> original, Point[] ranges)
        {
            List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
            for(int i = 0; i < ranges.Length; i++)
            {
                images.Add(original.InRange(new Hsv(ranges[i].X, 70, 100), new Hsv(ranges[i].Y, 255, 255)));
            }

            Image<Gray, byte> threshold = new Image<Gray, byte>(original.Size);
            for (int j = 0; j < images.Count; j++)
            {
                CvInvoke.AddWeighted(images[j], 1, threshold, 1, 0, threshold);
            }

            threshold.SmoothGaussian(9);

            Image<Gray, byte> temp = threshold.Clone();
            var element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            for (int n = 0; n < 50; n++)
            {
                // opening
                CvInvoke.Erode(threshold, temp, element, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
                CvInvoke.Dilate(temp, threshold, element, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
            }
            return threshold;
        }

        private Image<Bgr,byte> DetectAndDrawCircles(Image<Gray, byte> binaryImage, Image<Bgr,byte> imageFrame, MCvScalar color)
        {
            // detect circles
            CircleF[] circles = CvInvoke.HoughCircles(binaryImage, Emgu.CV.CvEnum.HoughType.Gradient, 2, binaryImage.Rows / 8, 100, 50, 0, 100);
            Image<Bgr, byte> temp = imageFrame.Clone();

            // draw circles
            if (circles.Length > 0)
            {
                Console.WriteLine(circles.Length);
                for (int current = 0; current < circles.Length; ++current)
                {
                    Point center = Point.Round(circles[current].Center);
                    int radius = (int)Math.Round(circles[current].Radius);

                    CvInvoke.Circle(temp, center, radius, color, 3);
                }
            }

            return temp;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch(NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                if(captureInProgress)
                {
                    btnStart.Text = "Start";
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    btnStart.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }
        }

        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            filter = Color.Blue;
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            filter = Color.Green;
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            filter = Color.Yellow;
        }
    }
}
