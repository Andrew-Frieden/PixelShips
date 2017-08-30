using UnityEngine;
using System.Collections;

namespace PixelShips.Verse
{
	public delegate void VerseUpdate(string data);

	public interface IVerseController
    {
		void Subscribe(VerseUpdate updateDelegate);
		void Unsubscribe(VerseUpdate updateDelegate);
		void StartUpdates();
		bool SubmitAction(string action);
    }
}