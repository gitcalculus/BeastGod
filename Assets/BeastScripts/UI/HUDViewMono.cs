using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

/// <summary>
/// </summary>
public class HUDViewMono : MonoBehaviour
{
    public TextMeshProUGUI TxtBeastCount;
    public TextMeshProUGUI TxtMaxLevelBeastCount;

    private static HUDViewMono _inst;

    public static HUDViewMono Inst
    {
        get
        {
            return _inst;
        }
    }

    private List<BeastBrain> _beasts;

    public void RegisterBeast(BeastBrain b)
    {
        _beasts.Add(b);
    }

    public void UnRegisterBeast(BeastBrain b)
    {
        _beasts.Remove(b);
    }

    public void Awake()
    {
        _inst = this;
        _beasts = new List<BeastBrain>();
    }

    public void Update()
    {
        TxtBeastCount.text = "Beast:" + _beasts.Count.ToString();
        TxtMaxLevelBeastCount.text = "Red Beast:" + GetMaxLevelBeastCount().ToString();
    }

    public int GetMaxLevelBeastCount()
    {
        int ret = 0;
        int maxLevel = 4;
        foreach (var b in _beasts)
        {
            if (b.Level >= maxLevel)
            {
                ret++;
            }
        }
        return ret;
    }
}