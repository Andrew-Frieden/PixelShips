using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollViewController : MonoBehaviour {

    public int CellCount = 20;
    public GameObject ScrollCellPrefab;
    public GameObject CellHolder;
    
    private bool Anchored = true;
    private ScrollCell LastActiveCell;
    private float LastCellHeight { get { return LastActiveCell.RectTransform.rect.height; }}

    private Queue<ScrollCell> ActiveCells;
    private List<ScrollCell> CachedCells;
    
	// Use this for initialization
	void Start () {

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
        //  scroll to bottom
        //  force all newly added cells to scroll view to bottom
    }

    public void ReleaseAnchor()
    {
        //  stay where you are
        //  newly added cells will pop on the bttom without moving the list
    }
    
    public void AddCell(string text)
    {
        var cell = GetNextRecycledCell();


        var rngText = Guid.NewGuid().ToString();
        
        cell.SetDisplay(rngText);

        var newCellHeight = cell.RectTransform.rect.height;
        //cell.RectTransform.localPosition = Vector2.zero;
        cell.RectTransform.anchoredPosition = Vector2.zero;
        //cell.RectTransform.position = Vector3.zero;
        
        foreach (var activeCell in ActiveCells)
        {
            activeCell.RectTransform.localPosition += new Vector3(0, newCellHeight, 0);
        }
        
        if (LastActiveCell != null)
        {
            //Debug.Log("last h:" + LastCellHeight);
            //Debug.Log("last cell x: " + LastActiveCell.RectTransform.localPosition);
            //cell.RectTransform.localPosition = new Vector2(0, LastActiveCell.RectTransform.localPosition.y + LastCellHeight);
            
        }
        else
        {
        }
        
        ActiveCells.Enqueue(cell);
        LastActiveCell = cell;
        
       // cell.RectTransform.localPosition = Vector2.zero;
        
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
