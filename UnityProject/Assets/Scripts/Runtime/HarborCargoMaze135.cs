using UnityEngine;
using System.Collections.Generic;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<GameObject> CargoMazeBoxes=new List<GameObject>();
  public readonly List<Vector3[]> CargoPassages=new List<Vector3[]>();
  public readonly List<Vector3> CargoRouteChecks=new List<Vector3>();
  void BuildCargoMaze(){BuildCargoMaze2712();}
 }
}
