using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zero.Dialogue;

namespace Duan1998
{
    public class PlayerCtrl : MonoBehaviour
    {
        public LayerMask m_groundLayer;
        public LayerMask m_goodsLayer;
        public LayerMask m_touchLayer;
        public LayerMask m_wallLayer;

        private Transform m_footPoint;
        private Transform m_headPoint;
        private Transform m_chestPoint;
        private Transform m_rightHandPoint;
        private Transform m_leftHandPoint;

        [SerializeField]
        private float m_footDetectionRadius = 0.1f;
        [SerializeField]
        private float m_handDetectionRadius = 0.1f;

        private static Collider2D[] s_detectionColloder = new Collider2D[100];
        private Rigidbody2D m_rigid2D;

        
        [SerializeField]
        private float m_norMoveSpeed = 150f;
        [SerializeField]
        private float m_bearMoveSpeed = 100f;
        [SerializeField]
        private float m_jumpForce = 5f;
        [SerializeField]
        private float m_bearGoodJF = 1f;


        private BearState m_curBearState;
        private FaceDir m_curFaceDir;

        [SerializeField]
        private bool m_isGround;

        public bool _IsGround => m_isGround;
        public bool _HorizontalMove
        {
            private set;
            get;
        }

        public bool IsBearGoods=> m_curBearState==BearState.Good;

        private bool m_isDrag;
        public bool _IsDrag => m_isDrag;

        public bool _LastFrameIsGround
        {
            get;
            private set;
        }


        public enum FaceDir
        {
            Right,
            Left
        }

        public UnityAction _FilpRight;
        public UnityAction _FilpLeft;

        public enum BearState
        {
            None,
            Good,

        }
     



        private void Awake()
        {
            m_footPoint = transform.Find("FootPoint");
            m_headPoint = transform.Find("HeadPoint");
            m_chestPoint = transform.Find("ChestPoint");
            m_rightHandPoint = transform.Find("RightHandPoint");
            m_leftHandPoint = transform.Find("LeftHandPoint");
            m_rigid2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            m_curBearState = BearState.None;
            m_curFaceDir = FaceDir.Right;
            m_isRun = true;
            m_isDrag = false;

        }


        bool jump;
        // Update is called once per frame
        void Update()
        {
            Talk();
            //一直被拉着，从未停止！奥里给！！！
            if (m_isDrag)
            {
                

            }
            else
            {
                _LastFrameIsGround = m_isGround;
                m_isGround = IsGround();

                jump = false;
                if (Input.GetKeyDown(KeyCode.Space) && m_isGround)
                {
                    jump = true;
                }
                float h = Input.GetAxis("Horizontal");



                int moveDir = 0;
                if (Mathf.Abs(h) >= 0.0005f)
                    moveDir = (int)Mathf.Sign(h);
                Movement(moveDir, h, jump);
                if (Input.GetKeyDown(KeyCode.J))
                {
                    if (m_curBearState == BearState.None)
                        PickUpGoods();
                    else if (m_curBearState == BearState.Good)
                        PutDownGoodsToGround();
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    StretchedHand();
                }
               
            }
        }

        void Movement(int moveDir,float h, bool jump)
        {
            if (moveDir == 1)
                FaceRight();
            else if(moveDir==-1)
                FaceLeft();
            Vector2 targetVelocity = m_rigid2D.velocity;
            targetVelocity.x = 0;
            if (moveDir != 0)
            {
                _HorizontalMove = true;
                if (m_curBearState == BearState.None)
                    targetVelocity.x = m_norMoveSpeed * h;
                else
                    targetVelocity.x = m_bearMoveSpeed * h;
            }
            else
                _HorizontalMove = false;
            if (jump)
            {
                if (m_curBearState == BearState.Good)
                    targetVelocity.y = m_bearGoodJF;
                else if(m_curBearState==BearState.None)
                    targetVelocity.y = m_jumpForce;
            }
            m_rigid2D.velocity = targetVelocity;
        }

