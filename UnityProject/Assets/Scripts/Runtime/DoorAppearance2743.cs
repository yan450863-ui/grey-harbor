using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
namespace GreyHarbor { public sealed partial class RaidMap {
 // Cosmetic geometry follows the original leaf. Collision, hinge, yaw and safety remain untouched.
 internal void FinishInteriorDoors2743(){
  foreach(var door in Doors){
   if(door.Portal||!door.Pivot||!door.Collider)continue;
   bool finished=false;foreach(Transform child in door.Pivot)if(child.name.StartsWith("DoorArt2743_")){finished=true;break;}
   if(finished)continue;
   var box=door.Collider as BoxCollider;if(!box)continue;
   var slab=box.transform;var old=slab.GetComponent<MeshRenderer>();if(!old||!old.sharedMaterial)continue;
   string style=DoorStyle2743(door);
   FinishDoorLeaf2743(slab,box.center,box.size,old,door.Pivot,style,"DoorArt2743_"+style);
  }
  if(!IsInterior||!Root)return;
  // Explicit existing transition slabs only: never manufacture a second leaf near a portal.
  foreach(var leaf in Root.GetComponentsInChildren<MeshRenderer>()){
   string name=leaf.gameObject.name;
   if(name!="OfficeBridgeEndDoor2744"&&name!="OfficeReturnDoor2726"&&name!="OfficeBridgeEntrancePanel2741"&&name!="FreightReturnDoor2716"&&name!="FerryExitDoor"&&name!="PortExitDoor"&&name!="YardExitDoor"&&name!="ExitDoor"&&name!="PortLockedDoor")continue;
   bool moving=false;foreach(var d in Doors)if(d.Collider&&d.Collider.gameObject==leaf.gameObject&&d.Pivot){moving=true;break;}if(moving)continue;
   var mesh=leaf.GetComponent<MeshFilter>();if(!mesh||!mesh.sharedMesh)continue;
   string style=name=="OfficeReturnDoor2726"?"OfficeGlass":name=="PortLockedDoor"?"SecureSteel":name=="FreightReturnDoor2716"?"WarehouseIndustrial":"FireSteel";
   string marker="DoorArt2743_"+style+"_"+name;if(name=="OfficeBridgeEndDoor2744")marker+=leaf.transform.position.x<0?"_West":"_East";Transform parent=leaf.transform.parent?leaf.transform.parent:Root;
   if(parent.Find(marker))continue;var bounds=mesh.sharedMesh.bounds;
   FinishDoorLeaf2743(leaf.transform,bounds.center,bounds.size,leaf,parent,style,marker);
  }
 }
 void FinishDoorLeaf2743(Transform slab,Vector3 localCenter,Vector3 localSize,MeshRenderer old,Transform parent,string style,string marker){
   Vector3 dims=new Vector3(slab.TransformVector(Vector3.right*localSize.x).magnitude,slab.TransformVector(Vector3.up*localSize.y).magnitude,slab.TransformVector(Vector3.forward*localSize.z).magnitude);
   bool alongX=dims.x>=dims.z;float width=alongX?dims.x:dims.z,height=dims.y,thickness=alongX?dims.z:dims.x;
   if(width<.3f||height<.5f)return;
   string body=style=="Wood"?"8D6848":style=="OfficeGlass"?"526C75":style=="FireSteel"?"7B493D":style=="WarehouseIndustrial"?"6B8270":style=="Restroom"?"778F85":"465E77";
   var view=new GameObject(marker);view.transform.SetParent(parent,false);view.transform.position=slab.TransformPoint(localCenter);view.transform.rotation=slab.rotation;
   var life=view.AddComponent<WorkshopArtResources>();var skin=new Material(old.sharedMaterial){name="DoorLeaf2743_"+style};skin.color=PixelArt.Hex(body);if(skin.HasProperty("_Detail"))skin.SetFloat("_Detail",.06f);old.sharedMaterial=skin;life.Materials.Add(skin);
   var art=new WorkshopWastelandArt(this);Vector3 across=alongX?Vector3.right:Vector3.forward,normal=alongX?Vector3.forward:Vector3.right;
   float edge=Mathf.Min(.055f,width*.04f),face=thickness*.5f+.008f;
   Vector3 hinge=view.transform.InverseTransformPoint(parent.position);float hingeSide=Mathf.Sign(Vector3.Dot(hinge,across));if(hingeSide==0)hingeSide=-1;
   foreach(float side in new[]{-1f,1f}){
    Vector3 n=normal*side;
    // Clear trim and kick plate on both sides rather than one camera-facing decal.
    art.Panel("DoorMetal","9CA79B",n*face+Vector3.down*(height*.5f-.12f),across,Vector3.up,width-.08f,.19f);
    foreach(float x in new[]{-1f,1f})art.Panel("DoorMetal","3E514D",across*x*(width*.5f-edge)+n*face,across,Vector3.up,edge,height-.04f);
    foreach(float y in new[]{-1f,1f})art.Panel("DoorMetal","3E514D",Vector3.up*y*(height*.5f-edge)+n*face,across,Vector3.up,width-.04f,edge);
    if(style=="Wood"){
     for(int i=0;i<9;i++){float x=(i-4)*width*.095f;art.Panel("DoorMarks",i%2==0?"715237":"A07A53",across*x+n*(face+.003f),across,Vector3.up,.008f,height*.86f);}
     foreach(float y in new[]{-.27f,.27f}){art.Panel("DoorPaint","AB875E",Vector3.up*y*height+n*(face+.002f),across,Vector3.up,width*.72f,height*.25f);art.Panel("DoorPaint","806044",Vector3.up*y*height+n*(face+.004f),across,Vector3.up,width*.65f,height*.19f);}
    }else if(style=="OfficeGlass"){
     art.Panel("DoorMetal","BCC1AB",Vector3.up*height*.10f+n*(face+.002f),across,Vector3.up,width*.73f,height*.58f);
     art.Panel("DoorGlass","729795",Vector3.up*height*.10f+n*(face+.005f),across,Vector3.up,width*.66f,height*.52f);
     for(int i=0;i<3;i++)art.Panel("DoorMarks","A6B8AA",Vector3.up*(height*(.02f+i*.052f))+n*(face+.008f),across,Vector3.up,width*.64f,.015f);
     art.Panel("DoorMetal","526C75",Vector3.up*height*.1f+n*(face+.01f),across,Vector3.up,.032f,height*.53f);
    }else if(style=="FireSteel"){
     art.Panel("DoorPaint","965C45",n*(face+.002f),across,Vector3.up,width*.8f,height*.75f);
     art.Panel("DoorMetal","ADB1A2",Vector3.up*height*.20f+n*(face+.004f),across,Vector3.up,width*.31f,height*.28f);
     art.Panel("DoorGlass","4F6E70",Vector3.up*height*.20f+n*(face+.008f),across,Vector3.up,width*.24f,height*.22f);
     art.Panel("DoorMetal","A7ADA0",Vector3.up*(-height*.1f)+n*(face+.052f),across,Vector3.up,width*.68f,.075f);
     art.Panel("DoorMarks","AFB396",Vector3.up*height*.38f+n*(face+.006f),across,Vector3.up,width*.4f,.1f);
    }else if(style=="WarehouseIndustrial"){
     for(int i=0;i<6;i++)art.Panel("DoorMetal",i%2==0?"809483":"4F685D",Vector3.up*((i-2.5f)*height*.125f)+n*(face+.01f),across,Vector3.up,width*.8f,.035f);
     foreach(float y in new[]{-.27f,.27f})art.Panel("DoorMarks","A99B63",Vector3.up*y*height+n*(face+.015f),across,Vector3.up,width*.78f,.055f);
     for(int i=0;i<5;i++)art.Panel("DoorMarks","65543E",across*((i-2)*width*.15f)+Vector3.up*(-height*.27f)+n*(face+.018f),across,Vector3.up,width*.075f,.055f);
    }else if(style=="Restroom"){
     art.Panel("DoorPaint","A5B1A0",n*(face+.002f),across,Vector3.up,width*.8f,height*.78f);
     art.Panel("DoorPaint","587F83",Vector3.up*height*.2f+n*(face+.007f),across,Vector3.up,width*.27f,.29f);
     // Simple human figure and louvred ventilation identify the washroom without text textures.
     art.Panel("DoorMarks","CFD0BA",Vector3.up*(height*.2f+.08f)+n*(face+.011f),across,Vector3.up,.045f,.045f);
     art.Panel("DoorMarks","CFD0BA",Vector3.up*height*.2f+n*(face+.011f),across,Vector3.up,.075f,.10f);
     for(int i=0;i<5;i++)art.Panel("DoorMarks","536F68",Vector3.up*(-height*.28f+i*.031f)+n*(face+.011f),across,Vector3.up,width*.65f,.013f);
    }else{
     art.Panel("DoorPaint","5D7790",n*(face+.002f),across,Vector3.up,width*.8f,height*.8f);
     foreach(float y in new[]{-.28f,.28f})art.Panel("DoorMetal","A7ADA4",Vector3.up*y*height+n*(face+.012f),across,Vector3.up,width*.85f,.065f);
     for(int i=0;i<4;i++)foreach(float x in new[]{-.36f,.36f})art.Panel("DoorMetal","C0B690",across*x*width+Vector3.up*(i-1.5f)*height*.18f+n*(face+.016f),across,Vector3.up,.032f,.032f);
     art.Panel("DoorMarks","C0B690",Vector3.up*height*.3f+n*(face+.018f),across,Vector3.up,width*.32f,.10f);
    }
    float latch=-hingeSide*(width*.5f-.14f);float handleY=-height*.5f+Mathf.Min(.98f,height*.46f);
    Vector3 pull=across*latch+Vector3.up*handleY+n*(face+.025f);
    art.Panel("DoorMetal","BEB894",pull,across,Vector3.up,.075f,.22f);
    art.Tube("DoorHardware","BCBAA4",pull+n*.055f,pull+n*.055f-across*hingeSide*.17f,.022f);
    art.Panel("DoorMarks","263936",pull-Vector3.up*.16f+n*.005f,across,Vector3.up,.024f,.05f);
    foreach(float y in new[]{-.32f,0f,.32f})art.Tube("DoorHardware","A3A68E",across*hingeSide*(width*.5f-.045f)+Vector3.up*(y*height-.05f)+n*(face+.01f),across*hingeSide*(width*.5f-.045f)+Vector3.up*(y*height+.05f)+n*(face+.01f),.018f);
    for(int i=0;i<5;i++){float x=(i-2)*width*.16f,y=-height*.35f+(i%3)*.04f;art.Panel("DoorMarks",style=="Wood"?"66533C":"8D8060",across*x+Vector3.up*y+n*(face+.02f),across,Vector3.up,.10f,.013f);}
   }
   art.Bake(view,life);
   MergeDoorPalette2743(view,life,skin,style);
 }
 void MergeDoorPalette2743(GameObject view,WorkshopArtResources life,Material skin,string style){
  var sources=view.GetComponentsInChildren<MeshRenderer>();var colors=new List<Color>();
  foreach(var renderer in sources)if(renderer.sharedMaterial&&!colors.Contains(renderer.sharedMaterial.color))colors.Add(renderer.sharedMaterial.color);
  if(colors.Count==0)return;
  int pixels=Mathf.NextPowerOfTwo(colors.Count);var palette=new Texture2D(pixels,1,TextureFormat.RGBA32,false){name="DoorPalette2743_"+style,filterMode=FilterMode.Point,wrapMode=TextureWrapMode.Clamp,anisoLevel=0};
  var paint=new Color[pixels];for(int i=0;i<pixels;i++)paint[i]=colors[Mathf.Min(i,colors.Count-1)];palette.SetPixels(paint);palette.Apply(false,false);
  view.AddComponent<DoorPaletteLifetime2743>().Palette=palette;
  var vertices=new List<Vector3>();var normals=new List<Vector3>();var uv=new List<Vector2>();var triangles=new List<int>();
  foreach(var renderer in sources){
   var filter=renderer.GetComponent<MeshFilter>();if(!filter||!filter.sharedMesh)continue;var mesh=filter.sharedMesh;var transform=view.transform.worldToLocalMatrix*filter.transform.localToWorldMatrix;int first=vertices.Count;
   var sourceVertices=mesh.vertices;var sourceNormals=mesh.normals;int color=colors.IndexOf(renderer.sharedMaterial.color);var swatch=new Vector2((color+.5f)/pixels,.5f);
   for(int i=0;i<sourceVertices.Length;i++){vertices.Add(transform.MultiplyPoint3x4(sourceVertices[i]));normals.Add(transform.MultiplyVector(sourceNormals.Length==sourceVertices.Length?sourceNormals[i]:Vector3.up).normalized);uv.Add(swatch);}
   foreach(int index in mesh.triangles)triangles.Add(first+index);renderer.enabled=false;
  }
  var merged=new Mesh{name="DoorPaletteMesh2743_"+style};if(vertices.Count>65535)merged.indexFormat=IndexFormat.UInt32;merged.SetVertices(vertices);merged.SetNormals(normals);merged.SetUVs(0,uv);merged.SetTriangles(triangles,0);merged.RecalculateBounds();life.Meshes.Add(merged);
  var combined=new GameObject("DoorPalette2743");combined.transform.SetParent(view.transform,false);combined.AddComponent<MeshFilter>().sharedMesh=merged;
  var material=new Material(skin){name="DoorPaletteMaterial2743_"+style,color=Color.white,mainTexture=palette};if(skin.shader&&skin.shader.name=="GreyHarbor/FreightWall2718"){var shader=GreyHarbor.FlashlightShaders2763.Load("DoorFreight2743");if(shader)material.shader=shader;}if(material.HasProperty("_UseStableGround"))material.SetFloat("_UseStableGround",1);if(material.HasProperty("_Detail"))material.SetFloat("_Detail",.045f);life.Materials.Add(material);
  var visible=combined.AddComponent<MeshRenderer>();visible.sharedMaterial=material;visible.shadowCastingMode=ShadowCastingMode.Off;visible.receiveShadows=true;
 }

