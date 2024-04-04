using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.Serialization;
using UnityEngine;

public struct DataPlayer
{
    public int day;
    public int level;
    public ItemMoveMode modeMoveItem;
}

public class WrapperData : IInitialize
{
    public DataPlayer Client => _client;
    private DataPlayer _client;

    
    public void OnInitialize()
    {
        var data = PlayerPrefs.GetString(nameof(DataPlayer), string.Empty);
        _client = !string.IsNullOrEmpty(data)
            ? JsonConvert.DeserializeObject<DataPlayer>(data)
            : new DataPlayer()
            {
                day = 0,
                level = 0,
                modeMoveItem = ItemMoveMode.DragDrop
            };
        _client.level = 0;
    }

    private void SaveOnClient()
    {
        PlayerPrefs.SetString(nameof(DataPlayer), JsonConvert.SerializeObject(_client));
    }
    
    public void ModifyLevel(int value)
    {
        _client.level += value;
        SaveOnClient();
    }

    public void ModifyModeItemMove(ItemMoveMode itemMoveMode)
    {
        _client.modeMoveItem = itemMoveMode;
        SaveOnClient();
    }
}

[Serializable]
public class SelfData
{
    public int idSelf;
    public List<LayerData> listLayerData;
    public SelfData(int idSelf, List<LayerData> listLayerData)
    {
        this.idSelf = idSelf;
        this.listLayerData = listLayerData;
    } 
}

[Serializable]
public class LayerData
{
    public List<ItemData> listItemData;

    public LayerData(List<ItemData> listItemData)
    {
        this.listItemData = listItemData;
    }
}

[Serializable]
public class ItemData
{
    
    public ItemTypeEnum itemType;
    public ItemData(ItemTypeEnum itemTypeEnum)
    {
        itemType = itemTypeEnum;
    }
}
