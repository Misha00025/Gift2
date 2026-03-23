using TMPro;
using UnityEngine;

namespace Wof.Types.UI
{
    public class PropertyView : MonoBehaviour
    {
        public SlicedFilledImage Filler;
        public TextMeshProUGUI Text;
        
        public bool HideOnMax = true;
        public float HideAfterSeconds = -1f;
        private Property _property;
        private float _secondsAfterChange;
        
        public void SetProperty(Property property)
        {
            if (_property != null)
                _property.Changed?.RemoveListener(OnChange);
                
            _property = property;
            _property.Changed?.AddListener(OnChange);
            Debug.Log($"Set Property: {_property.Value}/{_property.MaxValue}");
            OnChange(_property);
        }
        
        void Update()
        {
            if (HideAfterSeconds > 0)
            {
                _secondsAfterChange += Time.deltaTime;
                if (_secondsAfterChange > HideAfterSeconds && gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
        }
        
        protected virtual void OnChange(Property property)
        {
            if (property.MaxValue != property.Value || HideOnMax == false) 
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
            Filler.fillAmount = ((float)property.Value)/(float)property.MaxValue;
            if (Text != null)
            {
                Text.SetText($"{property.Value}/{property.MaxValue}");
            }
            _secondsAfterChange = 0;
        }
    }
}