 string DoorStyle2743(WorldDoor door){
  string n=door.Name??"",area=InteriorId??"";
  if(n.Contains("卫生")||n.Contains("厕所")||n.Contains("洗手")||n.Contains("男卫")||n.Contains("女卫"))return "Restroom";
  if(n.Contains("楼梯")||n.Contains("连廊")||n.Contains("消防"))return "FireSteel";
  if(door.Locked||n.Contains("研究")||n.Contains("资料库")||n.Contains("机房")||n.Contains("配电"))return "SecureSteel";
  if(n.Contains("维修")||n.Contains("工间")||n.Contains("备件")||n.Contains("工具")||n.Contains("仓")||n.Contains("货物")||n.Contains("测试")||n.Contains("检测")||n.Contains("通信"))return "WarehouseIndustrial";
  if(n.Contains("休息")||n.Contains("更衣")||n.Contains("茶水")||n.Contains("卧室")||n.Contains("生活")||area.Contains("yardhome"))return "Wood";
  if(area.Contains("office")||n.Contains("办公")||n.Contains("会议")||n.Contains("接待")||n.Contains("值班")||n.Contains("管理"))return "OfficeGlass";
  return "Wood";
 }
}
 public sealed class DoorPaletteLifetime2743:MonoBehaviour {
  public Texture2D Palette;void OnDestroy(){if(Palette){if(Application.isPlaying)Destroy(Palette);else DestroyImmediate(Palette);}}
 }
}
