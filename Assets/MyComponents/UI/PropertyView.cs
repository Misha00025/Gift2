using TMPro;
using UnityEngine;

public class PropertyView : MonoBehaviour
{
    public SlicedFilledImage Filler;
    public TextMeshProUGUI Text;
    
    private Property _property;
    
    public void SetProperty(Property property)
    {
        if (_property != null)
            _property.Changed?.RemoveListener(OnChange);
            
        _property = property;
        _property.Changed?.AddListener(OnChange);
        Debug.Log($"Set Property: {_property.Value}/{_property.MaxValue}");
        OnChange(_property);
    }
    
    protected virtual void OnChange(Property property)
    {
        if (property.MaxValue != property.Value) 
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
        Filler.fillAmount = ((float)property.Value)/(float)property.MaxValue;
        if (Text != null)
        {
            Text.SetText($"{property.Value}/{property.MaxValue}");
        }
    }
}
