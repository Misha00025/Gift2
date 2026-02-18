using UnityEngine;

public class PropertyView : MonoBehaviour
{
    public SlicedFilledImage Filler;
    
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
        Filler.fillAmount = ((float)property.Value)/(float)property.MaxValue;
    }
}
