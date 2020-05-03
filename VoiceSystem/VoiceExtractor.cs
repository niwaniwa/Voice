using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voice.VoiceSystem;

namespace Voice.VoVoiceSystemice
{
	// 音声データの摘出
	public class VoiceExtractor
	{

		private WaveInEvent waveIn = new WaveInEvent();
		private WaveFileWriter waveWriter;
		private const string filePath = @"";

		public void Start()
		{
			if (IsRecording)
			{
				return;
			}

			// 入力
			var bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 16, 2));
			

			// 音量
			var wavProvider = new VolumeWaveProvider16(bufferedWaveProvider);
			wavProvider.Volume = 0.1f;

			var device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

			IsRecording = true;
			//外部からの音声入力を受け付け開始
			waveWriter = new WaveFileWriter(filePath, waveIn.WaveFormat);
			using (var wavPlayer = new WasapiOut(device, AudioClientShareMode.Shared, false, 200))
			{
				waveIn.DataAvailable += (_, e) =>
				{
					bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
					waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
					waveWriter.Flush();
				};
				waveIn.StartRecording();

			}

		}

		public void Stop()
		{
			waveIn.RecordingStopped += (_, __) =>
			{
				waveWriter.Flush();
			};
			waveIn.StopRecording();
			IsRecording = false;
		}

		public bool IsRecording
		{
			get; private set;
		}

	}
}
