using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    
}

[Serializable]
public class SelfData
{
    public int idSelf;
    public List<LayerItemBehavior> storageData;
}

[Serializable]
public class ItemData
{
    public int idItem;
}