        #region 十一 Talk
        private List<NPC> _npcList = new List<NPC>(); // 可交互NPC列表
        private List<PickupItem> _pickupItems = new List<PickupItem>(); // 可拾取道具列表
        void Talk()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (!UIManager.Instance.HasDialogueMessages())
                {
                    // 没有对话消息...

                    // 开始初始化对话消息
                    Talk talk;
                    if (_pickupItems.Count > 0)
                    {
                        // 先拾取道具
                        var pickupItem = _pickupItems[0];
                        talk = pickupItem.GetTalk();
                        pickupItem.Pickup();
                        _pickupItems.Remove(pickupItem); // 拾取后移除
                    }
                    else if (_npcList.Count > 0)
                    {
                        // 跟npc对话
                        var npc = _npcList[0];
                        talk = npc.GetTalk();
                    }
                    else return;

                    // 没有对话消息则返回
                    if (talk == null) return;

                    // 设置对话消息并显示对话
                    UIManager.Instance.DialogueInit(talk.GetDialogueMessages());
                    UIManager.Instance.DialogueInput();
                }
                else
                {
                    UIManager.Instance.DialogueInput();
                }
            }
        }

        public void AddNPC(NPC npc)
        {
            _npcList.Add(npc);
        }

        public void RemoveNPC(NPC npc)
        {
            _npcList.Remove(npc);
        }

        public void AddPickupItem(PickupItem pickupItem)
        {
            _pickupItems.Add(pickupItem);
        }

        public void RemovePickupItem(PickupItem pickupItem)
        {
            _pickupItems.Remove(pickupItem);
        }

        #endregion



        bool m_isRun = false;
        private void OnDrawGizmos()
        {
            if(m_isRun)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(m_footPoint.position, m_footDetectionRadius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(m_chestPoint.position, new Vector3(m_detectionGoodsSize.x, m_detectionGoodsSize.y, 0));
                Gizmos.DrawLine(m_chestPoint.position, m_chestPoint.position + new Vector3(m_detectionGoodsDistance, 0, 0));
                Gizmos.DrawLine(m_chestPoint.position, m_chestPoint.position + new Vector3(-m_detectionGoodsDistance, 0, 0));
                Gizmos.color = Color.green;
                if (m_curFaceDir == FaceDir.Right)
                    Gizmos.DrawWireSphere(m_rightHandPoint.position, m_handDetectionRadius);
                else
                    Gizmos.DrawWireSphere(m_leftHandPoint.position, m_handDetectionRadius);
                if(m_goods!=null)
                {
                    BoxCollider2D box2D = m_goods.transform.Find("Trigger").GetComponent<BoxCollider2D>();
                    Gizmos.DrawWireCube(m_chestPoint.position-new Vector3(box2D.size.x/2,0,0), box2D.size);
                }
            }
        }
        private bool IsGround()
        {
            
            int num=Physics2D.OverlapCircleNonAlloc(m_footPoint.position, m_footDetectionRadius, s_detectionColloder,m_groundLayer);
            if(num>0)
            {
                return true;
            }
            return false;
        }


        private void FaceRight()
        {
            if (m_curFaceDir == FaceDir.Right)
                return;
            else
            {
                //动画方面
                _FilpRight();
                m_curFaceDir = FaceDir.Right;
            }
        }
        private void FaceLeft()
        {
            if (m_curFaceDir == FaceDir.Left)
                return;
            else
            {
                _FilpLeft();
                m_curFaceDir = FaceDir.Left;
            }
        }




        private Goods m_goods;
        [SerializeField]
        private Vector2 m_detectionGoodsSize;
        [SerializeField]
        private float m_detectionGoodsDistance;
        void PickUpGoods()
        {
            RaycastHit2D hit2D=default;
            if (m_curFaceDir == FaceDir.Left)
                hit2D = Physics2D.BoxCast(m_chestPoint.position, m_detectionGoodsSize, 0, Vector2.left, m_detectionGoodsDistance,m_goodsLayer);
            else if(m_curFaceDir==FaceDir.Right)
                hit2D= Physics2D.BoxCast(m_chestPoint.position, m_detectionGoodsSize, 0, Vector2.right, m_detectionGoodsDistance, m_goodsLayer);
            if(hit2D.collider!=null)
            {
                if(hit2D.collider.CompareTag("Goods"))
                {
                    m_goods = hit2D.collider.gameObject.GetComponent<Goods>();
                    if(m_goods._CouldTrigger)
                    {
                        m_goods.gameObject.SetActive(false);
                        m_curBearState = BearState.Good;
                    }
                }
            }
        }


        [SerializeField]
        private float m_goodsDistance=0.3f;
        void PutDownGoods(Vector3 targetPos)
        {
            if(m_goods!=null)
            {
                m_goods.gameObject.SetActive(true);
                m_curBearState = BearState.None;
                m_goods.transform.position= targetPos;
            }
        }

        void PutDownGoodsToGround()
        {
            Vector3 targetPos;
            if(GetSuitGoodPosition(out targetPos))
            {
                PutDownGoods(targetPos);
                m_goods = null;
            }
            else
            {
                //无法放下背包
            }
        }
        bool GetSuitGoodPosition(out Vector3 pos)
        {
            pos = m_chestPoint.position;
            Vector3 originPos, endPos;
            BoxCollider2D goodsBox2D = m_goods.transform.Find("Trigger").GetComponent<BoxCollider2D>();
            if (m_curFaceDir==FaceDir.Right)
            {
                originPos = m_chestPoint.position + new Vector3(m_goodsDistance, 0, 0);
                endPos = m_chestPoint.position + new Vector3(-m_goodsDistance, 0, 0);
            }
            else
            {
                originPos = m_chestPoint.position + new Vector3(-m_goodsDistance, 0, 0);
                endPos = m_chestPoint.position + new Vector3(m_goodsDistance, 0, 0);
            }
            Vector3 curPosition = originPos;
            //增量
            float t = 0.01f;
            while(true)
            {
                float sign = Mathf.Sign(curPosition.x - transform.position.x);

                if(sign<0)
                {
                    Debug.Log("666");
                }
                RaycastHit2D hit = Physics2D.BoxCast(m_chestPoint.position+new Vector3(sign*(goodsBox2D.size.x/2),0,0), goodsBox2D.size, 0, Vector2.right*Mathf.Sign(curPosition.x-transform.position.x),Mathf.Abs(curPosition.x- m_chestPoint.position.x), m_wallLayer);
                if(hit.collider!=null)
                {
                    //说明会碰到墙，那就换个位置
                    if (endPos.x > originPos.x)
                        curPosition.x += t;
                    else
                        curPosition.x -= t;
                    if(Mathf.Abs(curPosition.x-endPos.x)<0.01f)
                    {
                        break;
                    }
                }
                else
                {
                    pos = curPosition;
                    return true;
                }
            }
            return false;
        }

        

        void PutDownGoodsToClaw(Claw claw)
        {
            m_goods.BeDrag(claw);
            PutDownGoods(claw._HandlePosition);
            m_goods = null;
        }

        void StretchedHand()
        {


            int num=0;
            if(m_curFaceDir==FaceDir.Right)
                num=Physics2D.OverlapCircleNonAlloc(m_rightHandPoint.position, m_handDetectionRadius,s_detectionColloder, m_touchLayer);
            else
                num = Physics2D.OverlapCircleNonAlloc(m_leftHandPoint.position, m_handDetectionRadius, s_detectionColloder, m_touchLayer);
            if (num>0)
            {
                if(s_detectionColloder[0].CompareTag("Claw"))
                {
                    Claw claw = s_detectionColloder[0].GetComponentInParent<Claw>();
                    if(claw._CouldUse)
                    {
                        //如果有物资，不好意思，物资先上
                        if (m_curBearState == BearState.Good)
                        {
                            PutDownGoodsToClaw(claw);
                        }
                        else
                        {
                            ComeIntoSpeciallyState();
                            claw._OnCompete = RecoveryFromSpeciallyState;
                            //claw
                            claw.UpwardMotion(this.transform);
                        }
                    }
                }
                else if(s_detectionColloder[0].CompareTag("Door"))
                {
                    Door door = s_detectionColloder[0].GetComponent<Door>();
                    door.OpenDoor();
                }
            }
        }
       



        //比如被拉着飞起来！！！
        void ComeIntoSpeciallyState()
        {
            m_isDrag = true;
            GetComponentInChildren<CapsuleCollider2D>().enabled = false;
            m_rigid2D.simulated = false;
            m_isDrag = true;
        }
        void RecoveryFromSpeciallyState()
        {
            m_isDrag = false;
            GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            m_rigid2D.simulated = true;
            m_isDrag = false;
        }


    }
}

