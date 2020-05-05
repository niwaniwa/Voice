using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Voice.MVVM;
using Voice.Core.Event;
using Voice.VoiceSystem;
using Voice.VoiceSystem.Effect;
using Voice.VoiceSystem.Effect.Abs;
using NAudio.Wave.SampleProviders;
using System.Threading;

namespace Voice.VoVoiceSystemice
{
	// 音声データの摘出
	public class VoiceExtractor
	{

		private WaveInEvent waveIn = new WaveInEvent();
		private WaveFileWriter waveWriter;
		private BufferedWaveProvider bufferedWaveProvider;
		private AudioService audioService;
		public List<IEffectModule> effectModules = new List<IEffectModule>();

		private const string filePath = @"";

		public VoiceExtractor()
		{
			audioService = AudioService.Instance;
		}

		public void Start()
		{
			if (IsRecording)
			{
				return;
			}

			var wavFormat = new WaveFormat(44100, 16, 2);

			bufferedWaveProvider = new BufferedWaveProvider(wavFormat);
			// 音量
			var wavProvider = new VolumeWaveProvider16(bufferedWaveProvider);
			wavProvider.Volume = 0.1f;

			// デバッグ用Wav出力設定
			waveWriter = new WaveFileWriter(filePath, bufferedWaveProvider.WaveFormat);

			// 外部からの音声入力を受け付け設定
			waveIn.WaveFormat = wavFormat;
			waveIn.DeviceNumber = audioService.CaputureDevices.ToList<MMDevice>().IndexOf(audioService.SelectedCaptureDevice);
			waveIn.DataAvailable += DataAvailable;
			waveIn.RecordingStopped += ( _, __) =>
			{
				waveIn.StopRecording();
				waveIn.Dispose();
				waveWriter.Dispose();
				IsRecording = false;
			};

			// start
			waveIn.StartRecording();
			IsRecording = true;

			var wavPlayer = new WasapiOut();
			wavPlayer.Init(wavProvider);
			wavPlayer.Play();

		}

		private void DataAvailable(object sender, WaveInEventArgs wave)
		{
			// buffer渡し
			bufferedWaveProvider.AddSamples(wave.Buffer, 0, wave.BytesRecorded);

			// wav write
			waveWriter.Write(wave.Buffer, 0, wave.BytesRecorded);
			waveWriter.Flush();

		}

		public void Stop()
		{
			waveIn.StopRecording();
		}
		
		public void AddEffector(IEffectModule module)
		{
			EffectModules.Add(module);
		}


		public bool IsRecording
		{
			get; private set;
		}

		public List<IEffectModule> EffectModules
		{
			get
			{
				return effectModules;
			}
			private set
			{
				effectModules = value;
			}
		}

	}
}
