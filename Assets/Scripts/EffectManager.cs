using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance { get; private set; }
        [SerializeField] private GameObject SuperFX;
        [SerializeField] private GameObject PowerFX;
        [SerializeField] private GameObject SpeedFX;
        [SerializeField] private GameObject StrongFX;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(this.gameObject);
            }
            else if (this != Instance)
            {
                Destroy(this.gameObject);
            }
        }
        public void DoSuperFX(GameObject point,float duration)
        {
            GameObject obj = Instantiate(SuperFX, point.transform.position, Quaternion.identity, point.transform);
            Destroy(obj, duration);
        }
        public void DoPowerFX(GameObject point, float duration)
        {
            GameObject obj = Instantiate(PowerFX, point.transform.position, Quaternion.identity, point.transform);
            Destroy(obj, duration);
        }
        public void DoSpeedFX(GameObject point, float duration)
        {
            GameObject obj = Instantiate(SpeedFX, point.transform.position, Quaternion.identity, point.transform);
            Destroy(obj, duration);
        }
        public void DoStrongFX(GameObject point, float duration)
        {
            GameObject obj = Instantiate(StrongFX, point.transform.position, Quaternion.identity, point.transform);
            Destroy(obj, duration);
        }
    }
}
