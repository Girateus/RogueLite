using System;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public struct Corridor
    {
        public BspNode A;
        public BspNode B;

        public Corridor(BspNode a, BspNode b)
        {
            A = a;
            B = b;
        }

        public enum CorridorIntersectType
        {
            FirstXThenY,
            FirstYThenX
        }

        // public static List<Vector2Int> StraightCorridors(Corridor corridor)
        // {
        //     Vector3Int start = Vector3Int.RoundToInt(corridor.A.Center);
        //     Vector3Int end = Vector3Int.RoundToInt(corridor.B.Center);
        //     List<Vector2Int> tiles = new List<Vector2Int>();
        //
        //     var direction = end - start;
        //     direction.Clamp(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));
        //
        //     Vector3Int current = start;
        //     do
        //     {
        //         current += direction;
        //         Debug.Log($"Current corridor: {current}");
        //         tiles.Add(new Vector2Int(current.x, current.y));
        //
        //     } while (Vector3.Distance(current, end) >= Mathf.Epsilon);
        //
        //     return tiles;
        //
        // }

        public static List<Vector2Int> LCorridors(Corridor corridor, CorridorIntersectType type)
        {

            // Vector3Int start = Vector3Int.RoundToInt(Vector3.Min(corridor.A.Room.center, corridor.B.Room.center));
            // Vector3Int end = Vector3Int.RoundToInt(Vector3.Max(corridor.A.Room.center, corridor.B.Room.center));
            // return GenerateCorridorTiles(type, start, end);
            
            // Fix ---------------
            Vector3Int start = Vector3Int.RoundToInt(corridor.A.Room.center);
            Vector3Int end = Vector3Int.RoundToInt(corridor.B.Room.center);
            Vector3Int direction = end - start;
            direction.Clamp(new Vector3Int(-1, -1, -1), new Vector3Int(1, 1, 1));
            
            List<Vector2Int> tiles = new List<Vector2Int>();

            Vector2Int pos = new Vector2Int(start.x, start.y);
            bool XCondition() => direction.x > 0 ? pos.x < end.x : pos.x > end.x;
            bool YCondition() => direction.y > 0 ? pos.y < end.y : pos.y > end.y;

            switch (type)
            {
                case CorridorIntersectType.FirstXThenY:
                    for (/* iteration variable already set*/ ; XCondition(); pos.x += direction.x)
                    {
                        tiles.Add(pos);
                    }
                    for (/* iteration variable already set*/; YCondition(); pos.y += direction.y)
                    {
                        tiles.Add(pos);
                    }
                    break;
        
                case CorridorIntersectType.FirstYThenX:
                default:
                    for (/* iteration variable already set*/; YCondition(); pos.y +=  direction.y)
                    {
                        tiles.Add(pos);
                    }
                    for (/* iteration variable already set*/ ; XCondition(); pos.x += direction.x)
                    {
                        tiles.Add(pos);
                    }
                    break;
            }

            return tiles;

        }

        #region Unfix

        // private static List<Vector2Int> GenerateCorridorTiles(CorridorIntersectType type, Vector3Int start, Vector3Int end)
        // {
        //
        //     List<Vector2Int> tiles = new List<Vector2Int>();
        //
        //     Vector2Int pos = new Vector2Int(start.x, start.y);
        //     
        //     switch (type)
        //     {
        //         case CorridorIntersectType.FirstXThenY:
        //
        //             for (/* iteration variable already set*/ ; pos.x <= end.x; pos.x++)
        //             {
        //                 tiles.Add(pos);
        //             }
        //             for (/* iteration variable already set*/; pos.y <= end.y; pos.y++)
        //             {
        //                 tiles.Add(pos);
        //             }
        //             break;
        //
        //         case CorridorIntersectType.FirstYThenX:
        //         default:
        //             for (/* iteration variable already set*/; pos.y <= end.y; pos.y++)
        //             {
        //                 tiles.Add(pos);
        //             }
        //             for (/* iteration variable already set*/ ; pos.x <= end.x; pos.x++)
        //             {
        //                 tiles.Add(pos);
        //             }
        //             break;
        //     }
        //
        //     return tiles;
        //
        // }

        #endregion
        
        
        // Fix ---------------
        public static List<Vector2Int> LCorridors(Corridor corridor, CorridorIntersectType type, int thickness = 1)
        {
            // Use the center of the rooms for connection
            Vector3Int start = Vector3Int.RoundToInt(corridor.A.Room.center);
            Vector3Int end = Vector3Int.RoundToInt(corridor.B.Room.center);
    
            List<Vector2Int> tiles = new List<Vector2Int>();
            Vector2Int pos = new Vector2Int(start.x, start.y);

            // Helper to add a "brush" of tiles for thickness
            void AddThickTile(int x, int y) {
                for (int i = 0; i < thickness; i++) {
                    for (int j = 0; j < thickness; j++) {
                        tiles.Add(new Vector2Int(x + i, y + j));
                    }
                }
            }

            if (type == CorridorIntersectType.FirstXThenY) {
                // Move along X
                int xDir = end.x > start.x ? 1 : -1;
                while (pos.x != end.x) {
                    AddThickTile(pos.x, pos.y);
                    pos.x += xDir;
                }
                // Move along Y
                int yDir = end.y > start.y ? 1 : -1;
                while (pos.y != end.y) {
                    AddThickTile(pos.x, pos.y);
                    pos.y += yDir;
                }
            } else {
                // Move along Y first
                int yDir = end.y > start.y ? 1 : -1;
                while (pos.y != end.y) {
                    AddThickTile(pos.x, pos.y);
                    pos.y += yDir;
                }
                int xDir = end.x > start.x ? 1 : -1;
                while (pos.x != end.x) {
                    AddThickTile(pos.x, pos.y);
                    pos.x += xDir;
                }
            }
            return tiles;
        }
    
    }




}
