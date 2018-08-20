using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {

    public int CellCount = 20;
    public GameObject ScrollCellPrefab;
    public GameObject CellHolder;
    
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
    
    public void AddCell(ITextEntity entity)
    {
        var cell = GetNextRecycledCell();

        cell.gameObject.SetActive(true);
        cell.SetupScrollCell(entity);
        cell.RectTransform.localScale = Vector2.one;
        cell.RectTransform.SetSiblingIndex(CellCount - 1);

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
