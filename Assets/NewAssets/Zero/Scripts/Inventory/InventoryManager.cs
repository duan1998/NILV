using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zero.Utils;

namespace Zero.Inventory
{
    public enum ItemKey
    {
        Mask, // 口罩
        Snowman, // 雪人
        Beer, // 啤酒
        Photo // 照片
    }

    public class InventoryManager : Singleton<InventoryManager>
    {
        private Dictionary<ItemKey, int> Inventory { get; } = new Dictionary<ItemKey, int>();

        /// <summary>
        /// 获得道具数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetItemCount(ItemKey key)
        {
            return Inventory.TryGetValue(key, out var count) ? count : 0;
        }

        /// <summary>
        /// 是否拥有道具
        /// </summary>
        /// <param name="key">道具Key</param>
        /// <returns></returns>
        public bool HasItem(ItemKey key) => GetItemCount(key) > 0;

        /// <summary>
        /// 增加道具
        /// </summary>
        /// <param name="key">道具Key</param>
        /// <param name="amount">增加数量</param>
        public void AddItem(ItemKey key, int amount = 1)
        {
            if (Inventory.ContainsKey(key)) Inventory[key] += amount;
            else Inventory[key] = amount;
        }

        /// <summary>
        /// 移除道具
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRemoveAll"></param>
        public void RemoveItem(ItemKey key, bool isRemoveAll = false)
        {
            if (isRemoveAll) Inventory.Remove(key); // 直接移除道具
            else RemoveItem(key, 1); // 移除1个道具
        }

        /// <summary>
        /// 移除道具
        /// </summary>
        /// <param name="key"></param>
        /// <param name="amount">移除数量</param>
        public void RemoveItem(ItemKey key, int amount)
        {
            // 是否有足够的数量来删除
            if (GetItemCount(key) > amount)
            {
                // 减少道具数量
                Inventory[key] -= amount;
            }
            else
            {
                // 道具数量为0，直接移除道具
                Inventory.Remove(key);
            }
        }
    }
}