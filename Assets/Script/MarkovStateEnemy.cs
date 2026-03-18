using System.Collections.Generic;
using UnityEngine;

public struct MarkovLinkEnemy
{
    public float Probability;
    public MarkovStateEnemy State;

    /*public MarkovLink(float probability, MarkovState state)
    {
        Probability = probability;
        State = state;
    }*/

}
public class MarkovStateEnemy
{
    private List<MarkovLinkEnemy> _links;
    private string _name;
    
    public string Name => _name;

    public MarkovStateEnemy(string name)
    {
        _name = name;
        _links = new List<MarkovLinkEnemy>();
    }

    public void AddLink(MarkovLinkEnemy link)
    {
        if(!_links.Exists(l => l.State == link.State))
        {
            _links.Add(link);
            //_links.Sort((l1, l2) => l1.Probability.CompareTo(l2.Probability));
        }
    }
    
    public MarkovStateEnemy NextEnemyState()
    {
        if (_links.Count > 0)
        {
            float rng = Random.value;
            float rngSum = 0;
            foreach (MarkovLinkEnemy link in _links)
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
