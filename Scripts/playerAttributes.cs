using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttributes : MonoBehaviour
{
    private int[] _stats;
    private string[] _keys;
    private GameObject _atrNames;
    private GameObject _atrStats;
    private void Awake()
    {
        _stats = new int[]{5,5,5,5};
        _keys = new string[]{"STR","DEX","END","INT"};

        _atrNames = GameObject.Find("Player/Camera/UI/Attributes/Names");
        _atrStats = GameObject.Find("Player/Camera/UI/Attributes/Stats");

        updateStats();

    }
    
    public void updateStats()
    {
        int i = 0;
        foreach (int s in _stats)
        {
            _atrNames.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = _keys[i];
            _atrStats.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = s.ToString();
            i++;
        }         
    }

    public void addStats(int[] stat)
    {
        int j = 0;
        foreach (int i in stat)
        {
            _stats[j] += i;
            j++;
        }

        updateStats();
    }

    public void removeStats(int[] stat)
    {
        int j = 0;
        foreach (int i in stat)
        {
            _stats[j] -= i;
            j++;
        }
        updateStats();
    }

}
