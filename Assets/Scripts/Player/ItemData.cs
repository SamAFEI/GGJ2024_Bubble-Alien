using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ItemData : MonoBehaviour
    {
        public ItemEnum itemType;
        public float DurationTime;
    }
    
    public enum ItemEnum
    {
        PowerUP, SpeedUP, Strong, Super
    }
}
