using UnityEngine;

public static class UiUtilities
{
    public const int sortingOrderDefault = 5000;

    // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
    public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
    {
        return (int)(baseSortingOrder - position.y) + offset;
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Font font, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, font, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }


    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Font font, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text");
        
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPosition;
        
        TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        font = font == null ? Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") : font;
        textMesh.font = font;
        textMesh.GetComponent<MeshRenderer>().material = font.material;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        
        return textMesh;
    }
}
