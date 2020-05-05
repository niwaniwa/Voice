using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voice.VoiceSystem.Effect.Abs
{
	public interface IEffectModule
	{

		bool CanExecute();

		void Execute();

	}
}
