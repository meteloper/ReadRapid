using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Metelab
{
    public class MeteToggle : Toggle
    {
        public void Init(Action<bool> OnValueChanged)
        {
            onValueChanged.RemoveAllListeners();
            onValueChanged.AddListener(new UnityAction<bool>(OnValueChanged));
        }
    }
}

