using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    BoxCollider2D boxCol;
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCtrl pc = collision.GetComponent<PlayerCtrl>();
            if(pc == null)
            {
                return;
            }
            if (pc.IsBearGoods)
            {

                TellPlayerWin(collision);

                LevelManager.Instance.LoadNextLevel();
            }
        }
    }

    private void TellPlayerWin(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Win();
        }
    }
}
