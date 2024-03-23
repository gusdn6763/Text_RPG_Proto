using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemAreaUI : UIScript
{
    private List<Area> areas = new List<Area>();

    private void Start()
    {
        Area[] foundAreas = GetComponentsInChildren<Area>(true);
        areas.AddRange(foundAreas);
    }

    public Area FindArea(Vector2 position)
    {
        foreach (Area area in areas)
            if (RectTransformUtility.RectangleContainsScreenPoint(area.RectTrans, position) && area.gameObject.activeSelf)
                return area;

        return null;
    }
}
