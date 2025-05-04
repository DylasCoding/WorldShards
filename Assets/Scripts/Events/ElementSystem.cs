using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementSystem
{
    // element circle system, ex: fire, water, Wood, Light, Dark. fire > water > wood > fire, light > dark > fire > light
    public ElementDatabase elementDatabase;

    public enum ElementCollaborationType
    {
        FireDark,
        FireWater,
        WaterDark,
        WaterWood,
        WoodLight
    }

    private ElementCollaborationType elementCollaborationType;

    private void CompareElement(String element1, String element2)
    {
        ElementData elementData1 = elementDatabase.GetElementByName(element1);
        ElementData elementData2 = elementDatabase.GetElementByName(element2);

        if (elementData1 == null || elementData2 == null)
        {
            Debug.LogError("Element not found in database!");
            return;
        }
    }

    public GameObject GetElementCollaborationPrefab(String element1, String element2)
    {
        ElementData elementData1 = elementDatabase.GetElementByName(element1);
        ElementData elementData2 = elementDatabase.GetElementByName(element2);
        if (elementData1 == null || elementData2 == null)
        {
            Debug.LogError("Element not found in database!");
            return null;
        }

        if (elementData1.elementType == ElementData.ElementType.Fire && elementData2.elementType == ElementData.ElementType.Dark)
        {
            return elementDatabase.fireDarkPrefab;
        }
        else if (elementData1.elementType == ElementData.ElementType.Fire && elementData2.elementType == ElementData.ElementType.Water)
        {
            return elementDatabase.fireWaterPrefab;
        }
        else if (elementData1.elementType == ElementData.ElementType.Water && elementData2.elementType == ElementData.ElementType.Dark)
        {
            return elementDatabase.waterDarkPrefab;
        }
        else if (elementData1.elementType == ElementData.ElementType.Water && elementData2.elementType == ElementData.ElementType.Wood)
        {
            return elementDatabase.waterWoodPrefab;
        }
        else if (elementData1.elementType == ElementData.ElementType.Wood && elementData2.elementType == ElementData.ElementType.Light)
        {
            return elementDatabase.woodLightPrefab;
        }
        return null;
    }
}
