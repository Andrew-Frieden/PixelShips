using UnityEngine;
using System.Collections;

namespace PixelShips.Verse
{
    public class VerseManager
    {
        private IVerseController _ctrl;
        public IVerseController Ctrl
        {
            get
            {
                if (_ctrl == null)
                    _ctrl = new MockVerseController();
                return _ctrl;
            }
        }

		private static VerseManager _instance;
		public static VerseManager instance
		{
			get
			{
				if (_instance == null)
					_instance = new VerseManager();
				return _instance;
			}
		}

        private VerseManager() { }
    }
}