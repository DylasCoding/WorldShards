using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementDatabase", menuName = "GameData/Element Database")]
public class ElementDatabase : ScriptableObject
{
    [Header("Element Database")]
    public List<ElementData> elements;

    [Header("Element Collaboration")]
    public GameObject fireDarkPrefab;
    public GameObject fireWaterPrefab;
    public GameObject waterDarkPrefab;
    public GameObject waterWoodPrefab;
    public GameObject woodLightPrefab;

    public ElementData GetElementByName(string name)
    {
        foreach (ElementData element in elements)
        {
            if (element.elementType.ToString() == name)
            {
                return element;
            }
        }
        return null;
    }
}
