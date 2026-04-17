using UnityEngine;

public class MarkovChain : MonoBehaviour
{
    
    private MarkovState _sunny = new MarkovState("Sunny");
    private MarkovState _rainy = new MarkovState("Rainy");
    
    private MarkovState _chain;

    [SerializeField] private int _maxStates = 10;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sunny.AddLink(new MarkovLink { Probability = 0.9f, State = _sunny });
        _sunny.AddLink(new MarkovLink { Probability = 0.1f, State = _rainy });

        _rainy.AddLink(new MarkovLink { Probability = 0.5f, State = _sunny });
        _rainy.AddLink(new MarkovLink { Probability = 0.5f, State = _rainy });

        _chain = _sunny;
        string  chainStr = "";

        for (int i = 0; i < _maxStates; i++)
        {
            chainStr += _chain.Name + " - ";
            _chain = _chain.NextState();
        }
        Debug.Log(chainStr);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
