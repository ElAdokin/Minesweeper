using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RendererExtensionsUI
{
    private static int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        Vector3 tempScreenSpaceCorner;

        for (var i = 0; i < objectCorners.Length; i++)
        {
            tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); 
            if (screenBounds.Contains(tempScreenSpaceCorner)) 
            {
                visibleCorners++;
            }
        }
        return visibleCorners;
    }

    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) == 4;
    }

    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) > 0;
    }

    public static bool Overlaps(RectTransform a, RectTransform b)
    {
        return WorldRect(a).Overlaps(WorldRect(b));
    }
    public static bool Overlaps(RectTransform a, RectTransform b, bool allowInverse)
    {
        return WorldRect(a).Overlaps(WorldRect(b), allowInverse);
    }

    public static Rect WorldRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }

	public static bool IsPointerOverUIElement()
	{
		return IsPointerOverUIElement(GetEventSystemRaycastResults());
	}

	public static bool IsPointerOverUIElement(Vector2 checkerPosition)
	{
		return IsPointerOverUIElement(GetEventSystemRaycastResults(checkerPosition));
	}

	public static GameObject UIElementOver()
	{
		return OverUIElement(GetEventSystemRaycastResults());
	}

	public static GameObject UIElementOver(Vector2 checkPosition)
	{
		return OverUIElement(GetEventSystemRaycastResults(checkPosition));
	}

	public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
	{
		for (int index = 0; index < eventSystemRaysastResults.Count; index++)
		{
			RaycastResult curRaysastResult = eventSystemRaysastResults[index];

			if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
			{
				return true;
			}
		}

		return false;
	}

	public static GameObject OverUIElement(List<RaycastResult> eventSystemRaysastResults)
	{
		for (int index = 0; index < eventSystemRaysastResults.Count; index++)
		{
			RaycastResult curRaysastResult = eventSystemRaysastResults[index];
			if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
			{
				return curRaysastResult.gameObject;
			}
		}

		return null;
	}

	public static List<RaycastResult> GetEventSystemRaycastResults()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		List<RaycastResult> raysastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raysastResults);
		return raysastResults;
	}

	public static List<RaycastResult> GetEventSystemRaycastResults(Vector2 checkerPosition)
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = checkerPosition;
		List<RaycastResult> raysastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raysastResults);
		return raysastResults;
	}

	public static GameObject GetUIElementClicked(List<RaycastResult> eventSystemRaysastResults)
	{
		for (int index = 0; index < eventSystemRaysastResults.Count; index++)
		{
			RaycastResult curRaysastResult = eventSystemRaysastResults[index];

			if (curRaysastResult.gameObject != null && curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
			{
				return curRaysastResult.gameObject;
			}
		}

		return null;
	}
}
