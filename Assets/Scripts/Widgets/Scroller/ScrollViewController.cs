using System.Collections.Generic;
using System.Linq;
using PixelSpace.Models.SharedModels.Helpers;
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
    
    public delegate void CellAddedEvent();
    public static event CellAddedEvent cellAddedEvent;
    
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

    //Flag to start the textTyper on the first cell
    private bool _first = true;
    private void AddCell(string encodedText)
    {
        var cell = GetNextRecycledCell();

        cell.gameObject.SetActive(true);
        var verticalSize = cell.SetupScrollCell(encodedText, _first);
        cell.RectTransform.localScale = Vector2.one;
        cell.RectTransform.SetSiblingIndex(CellCount - 1);

        ActiveCells.Enqueue(cell);
        LastActiveCell = cell;
        
        cell.RectTransform.sizeDelta = new Vector2 (cell.RectTransform.sizeDelta.x, verticalSize);
        _first = false;
        
        cellAddedEvent?.Invoke();
    }

    public void AddCells(IEnumerable<string> text)
    {
        text.ForEach(AddCell);
        //After all the cells have been added reset the first flag
        _first = true;
    }

    public void ClearScreen()
    {
        foreach (var cell in ActiveCells)
        {
            cell.gameObject.SetActive(false);
            CachedCells.Add(cell);
        }
        
        ActiveCells.Clear();
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
