using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class NationFeatureDefinition : FeatureDefinition
{
    [SerializeField]
    public List<NationDefinition> nations;
}
