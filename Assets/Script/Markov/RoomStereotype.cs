using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomType
{
    Fight,
    BigFight,
    Shop,
    Boss,
    End,
    Pool,
    Start,
}

[ System.Serializable]
public struct Link
{
    public float Probability;
    public RoomStereotype State;

}

[CreateAssetMenu(fileName = "RoomStereotype", menuName = "Scriptable Objects/RoomStereotype")]
public class RoomStereotype : ScriptableObject
{
    public Vector2 Size;
    public RoomType Type;
    
    public List<Link> Links = new List<Link>();

    public RoomStereotype NextRoom()
    {
        if (Links.Count > 0)
        {
            float rng = Random.value * Links.Sum(l => l.Probability);
            float rngSum = 0;
            foreach (Link link in Links)
            {
                if (rng < rngSum + link.Probability)
                {
                    return link.State;
                }
                rngSum += link.Probability;
            }
        } 
        return null;

    }
    
}
