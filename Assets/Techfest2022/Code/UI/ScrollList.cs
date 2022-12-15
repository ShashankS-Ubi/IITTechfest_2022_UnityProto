using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface IContext
{
    public void SetContext(object obj);    
}
public class ScrollList : MonoBehaviour
{
    [SerializeField]
    private Transform scrollParent;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private List<IContext> cells;
    
    [HideInInspector]
    public ScrollRect scrollRect;

    private List<GameObject> cellObjects;

    private RectTransform contentRect;
    public float cellHeight;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (scrollParent == null)
        {
            Debug.LogError("Scroll parent should'nt be null");
            return;
        }
        scrollRect = this.GetComponent<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.Log("Scroll Rect Missing");
            return;
        }
        contentRect = scrollParent.GetComponent<RectTransform>();
        cellHeight = cellPrefab.GetComponent<RectTransform>().sizeDelta.y;
        cells = new List<IContext>();
    }


    public void Draw<T>(List<T> t)
    {
        if (scrollParent == null)
        {
            return;
        }
        if (cells == null)
        {
            cells = new List<IContext>();
        }
        if (cellObjects == null)
        {
            cellObjects = new List<GameObject>();
        }
        for (int i = 0; i < t.Count; i++)
        {
            GameObject obj = (GameObject)Instantiate(cellPrefab, scrollParent);
            IContext context = obj.GetComponent<IContext>();
            context.SetContext(t[i]);
            cells.Add(context);
            cellObjects.Add(obj);
        }

    }

    public List<IContext> Cells
    {
        get
        {
            return cells;
        }
    }
    public List<GameObject> CellObjects
    {
        get
        {
            return cellObjects;
        }
    }

}

