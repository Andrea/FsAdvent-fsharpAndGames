using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace InTheKeyOfCSharp
{
	public class SoundStuff
	{
		private const int SamplingFrequency = 44100;

		private Dictionary<string, float> _notes = new Dictionary<string, float>
		{
			{"C", 261.626f},
			{"C#", 277.183f},
			{"D", 293.665f},
			{"D#", 311.127f},
			{"E", 329.628f},
			{"F", 349.228f},
			{"F#", 369.994f},
			{"G", 391.995f},
			{"G#", 415.305f},
			{"A", 440f},
			{"A#", 466.164f},
			{"B", 493.883f},
		}; 
		private AudioContext _audioContext;
		private int _audioSourceIndex;
		private int _buffer;

		private string _song = "F# F# G# F# B A#";

		public SoundStuff()
		{
			_audioContext = new AudioContext();

			_buffer = AL.GenBuffer();
			_audioSourceIndex = AL.GenSource();

			var notes = _song.Split(' ');
			var data = new short[SamplingFrequency * notes.Length];
			var dataOffset = 0;
			var noteLength = 0.5f;
			foreach (var note in notes)
			{
				var freq = _notes[note];
				for (var i = 0; i < SamplingFrequency * noteLength; i++)
				{
					var factor = (2 * Math.PI * freq) / SamplingFrequency * i;
					var sinf = Math.Sin(factor);

					data[dataOffset + i] = (short)(sinf * short.MaxValue);
				}

				dataOffset += (int)(SamplingFrequency * noteLength);
			}
			
			AL.BufferData(_buffer, ALFormat.Mono16, data, data.Length * 2, SamplingFrequency);
			AL.Source(_audioSourceIndex, ALSourcei.Buffer, _buffer);
			
			AL.SourcePlay(_audioSourceIndex);
			Console.WriteLine(AL.GetError());

			Console.ReadLine();
		}

		public void Play(object song)
		{

		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			var sound = new SoundStuff();
		}
	}
}
