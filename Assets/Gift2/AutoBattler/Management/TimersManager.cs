using System;
using System.Collections.Generic;
using UnityEngine;

public class TimersManager : MonoBehaviour
{
    // Исправлено: статическое свойство для Singleton
    public static TimersManager Instance { get; private set; }
    
    // Список активных таймеров
    private List<Timer> _activeTimers = new List<Timer>();
    
    private bool _pause = false;

    private void Awake()
    {
        // Singleton реализация
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Обычно для менеджеров делают персистентными
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_pause) return;
        // Обновление всех активных таймеров
        for (int i = _activeTimers.Count - 1; i >= 0; i--)
        {
            _activeTimers[i].Update(Time.deltaTime);
            
            // Удаляем завершенные таймеры
            if (_activeTimers[i].IsComplete)
            {
                _activeTimers.RemoveAt(i);
            }
        }
    }

    public void Pause(bool value)
    {
        _pause = value;
    }

    /// <summary>
    /// Регистрирует новый таймер
    /// </summary>
    /// <param name="delay">Время в секундах</param>
    /// <param name="callback">Действие по завершении</param>
    /// <returns>Экземпляр таймера для контроля</returns>
    public Timer Register(float delay, Action callback)
    {
        var timer = new Timer(delay, callback);
        _activeTimers.Add(timer);
        return timer;
    }

    /// <summary>
    /// Отменяет и удаляет таймер
    /// </summary>
    public void CancelTimer(Timer timer)
    {
        if (timer != null && _activeTimers.Contains(timer))
        {
            timer.Cancel();
            _activeTimers.Remove(timer);
        }
    }

    /// <summary>
    /// Очищает все активные таймеры
    /// </summary>
    public void ClearAllTimers()
    {
        foreach (var timer in _activeTimers)
        {
            timer.Cancel();
        }
        _activeTimers.Clear();
    }

    /// <summary>
    /// Класс таймера
    /// </summary>
    public class Timer
    {
        public float Delay { get; private set; }
        public float ElapsedTime { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsComplete { get; private set; }
        
        private Action _callback;
        private bool _isCancelled;

        public Timer(float delay, Action callback)
        {
            Delay = delay;
            _callback = callback;
            ElapsedTime = 0f;
            IsRunning = true;
            IsComplete = false;
            _isCancelled = false;
        }

        /// <summary>
        /// Обновляет состояние таймера
        /// </summary>
        public void Update(float deltaTime)
        {
            if (!IsRunning || IsComplete || _isCancelled) return;

            ElapsedTime += deltaTime;
            
            if (ElapsedTime >= Delay)
            {
                Complete();
            }
        }

        /// <summary>
        /// Запускает таймер
        /// </summary>
        public void Start(float? newDelay = null)
        {
            if (newDelay.HasValue)
            {
                Delay = newDelay.Value;
            }
            
            IsRunning = true;
            IsComplete = false;
            _isCancelled = false;
            ElapsedTime = 0f;
        }

        /// <summary>
        /// Приостанавливает таймер
        /// </summary>
        public void Pause()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Возобновляет таймер
        /// </summary>
        public void Resume()
        {
            if (!IsComplete && !_isCancelled)
            {
                IsRunning = true;
            }
        }

        /// <summary>
        /// Отменяет таймер
        /// </summary>
        public void Cancel()
        {
            _isCancelled = true;
            IsRunning = false;
            IsComplete = false;
            _callback = null;
        }

        /// <summary>
        /// Завершает таймер и вызывает callback
        /// </summary>
        private void Complete()
        {
            IsComplete = true;
            IsRunning = false;
            
            if (!_isCancelled && _callback != null)
            {
                _callback.Invoke();
            }
        }

        /// <summary>
        /// Оставшееся время
        /// </summary>
        public float RemainingTime => Mathf.Max(0f, Delay - ElapsedTime);

        /// <summary>
        /// Прогресс от 0 до 1
        /// </summary>
        public float Progress => Mathf.Clamp01(ElapsedTime / Delay);
    }
}