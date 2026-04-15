using System.Collections.Generic;
using UnityEngine;

public struct MarkovLinkEnemy
{
    public float Probability;
    public MarkovStateEnemy State;
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
        if (!_links.Exists(l => l.State == link.State))
            _links.Add(link);
    }

    public MarkovStateEnemy NextEnemyState()
    {
        if (_links.Count > 0)
        {
            float rng = Random.value;
            float rngSum = 0;
            foreach (MarkovLinkEnemy link in _links)
            {
                rngSum += link.Probability;
                if (rng < rngSum)
                    return link.State;
            }
        }
        return null;
    }
}