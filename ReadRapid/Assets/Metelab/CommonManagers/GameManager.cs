using Unity.VisualScripting;
using UnityEngine;

namespace Metelab.CommonManagers
{
    public class GameManager: MeteSingleton<GameManager>
    {
        [SerializeField] private MeteMono[] Managers;
        [SerializeField] private MeteScriptableObject[] IndependentScripableObjects;

        private void Awake()
        {
            Metelab.Log(this);
            EarlyInit();

            for (int i = 0; i < IndependentScripableObjects.Length; i++)
            {
                IndependentScripableObjects[i].EarlyInit();
            }

            for (int i = 0; i < Managers.Length; i++)
            {
                Managers[i].EarlyInit();
            }
        }

        private void Start()
        {
            Metelab.Log(this);
            Init();

            for (int i = 0; i < IndependentScripableObjects.Length; i++)
            {
                IndependentScripableObjects[i].Init();
            }

            for (int i = 0; i < Managers.Length; i++)
            {
                Managers[i].Init();
            }
        }

        private void OnApplicationQuit()
        {
            Metelab.Log(this);

            SaveAllData();

            for (int i = 0; i < IndependentScripableObjects.Length; i++)
            {
                IndependentScripableObjects[i].ResetData();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                SaveAllData();
        }

        private void SaveAllData()
        {
            for (int i = 0; i < IndependentScripableObjects.Length; i++)
            {
                if (IndependentScripableObjects[i] is IDataHandler)
                {
                    Metelab.Log(this,$"{IndependentScripableObjects[i].name} was saved.");
                    ((IDataHandler)IndependentScripableObjects[i]).Save();
                }
            }

        }
    }
}