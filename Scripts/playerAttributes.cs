using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttributes : MonoBehaviour
{
    private UnityEngine.UI.Text _atrNames;
    private UnityEngine.UI.Text _atrStats;

    public GameObject player;

    public enum attributes {strength, dexterity, endurance, inteligence, luck ,health,maxHealth,mana,maxMana};
    private Dictionary<attributes, float> _currentStats = new Dictionary<attributes, float>();

    public List<Buff> activeBuffs;

    private UnityEngine.UI.Image _hpBar;
    private UnityEngine.UI.Image _mpBar;
    private UnityEngine.UI.Text _hpText;
    private UnityEngine.UI.Text _mpText;

    private GameObject _buffContainer;




    private void Awake()
    {
        //Setup starting stats
        _currentStats.Add(attributes.strength, 1);
        _currentStats.Add(attributes.dexterity, 6);
        _currentStats.Add(attributes.endurance, 5);
        _currentStats.Add(attributes.inteligence, 2);
        _currentStats.Add(attributes.luck, 1);
        _currentStats.Add(attributes.health, 7);
        _currentStats.Add(attributes.maxHealth, 10);
        _currentStats.Add(attributes.mana, 5);
        _currentStats.Add(attributes.maxMana, 8);

        _atrNames = GameObject.Find("Attributes/Names").transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        _atrStats = GameObject.Find("Attributes/Stats").transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();


        //Setup heath bar
        _hpBar = player.GetComponent<playerUI>().playerOverlay.GetComponent<playerOverlay>().hpContainer.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _mpBar = player.GetComponent<playerUI>().playerOverlay.GetComponent<playerOverlay>().hpContainer.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        _hpText = _hpBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        _mpText = _mpBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();

        _buffContainer = player.GetComponent<playerUI>().playerOverlay.GetComponent<playerOverlay>().buffContainer;
        updateStatsPanel();
        gameObject.SetActive(false);
        
    }



    public void updateStatsPanel()
    {
        _atrNames.text = "";
        _atrStats.text = "";

        foreach (KeyValuePair<attributes, float> stat in _currentStats)
        {
            _atrNames.text += stat.Key.ToString() + "\n";
            _atrStats.text += stat.Value.ToString() + "\n";

        }

        updateHpPanel();
    }
    public void updateHpPanel()
    {
        float hpFill = (float)_currentStats[attributes.health] / (float)_currentStats[attributes.maxHealth];
        float mpFill = (float)_currentStats[attributes.mana] / (float)_currentStats[attributes.maxMana];
   
        _hpBar.fillAmount = hpFill;
        _mpBar.fillAmount = mpFill;

        _hpText.text = _currentStats[attributes.health].ToString() + " / " + _currentStats[attributes.maxHealth].ToString();
        _mpText.text = _currentStats[attributes.mana].ToString() + " / " + _currentStats[attributes.maxMana].ToString();
    }

    //Reduce each activeBuff timer by 1, and resolve its effects if any
    public void resolveBuffs()
    {
        for(int t = 0; t<activeBuffs.Count;t++)
        {
            Buff b = activeBuffs[t];
            b.durationTimer -= 1;
            //Update buff sprite
            float fAmount = (float)b.durationTimer / (float)b.duration;
            _buffContainer.transform.GetChild(t).GetComponent<UnityEngine.UI.Image>().fillAmount = fAmount;

            if (b.durationTimer <= 0)
            {
                activeBuffs.Remove(b);
                Destroy(_buffContainer.transform.GetChild(t).gameObject);
                
                if (!b.isPermanent) // If Buff is permanent, don't revert stats to previous state, example - posion bottle
                {
                    if (b.type == Buff.buffType.incremental) 
                    {
                        for (int i = 0; i < b.modifier.Length; i++)
                        {
                            _currentStats[b.attribute[i]] -= b.modifier[i] * b.duration;
                        }
                    }
                    else 
                    {
                        for (int i = 0; i < b.modifier.Length; i++)
                        {
                            _currentStats[b.attribute[i]] -= b.modifier[i];
                        }
                    }
                }

            }
            else
            {
                if(b.type == Buff.buffType.incremental) //If incremental, apply mod on each resolveBuff()
                {
                    for( int i = 0; i< b.modifier.Length; i++)
                    {
                        _currentStats[b.attribute[i]] += b.modifier[i];
                    }
                }
                else if(b.type == Buff.buffType.ramp) //If ramp, apply mod/rampTime on each resolveBuff() for rampTime
                {
                    if(b.durationTimer >= (b.duration - b.rampTime))
                    {
                        for (int i = 0; i < b.modifier.Length; i++)
                        {
                            float rampVal = (float)b.modifier[i] / (float)b.rampTime;
                            _currentStats[b.attribute[i]] += rampVal;
                        }
                    }
                }
            }

        }

        
        updateStatsPanel();
    }

    //Apply limits to certain stats
    private void correctStats()
    {
        if(_currentStats[attributes.health] > _currentStats[attributes.maxHealth]) { _currentStats[attributes.health] = _currentStats[attributes.maxHealth]; }
        if (_currentStats[attributes.mana] > _currentStats[attributes.maxMana]) { _currentStats[attributes.mana] = _currentStats[attributes.maxMana]; }

    }
    public void addStats(Dictionary<playerAttributes.attributes,atrVal> modifiers)
    {

        foreach (KeyValuePair<attributes,atrVal> stat in modifiers)
        {
            if (stat.Value.isPercentage)
            {             
                _currentStats[stat.Key] = _currentStats[stat.Key]* (1f + stat.Value.value*0.01f);               
            }
            else
            {
                _currentStats[stat.Key] += stat.Value.value;
            }         
        }

        correctStats();
        updateStatsPanel();
    }

    public void removeStats(Dictionary<playerAttributes.attributes, atrVal> modifiers)
    {

        foreach (KeyValuePair<attributes, atrVal> stat in modifiers)
        {
            if (stat.Value.isPercentage)
            {
                _currentStats[stat.Key] = _currentStats[stat.Key] * (1f/(1f+(float)stat.Value.value*0.01f));
            }
            else
            {
                _currentStats[stat.Key] -= stat.Value.value;
            }
        }

        correctStats();
        updateStatsPanel();
    }
    public void addBuff(Buff buff)
    {
        Debug.Log("added buff " + buff.buffName);
        Buff newBuff = Object.Instantiate(buff);

        newBuff.resetTimer();
        activeBuffs.Add(newBuff);

        //If type instant, apply mod
        if(buff.type == Buff.buffType.instant)
        {
            for (int i = 0; i < buff.modifier.Length; i++)
            {
                if (!buff.isPercentageModifier[i])
                {
                    _currentStats[buff.attribute[i]] += buff.modifier[i];
                }
                else
                {
                    _currentStats[buff.attribute[i]] += _currentStats[buff.attribute[i]]*((float)buff.modifier[i]*0.01f);
                }
               
            }
        }

        //Create buff ui sprite
        GameObject buffImage = new GameObject(newBuff.buffName);

        RectTransform t = buffImage.AddComponent<RectTransform>();
        t.transform.SetParent(_buffContainer.transform);
        t.localScale = Vector3.one;
        t.anchoredPosition = new Vector2(0f, 0f);

        buffImage.AddComponent<CanvasRenderer>();
        UnityEngine.UI.Image img = buffImage.AddComponent<UnityEngine.UI.Image>();
        img.sprite = buff.sprite;

        img.type = UnityEngine.UI.Image.Type.Filled;
        img.raycastTarget = false;
        img.fillMethod = UnityEngine.UI.Image.FillMethod.Radial360;
        img.fillClockwise = false;

        img.fillAmount = 1;
    }
}

