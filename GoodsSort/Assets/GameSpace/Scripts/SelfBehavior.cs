using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBehavior : MonoBehaviour
{
    private List<LayerItemBehavior> _listLayerItem;
    private SelfData selfData;

    public SelfBehavior InitData(SelfData selfData)
    {
        this.selfData = selfData;
        return this;
    }
}
