using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image _healthBarFill; 
    [SerializeField] private DamageTaker _playerDamageTaker;
    [Header("Boss")]
    
    [SerializeField] private GameObject _lancelotHud;
    [SerializeField] private GameObject _baldulfHud;
    [SerializeField] private GameObject _morganeHud;

    [SerializeField] private Image _lancelotHealthBarFill;
    [SerializeField] private Image _baldulfHealthBarFill;
    [SerializeField] private Image _morganeHealthBarFill;
    private DamageTaker _bossDamageTaker;
    
    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playerDamageTaker != null)
        {
            _playerDamageTaker.OnHealthChanged.AddListener(UpdateHealthBar);
            UpdateHealthBar(_playerDamageTaker._hp); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealthBar(float currentHealth)
    {
        if (_healthBarFill != null && _playerDamageTaker != null)
        {
            float maxHealth = _playerDamageTaker.HpMax;
            
            float healthRatio = currentHealth / maxHealth;
            
            _healthBarFill.fillAmount = healthRatio;
        }
    }
    
    public void SetDamageTaker(DamageTaker damageTaker)
    {
        if (_playerDamageTaker != null)
            _playerDamageTaker.OnHealthChanged.RemoveListener(UpdateHealthBar);

        _playerDamageTaker = damageTaker;
        _playerDamageTaker.OnHealthChanged.AddListener(UpdateHealthBar);
        UpdateHealthBar(_playerDamageTaker._hp);
    }
    
    public void SetBossDamageTaker(DamageTaker damageTaker, string bossName)
    {
        if (_bossDamageTaker != null)
        {
            _bossDamageTaker.OnHealthChanged.RemoveListener(UpdateBossHealthBar);
            _bossDamageTaker._onDeath.RemoveListener(HideBossHud);
        }

        _bossDamageTaker = damageTaker;
        _bossDamageTaker.OnHealthChanged.AddListener(UpdateBossHealthBar);
        _bossDamageTaker._onDeath.AddListener(HideBossHud);
        
        string lowerName = bossName.ToLower();
        
        _lancelotHud?.SetActive(lowerName.Contains("lancelot"));
        _baldulfHud?.SetActive(lowerName.Contains("baldulf"));
        _morganeHud?.SetActive(lowerName.Contains("morgane"));

        UpdateBossHealthBar(_bossDamageTaker._hp);
        Debug.Log($"[HUD] HUD activé pour : {bossName}");
    }

    private void UpdateBossHealthBar(float currentHealth)
    {
        if (_bossDamageTaker == null) return;
        float ratio = currentHealth / _bossDamageTaker.HpMax;

        // Mettre à jour uniquement la barre active
        if (_lancelotHud.activeSelf) _lancelotHealthBarFill.fillAmount = ratio;
        if (_baldulfHud.activeSelf)  _baldulfHealthBarFill.fillAmount  = ratio;
        if (_morganeHud.activeSelf)  _morganeHealthBarFill.fillAmount  = ratio;
    }

    private void HideBossHud()
    {
        _lancelotHud?.SetActive(false);
        _baldulfHud?.SetActive(false);
        _morganeHud?.SetActive(false);
    }
    
    public void ShowGameOver()
    {
        _gameOverPanel?.SetActive(true);
    }
}
