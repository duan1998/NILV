using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duan1998
{
    public class Door : MonoBehaviour
    {
        public GameObject m_outScene;
        public GameObject m_inScene;

        private PlayerCtrl m_player;
        private Goods m_goods;

        private void Awake()
        {
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
            m_goods = GameObject.FindGameObjectWithTag("Goods").GetComponent<Goods>();
        }
        public void OpenDoor()
        {
            if(IsShowDoorOut())
            {
                m_outScene.SetActive(false);
                m_inScene.SetActive(true);
                if(m_player.IsBearGoods)
                {
                    m_goods.transform.parent = m_inScene.transform;
                }
               
            }
            else
            {
                m_outScene.SetActive(true);
                m_inScene.SetActive(false);
                if(m_player.IsBearGoods)
                {
                    m_goods.transform.parent = m_outScene.transform;
                }
            }
        }

        private bool IsShowDoorOut()
        {
            return m_outScene.activeSelf;
        }
    }

}
