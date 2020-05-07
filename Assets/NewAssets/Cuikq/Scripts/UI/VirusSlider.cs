using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusSlider : MonoBehaviour
{
    Slider slider;
    GameObject player;
    public float offset = 0f;
    float remainingTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        gameObject.SetActive(false);
        slider.minValue = 0;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length != 1)
        {
            foreach(GameObject go in players)
            {
                if (go.name == "Player")
                {
                    player = go;
                    break;
                }
            }
        }
        if(player == null)
        {
            Debug.LogError("需要有一个Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + Vector3.up * offset;
        if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            slider.value = remainingTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void StartTimer(float time)
    {
        gameObject.SetActive(true);
        remainingTime = time;
        slider.maxValue = time;
    }

    public void StopTimer()
    {
        gameObject.SetActive(false);
        remainingTime = 0;
    }
}
