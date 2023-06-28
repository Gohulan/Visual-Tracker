using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Visual_Tracker
{
    public partial class Form1 : Form
    {
        private bool isCameraOpen = false;
        private string savedProductName;
        private string savedProductCode;
        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;

            // Start capturing from the video source
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Display the captured frame in the PictureBox control
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            // Update the PictureBox on the UI thread
            pictureBox1.Invoke((MethodInvoker)(() =>
            {
                pictureBox1.Image = frame;
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Capture the current frame and save it as an image with the product name and product code
            if (pictureBox1.Image != null && !string.IsNullOrEmpty(txtProductName.Text) && !string.IsNullOrEmpty(txtProductCode.Text))
            {
                // Create the directory if it doesn't exist
                string directoryPath = @"C:\products";
                Directory.CreateDirectory(directoryPath);

                // Generate the file name using the product name and product code
                string productName = txtProductName.Text;
                string productCode = txtProductCode.Text;
                string fileName = $"{productName}_{productCode}.jpg";
                string filePath = Path.Combine(directoryPath, fileName);

                // Save the image
                pictureBox1.Image.Save(filePath);
                MessageBox.Show("Image captured and saved successfully.");
            }
            else
            {
                MessageBox.Show("Please enter a product name and product code before capturing.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Check if the product exists in the saved product folder
            string directoryPath = @"C:\products";
            string productName = txtProductName.Text;
            string productCode = txtProductCode.Text;
            string fileName = $"{productName}_{productCode}.jpg";
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                MessageBox.Show("Product exists.");
            }
            else
            {
                MessageBox.Show("Product does not exist.");
            }
        }
    }
}


