namespace PuzzleCubes.Utility
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;


    using System.Collections;

    [System.Serializable]
    public class TimedInterpolator 
    {
        [UnityEngine.SerializeField]
        private float duration = 1;

        [UnityEngine.SerializeField]
        private bool running = false;

        [NonReorderable]  
        [UnityEngine.SerializeField]
        private List<InterpolationParameter> parameters = new List<InterpolationParameter>();

        public float Duration { get => duration; set => duration = value; }
        public List<InterpolationParameter> Parameters { get => parameters; set => parameters = value; }
        public bool Running { get => running;  }

        public UnityEvent InterpolationFinished = new UnityEvent();


        public IEnumerator Interpolate(bool increase = true)
        {
            running = true;
            float t0 = Time.time;
            while(Time.time < t0+duration && duration != 0)
            {
                float t = Mathf.Clamp01((Time.time - t0) / duration);
                t = (increase) ? t : 1 - t;
                float tx;
                foreach(InterpolationParameter p in parameters)
                {
                    tx = (p.EaseFunction != null) ? Mathf.Clamp01(p.EaseFunction(t)) : t;
                    float newValue = p.MinValue + (p.MaxValue - p.MinValue) * tx;
                    p.UpdateAction.Invoke(newValue);
                }
                yield return null;
                
            }
            foreach(InterpolationParameter p in parameters)
            {
                float newValue =  (increase) ? p.MaxValue : p. MinValue;
                p.UpdateAction.Invoke(newValue);
            }
            running = false;
            this.InterpolationFinished?.Invoke();
            

        }

        
    }

    public delegate float EaseFunction(float x);

    [System.Serializable]
    public class InterpolationParameter
    {
        [UnityEngine.SerializeField]
        private string name;
        [UnityEngine.SerializeField]
        private float min = 0;
        [UnityEngine.SerializeField]
        private float max = 1;

        private EaseFunction easeFunction;

      
        private System.Action<float> updateAction;

        public EaseFunction EaseFunction { get => easeFunction; set => easeFunction = value; }
        public System.Action<float> UpdateAction { get => updateAction; set => updateAction = value; }
        public string Name { get => name; set => name = value; }
        public float MinValue { get => min; set => min = value; }
        public float MaxValue { get => max; set => max = value; }
      
    }
}