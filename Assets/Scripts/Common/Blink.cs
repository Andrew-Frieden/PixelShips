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

		private bool _init;
		private bool _cellAdded;
		
		private void Start()
		{
			ScrollViewController.cellAddedEvent += HandleCellAdded;
		}
		
		private void HandleCellAdded()
		{
			_cellAdded = true;
		}
		
		private void Update()
		{
			if (!_init)
			{
				Scroll();
				_init = true;
			}

			if (_cellAdded)
			{
				Invoke(nameof(Scroll), 0.05f);
				_cellAdded = false;
			}
		}

		private void Scroll()
		{
			gameObject.transform.SetAsLastSibling();
			_scrollRect.verticalNormalizedPosition = 0f;
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