using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerosolController : MonoBehaviour
{
    public float deathTime = 4f; // 停留多久后死亡
    public VirusSlider virusSlider;

    private PlayerHealth _playerHealth;
    private float _lastEnterTime;

    private void Update()
    {
        if (_playerHealth != null)
        {
            if (Time.time > _lastEnterTime + deathTime)
            {
                _playerHealth.Death();
                _playerHealth = null;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerHealth = other.GetComponent<PlayerHealth>();
            _lastEnterTime = Time.time;
            if(virusSlider != null)
            {
                virusSlider.StartTimer(deathTime);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerHealth = null;
            if (virusSlider != null)
            {
                virusSlider.StopTimer();
            }
        }
    }
}