using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public class BspNode
    {
        public BoundsInt Bounds;
        public CutType CutType;
        
        public List<BspNode> Children;
        public BspNode Parent = null;

        public BoundsInt Room;

        public BspNode(BoundsInt bounds, CutType cutType, BspNode parent)
        {
            Bounds = bounds;
            CutType = cutType;
            
            Parent = parent;
            Children = new List<BspNode>();

        }

        public void SetRoom(float jitter, float shrinkFactor)
        {
            float inverseShrinkFactor = 1f - shrinkFactor;
            Vector3Int min = Bounds.min + Vector3Int.RoundToInt(new Vector3(inverseShrinkFactor / 2f * Bounds.size.x, inverseShrinkFactor / 2f * Bounds.size.y, 0));
            Vector3Int max = Bounds.max - Vector3Int.RoundToInt(new Vector3(inverseShrinkFactor / 2f * Bounds.size.x, inverseShrinkFactor / 2f * Bounds.size.y, 0));

            BoundsInt shrinkBounds = Bounds;
            shrinkBounds.min = min;
            shrinkBounds.max = max;
            
            Room = new BoundsInt(shrinkBounds.position + Vector3Int.RoundToInt(jitter * Random.insideUnitSphere), shrinkBounds.size);
            
        }

        public bool IsLeaf => Children.Count == 0;
        
        
        
    }
}