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
    public attributeData.attributes[] attribute;
    public int[] modifier;
    public bool[] isPercentageModifier;

    [HideInInspector]
    public float[] deltaStat; //Used for returning attribute to previous value

    public void resetTimer()
    {
       deltaStat = new float[modifier.Length];
       for (int i = 0; i < deltaStat.Length; i++)
       {
           deltaStat[i] = 0;
       }
       durationTimer = duration;
    }
}
