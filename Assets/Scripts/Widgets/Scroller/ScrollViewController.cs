using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {

    public int CellCount = 2;
    public GameObject ScrollCellPrefab;
    public GameObject CellHolder;
    
    private bool Anchored = true;
    private ScrollCell LastActiveCell;

    private ScrollRect ScrollRect;

    private Queue<ScrollCell> ActiveCells;
    private List<ScrollCell> CachedCells;
    
	// Use this for initialization
	void Awake () {

        ScrollRect = GetComponent<ScrollRect>();

        if (ScrollCellPrefab == null)
            throw new MissingComponentException("ScrollViewController -> no ScrollCell prefab set");
            
        BuildCache();
	}
    
    void BuildCache()
    {
        ActiveCells = new Queue<ScrollCell>();
        CachedCells = new List<ScrollCell>();

        for (int i = 0; i < CellCount; i++)
        {
            var cellObject = Instantiate(ScrollCellPrefab);
            var scrollCell = cellObject.GetComponent<ScrollCell>();
            scrollCell.transform.SetParent(CellHolder.transform);
            scrollCell.gameObject.SetActive(false);
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
    
    public void AddCell(ITextEntity entity)
    {
        var cell = GetNextRecycledCell();

        cell.SetupScrollCell(entity);
        cell.RectTransform.localScale = Vector2.one;
        cell.RectTransform.SetAsLastSibling();
        cell.gameObject.SetActive(true);

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
