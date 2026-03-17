using System.Collections;
using Gift2.Core;
using UnityEngine;
using UnityEngine.Events;
using Wof.Views;

namespace Gift2
{
    public class EndGameScript : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEventHandler _eventHandler;
        
        public Transform point;
        public Player Player;
        public UnityEvent Showed = new ();
        
        public void Play()
        {
            StartCoroutine(OnPlay());
        }
        
        private IEnumerator OnPlay()
        {
            yield return new WaitForEndOfFrame();
            _animator.gameObject.SetActive(true);
            _animator.Play("Show");
            _eventHandler.AddListener("Showed", OnShow);
        }
        
        public void OnShow()
        {
            _eventHandler.RemoveListener("Showed", OnShow);
            if (Player?.Character != null)
            {
                Player.Character.transform.position = point.position;
            }
            Showed.Invoke();
            _animator.Play("Hide");
            _eventHandler.AddListener("Hided", OnHide);
        }
        
        public void OnHide()
        {
            _eventHandler.RemoveListener("Hided", OnHide);
            _animator.gameObject.SetActive(false);
        }
    }
}
