using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buff")]
public class Buff : ScriptableObject
{
    public string buffName;
    public Sprite sprite;
    public int duration;
   
    [HideInInspector]
    public int durationTimer;
    public enum buffType { instant, incremental , ramp}
    public buffType type;

    public bool isPermanent;
    public int rampTime;
    public playerAttributes.attributes[] attribute;
    public int[] modifier;
    public bool[] isPercentageModifier;

    public void resetTimer()
    {
        durationTimer = duration;
    }
}
