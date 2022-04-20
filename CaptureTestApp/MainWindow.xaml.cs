using MediaCaptureWPF;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace CaptureTestApp
{
    public partial class MainWindow : Window
    {
        bool m_initialized;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnActivated(EventArgs e)
        {
            if (m_initialized)
            {
                return; // Already initialized
            }
            m_initialized = true;

            AllocConsole();

            var devsVideo = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)).ToList();
            var capture = new MediaCapture();
            await capture.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = devsVideo.First().Id, 
                    StreamingCaptureMode = StreamingCaptureMode.Video // No audio
                });

            var preview = new CapturePreview(capture);
            Preview.Source = preview;
            await preview.StartAsync();
        }

        [DllImport("Kernel32")]
        public static extern void AllocConsole();
    }
}
