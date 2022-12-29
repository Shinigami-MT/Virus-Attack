using UnityEngine;
using System;
using UnityEngine.UI;
using Lean.Pool;

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour
{
    #region Singleton and Statics
    public static GameManager Instance;
    public static Server Server;
    public static LeanGameObjectPool DataPool;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Server = FindObjectOfType<Server>();
        DataPool = FindObjectOfType<LeanGameObjectPool>();
        if (Server == null)
        {
            Debug.LogError("No server in scene");
        }
        if (DataPool == null)
        {
            Debug.LogError("No Leanpool in scene");
        }
    }
    #endregion

    public bool serverIsOnline { get; set; }
    public bool FirewallMode { get; set; }
    public event Action onServerOnline;
    private int _score;
    public int Score { get { return _score; } set
        {
            _score = value;
            UIManager.Instance.UpdateScore(value);
        } }

    public void ToggleServerState()
    {
        serverIsOnline = !serverIsOnline;
        if(serverIsOnline)
            onServerOnline?.Invoke();
    }
    
    public void RepairServer()
    {
        Score = 0;
        Server.Health = 10;
        UIManager.Instance.ServerOnlineToggle.interactable = true;
    }


    #region Extra
    public void SetFrameRate(Slider FPS)
    {
        Application.targetFrameRate = (int)FPS.value;
    }
    public void SetRedChance(Slider chanceForRed)
    {
        DataObject.ChanceForRed = (int)chanceForRed.value;
    }
    #endregion
}
