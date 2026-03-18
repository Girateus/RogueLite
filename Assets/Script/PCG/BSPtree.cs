using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCG
{

    public enum CutType
    {
        Horizontal,
        Vertical
    }

    public class BspTree
    {

        public static void Generate(out BspNode root, BoundsInt originBounds, float minCutRatio, float maxCutRatio,
            float surfaceCriteria, float jitter, float shrinkFactor)
        {
            CutType cutType = Random.value > 0.5f ? CutType.Horizontal : CutType.Vertical;

            Queue<BspNode> cutsQueue = new Queue<BspNode>();
            root = new BspNode(originBounds, cutType, null);
            cutsQueue.Enqueue(root);

            do
            {
                BspNode cutNode = cutsQueue.Dequeue();

                if ((cutNode.Bounds.size.x * cutNode.Bounds.size.y) > surfaceCriteria)
                {

                    float cutRatio = Random.Range(Mathf.Min(minCutRatio, maxCutRatio), Mathf.Max(minCutRatio, maxCutRatio));

                    BoundsInt[] boundsCutResult = new BoundsInt[2];
                    CutType cutTypeResult = CutType.Horizontal;
                    switch (cutNode.CutType)
                    {
                        case CutType.Horizontal:
                            boundsCutResult = CutVertical(cutNode.Bounds, cutRatio);
                            cutTypeResult = CutType.Vertical;
                            break;
                        case CutType.Vertical:
                            boundsCutResult = CutHorizontal(cutNode.Bounds, cutRatio);
                            cutTypeResult = CutType.Horizontal;
                            break;
                    }
                    foreach (BoundsInt boundsInt in boundsCutResult)
                    {
                        BspNode newNode = new BspNode(
                            boundsInt,
                            cutTypeResult,
                            cutNode);
                        cutNode.Children.Add(newNode);

                        cutsQueue.Enqueue(newNode);
                    }
                }
                else
                {
                    cutNode.SetRoom(jitter, shrinkFactor);
                }

            } while (cutsQueue.Count > 0);

            Debug.Log($"C'est fini !!!!!!!!!!!");

        }

        private static BoundsInt[] CutVertical(BoundsInt room, float cutRatio)
        {

            BoundsInt roomA = new BoundsInt(
                room.min,
                new Vector3Int(Mathf.RoundToInt(room.size.x * cutRatio), room.size.y, room.size.z)
            );
            BoundsInt roomB = new BoundsInt(
                new Vector3Int(roomA.max.x, roomA.min.y, roomA.min.z),
                new Vector3Int(Mathf.RoundToInt(room.size.x * (1 - cutRatio)), room.size.y, room.size.z)
            );

            return new[] { roomA, roomB };

        }

        private static BoundsInt[] CutHorizontal(BoundsInt room, float cutRatio)
        {

            BoundsInt roomA = new BoundsInt(
                room.min,
                new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * cutRatio), room.size.z)
            );
            BoundsInt roomB = new BoundsInt(
                new Vector3Int(roomA.min.x, roomA.max.y, roomA.min.z),
                new Vector3Int(room.size.x, Mathf.RoundToInt(room.size.y * (1 - cutRatio)), room.size.z)
            );

            return new[] { roomA, roomB };

        }

        public static List<BoundsInt> GetRooms(BspNode root, float shrinkFactor)
        {
            List<BoundsInt> rooms = new List<BoundsInt>();
            Stack<BspNode> nodeStack = new Stack<BspNode>();

            nodeStack.Push(root);

            do
            {

                BspNode current = nodeStack.Pop();

                if (current.IsLeaf)
                {
                    rooms.Add(current.Room);
                }
                else
                {
                    foreach (var child in current.Children)
                    {
                        nodeStack.Push(child);
                    }
                }

            } while (nodeStack.Count > 0);

            return rooms;

        }public static List<BoundsInt> GetBounds(BspNode root)
        {
            List<BoundsInt> bounds = new List<BoundsInt>();
            Stack<BspNode> nodeStack = new Stack<BspNode>();

            nodeStack.Push(root);

            do
            {

                BspNode current = nodeStack.Pop();

                if (current.IsLeaf)
                {
                    bounds.Add(current.Bounds);
                }
                else
                {
                    foreach (var child in current.Children)
                    {
                        nodeStack.Push(child);
                    }
                }

            } while (nodeStack.Count > 0);

            return bounds;

        }

        // private static BoundsInt SampleRoom(BspNode node, float shrinkFactor)
        // {
        //     Vector3Int newSize = Vector3Int.RoundToInt(new Vector3(node.Bounds.size.x * shrinkFactor, node.Bounds.size.y * shrinkFactor, 1));
        //     Vector3Int newPosition =
        //         Vector3Int.RoundToInt(
        //             node.Center -
        //             new Vector3(0.5f * newSize.x, 0.5f * newSize.y, 1)
        //         );
        //
        //     return new BoundsInt(newPosition, newSize);
        //
        // }

        public static List<Corridor> MakeCorridors(BspNode root)
        {
            List<Corridor> corridors = new List<Corridor>();
            if (root.IsLeaf) return corridors;

            // Recursive call to get corridors from children
            foreach (var child in root.Children) {
                corridors.AddRange(MakeCorridors(child));
            }

            // Connect the two children of this node
            if (root.Children.Count == 2) {
                // Get a leaf from each subtree to find a room to connect
                BspNode leafA = GetRandomLeaf(root.Children[0]);
                BspNode leafB = GetRandomLeaf(root.Children[1]);
                corridors.Add(new Corridor(leafA, leafB));
            }

            return corridors;
        }
        
        private static BspNode GetRandomLeaf(BspNode root)
        {
            Stack<BspNode> roomStack = new Stack<BspNode>();
            roomStack.Push(root);

            do
            {

                BspNode current = roomStack.Pop();

                if (current.IsLeaf)
                {
                    return current;
                }
                
                roomStack.Push(current.Children[Random.Range(0, current.Children.Count)]);

            } while (roomStack.Count > 0);
            
            return null;
            
        }
    }
}
