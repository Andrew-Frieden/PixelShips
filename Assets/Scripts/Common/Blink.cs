using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
	public class Blink : MonoBehaviour
	{
		[SerializeField] private TMP_Text _underScore;
		[SerializeField] private ScrollRect _scrollRect;

		private bool _hasScrolled;
		
		private void Update()
		{
			if (_underScore.enabled)
			{
				gameObject.transform.SetAsLastSibling();

				if (!_hasScrolled)
				{
					_scrollRect.verticalNormalizedPosition = 0f;
					_hasScrolled = false;
				}
			}
			
			_scrollRect.onValueChanged.AddListener(Listener);
		}
		
		private void Listener(Vector2 value)
		{
			_hasScrolled = true;
		}

		public IEnumerator BlinkLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(1.0f);
				_underScore.enabled = !_underScore.enabled;
			}
		}
	}
}