using System.Collections.Generic;
using System.Linq;
using EnumerableExtensions;
using Models.Actions;
using TextSpace.Events;
using UnityEngine;
using UnityEngine.UI;
using Widgets.Scroller;

public class ScrollViewController : MonoBehaviour {

    [SerializeField] private bool ScrollToBottom = false;
    [SerializeField] private int WordSpeed = 3;
    [SerializeField] private float TypeDelay = 0.1f;

    public int CellCount = 20;
    public GameObject ScrollCellPrefab;
    public GameObject CellHolder;
    
    private ScrollCell LastActiveCell;
    private ScrollRect ScrollRect;

    private Queue<ScrollCell> ActiveCells;
    private List<ScrollCell> CachedCells;
    
    private Vector2 _scrollViewPos;
    private RectTransform _scrollViewRectTransform;
    
    public delegate void CellAddedEvent();
    public static event CellAddedEvent cellAddedEvent;

    public bool RespondsToEventTags;

    private void Start()
    {
        UIResponseBroadcaster.UIResponseTagTrigger += RespondToEventTag;
    }

    private void RespondToEventTag(UIResponseTag tag)
    {
        if (!RespondsToEventTags)
            return;

        switch (tag)
        {
            case UIResponseTag.PlayerDamaged:
                Shake();
                break;
        }
    }

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
            scrollCell.GetComponentInChildren<ScrollCellTextTyper>().Characters_Per_Interval = WordSpeed;
            scrollCell.GetComponentInChildren<ScrollCellTextTyper>().Delay = TypeDelay;
            scrollCell.transform.SetParent(CellHolder.transform);
            scrollCell.gameObject.SetActive(false);
            CachedCells.Add(scrollCell);
        }
    }

    //Flag to start the textTyper on the first cell
    private bool _first = true;
    private void AddCell(TagString result)
    {
        var cell = GetNextRecycledCell();

        cell.gameObject.SetActive(true);
        var verticalSize = cell.SetupScrollCell(result, _first);
        //var verticalSize = cell.CellHeight;
        cell.RectTransform.localScale = Vector2.one;
        cell.RectTransform.SetSiblingIndex(CellCount);
        cell.Dim(false);
        ActiveCells.Enqueue(cell);
        LastActiveCell = cell;
        
        cell.RectTransform.sizeDelta = new Vector2 (cell.RectTransform.sizeDelta.x, verticalSize);
        _first = false;

        if (ScrollToBottom)
            Invoke(nameof(ScrollDown), 0.1f);

        cellAddedEvent?.Invoke();
    }

    private void ScrollDown()
    {
        ScrollRect.verticalNormalizedPosition = 0f;
    }

    public void AddCells(IEnumerable<TagString> text)
    {
        text.ForEach(AddCell);
        //After all the cells have been added reset the first flag
        _first = true;
    }

    public void DimCells()
    {
        foreach(var cell in ActiveCells)
        {
            cell.Dim(true);
        }
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

    public void DisableInteractions()
    {
        foreach (var cell in ActiveCells)
        {
            cell.DisableTouchEvents = true;
        }
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
    
    public void Shake()
    {
        _scrollViewRectTransform = GetComponent<RectTransform>();
        _scrollViewPos = new Vector2(_scrollViewRectTransform.anchoredPosition.x,
                                     _scrollViewRectTransform.anchoredPosition.y);
            
        InvokeRepeating(nameof(BeginShake), 0, 0.05f);
        Invoke(nameof(StopShake), 1.0f);
    }

    private void BeginShake()
    {
        var scrollViewPos = _scrollViewRectTransform.anchoredPosition;

        var offsetX = Random.value * 5;
        var offsetY = Random.value * 5;

        if (Random.Range(0, 100) > 50)
        {
            scrollViewPos.x += offsetX;
        }
        else
        {
            scrollViewPos.x -= offsetX;
        }
            
        if (Random.Range(0, 100) > 50)
        {
            scrollViewPos.y += offsetY;
        }
        else
        {
            scrollViewPos.y -= offsetY;
        }

        _scrollViewRectTransform.anchoredPosition = scrollViewPos;
    }

    private void StopShake()
    {
        CancelInvoke(nameof(BeginShake));
        _scrollViewRectTransform.anchoredPosition = _scrollViewPos;
    }
}

public interface IScrollViewController
{
    void AnchorToLatest();
    void ReleaseAnchor();
    void AddCell(ScrollCell cell);
    void DeactivateLinks(string id);
}
