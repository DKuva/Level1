using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttributes : MonoBehaviour
{
    private UnityEngine.UI.Text _atrNames;
    private UnityEngine.UI.Text _atrStats;

    public GameObject player;
    public GameObject attributeNamesObject;
    public GameObject attributeValueObject;
    public GameObject hpContainer;

    private UnityEngine.UI.Image _hpBar;
    private UnityEngine.UI.Image _mpBar;
    private UnityEngine.UI.Text _hpText;
    private UnityEngine.UI.Text _mpText;

    private GameObject _buffContainer;
    private attributeHandler _attributes;

    private List<attributeData.attributes> _showAttributes = new List<attributeData.attributes> {
        attributeData.attributes.strength,
        attributeData.attributes.dexterity,
        attributeData.attributes.endurance,
        attributeData.attributes.inteligence,
        attributeData.attributes.luck};

    private void Awake()
    {
        Debug.Log("playerAttributes");
        attributeData.onAttributeChanged += updateStatsPanel;
        attributeData.onAttributeChanged += updateHpPanel;
        attributeHandler.buffTick += updateBuffSprite;
        attributeHandler.addedBuff += addBuffSprite;
        //PlayerScript.playerAwake += updateStatsPanel;
        //PlayerScript.playerAwake += updateHpPanel;
        
        //gameObject.SetActive(false);

    }
    private void Start()
    {
        setupPanel();
        
    }
    public void setupPanel()
    {
        Debug.Log("setup panel");
        _atrNames = attributeNamesObject.GetComponent<UnityEngine.UI.Text>();
        _atrStats = attributeValueObject.GetComponent<UnityEngine.UI.Text>();

        //Setup heath bar
        _hpBar = hpContainer.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _mpBar = hpContainer.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
        _hpText = _hpBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        _mpText = _mpBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();

        _buffContainer = player.GetComponent<playerUI>().playerOverlay.GetComponent<playerOverlay>().buffContainer;
        _attributes = player.GetComponent<attributeHandler>();
        updateStatsPanel();
        updateHpPanel();
        gameObject.SetActive(false);
    }

    public void updateStatsPanel()
    {
        _atrNames.text = "";
        _atrStats.text = "";

        foreach (KeyValuePair<attributeData.attributes, attribute> stat in _attributes.getCurrentStats())
        {
            if (_showAttributes.Contains(stat.Key))
            {
                _atrNames.text += stat.Key.ToString() + "\n";
                _atrStats.text += stat.Value.getCurrentValue().ToString() + "\n";
            }

        }
    }
    public void updateHpPanel()
    {
        attribute hp = _attributes.getStat(attributeData.attributes.health);
        attribute mp = _attributes.getStat(attributeData.attributes.mana);

        float hpFill = (float)hp.getCurrentValue() / hp.max;
        float mpFill = (float)mp.getCurrentValue() / mp.max;
   
        _hpBar.fillAmount = hpFill;
        _mpBar.fillAmount = mpFill;

        _hpText.text = hp.getCurrentValue().ToString() + " / " + hp.max.ToString();
        _mpText.text = mp.getCurrentValue().ToString() + " / " + mp.max.ToString();
    }

    public void updateBuffSprite()
    {

        for(int t = 0; t<_attributes.activeBuffs.Count;t++)
        {
            Buff b = _attributes.activeBuffs[t];

            float fAmount = (float)b.durationTimer / (float)b.duration;
            _buffContainer.transform.GetChild(t).GetComponent<UnityEngine.UI.Image>().fillAmount = fAmount;

            if (b.durationTimer <= 0)
            {
                Destroy(_buffContainer.transform.GetChild(t).gameObject);

            }          
        }
    }
 

    public void addBuffSprite(Buff buff)
    {
        Debug.Log("added buff sprite" + buff.buffName);

        //Create buff ui sprite
        GameObject buffImage = new GameObject(buff.buffName);

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

