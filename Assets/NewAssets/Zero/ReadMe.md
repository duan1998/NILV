# ZERO

## 预设介绍

### UIManager(UI管理器)

作用: 控制游戏内的UI, 对话/拾取提示等都需要此预设

路径: Assets/NewAssets/Zero/Prefabs/UI/UIManager.prefab

------

### DialogueItem(对话时显示的道具)

作用: 对话时显示的道具,

制作: 在"UIManager/DialoguePanel/Content/ShowItem"下创建显示内容, 制作完成后可以拖成预设物体

路径: Assets/NewAssets/Zero/Prefabs/UI/Dialogue/Items/*

------

### NPC

作用: 主要负责对话提供信息, 以及提供任务

路径: Assets/NewAssets/Zero/Prefabs/NPC/*

------

### PickupItem(可拾取道具)

作用: 大部分道具为任务需求道具, 口罩用处不一样

路径: Assets/NewAssets/Zero/Prefabs/PickupItem/*

------



## 制作NPC

方式1:

1. 创建NPC游戏对象,
2. 添加"2D触发器/NPC"
3. 增加对话脚本, 重复对话使用"LoopTalk"/任务对话使用"QuestTalk"

方式2:

1. 复制已存在的NPC对象
2. 根据需求需改对话脚本



## 制作PickupItem

方式1:

1. 创建道具游戏对象,
2. 添加"2D触发器/PickupItem"
3. 增加对话脚本, 重复对话使用"LoopTalk"/任务对话使用"QuestTalk"

方式2:

1. 复制已存在的道具对象
2. 根据需求需改对话脚本



## 对话消息

Message

- Size(决定对话有多少页消息)
- Element X(页消息)
  - Is Hold Previous Item(勾选后, 不会更改对话道具的显示)
  - Dialogue Item(对话时显示的道具)
  - Message(消息内容)