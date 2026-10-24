using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  // Exterior decoration only: the original shell remains the sole building collider.
  public void BuildOfficeFacade2746(GameObject shell,bool right){
   string rootName="OfficeFacade2746_"+(right?"Right":"Left");if(Root.Find(rootName))return;
   var origin=PortPoint(right?93:70,164);var root=new GameObject(rootName);root.transform.SetParent(Root,false);root.transform.position=origin;
   shell.name=(right?"办公右塔":"办公左塔")+"外壳2746";shell.GetComponent<Renderer>().sharedMaterial=Mat(right?"AEA78B":"A49F86",true);
   var life=root.AddComponent<WorkshopArtResources>();var pieces=new Dictionary<string,List<CombineInstance>>();
   var primitive=GameObject.CreatePrimitive(PrimitiveType.Cube);var cube=primitive.GetComponent<MeshFilter>().sharedMesh;primitive.SetActive(false);
   if(Application.isPlaying)Object.Destroy(primitive);else Object.DestroyImmediate(primitive);
   System.Action<string,Vector3,Vector3,float> box=(hex,pos,size,yaw)=>{
    if(!pieces.TryGetValue(hex,out var list)){list=new List<CombineInstance>();pieces.Add(hex,list);}
    list.Add(new CombineInstance{mesh=cube,transform=Matrix4x4.TRS(pos,Quaternion.Euler(0,yaw,0),size)});
   };
   // Painted lower storey, thin floor bands and corners make the three-storey scale legible.
   foreach(int side in new[]{0,1,2,3}){
    float yaw=side*90;Vector3 normal=Quaternion.Euler(0,yaw,0)*Vector3.back;
    Vector3 across=Quaternion.Euler(0,yaw,0)*Vector3.right;float reach=side%2==0?9:8,width=side%2==0?16.03f:18.03f;
    box(right?"687F73":"738171",normal*(reach+.015f)+Vector3.up*.63f,new Vector3(width,1.26f,.035f),yaw);
    foreach(float y in new[]{3.4f,6.8f,10.15f}){
     box("CBC3A5",normal*(reach+.045f)+Vector3.up*y,new Vector3(width+.08f,.13f,.10f),yaw);
     box("797D69",normal*(reach+.023f)+Vector3.up*(y-.105f),new Vector3(width,.045f,.052f),yaw);
    }
    // Six bays per wall; avoid the entrance and the second-floor bridge junction.
    for(int floor=0;floor<3;floor++)for(int bay=0;bay<6;bay++){
     float offset=(bay-2.5f)*(side%2==0?2.35f:2.65f);
     bool entrance=side==0&&floor==0&&(bay==2||bay==3);
     bool inner=right?side==3:side==1;
     if(entrance||(inner&&floor==1&&Mathf.Abs((across*offset).z-3.5f)<2.1f))continue;
     float y=floor*3.4f+1.98f;var p=normal*(reach+.11f)+across*offset+Vector3.up*y;
     var marker=new GameObject("OfficeWindow2746");marker.transform.SetParent(root.transform,false);marker.transform.localPosition=p;
     box("465C58",p,new Vector3(1.85f,1.65f,.10f),yaw);
     string glass=(bay+floor+side)%5==0?"435B61":(bay+floor)%3==0?"678C8E":"557B7E";
     box(glass,p+normal*.065f,new Vector3(1.63f,1.42f,.045f),yaw);
     box("ADB19D",p+normal*.10f+Vector3.up*.42f,new Vector3(1.61f,.045f,.025f),yaw);
     box("B4B298",p+normal*.15f-Vector3.up*.87f,new Vector3(2.02f,.11f,.29f),yaw);
     box("465C58",p+normal*.105f,new Vector3(.06f,1.46f,.025f),yaw);
     // Uneven blinds and reflections, deliberately restrained rather than bright white.
     if((bay+floor+side)%4==1)for(int i=0;i<4;i++)box("9EA68E",p+normal*.095f+Vector3.up*(.58f-i*.115f),new Vector3(1.57f,.045f,.018f),yaw);
    }
    // Deterministic weathering patches follow seams and window edges, never floating off the wall.
    for(int i=0;i<8;i++){
     float offset=(i-3.5f)*(width*.11f);float y=.40f+(i*17+side*7)%9;
     box(i%3==0?"8B7C60":"96957B",normal*(reach+.055f)+across*offset+Vector3.up*y,new Vector3(.30f+(i%3)*.18f,.09f,.025f),yaw);
    }
   }
   foreach(float x in new[]{-8.04f,8.04f})foreach(float z in new[]{-9.04f,9.04f})box("C1BCA1",new Vector3(x,5.1f,z),new Vector3(.13f,10.2f,.13f),0);
   // Drain pipes with support brackets at the rear corners.
   foreach(float x in new[]{-7.7f,7.7f}){
    box("6B7770",new Vector3(x,5,9.13f),new Vector3(.11f,9.8f,.12f),0);
    foreach(float y in new[]{1.1f,4.1f,7.2f})box("8C8F7D",new Vector3(x,y,9.16f),new Vector3(.23f,.06f,.15f),0);
    box("725B45",new Vector3(x,.26f,9.21f),new Vector3(.14f,.34f,.30f),0);
   }
   // A dark flat roof with a low parapet and practical ventilation plant.
   box("616D64",new Vector3(0,10.22f,0),new Vector3(15.95f,.08f,17.95f),0);
   foreach(float z in new[]{-8.91f,8.91f})box("979A80",new Vector3(0,10.37f,z),new Vector3(16.12f,.28f,.20f),0);
   foreach(float x in new[]{-7.91f,7.91f})box("979A80",new Vector3(x,10.37f,0),new Vector3(.20f,.28f,17.7f),0);
   foreach(var p in new[]{new Vector3(-4,10.69f,4.8f),new Vector3(3.8f,10.69f,5.2f)}){
    box("819283",p,new Vector3(2.0f,.85f,1.6f),0);box("ADB19D",p+Vector3.up*.44f,new Vector3(2.1f,.06f,1.7f),0);
    for(int i=0;i<7;i++)box("465C58",p+new Vector3(-.72f+i*.24f,0,-.813f),new Vector3(.10f,.58f,.025f),0);
    box("6B7770",p+new Vector3(0,-.16f,1.1f),new Vector3(.7f,.32f,1.1f),0);
   }
   box("797D69",new Vector3(4.7f,10.6f,-2.2f),new Vector3(1.05f,.76f,1.05f),0);
   box("465C58",new Vector3(4.7f,11.02f,-2.2f),new Vector3(1.28f,.13f,1.28f),0);
   // Existing south portal keeps its position. The left door is sealed by the parent's overturned truck.
   var front=new Vector3(0,0,-9.12f);
   box("465C58",front+Vector3.up*1.25f,new Vector3(3.1f,2.5f,.13f),0);
   box(right?"678C8E":"79664F",front+new Vector3(0,1.25f,-.085f),new Vector3(2.76f,2.28f,.05f),0);
   foreach(float x in new[]{-1.55f,0,1.55f})box("ADB19D",front+new Vector3(x,1.25f,-.13f),new Vector3(.085f,2.55f,.08f),0);
   box("C1BCA1",front+new Vector3(0,2.57f,-.08f),new Vector3(3.4f,.20f,.25f),0);
   box("687F73",front+new Vector3(0,2.87f,-.80f),new Vector3(4.25f,.20f,1.75f),0);
   box("ADB19D",front+new Vector3(0,2.79f,-1.59f),new Vector3(4.3f,.13f,.10f),0);
   foreach(float x in new[]{-.19f,.19f})box("C1BCA1",front+new Vector3(x,1.13f,-.2f),new Vector3(.035f,.4f,.06f),0);
   box("465C58",front+new Vector3(0,3.15f,-.02f),new Vector3(3.3f,.42f,.08f),0);
   // Abstract company plaque, so no illegible tiny generated text is introduced.
   box("B4B298",front+new Vector3(-1.21f,3.15f,-.07f),new Vector3(.20f,.25f,.03f),0);
   for(int i=0;i<6;i++)box("ADB19D",front+new Vector3(-.7f+i*.3f,3.15f,-.07f),new Vector3(.17f,.11f,.03f),0);
   foreach(var entry in pieces){
    var child=new GameObject("OfficeFacadeBatch2746_"+entry.Key);child.transform.SetParent(root.transform,false);
    var mesh=new Mesh{name=child.name};mesh.indexFormat=IndexFormat.UInt32;mesh.CombineMeshes(entry.Value.ToArray(),true,true);life.Meshes.Add(mesh);
    child.AddComponent<MeshFilter>().sharedMesh=mesh;child.AddComponent<MeshRenderer>().sharedMaterial=Mat(entry.Key,true);
   }
  }
 }
}
