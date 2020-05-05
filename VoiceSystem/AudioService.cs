using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voice.VoiceSystem
{
	public sealed class AudioService
	{

		private static AudioService instance;

		private MMDeviceCollection caputureDevices;
		private MMDeviceCollection renderDevices;

		private MMDevice selectedCaptureDevice;
		private MMDevice selectedRenderDevice;

		private AudioService()
		{
			ReloadAudioDevices();
			selectedCaptureDevice = caputureDevices[0];
			selectedRenderDevice = renderDevices[0];
		}

		public void ReloadAudioDevices()
		{
			RenderDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
			caputureDevices  = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
		}

		public MMDeviceCollection CaputureDevices
		{
			get
			{
				return caputureDevices;
			}
			private set
			{
				caputureDevices = value;
			}
		}

		public MMDeviceCollection RenderDevices
		{
			get
			{
				return renderDevices;
			}
			private set
			{
				renderDevices = value;
			}
		}

		public MMDevice SelectedCaptureDevice
		{
			get
			{
				return selectedCaptureDevice;
			}
			set
			{
				selectedCaptureDevice = value;
			}
		}

		public MMDevice SelectedRenderDevice
		{
			get
			{
				return selectedRenderDevice;
			}
			set
			{
				selectedRenderDevice = value;
			}
		}

		public static AudioService Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new AudioService();
				}
				return instance;
			}
			private set
			{
				instance = value;
			}
		}

	}
}
