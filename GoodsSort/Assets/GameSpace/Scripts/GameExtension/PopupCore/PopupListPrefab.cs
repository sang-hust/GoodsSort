using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ListPopup", menuName = "ScriptableObjects/ListPopup", order = 3)]
public class PopupListPrefab : SerializedScriptableObject
{
    public Dictionary<Type, GameObject> dictionaryPopup = new Dictionary<Type, GameObject>();

#if UNITY_EDITOR
    [Button]
    public void UpdateListPopup()
    {
        string folderPath = "Assets/GameSpace/Prefabs/PopupPrefab"; // Đường dẫn tới folder chứa prefab
        dictionaryPopup.Clear(); // Xoá dictionary cũ nếu đã có

        // Lấy tất cả các file .prefab trong folder và các folder con
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            // Kiểm tra xem prefab có chứa component kế thừa từ IPopup không
            MonoBehaviour[] components = prefab.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var component in components)
            {
                if (component is IPopup)
                {
                    Type type = component.GetType();
                    if (!dictionaryPopup.ContainsKey(type))
                    {
                        dictionaryPopup.Add(type, prefab);
                    }
                    break; // Chỉ thêm prefab một lần dù có nhiều component IPopup
                }
            }
        }
    }
#endif
}