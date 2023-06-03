using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using System.Data.SqlTypes;

namespace Metelab
{
    [Serializable]
    public class MeteValue<T>: IDataHandler
    {
        public event Action<T> ActionOnChangedValue;
        [SerializeField] private T _Current;
        public T Current
        {
            get { return _Current; }
            set
            {
                if (!_Current.Equals(value))
                {
                    _Current = value;
                    TriggerChange();
                }
                else
                    _Current = value;
            }
        }
      
        public T Default;
        public bool IsHaveChange;

#if UNITY_EDITOR
        private T _StartValue;
#endif

        public void Init(Func<MeteValue<T>,T> load, Action<T> save)
        {
#if UNITY_EDITOR
            _StartValue = Current;
#endif

            IsHaveChange = false;
            FuncLoad = load;
            ActionSave = save;
            ActionOnChangedValue = null;
            Load();
        }

        private Func<MeteValue<T>, T> FuncLoad;
        private Action<T> ActionSave;
        


        private void TriggerChange()
        {
            IsHaveChange = true;
            ActionOnChangedValue?.Invoke(_Current);
        }

        public void SetStartValue()
        {

#if UNITY_EDITOR
            _Current = _StartValue;

#endif
        }

        public void Save(bool isUseControls = true)
        {
            if ((isUseControls && IsHaveChange) || !isUseControls)
            {
                ActionSave?.Invoke(Current);
                IsHaveChange = false;
            }
        }

        public void Load()
        {
            if (FuncLoad != null)
                Current = FuncLoad(this);
        }
    }


    [Serializable]
    public class MeteValueList<T>: IDataHandler
    {
        public event Action<List<T>> ActionOnChangedValue;
        public event Action<T> ActionOnAdd;
        public event Action<T> ActionOnRemoved;
        public List<T> Current;
        public List<T> Default;
        public bool IsHaveChange;
#if UNITY_EDITOR
        private List<T> _StartValue;
#endif

        private Func<MeteValueList<T>,List<T>> FuncLoad;
        private Action<List<T>> ActionSave;
        private Action ActionDelete;

        public MeteValueList()
        {
            Current = new List<T>();
            Default = new List<T>();
        }

        public void Init(Func<MeteValueList<T>,List<T>> load, Action<List<T>> save, Action delete = null)
        {
            
#if UNITY_EDITOR
            _StartValue = new List<T>(Current);
#endif

            IsHaveChange = false;
            FuncLoad = load;
            ActionSave = save;
            ActionDelete = delete;
            ActionOnChangedValue = null;
            ActionOnAdd = null;
            ActionOnRemoved = null;
            Load();

            if(Current == null)
                Current = new List<T>();
        }

        public void TriggerChange()
        {
            IsHaveChange = true;
            ActionOnChangedValue?.Invoke(Current);
        }

        public List<T> GetList()
        {
            return new List<T>(Current);
        }

        public void Add(T element)
        {
            if (!Current.Contains(element))
            {
                Current.Add(element);
                ActionOnAdd?.Invoke(element);
                TriggerChange();
            }
        }

        public void Remove(T element)
        {
            if (Current.Contains(element))
            {
                Current.Remove(element);
                ActionOnRemoved?.Invoke(element);
                TriggerChange();
            }
        }

        public void RemoveAt(int index)
        {
            if (index < Current.Count)
            {
                var element = Current[index];
                Current.RemoveAt(index);
                ActionOnRemoved?.Invoke(element);
                TriggerChange();
            }
        }

        public void SetStartValue()
        {
#if UNITY_EDITOR
            Current = _StartValue;
#endif
        }

        public void Save(bool isUseControls = true)
        {
            if( (isUseControls && IsHaveChange) || !isUseControls)
            {
                ActionSave?.Invoke(Current);
                IsHaveChange = false;
            }
        }

        public void Load()
        {
            if (FuncLoad != null)
                Current = FuncLoad(this);
            else
                Current = new List<T>();
        }

        public void Delete()
        {
            ActionDelete?.Invoke();
        }
    }


    public interface IDataHandler
    {
        public void SetStartValue();
        public void Save(bool isUseControls = true);
        public void Load();
    }
}

