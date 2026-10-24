using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  public readonly List<Rect> OfficeInitialAreas2730=new List<Rect>();
  public void ConfigureOfficeExploration2730(){
   int publicNode=OfficeRooms2726.Count;
   var links=new List<int>[publicNode+1];for(int i=0;i<links.Length;i++)links[i]=new List<int>();
   foreach(var opening in OfficeOpenings2726)if(opening.Type!="door"){
    var adjacent=OfficeAdjacent2730(opening);if(adjacent.Count==1)adjacent.Add(publicNode);
    foreach(int a in adjacent)foreach(int b in adjacent)if(a!=b&&!links[a].Contains(b))links[a].Add(b);
   }
   var corridors=new[]{new Rect(-2,-24,4,48),new Rect(-20,-6,40,3),new Rect(-20,11,40,3),new Rect(14,14,6,2)};
   System.Func<List<int>,List<Rect>> areas=starts=>{
    var result=new List<Rect>();var queue=new Queue<int>(starts);var visited=new HashSet<int>();
    while(queue.Count>0){int node=queue.Dequeue();if(!visited.Add(node))continue;
     if(node==publicNode){foreach(var r in corridors)result.Add(OfficeExplorePadding2730(r));}
     else result.Add(OfficeExplorePadding2730(OfficeRooms2726[node].Area));
     foreach(int next in links[node])queue.Enqueue(next);
    }
    return result;
   };
   OfficeInitialAreas2730.AddRange(areas(new List<int>{publicNode}));
   foreach(var opening in OfficeOpenings2726)if(opening.Door!=null){
    var adjacent=OfficeAdjacent2730(opening);if(adjacent.Count==1)adjacent.Add(publicNode);
    var reveal=areas(adjacent);opening.Door.ExtraRevealRooms.Clear();
    if(reveal.Count>0){opening.Door.RevealRoom=reveal[0];for(int i=1;i<reveal.Count;i++)opening.Door.ExtraRevealRooms.Add(reveal[i]);}
   }
  }
  List<int> OfficeAdjacent2730(OfficeOpening2726 o){
   var adjacent=new List<int>();for(int i=0;i<OfficeRooms2726.Count;i++){
    Rect r=OfficeRooms2726[i].Area;
    if(o.X>=r.xMin-.01f&&o.X<=r.xMax+.01f&&o.Z>=r.yMin-.01f&&o.Z<=r.yMax+.01f)adjacent.Add(i);
   }return adjacent;
  }
  // Include the wall thickness and a fog texel margin to avoid black seams at corridor edges.
  Rect OfficeExplorePadding2730(Rect r)=>Rect.MinMaxRect(r.xMin-.25f,r.yMin-.25f,r.xMax+.25f,r.yMax+.25f);
 }
}
