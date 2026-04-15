

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class AStarPoint
{
        public Vector3Int Pos;
        
        public float G; //Dijkstra
        public float H; //Heuristic
        public AStarPoint Parent = null;
        
        public AStarPoint(Vector3Int pos, float g, float h, AStarPoint parent)
        {
                Pos = pos;
                G = g;
                H = h;
                Parent = parent;
        }
        
        public float F => G + H;//Global score
        //public Vector3Int Pos => _pos;
       // public float G => _g;
        //public float H => _h;
       
}

public static class AStarProcess
{
        public static Vector3Int[] Process(List<Vector3Int> walkables, Vector3Int start, Vector3Int end)
        {
               List <Vector3Int> path = new List<Vector3Int>();
               
               List<Vector3Int> aiWalkables = walkables.Where(w =>GetWalkablesNeighbours(walkables,w) >= 8).ToList();

                if (aiWalkables.Contains(start) && aiWalkables.Contains(end))
                {
                        //Basic Stuff ------
                        //path.Add(start);
                        //path.Add(end);

                        List<AStarPoint> openPoints = new List<AStarPoint>();
                        openPoints.Add(new AStarPoint(start, 0, Vector3Int.Distance(start, end), null));
                        
                        List<Vector3Int> closedPoints = new List<Vector3Int>();

                        do
                        {
                                
                                //BFS pathing
                                AStarPoint currentPoint = openPoints.OrderBy(p =>p.F).First();
                                openPoints.Remove(currentPoint);
                                
                                //We found the end
                                if (currentPoint.Pos == end)
                                {
                                        Debug.Log("J'ai trouver la sortie, elle est dans cette pieces");
                                        GetPath(currentPoint,path);
                                        return path.ToArray();
                                        
                                }
                                
                                //closedPoint.Add(currentPoint.Pos);

                                //Check neighbours
                                foreach (var neighbour in Utils.MooreDirections.OrderBy( _ => Random.value))
                                {
                                        Vector3Int pos = currentPoint.Pos + neighbour;
                                        //Add a point if
                                        //- the point is in the map
                                        //- the point is not already checked
                                        //if (walkables.Contains(pos) && !closedPoints.Contains(pos))
                                        if (aiWalkables.Contains(pos))
                                        {
                                                float newG = currentPoint.G + Vector3Int.Distance(pos, currentPoint.Pos);
                                                float newH = Vector3Int.Distance(pos, end);

                                                var existingPoint = openPoints.FirstOrDefault(p => p.Pos == pos);

                                                if (existingPoint == null)
                                                {
                                                        openPoints.Add(new AStarPoint(pos, newG, newH, currentPoint));
                                                }
                                                
                                               else if (existingPoint.F > newG + newH)
                                               {
                                                       existingPoint.G = newG;
                                                        existingPoint.H = newH;
                                                        existingPoint.Parent = currentPoint;
                                               }
                                              
                                                //Check for points already checked
                                                //closedPoints.Add(pos);
                                        }
                                }
                                

                        } while (openPoints.Count > 0);

                }
                
                return path.ToArray();
        }

        private static void GetPath(AStarPoint pathPoint, List<Vector3Int> path)
        {
                path.Add(pathPoint.Pos);
                if (pathPoint.Parent != null) GetPath(pathPoint.Parent, path);  
                
        }

        private static int GetWalkablesNeighbours(List<Vector3Int> walkables, Vector3Int pos)
        {
                int neighbourCount = 0;
                foreach (var neighbour in Utils.MooreDirections)
                {
                      if(walkables.Contains(pos + neighbour)) neighbourCount++;
                }
                return neighbourCount;
        }
}
