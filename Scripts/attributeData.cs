using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attributeData 
{
    public enum attributes { strength, dexterity, endurance, inteligence, luck , health, mana};
    private Dictionary<attributeData.attributes, attribute> _currentStats = new Dictionary<attributeData.attributes, attribute>();

    public delegate void AttributeChanged();
    public static event AttributeChanged onAttributeChanged;

    public attributeData()
    {
        setupAttributes();
    }

    public void setupAttributes() //setup default values
    {
        _currentStats.Add(attributes.strength, new attribute(5f));
        _currentStats.Add(attributes.dexterity, new attribute(5f));
        _currentStats.Add(attributes.endurance, new attribute(5f));
        _currentStats.Add(attributes.inteligence, new attribute(5f));
        _currentStats.Add(attributes.luck, new attribute(5f));
        _currentStats.Add(attributes.health, new attribute(100f,0f,200f,true));
        _currentStats.Add(attributes.mana, new attribute(70f,0f,150f,true));

    }
    public Dictionary<attributeData.attributes, attribute> getCurrentStats()
    {
        return _currentStats;
    }
    public attribute getStat(attributes key)
    {
        return _currentStats[key];
    }
    public bool isSpendable(attributes key)
    {
        return _currentStats[key].isSpendable;
    }

    public void addStat(attributes key, float value, bool isPercentage)
    {
        
        if (isPercentage)
        {
            _currentStats[key].addPercentageModifier(value);
        }
        else
        {
            _currentStats[key].addModifier(value);
        }

        if (onAttributeChanged != null)
        {
            onAttributeChanged();
        }

    }
    public void removeStat(attributes key, float value, bool isPercentage)
    {
        if (isPercentage)
        {
            _currentStats[key].removePercentageModifier(value);
        }
        else
        {
            _currentStats[key].addModifier(-value);
        }

        if (onAttributeChanged != null)
        {
            onAttributeChanged();
        }
    }
}
// Attribute class- each attribute has a min, max value, base and current value. Base value is the sum of all linear modifiers (+2,-3...), current value
//applies the list _activePercentageModifiers onto the linear modifiers (base + (30%,60%,-20%))
public class attribute
{
    public bool isSpendable = false;
    private float _baseValue = 0f;
    public float min = 0f;
    public float max = 100f;
    private List<float> _activePercentageModifiers = new List<float>();

    public attribute(float value, float min, float max, bool isSpendable)
    {
        this._baseValue = value;
        this.min = min;
        this.max = max;
        this.isSpendable = isSpendable;
    }
    public attribute(float value)
    {
        this._baseValue = value;
    }
    public void addModifier(float val)
    {
        _baseValue += val;
        if(_baseValue > max)
        {
            _baseValue = max;
        }
        if(_baseValue < min)
        {
            _baseValue = min;
        }
    }
    public void addPercentageModifier(float val)
    {
        _activePercentageModifiers.Add(val);
    }
    public void removePercentageModifier(float val)
    {
        _activePercentageModifiers.Remove(val);
    }
    public float getCurrentValue()
    {
        float cVal = _baseValue;
        foreach(float fl in _activePercentageModifiers)
        {
            cVal = cVal + fl*cVal*0.01f;
        }
        if(cVal > max)
        {
            cVal = max;
        }
        if(cVal < min)
        {
            cVal = min;
        }
        return cVal;
    }



}
