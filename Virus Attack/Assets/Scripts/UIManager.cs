using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region update data
    [SerializeField] Slider S_HealthBar;
    [SerializeField] TextMeshProUGUI T_Score;
    [SerializeField] GameObject GameOverPanel;
    public Toggle ServerOnlineToggle;
    internal void UpdateScore(int value)
    {
        T_Score.text = "Score : " + value;
    }
    internal void UpdateHealth(int value)
    {
        //S_HealthBar.value = value;
        LeanTween.value(S_HealthBar.value, value, 0.5f).setEaseInSine().setOnUpdate((value) => S_HealthBar.value = value);
    }

    #endregion
    public void OnGameOver()
    {
        Debug.Log("Game Over");
        GameManager.Instance.serverIsOnline = false;
        ServerOnlineToggle.isOn = false;
        ServerOnlineToggle.interactable = false;
        GameOverPanel.SetActive(true);
    }
}
