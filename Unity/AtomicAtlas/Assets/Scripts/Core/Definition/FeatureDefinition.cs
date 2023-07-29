using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FeatureDefinition
{
    public string Serialize()
    {
        return JsonUtility.ToJson(this, true);
    }
}
