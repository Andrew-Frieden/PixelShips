using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NavBar : MonoBehaviour {

    public List<GameObject> Views;
    public GameObject StartingView;
    public RectTransform ViewBase;
    public Canvas Canvas;
    public float smoothFactor;

    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    private float Width;

	void Start () {
        var rt = StartingView.GetComponent<RectTransform>();
        Width = rt.rect.width * Canvas.scaleFactor;

        leftPosition = StartingView.transform.position + new Vector3(-Width, 0, 0);
        rightPosition = StartingView.transform.position + new Vector3(Width, 0, 0);

        Views[1].GetComponent<RectTransform>().anchoredPosition = rightPosition;
        Views[2].GetComponent<RectTransform>().anchoredPosition = leftPosition;

        startPosition = ViewBase.transform.position;
        targetPosition = startPosition;
    }

    void Update()
    {
        ViewBase.anchoredPosition = Vector3.Lerp(ViewBase.anchoredPosition, targetPosition, Time.deltaTime * smoothFactor);
    }

    public void ShowView(GameObject view)
    {
        if (view == Views[0])
        {
            targetPosition = startPosition;
        }

        if (view == Views[1])
        {
            targetPosition = startPosition + new Vector3(-Width, 0, 0);
        }

        if (view == Views[2])
        {
            targetPosition = startPosition + new Vector3(Width, 0, 0);
        }
    }
}
