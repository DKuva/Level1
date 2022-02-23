using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attributeHandler : MonoBehaviour
{
    
    public List<Buff> activeBuffs;

    [HideInInspector]
    public attributeData attributeData = new attributeData();

    public delegate void BuffTick();
    public static event BuffTick buffTick;

    public delegate void NewBuff(Buff buff);
    public static event NewBuff addedBuff;

    private void Awake()
    {
        Debug.Log("attributeHandler");
        PlayerScript.oneSecondTick += resolveBuffs;

    }
    public Dictionary<attributeData.attributes,attribute> getCurrentStats()
    {
        return attributeData.getCurrentStats();       
    }
    public attribute getStat(attributeData.attributes key)
    {
        return attributeData.getStat(key);
    }
    public void addStats(Dictionary<attributeData.attributes, atrVal> modifiers)
    {

        foreach (KeyValuePair<attributeData.attributes, atrVal> stat in modifiers)
        {
            attributeData.addStat(stat.Key, stat.Value.value, stat.Value.isPercentage);
        }

    }

    public void removeStats(Dictionary<attributeData.attributes, atrVal> modifiers)
    {

        foreach (KeyValuePair<attributeData.attributes, atrVal> stat in modifiers)
        {
            attributeData.removeStat(stat.Key, stat.Value.value, stat.Value.isPercentage);
        }

    }

    public void addBuff(Buff buff)
    {
        Debug.Log("added buff " + buff.buffName);
        Buff newBuff = Object.Instantiate(buff);

        newBuff.resetTimer();
        activeBuffs.Add(newBuff);
        addedBuff(newBuff);

        //If type instant, apply mod
        if (newBuff.type == Buff.buffType.instant)
        {
            for (int i = 0; i < newBuff.modifier.Length; i++)
            {
                attributeData.addStat(newBuff.attribute[i], newBuff.modifier[i], newBuff.isPercentageModifier[i]);
            }
        }
    }

    public void resolveBuffs()
    {
        for (int t = 0; t < activeBuffs.Count; t++)
        {
            Buff b = activeBuffs[t];
            b.durationTimer -= 1;

            if(buffTick != null)
            {
                buffTick();
            }
            if (b.durationTimer <= 0)
            {
                activeBuffs.Remove(b);

                for (int i = 0; i < b.modifier.Length; i++)
                {
                    if (!attributeData.isSpendable(b.attribute[i])) // If Buff is permanent, don't revert stats to previous state, example - posion bottle
                    {
                        attributeData.removeStat(b.attribute[i], b.modifier[i], b.isPercentageModifier[i]);
                    }
                }
            }
            else
            {
                if (b.type == Buff.buffType.incremental) //If incremental, apply mod on each resolveBuff()
                {
                    for (int i = 0; i < b.modifier.Length; i++)
                    {
                        attributeData.addStat(b.attribute[i], b.modifier[i], b.isPercentageModifier[i]); // deltaStat is used for reverting the stat
                    }
                }
                else if (b.type == Buff.buffType.ramp) //If ramp, apply mod/rampTime on each resolveBuff() for rampTime
                {
                    if (b.durationTimer >= (b.duration - b.rampTime))
                    {
                        for (int i = 0; i < b.modifier.Length; i++)
                        {
                            float rampVal = (float)b.modifier[i] / (float)b.rampTime;
                            attributeData.addStat(b.attribute[i], rampVal, b.isPercentageModifier[i]);
                        }
                    }
                }
            }
        }      
    }
}
