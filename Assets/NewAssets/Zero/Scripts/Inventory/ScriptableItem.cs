using UnityEngine;

namespace Zero.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Zero/Item")]
    public class ScriptableItem : ScriptableObject
    {
        [Tooltip("道具名字")]
        public string itemName;
        [Tooltip("道具图标")]
        public Sprite icon;
        [Tooltip("道具详情"), TextArea]
        public string tooltip;
        [Tooltip("道具KEY")]
        public ItemKey itemKey;
    }
}