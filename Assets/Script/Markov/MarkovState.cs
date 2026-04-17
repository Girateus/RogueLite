using System.Collections.Generic;
using UnityEngine;

public struct MarkovLink
{
    public float Probability;
    public MarkovState State;
}

public class MarkovState
{
    private List<MarkovLink> _links;
    private string _name;

    public string Name => _name;

    public MarkovState(string name)
    {
        _name = name;
        _links = new List<MarkovLink>();
    }

    public void AddLink(MarkovLink link)
    {
        if (!_links.Exists(l => l.State == link.State))
            _links.Add(link);
    }

    public MarkovState NextState()
    {
        if (_links.Count > 0)
        {
            float rng = Random.value;
            float rngSum = 0;
            foreach (MarkovLink link in _links)
            {
                rngSum += link.Probability;
                if (rng < rngSum)
                    return link.State;
            }
        }
        return null;
    }
}