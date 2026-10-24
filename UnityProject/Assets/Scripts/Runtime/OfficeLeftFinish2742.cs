using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidMap {
 public GameObject OfficeLeftFurnitureBarrier2742;
 void BuildOfficeLeftSurfaces2742(GameObject support){BuildOfficeFirstSurfaces2736(support);}
 Texture2D OfficeLeftFloorPaint2742(){
  const int w=960,h=1152;var pixels=new Color32[w*h];
  for(int j=0;j<h;j++)for(int i=0;i<w;i++){
   float x=(i+.5f)/w*40-20,z=(j+.5f)/h*48-24;var room=OfficeRooms2726.Find(r=>r.Area.Contains(new Vector2(x,z)));string name=room==null?"公共走廊":room.Name;
   bool tile=room==null||room.Stair||name=="卫生间",wood=name.Contains("主管")||name.Contains("休息")||name.Contains("会议"),carpet=name.Contains("调度")||name=="技术办公室",epoxy=name.Contains("机房")||name.Contains("通信")||name.Contains("配电");
   Color c=PixelArt.Hex(tile?"949E8E":wood?"97836A":carpet?"6C8285":epoxy?"71867E":"8B9080");float fine=OfficeHash2736(i+2742,j)-.5f;c*=1+fine*.055f;
   if(tile){float side=name=="卫生间"?.55f:1f;float tx=Mathf.Repeat(x+20,side),tz=Mathf.Repeat(z+24,side);if(tx<.018f||tz<.018f)c*=.77f;c*=.97f+.06f*OfficeHash2736(Mathf.FloorToInt(x/side),Mathf.FloorToInt(z/side));}
   else if(wood){int row=Mathf.FloorToInt((x+20)/.24f);if(Mathf.Repeat(x+20,.24f)<.011f||Mathf.Repeat(z+24+(row%3)*.71f,2.1f)<.012f)c*=.76f;c*=.94f+.12f*OfficeHash2736(row,Mathf.FloorToInt(z/2));}
   else if(carpet){if((i+j)%3==0)c*=.97f;}
   else{c*=1+(OfficeHash2736(Mathf.FloorToInt(x*4),Mathf.FloorToInt(z*4))-.5f)*.07f;if(!epoxy&&Mathf.Repeat(x+20,4)<.012f)c*=.88f;}
   if(room!=null){float edge=Mathf.Min(Mathf.Min(x-room.Area.xMin,room.Area.xMax-x),Mathf.Min(z-room.Area.yMin,room.Area.yMax-z));c*=Mathf.Lerp(.85f,1,Mathf.Clamp01(edge/.3f));}pixels[j*w+i]=c;
  }
  var tex=new Texture2D(w,h,TextureFormat.RGBA32,true){name="OfficeLeftFinishedFloor2742",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp,anisoLevel=4};tex.SetPixels32(pixels);tex.Apply(true,false);return tex;
 }
 void BuildOfficeLeftBarrier2742(){
  OfficeLeftFurnitureBarrier2742=new GameObject("OfficeLeftCollapsedCabinetObstruction2742");OfficeLeftFurnitureBarrier2742.transform.SetParent(Root,false);float z=OfficeBlockers2726[0].center.y;
  foreach(float sign in new[]{-1f,1f}){var shelf=new FutureAssetKit2717(this).Build(FutureProp2717.OpenArchiveShelf,new Vector3(sign*.98f,0,z),0);shelf.transform.rotation=Quaternion.Euler(0,0,sign*90);Physics.SyncTransforms();var body=shelf.GetComponentInChildren<Collider>();shelf.transform.position+=Vector3.right*(sign*.98f-body.bounds.center.x);FreightPlaceOnFloor2717(shelf);shelf.transform.SetParent(OfficeLeftFurnitureBarrier2742.transform,true);}
  var art=new WorkshopWastelandArt(this);foreach(float x in new[]{-1.3f,-.4f,.8f,1.4f}){art.Cube("ScatteredTechnicalPaper2742","D5C7A2",new Vector3(x,.04f,z-.45f),new Vector3(.25f,.025f,.16f),false);art.Cube("ScatteredTechnicalBinder2742",x<0?"547D99":"708969",new Vector3(x,.08f,z+.5f),new Vector3(.24f,.1f,.16f),false);}art.Bake(OfficeLeftFurnitureBarrier2742,OfficeLeftFurnitureBarrier2742.AddComponent<WorkshopArtResources>());Physics.SyncTransforms();
 }
}}
