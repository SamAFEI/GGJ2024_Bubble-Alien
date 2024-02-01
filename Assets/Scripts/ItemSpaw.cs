using Assets.Scripts.Player;
using UnityEngine;

public class ItemSpaw : MonoBehaviour
{
    public void SpawItem()
    {
        ItemData[] _itmes = Resources.LoadAll<ItemData>("Prefabs/Item/");
        ItemData _item = _itmes[Random.Range(0, _itmes.Length)];
        Instantiate(_item, this.transform.position, Quaternion.identity ,this.transform);
    }
}
