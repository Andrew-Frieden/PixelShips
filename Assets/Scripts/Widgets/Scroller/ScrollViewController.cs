using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {

    public int CellCount = 50;
    public GameObject ScrollCellPrefab;
    public GameObject CellHolder;
    
    private bool Anchored = true;
    private ScrollCell LastActiveCell;

    private ScrollRect ScrollRect;

    private Queue<ScrollCell> ActiveCells;
    private List<ScrollCell> CachedCells;
    
	// Use this for initialization
	void Start () {

        ScrollRect = GetComponent<ScrollRect>();

        if (ScrollCellPrefab == null)
            throw new MissingComponentException("ScrollViewController -> no ScrollCell prefab set");
            
        BuildCache();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void BuildCache()
    {
        ActiveCells = new Queue<ScrollCell>();
        CachedCells = new List<ScrollCell>();

        for (int i = 0; i < CellCount; i++)
        {
            var cellObject = Instantiate(ScrollCellPrefab);
            var scrollCell = cellObject.GetComponent<ScrollCell>();
            scrollCell.SetupEmptyCell();
            scrollCell.transform.SetParent(CellHolder.transform);
            CachedCells.Add(scrollCell);
        }
    }
    
    public void AnchorToLatest()
    {
        Anchored = true;
        ScrollRect.verticalNormalizedPosition = 0f;
        //  scroll to bottom
        //  force all newly added cells to scroll view to bottom
    }

    public void ReleaseAnchor()
    {
        Anchored = false;
        //  stay where you are
        //  newly added cells will pop on the bttom without moving the list
    }
    
    public void AddCell(string text)
    {
        var cell = GetNextRecycledCell();

        //var rngText = Guid.NewGuid().ToString();

        //if (UnityEngine.Random.Range(0f,1f) > 0.3f)
        //    rngText += Guid.NewGuid().ToString();

        //if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
        //    rngText += Guid.NewGuid().ToString();


        //if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
        //    rngText += Guid.NewGuid().ToString();


        //if (UnityEngine.Random.Range(0f, 1f) > 0.3f)
        //    rngText += Guid.NewGuid().ToString();

        //cell.SetDisplay(rngText);


        cell.SetupTest();

        cell.RectTransform.SetAsLastSibling();

        //cell.RectTransform.sizeDelta = new Vector2(cell.RectTransform.sizeDelta.x, cell.RectTransform.sizeDelta.y * 2.0f);


        //if (Anchored)
        //    ScrollRect.verticalNormalizedPosition = 0f;

        ActiveCells.Enqueue(cell);
        LastActiveCell = cell;

        Debug.Log("============");
        Debug.Log("textBounds_size: " + cell.textBounds_size);
        Debug.Log("textBounds_center: " + cell.textBounds_center);
        Debug.Log("rectTransform_anchoredPostion: " + cell.rectTransform_anchoredPostion);
        Debug.Log("RectTransform.sizeDelta: " + cell.RectTransform.sizeDelta);
    }

    private ScrollCell GetNextRecycledCell()
    {
        if (CachedCells.Any())
        {
            var cachedCell = CachedCells.First();
            CachedCells.Remove(cachedCell);
            return cachedCell;
        }

        var oldestCell = ActiveCells.Dequeue();
        oldestCell.RecycleCell();
        return oldestCell;
    }
}

public interface IScrollViewController
{
    void AnchorToLatest();
    void ReleaseAnchor();
    void AddCell(ScrollCell cell);
    void DeactivateLinks(string id);
}
