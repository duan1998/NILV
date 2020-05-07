namespace Zero.Quest
{
    public class Quest
    {
        public ScriptableQuest Data { get; }

        /// <summary>
        /// 是否已经接收
        /// </summary>
        public bool IsAccepted { get; private set; }

        /// <summary>
        /// 是否完成了任务
        /// </summary>
        public bool IsCompleted { get; private set; } = false;

        public Quest(ScriptableQuest data)
        {
            Data = data;
        }

        /// <summary>
        /// 接受任务
        /// </summary>
        public void Accept()
        {
            // 已经接受
            if (IsAccepted) return;

            // 接受任务
            IsAccepted = true;
        }


        /// <summary>
        /// 完成任务
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
        }

        public bool CheckComplete => Data.CheckComplete;
    }
}