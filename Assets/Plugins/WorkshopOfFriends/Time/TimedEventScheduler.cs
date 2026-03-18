using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Wof.TimeManagement
{
    public class TimedEventScheduler : MonoBehaviour
    {
        [System.Serializable]
        public struct TimedEvent
        {
            [Tooltip("Интервал между вызовами в секундах")]
            public float interval;

            [Tooltip("Событие, которое будет вызываться каждый интервал")]
            public UnityEvent onInterval;
        }
        
        [SerializeField] private List<TimedEvent> events = new List<TimedEvent>();

        // Хранит оставшееся время до следующего вызова для каждого события
        private List<float> timers = new List<float>();

        private void Start()
        {
            ResetAllTimers();
        }

        private void Update()
        {
            // Если количество событий изменилось во время выполнения (через код или Inspector),
            // пересоздаём таймеры, чтобы синхронизировать списки.
            if (timers.Count != events.Count)
            {
                ResetAllTimers();
            }

            float deltaTime = Time.deltaTime;

            for (int i = 0; i < events.Count; i++)
            {
                // Уменьшаем таймер
                timers[i] -= deltaTime;

                // Если таймер ушёл в ноль или ниже, вызываем событие (возможно несколько раз при сильном лаге)
                while (timers[i] <= 0f)
                {
                    events[i].onInterval?.Invoke();   // безопасный вызов
                    timers[i] += events[i].interval;  // добавляем ещё один интервал
                }
            }
        }

        /// <summary>
        /// Сбросить таймер указанного события (следующий вызов произойдёт через его интервал).
        /// </summary>
        public void ResetEventTimer(int index)
        {
            if (index >= 0 && index < timers.Count)
            {
                timers[index] = events[index].interval;
            }
        }

        /// <summary>
        /// Сбросить таймеры всех событий.
        /// </summary>
        public void ResetAllTimers()
        {
            timers.Clear();
            foreach (var ev in events)
            {
                // Инициализируем значением interval, чтобы первый вызов произошёл через interval секунд.
                // Если нужно, чтобы событие сработало сразу, замените на 0f.
                timers.Add(ev.interval);
            }
        }
    }
}
