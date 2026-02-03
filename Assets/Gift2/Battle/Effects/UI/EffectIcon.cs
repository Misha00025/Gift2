using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class EffectIcon : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private GameObject _countBadge;
    
    [Header("Настройки")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _stackedColor = Color.white;
    [SerializeField] private bool _pulseOnNewStack = true;
    
    private IEffect _effect;
    private int _currentCount = 1;
    private Vector3 _originalScale;
    
    void Awake()
    {
        if (_iconImage == null)
            _iconImage = GetComponent<Image>();
        
        _originalScale = transform.localScale;
        
        // Скрываем бейдж количества по умолчанию
        if (_countBadge != null)
            _countBadge.SetActive(false);
    }
    
    public void Initialize(IEffect effect)
    {
        _effect = effect;
        UpdateDisplay();
    }
    
    public void SetCount(int count)
    {
        if (count == _currentCount) return;
        
        _currentCount = count;
        UpdateDisplay();
        
        // Анимация при изменении количества
        if (_pulseOnNewStack && count > 1)
        {
            PulseAnimation();
        }
    }
    
    public void UpdateDisplay()
    {
        if (_effect == null) return;
        
        // Устанавливаем иконку
        _iconImage.sprite = _effect.Icon;
        
        // Обновляем отображение количества
        bool showCount = _currentCount > 1;
        
        if (_countBadge != null)
            _countBadge.SetActive(showCount);
        
        if (_countText != null && showCount)
        {
            _countText.text = _currentCount.ToString();
            
            // Автоматически меняем размер шрифта для больших чисел
            if (_currentCount >= 100)
                _countText.fontSize = 8;
            else if (_currentCount >= 10)
                _countText.fontSize = 10;
            else
                _countText.fontSize = 12;
        }
        
        // Меняем цвет для эффектов со стеками
        _iconImage.color = _currentCount > 1 ? _stackedColor : _defaultColor;
    }
    
    private void PulseAnimation()
    {
        // Простая анимация пульсации
        // LeanTween.cancel(gameObject);
        // transform.localScale = _originalScale * 1.2f;
        // LeanTween.scale(gameObject, _originalScale, 0.3f)
        //     .setEase(LeanTweenType.easeOutBack);
    }
    
    public IEffect Effect => _effect;
    public int CurrentCount => _currentCount;
}