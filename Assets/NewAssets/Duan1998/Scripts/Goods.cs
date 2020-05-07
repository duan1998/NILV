using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    public class Goods : MonoBehaviour
    {

        private Rigidbody2D m_rigid2D;
        private BoxCollider2D m_box2D;
        public bool _CouldTrigger//比如在升降途中不可以背起来
        {
            get;
            private set;
        }

        private void Awake()
        {
            m_rigid2D = GetComponent<Rigidbody2D>();
            m_box2D = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            _CouldTrigger = true;
        }

        public void BeDrag(Claw claw)
        {
            claw._OnCompete = RecoveryFromSpeciallyStateForGoods;
            claw.UpwardMotion(this.transform);
            ComeIntoSpeciallyStateForGoods();
        }

        void ComeIntoSpeciallyStateForGoods()
        {
            m_box2D.enabled = false;
            m_rigid2D.simulated = false;
            _CouldTrigger = false;
        }
        void RecoveryFromSpeciallyStateForGoods()
        {
            m_box2D.enabled = true;
            m_rigid2D.simulated = true;
            _CouldTrigger = true;
        }
    }

}
