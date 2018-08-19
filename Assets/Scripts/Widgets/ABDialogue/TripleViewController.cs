using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleViewController : MonoBehaviour {

    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    [SerializeField] private List<GameObject> Views;
    [SerializeField] private GameObject StartingView;
    [SerializeField] private RectTransform ViewBase;
    [SerializeField] private Canvas Canvas;
    [SerializeField] private float smoothFactor;

    private float Width;

    protected void Start()
    {
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
