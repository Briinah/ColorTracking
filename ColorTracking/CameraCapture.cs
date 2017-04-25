using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
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

        private int lowSaturation;
        private int lowValue;

        Point blueCircle;
        Point yellowCircle;
        Point greenCircle;

        XmlDocument config;

        public CameraCapture()
        {
            InitializeComponent();
            config = new XmlDocument();
            config.Load(Application.StartupPath + "/config.xml");

            lowSaturation = int.Parse(config.SelectSingleNode("color/S").InnerText);
            lowValue = int.Parse(config.SelectSingleNode("color/V").InnerText);

            Saturation_bar.Value = lowSaturation;
            Value_bar.Value = lowValue;

            blueCircle = new Point();
            yellowCircle = new Point();
            greenCircle = new Point();
        }

        private void ProcessFrame(object senter, EventArgs arg)
        {
            Image<Bgr, byte> ImageFrame = capture.QueryFrame().ToImage<Bgr,Byte>();
            Image<Bgr, byte> filterImage = new Image<Bgr, byte>(ImageFrame.Size);
            CvInvoke.BilateralFilter(ImageFrame, filterImage, 9, 80, 150);
            Image<Hsv, byte> hsvImage = filterImage.Convert<Hsv, Byte>();
            
            // red circle
            //Image<Gray, byte> redImage = ThresholdImage(hsvImage, new Point[] { new Point(0, 10), new Point(163, 179) });

            // blue circle
            Image<Gray, byte> blueImage = ThresholdImage(hsvImage, new Point[] { new Point(100, 120)});

            // green circle
            Image<Gray, byte> greenImage = ThresholdImage(hsvImage, new Point[] { new Point(50, 85) });

            // yellow circle
            Image<Gray, byte> yellowImage = ThresholdImage(hsvImage, new Point[] { new Point(20, 40) });

            //CamImgBox.Image = DetectAndDrawCircles(redImage, ImageFrame, new MCvScalar(50, 50, 150));
            var temp = DetectAndDrawCircles(blueImage, ImageFrame, new MCvScalar(150, 50, 0));
            var temp2 = DetectAndDrawCircles(yellowImage, temp, new MCvScalar(0, 200, 200));
            CamImgBox.Image = DetectAndDrawCircles(greenImage, temp2, new MCvScalar(50, 150, 0));

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
        }

        private Image<Gray,byte> ThresholdImage(Image<Hsv,byte> original, Point[] ranges)
        {
            List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
            for(int i = 0; i < ranges.Length; i++)
            {
                images.Add(original.InRange(new Hsv(ranges[i].X, lowSaturation, lowValue), new Hsv(ranges[i].Y, 255, 255)));
            }

            Image<Gray, byte> threshold = new Image<Gray, byte>(original.Size);
            for (int j = 0; j < images.Count; j++)
            {
                CvInvoke.AddWeighted(images[j], 1, threshold, 1, 0, threshold);
            }

            threshold.SmoothGaussian(9);

            Image<Gray, byte> temp = threshold.Clone();
            var element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));

            // opening
            CvInvoke.Erode(threshold, temp, element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
            CvInvoke.Dilate(temp, threshold, element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));

            //// closing
            //CvInvoke.Dilate(threshold, temp, element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
            //CvInvoke.Erode(temp, threshold, element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));

            return threshold;
        }

        private Image<Bgr,byte> DetectAndDrawCircles(Image<Gray, byte> binaryImage, Image<Bgr,byte> imageFrame, MCvScalar color)
        {
            //// detect circles
            //CircleF[] circles = CvInvoke.HoughCircles(binaryImage, Emgu.CV.CvEnum.HoughType.Gradient, 2, binaryImage.Rows / 8, 100, 50, 0, 100);
            Image<Bgr, byte> temp = imageFrame.Clone();

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(binaryImage, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for(int i = 0; i < contours.Size; i++)
            {
                MCvMoments moment = CvInvoke.Moments(contours[i]);
                double area = moment.M00;

                if(area > 400)
                {
                    MCvPoint2D64f position = new MCvPoint2D64f(moment.M10 / area, moment.M01/area);

                    CvInvoke.Circle(temp, new Point((int)position.X, (int)position.Y), 2, color, 3);
                    CvInvoke.DrawContours(temp, contours, i, color, 4);
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

        private void Saturation_bar_Scroll(object sender, EventArgs e)
        {
            lowSaturation = Saturation_bar.Value;
            config.SelectSingleNode("color/S").InnerText = lowSaturation.ToString();
        }

        private void Value_bar_Scroll(object sender, EventArgs e)
        {
            lowValue = Value_bar.Value;
            config.SelectSingleNode("color/V").InnerText = lowValue.ToString();
        }
    }
}
