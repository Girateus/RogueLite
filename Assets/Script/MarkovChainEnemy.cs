using UnityEngine;

public class MarkovChainEnemy : MonoBehaviour
{
    private MarkovStateEnemy _mele = new MarkovStateEnemy("Mele");
    private MarkovStateEnemy _range = new MarkovStateEnemy("Range");
    private MarkovStateEnemy _summoner = new MarkovStateEnemy("Summoner");
    private MarkovStateEnemy _healer = new MarkovStateEnemy("Healer");
    
    private MarkovStateEnemy _chain;
    [SerializeField] private int _maxStates = 10;


    private void Start()
    {
        _mele.AddLink(new MarkovLinkEnemy {Probability = 0.3f, State = _mele});
        _range.AddLink(new MarkovLinkEnemy{Probability = 0.3f, State = _range});
        _healer.AddLink(new MarkovLinkEnemy{Probability = 0.2f, State = _healer});
        _summoner.AddLink(new MarkovLinkEnemy{Probability = 0.2f, State = _summoner});
        
        _chain = _mele;
        string ChainStr = "";
        for (int i = 0; i < _maxStates; i++)
        {
            ChainStr += _chain.Name + " - ";
            _chain = _chain.NextEnemyState();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
