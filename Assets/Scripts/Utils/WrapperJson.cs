using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wrapper<T>
{
    public List<T> Items;
    public Wrapper(List<T> items) => Items = items;
}