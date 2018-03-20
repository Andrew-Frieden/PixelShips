using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {

    public int CellCount = 20;
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

        var rngText = Guid.NewGuid().ToString();
        cell.SetDisplay(rngText);

        cell.RectTransform.SetAsLastSibling();

        if (Anchored)
            ScrollRect.verticalNormalizedPosition = 0f;

        ActiveCells.Enqueue(cell);
        LastActiveCell = cell;
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
