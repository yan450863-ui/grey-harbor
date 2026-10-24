using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace GreyHarbor {public sealed partial class RaidMap {
 public Texture2D OfficeFloorTexture2736;public MeshRenderer OfficeFloorRenderer2736;public int OfficeWallRenderers2736;
 void BuildOfficeFirstSurfaces2736(GameObject support){
  var surface=new GameObject("OfficeFirstFinishedFloor2736");surface.transform.SetParent(Root,false);var owner=surface.AddComponent<UnloadingFloor2725>();
  owner.Texture=OfficeFloorPaint2736();OfficeFloorTexture2736=owner.Texture;
  owner.Mesh=new Mesh{name="OfficeFirstFloorSurface2736"};owner.Mesh.vertices=new[]{new Vector3(-20,.012f,-24),new Vector3(-20,.012f,24),new Vector3(20,.012f,24),new Vector3(20,.012f,-24)};
  owner.Mesh.uv=new[]{Vector2.zero,Vector2.up,Vector2.one,Vector2.right};owner.Mesh.triangles=new[]{0,1,2,0,2,3};owner.Mesh.RecalculateNormals();owner.Mesh.RecalculateBounds();surface.AddComponent<MeshFilter>().sharedMesh=owner.Mesh;
  owner.Material=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld")){name="OfficeFirstFloorFog2736",color=Color.white};owner.Material.mainTexture=owner.Texture;owner.Material.SetFloat("_UseStableGround",1);owner.Material.SetFloat("_SolidFog",0);owner.Material.SetFloat("_Detail",0);
  OfficeFloorRenderer2736=surface.AddComponent<MeshRenderer>();OfficeFloorRenderer2736.sharedMaterial=owner.Material;OfficeFloorRenderer2736.shadowCastingMode=ShadowCastingMode.Off;
  support.GetComponent<MeshRenderer>().enabled=false;
  foreach(var mr in Root.GetComponentsInChildren<MeshRenderer>())if(mr.name.StartsWith("OfficeRoomTint2726"))mr.enabled=false;
  OfficeWallFinish2736();
 }
 static float OfficeHash2736(int x,int z){unchecked{uint h=(uint)(x*73856093)^(uint)(z*19349663)^2736u;h^=h>>13;h*=1274126177u;return(h&65535)/65535f;}}
 Texture2D OfficeFloorPaint2736(){if(OfficeLeft2741)return OfficeLeftFloorPaint2742();
  const int w=960,h=1152;var pixels=new Color32[w*h];var color=new Dictionary<string,Color>{{"01",PixelArt.Hex("788878")},{"02",PixelArt.Hex("A6A18A")},{"03",PixelArt.Hex("687D80")},{"04",PixelArt.Hex("8B8872")},{"05",PixelArt.Hex("99A899")},{"06",PixelArt.Hex("99A899")},{"07",PixelArt.Hex("968167")},{"08",PixelArt.Hex("8C9180")},{"09",PixelArt.Hex("73867D")},{"10",PixelArt.Hex("978571")}};
  if(OfficeFloor2733==2){color["01"]=PixelArt.Hex("7A8B82");color["02"]=PixelArt.Hex("87857B");color["04"]=PixelArt.Hex("94806A");color["07"]=PixelArt.Hex("687C7D");}
  if(OfficeFloor2733==3){color["01"]=PixelArt.Hex("94816A");color["02"]=PixelArt.Hex("818D83");color["03"]=PixelArt.Hex("657B83");color["04"]=PixelArt.Hex("94806A");color["06"]=PixelArt.Hex("8C9180");color["07"]=PixelArt.Hex("738284");color["10"]=PixelArt.Hex("71867E");}
  for(int j=0;j<h;j++)for(int i=0;i<w;i++){
   float x=(i+.5f)/w*40-20,z=(j+.5f)/h*48-24;var room=OfficeRooms2726.Find(r=>r.Area.Contains(new Vector2(x,z)));string id=room==null?"hall":room.Id;Color c=color.ContainsKey(id)?color[id]:PixelArt.Hex("929887");
   float fine=OfficeHash2736(i,j)-.5f,grain=OfficeHash2736(Mathf.FloorToInt(x*3),Mathf.FloorToInt(z*3))-.5f;c*=1+fine*.055f+grain*.045f;
   if((id=="02"&&OfficeFloor2733==1)||id=="hall"||id=="05"||(id=="06"&&OfficeFloor2733!=3)||id.StartsWith("S")){
    float tile=id=="05"||(id=="06"&&OfficeFloor2733!=3)?.55f:1.0f;float tx=Mathf.Repeat(x+20,tile),tz=Mathf.Repeat(z+24,tile);
    c*=.97f+.06f*OfficeHash2736(Mathf.FloorToInt((x+20)/tile),Mathf.FloorToInt((z+24)/tile));
    if(tx<.018f||tz<.018f)c=Color.Lerp(c,PixelArt.Hex("687565"),.4f);
    if((id=="02"&&OfficeFloor2733==1)||id=="hall")if(OfficeHash2736(i*3,j*7)>.975f)c=Color.Lerp(c,PixelArt.Hex("C4C1AC"),.30f);
   }else if((id=="07"&&OfficeFloor2733==1)||(id=="04"&&OfficeFloor2733==2)||(id=="10"&&OfficeFloor2733!=3)||(OfficeFloor2733==3&&(id=="01"||id=="04"))){
    float board=Mathf.Repeat(x+20,.24f);int row=Mathf.FloorToInt((x+20)/.24f);c*=.94f+.12f*OfficeHash2736(row,Mathf.FloorToInt((z+24+(row%3)*.71f)/2.1f));
    if(board<.011f||Mathf.Repeat(z+24+(row%3)*.71f,2.1f)<.012f)c*=.76f;
    c*=.97f+.06f*Mathf.Sin(z*30+row*1.9f);
   }else if(id=="03"||(id=="07"&&OfficeFloor2733==2)){if((i+j)%3==0)c*=.975f;}
   // Aged edge grime and threshold wear, restrained rather than random large paint patches.
   if(room!=null){float edge=Mathf.Min(Mathf.Min(x-room.Area.xMin,room.Area.xMax-x),Mathf.Min(z-room.Area.yMin,room.Area.yMax-z));c*=Mathf.Lerp(.84f,1,Mathf.Clamp01(edge/.30f));}
   foreach(var o in OfficeOpenings2726){float d=(new Vector2(x,z)-new Vector2(o.X,o.Z)).sqrMagnitude;if(d<1.1f)c=Color.Lerp(c,PixelArt.Hex("817E6C"),.09f*(1-d/1.1f));}
   pixels[j*w+i]=c;
  }
  var tex=new Texture2D(w,h,TextureFormat.RGBA32,true){name="OfficeFirstTerrazzoCarpetWoodTile2736",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp,anisoLevel=4};tex.SetPixels32(pixels);tex.Apply(true,false);return tex;
 }
 void OfficeWallFinish2736(){
  var art=new WorkshopWastelandArt(this);var structures=new List<MeshRenderer>();
  foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="OfficeWall2726"||r.name=="OfficeDoorHeader2726")structures.Add(r);
  foreach(var r in structures){var b=r.bounds;bool horizontal=b.size.x>b.size.z;var along=horizontal?Vector3.right:Vector3.forward;var normal=horizontal?Vector3.forward:Vector3.right;float length=horizontal?b.size.x:b.size.z,thickness=horizontal?b.size.z:b.size.x;
   foreach(float sign in new[]{-1f,1f}){
    var face=b.center+normal*sign*(thickness/2+.008f);var sample=face+normal*sign*.25f;var room=OfficeRooms2726.Find(q=>q.Area.Contains(new Vector2(sample.x,sample.z)));bool bathroom=room!=null&&(OfficeLeft2741?room.Name=="卫生间":room.Id=="05"||(room.Id=="06"&&OfficeFloor2733!=3));
    string lower=bathroom?"80998E":room!=null&&room.Id=="07"?"8C7864":"6E897D",upper="C1C0A5";
    float bottom=b.min.y,top=b.max.y,split=Mathf.Clamp(1.18f,bottom,top);
    if(split>bottom+.01f)art.Panel("OfficeLowerWainscot2736",lower,new Vector3(face.x,(bottom+split)/2,face.z),along,Vector3.up,length,split-bottom);
    if(top>split+.01f)art.Panel("OfficeUpperPlaster2736",upper,new Vector3(face.x,(split+top)/2,face.z),along,Vector3.up,length,top-split);
    if(bottom<.2f){art.Panel("OfficeSkirting2736","495F56",new Vector3(face.x,.08f,face.z)+normal*sign*.005f,along,Vector3.up,length,.14f);art.Panel("OfficePaintTrim2736","A4AE96",new Vector3(face.x,1.18f,face.z)+normal*sign*.005f,along,Vector3.up,length,.035f);}
    for(int k=0;k<Mathf.FloorToInt(length/2.5f);k++){float a=(k+.6f)*2.5f-length/2,y=1.5f+OfficeHash2736(k,Mathf.RoundToInt(b.center.x+b.center.z))*1.1f;if(y<bottom+.1f||y>top-.1f)continue;art.Panel("OfficePlasterWear2736","B2B49C",face+along*a+Vector3.up*(y-face.y)+normal*sign*.007f,along,Vector3.up,.16f,.055f);}
   }
  }
  var skin=FreightModelRoot2717("OfficeFirstWallFinish2736",Vector3.zero);FreightBakeModel2717(skin,art,0);
  var shader=GreyHarbor.FlashlightShaders2763.Load("FreightWall2718");var life=Root.gameObject.AddComponent<WorkshopArtResources>();var copies=new Dictionary<Material,Material>();
  foreach(var r in Root.GetComponentsInChildren<MeshRenderer>()){
   bool wall=r.name.StartsWith("OfficeBroken")||r.name=="OfficeWall2726"||r.name=="OfficeDoorHeader2726"||r.name=="OfficeRoomDoor2726"||r.name=="OfficeReturnDoor2726"||r.transform.IsChildOf(skin.transform);if(!wall||!r.sharedMaterial)continue;
   if(!copies.TryGetValue(r.sharedMaterial,out var mat)){mat=new Material(r.sharedMaterial){shader=shader,name="OfficeWallFog2736_"+r.sharedMaterial.name};mat.SetVector("_FreightInnerRect2718",new Vector4(-19.55f,-23.55f,19.55f,23.55f));if(r.name=="OfficeWall2726"||r.name=="OfficeDoorHeader2726")mat.color=PixelArt.Hex("B9B99F");copies.Add(r.sharedMaterial,mat);life.Materials.Add(mat);}
   r.sharedMaterial=mat;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=false;OfficeWallRenderers2736++;
  }
 }
}}
