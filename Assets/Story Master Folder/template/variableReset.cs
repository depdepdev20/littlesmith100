using UnityEngine;
using System.Collections.Generic;
using KasperDev.ModularComponents;

public class variableReset : MonoBehaviour
{
    [System.Serializable]
    public class BoolVariableSetting
    {
        public BoolVariableSO boolVariable;
        public bool initialValue;
    }

    [System.Serializable]
    public class IntVariableSetting
    {
        public IntVariableSO intVariable;
        public int initialValue;
    }

    [SerializeField]
    private List<BoolVariableSetting> boolVariableSettings = new List<BoolVariableSetting>();

    [SerializeField]
    private List<IntVariableSetting> intVariableSettings = new List<IntVariableSetting>();

    private void Awake()
    {
        ResetVariables();
    }

    private void ResetVariables()
    {
        ResetBoolVariables();
        ResetIntVariables();
    }

    private void ResetBoolVariables()
    {
        foreach (var setting in boolVariableSettings)
        {
            if (setting.boolVariable != null)
            {
                setting.boolVariable.SetValue(setting.initialValue);
            }
        }
    }

    private void ResetIntVariables()
    {
        foreach (var setting in intVariableSettings)
        {
            if (setting.intVariable != null)
            {
                setting.intVariable.SetValue(setting.initialValue);
            }
        }
    }
}
