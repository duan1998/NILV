using UnityEngine;
using Zero.Inventory;

namespace Zero.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Zero/Quest")]
    public class ScriptableQuest : ScriptableObject
    {
        [Tooltip("任务标题")]
        public string questTitle;
        [Tooltip("任务介绍"), TextArea]
        public string questText;

        [Header("需求")]
        [Tooltip("道具")]
        public ItemKey requireItemKey;
        [Tooltip("道具数量"), Range(min: 1, max: 999)]
        public int requireItemCount;

        /// <summary>
        /// 检查是否完成了任务
        /// </summary>
        public bool CheckComplete => InventoryManager.Instance.GetItemCount(requireItemKey) >= requireItemCount;
    }
}