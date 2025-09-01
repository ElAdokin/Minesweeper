using System.Collections.Generic;
using UnityEngine;

public class MathsUtilities
{
    public List<Vector3> CalculatePointsArroundCircle(int amountPositions, Vector3 center, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < amountPositions; i++)
        {
            float radians = 2 * Mathf.PI / amountPositions * i;

            float vertical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);

            Vector3 spawnDir = new Vector3(horizontal, vertical, 0);
            Vector3 newPoint = center + spawnDir * radius;

            points.Add(newPoint);
        }

        return points;
    }

    public Vector3 CalculatePointArroundUpMiddleCircle(Vector3 center, float radius)
    {
        float radians = Mathf.PI;

        float vertical = Mathf.Sin(radians);
        float horizontal = Mathf.Cos(radians);

        Vector3 spawnDir = new Vector3(horizontal, vertical, 0);
        Vector3 newPoint = center + spawnDir * radius;

        return newPoint;
    }

    public float CalculateHypotenuseOfTriangle(float aSide, float bSide)
    {
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(aSide, 2) + Mathf.Pow(bSide, 2));

        return hypotenuse;
    }

    public List<Vector3> CalculatePointsInsideSphere(int amountPositions, Vector3 center, float radius)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < amountPositions; i++)
        {
            Vector3 newPoint = Vector3.zero;

            float k = i + 0.5f;
            float r = Mathf.Sqrt((k) / amountPositions);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * k;
            float x = r * Mathf.Cos(theta) * radius;
            float y = r * Mathf.Sin(theta) * radius;

            newPoint = new Vector3(x, y, 0);

            points.Add(newPoint);
        }

        return points;
    }

    public Vector3 GetMiddlePositionWithTwoPoints(Vector3 point_01, Vector3 point_02)
    {
        Vector3 middlePoint = Vector3.Lerp(point_01, point_02, 0.5f); 
        return middlePoint;
    }

    public Vector3 GetPointInsideLine(Vector3 beginPosition, Vector3 endPosition, float factor)
    {
        Vector3 point = factor * Vector3.Normalize(endPosition - beginPosition) + beginPosition;

        return point;
    }

	public bool DistanceBetweenPointsLessThan(Vector3 firstPosition, Vector3 secondPosition, float distanceToCompare)
	{
		return Mathf.Sqrt((firstPosition - secondPosition).magnitude) < distanceToCompare * distanceToCompare;
	}

	public bool DistanceBetweenPointsLessThanOrEquals(Vector3 firstPosition, Vector3 secondPosition, float distanceToCompare)
	{
		return Mathf.Sqrt((firstPosition - secondPosition).magnitude) <= distanceToCompare * distanceToCompare;
	}

	public bool DistanceBetweenPointsMoreThan(Vector3 firstPosition, Vector3 secondPosition, float distanceToCompare)
	{
		return Mathf.Sqrt((firstPosition - secondPosition).magnitude) > distanceToCompare * distanceToCompare;
	}

	public bool DistanceBetweenPointsMoreThanOrEquals(Vector3 firstPosition, Vector3 secondPosition, float distanceToCompare)
	{
		return Mathf.Sqrt((firstPosition - secondPosition).magnitude) >= distanceToCompare * distanceToCompare;
	}

	public bool DistanceBetweenPointsEqualsTo(Vector3 firstPosition, Vector3 secondPosition, float distanceToCompare)
	{
		return Mathf.Sqrt((firstPosition - secondPosition).magnitude) == distanceToCompare * distanceToCompare;
	}

	public Vector3 CalculateBezier(Vector3 origin, Vector3 target, Vector3 startTangent, Vector3 endTanget, float time)
	{
		return (Mathf.Pow(1 - time, 3) * origin) + (3 * Mathf.Pow(1 - time, 2) * time * startTangent) + (3 * (1 - time) * time * time * endTanget) + (time * time * time * target);
	}

    public int CalculateDamageForCriticalHit(int criticalDamage, int damage)
    {
        return (int)(((float)criticalDamage) / 100 * damage);
    }

    public Vector2 WorldPositionToCanvasPosition(Camera activeCamera, RectTransform canvasRect, Vector3 worldPosition)
    {
        Vector2 viewportPosition = activeCamera.WorldToViewportPoint(worldPosition);
        Vector2 worldObject_ScreenPosition = new Vector2(
        ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        return worldObject_ScreenPosition;
    }

    public Vector3 WorldPostionToPerspectiveCameraPosition(Camera activeCamera, Vector3 worldPosition) 
    {
        Vector3 cameraRelative = activeCamera.transform.InverseTransformPoint(worldPosition);
        return cameraRelative;
    }
    
    public int GetPercentageFromValue(int value, int percentage)
    {
        return (int)(value * percentage) / 100;
    }

    public float GetValueFromCrossMultiply(float value, float referenceValue, float compareValue)
    {
        return (referenceValue * compareValue) / value;
    }

    public float GetValueFromInverseCrossMultiply(float value, float referenceValue, float compareValue)
    {
        return (referenceValue * value) / compareValue;
    }
}
