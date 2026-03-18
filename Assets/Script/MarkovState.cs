using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct MarkovLink
{
    public float Probability;
    public MarkovState State;

    /*public MarkovLink(float probability, MarkovState state)
    {
        Probability = probability;
        State = state;
    }*/

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
        if(!_links.Exists(l => l.State == link.State))
        {
            _links.Add(link);
            //_links.Sort((l1, l2) => l1.Probability.CompareTo(l2.Probability));
        }
    }

    public MarkovState NextState()
    {
        if (_links.Count > 0)
        {
            float rng = Random.value;
            float rngSum = 0;
            foreach (MarkovLink link in _links)
            {
               // rngSum += link.Probability;
                if (rng < rngSum + link.Probability)
                {
                    return link.State;
                }
                rngSum += link.Probability;
            }
            // int idx = Mathf.FloorToInt(rng * _links.Count);
            //return _links[idx].State;
        } 
        return null;

    }
}
