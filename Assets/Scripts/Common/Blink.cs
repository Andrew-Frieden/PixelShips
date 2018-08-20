using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
	public class Blink : MonoBehaviour
	{
		[SerializeField] private Image _cursorImage;
		
		private void Update()
		{
			if (_cursorImage.enabled)
			{
				gameObject.transform.SetAsLastSibling();
			}
		}

		public IEnumerator BlinkLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(1.0f);
				_cursorImage.enabled = !_cursorImage.enabled;
			}
		}
	}
}