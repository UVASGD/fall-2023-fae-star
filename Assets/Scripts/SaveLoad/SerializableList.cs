using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableList<T>
{
    public List<T> list;

    public SerializableList(T[] list)
    {
        this.list = new List<T>(list);
    }
}
