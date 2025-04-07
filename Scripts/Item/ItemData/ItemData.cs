using UnityEngine;
using System.Text;
using System.Collections.Generic;




#if UNITY_EDITOR
    using UnityEditor;
#endif
public enum ItemType
{
    Material,
    Equippment,
    Potion
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item/Item", order = 0)]
public class ItemData : ScriptableObject
{   
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemID;
    [Range(0,100)]
    public float DropChance;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftingMaterials = new List<InventoryItem>();
    protected StringBuilder stringBuilder = new StringBuilder();

    public virtual string GetBouns()
    {
        return "";
    }

    private void OnValidate() 
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
