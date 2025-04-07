using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike", menuName = "Data/Item/Item Effects/ThunderStrike_Effect")]
public class ThunderStrike_Effect : ItemEffect
{   
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        //ToDo: SetUp new Thunder Strike
        Destroy(newThunderStrike, 1f);
    }
}
