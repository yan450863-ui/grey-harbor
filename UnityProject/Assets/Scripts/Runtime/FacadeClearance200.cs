using UnityEngine;
namespace GreyHarbor {
 // Sprite shoulders and facade fittings extend beyond the original solid footprint.
 // This local strip only stops movement; the real building remains the sight/shot wall.
 public sealed class FacadeClearance200:MonoBehaviour {
  public Rect Footprint;public float Padding=.65f;
  public static FacadeClearance200 Create(Transform parent,string name,Rect footprint){
   var root=new GameObject("FacadeClearance200_"+name);root.transform.SetParent(parent,false);var data=root.AddComponent<FacadeClearance200>();data.Footprint=footprint;
   float p=data.Padding;Add(root.transform,"West",new Vector3(footprint.xMin-p/2,.8f,footprint.center.y),new Vector3(p,1.6f,footprint.height+p*2));
   Add(root.transform,"East",new Vector3(footprint.xMax+p/2,.8f,footprint.center.y),new Vector3(p,1.6f,footprint.height+p*2));
   Add(root.transform,"South",new Vector3(footprint.center.x,.8f,footprint.yMin-p/2),new Vector3(footprint.width,1.6f,p));
   Add(root.transform,"North",new Vector3(footprint.center.x,.8f,footprint.yMax+p/2),new Vector3(footprint.width,1.6f,p));return data;
  }
  static void Add(Transform parent,string name,Vector3 center,Vector3 size){var go=new GameObject("FacadeMovementOnly_"+name);go.transform.SetParent(parent,false);go.transform.position=center;var c=go.AddComponent<BoxCollider>();c.size=size;SceneObstacle.Apply(go,ObstacleKind.Low);}
 }
}
