﻿using MediaCaptureWPF.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using WinRT;

namespace MediaCaptureWPF
{
    public class CapturePreview : D3DImage
    {
        CapturePreviewNative m_preview;
        MediaCapture m_capture;
        uint m_width;
        uint m_height;

        public CapturePreview(MediaCapture capture)
        {
            var props = (VideoEncodingProperties)capture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            m_width = props.Width;
            m_height = props.Height;

            m_preview = new CapturePreviewNative(this, m_width, m_height);
            m_capture = capture;
        }

        public async Task StartAsync()
        {
            var profile = new MediaEncodingProfile
            {
                 
                Audio = null,
                Video = VideoEncodingProperties.CreateUncompressed(MediaEncodingSubtypes.Rgb32, m_width, m_height),
                Container = null
            };
            var ob = (IMediaExtension)m_preview.MediaSink;
            
            try
            {
                var w = ob.As< IMediaExtension>();
                var t = m_capture.StartPreviewToCustomSinkAsync(profile, w);
            }
            catch (Exception ex)
            {
                var ab = ex;
            }
        }

        public async Task StopAsync()
        {
            await m_capture.StopPreviewAsync();
        }
    }
}
