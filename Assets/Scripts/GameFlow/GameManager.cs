using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject LoseImag, WonImage;
    [SerializeField]
    private Button twrA, twrB, twrC, startWaveBtn, fastFwdBtn;
    [SerializeField]
    private TextMeshProUGUI infoLabel, towerInfoLabel;
    [SerializeField]
    private TowerInteractionManager twrInteractionManager;
    [SerializeField]
    private WaveController levelManager;
    [SerializeField]
    private List<Wave_SO> waves = new List<Wave_SO>();


    private int CurrentWave = 0;
    private int PlayerMoney = 10;
    private int playerHP = 5;
    private bool waitingLastLevelEnd = false;

    public int PlayerHP { get => playerHP; }
    private bool isFastFwd = false;
    void Start()
    {
        GenerateWaves();

        SetButtonEvents();
        WaveController.OnWaveEnded += WaveController_OnWaveEnded;
        Enemy.OnEnemyDied += Enemy_OnEnemyDied;

    }
    private void SetButtonEvents()
    {
        // Set the tower selection button events
        twrA.onClick.AddListener(() =>
        {
            twrInteractionManager.CurrentType = Tower_SO.TowerType.Arrow;
        });
        twrB.onClick.AddListener(() =>
        {
            twrInteractionManager.CurrentType = Tower_SO.TowerType.Bomb;
        });
        twrC.onClick.AddListener(() =>
        {
            twrInteractionManager.CurrentType = Tower_SO.TowerType.Magic;
        });

        // Set the start wave button event
        startWaveBtn.onClick.AddListener(() =>
        {
            levelManager.StartWave(waves[CurrentWave]);
            startWaveBtn.interactable = false;
            CurrentWave++;
        });
        fastFwdBtn.onClick.AddListener(() =>
        {
            isFastFwd = !isFastFwd;
            if (isFastFwd)
            {
                Time.timeScale = 3.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        });
     
    }
    void Update()
    {
        UpdateLabelData();

        CheckWinLoseStatus();
    }

    private void CheckWinLoseStatus()
    {
        //Manage the win or lose cases
        if (PlayerHP <= 0)
        {
            LoseImag.SetActive(true);
            startWaveBtn.interactable = false;
        }
        if (CurrentWave == waves.Count - 1 && waitingLastLevelEnd)
        {
            if (levelManager.enemies.Count < PlayerHP)
            {
                WonImage.SetActive(true);
                waitingLastLevelEnd = false;
            }
        }
    }

    private void UpdateLabelData()
    {
        // Update the information displayed in the UI label
        infoLabel.text = $"Player HP: {PlayerHP}\n" +
            $"Money: ${PlayerMoney}\n" +
            $"Wave:{CurrentWave}";

        towerInfoLabel.text = $"Selected Tower: {twrInteractionManager.CurrentType}\n" +
            $"Cost ${twrInteractionManager.GetSelectedTowerCost()}";

    }

    //Only call to generate scriptableObjects for the waves
    private void GenerateWaves()
    {
        for (int i = 0; i < 10; i++)
        {
            waves[i].enemies.Clear();
            int multiplier = i + 1;
            waves[i].timeBetweenEnemies = 10 / multiplier * .25f;
            waves[i].enemies.Add(new Wave(multiplier * 3, Enemy_SO.EnemyType.Normal, multiplier * 1.25f));
            waves[i].enemies.Add(new Wave(multiplier * 2, Enemy_SO.EnemyType.Air));
            waves[i].enemies.Add(new Wave(multiplier * 1, Enemy_SO.EnemyType.Tank));
        }
    }
    private void WaveController_OnWaveEnded(int BonusMoney)
    {
        PlayerMoney += (int)(BonusMoney / 2);
        startWaveBtn.interactable = true;
        if (CurrentWave == waves.Count - 1)
        {
            startWaveBtn.interactable = false;
            waitingLastLevelEnd = true;
        }
    }
    private void Enemy_OnEnemyDied(Enemy enemy)
    {
        PlayerMoney += enemy.Reward;
    }


    public void DamagePlayer()
    {
        playerHP--;
    }
    public bool CanBuyTower()
    {
        return (twrInteractionManager.GetSelectedTowerCost() < PlayerMoney) && (PlayerHP > 0);
    }
    public void BuyTower()
    {
        PlayerMoney -= twrInteractionManager.GetSelectedTowerCost();
    }

}
