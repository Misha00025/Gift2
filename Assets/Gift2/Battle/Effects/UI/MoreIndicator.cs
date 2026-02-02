using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class MoreIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Image _backgroundImage;
    
    public void Initialize(int count, Sprite icon = null, Color? backgroundColor = null)
    {
        if (_countText != null)
        {
            _countText.text = $"+{count}";
        }
        
        if (_backgroundImage != null)
        {
            if (icon != null)
            {
                _backgroundImage.sprite = icon;
            }
            
            _backgroundImage.color = backgroundColor ?? Color.gray;
        }
    }
    
    public void SetCount(int count)
    {
        if (_countText != null)
        {
            _countText.text = $"+{count}";
        }
    }
}