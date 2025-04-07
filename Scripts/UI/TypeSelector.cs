using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSelector : MonoBehaviour
{
    [SerializeField] private EquipmentType equipmentType_Filter;
    private void OnValidate()
    {
        gameObject.name = equipmentType_Filter.ToString() + "-Selector";
    }
}
