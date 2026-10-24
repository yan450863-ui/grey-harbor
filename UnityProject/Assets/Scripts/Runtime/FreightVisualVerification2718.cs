using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidVerification {
  IEnumerator RunRegionalFixes2718(){
   Check(Application.version=="2.7.18"&&Path.GetFileName(G.SavePath)=="verification-stash-2.7.18.json","2718 version and isolated verification save");
   string dir=Path.GetDirectoryName(G.SavePath);string[] formal={"stash-v40.json","stash-v41.json","stash-v42.json"};byte[][] bytes=Array.ConvertAll(formal,n=>File.Exists(Path.Combine(dir,n))?File.ReadAllBytes(Path.Combine(dir,n)):null);
   var controls=G.Controls;var random=UnityEngine.Random.state;FreshCamp();string inventory=JsonUtility.ToJson(G.Save);G.HarborWalkthrough=true;Check(G.BeginTraining(),"2718 actual harbor preview starts");G.Paused=true;G.Controls=new TouchInput();yield return null;
   try{
    bool annexOnly=Array.IndexOf(Environment.GetCommandLineArgs(),"--verify-annex-containers-2718")>=0;
    if(!annexOnly){
     Check(FreightPreview2716.Apply(G),"2718 freight preview starts safely");FreightPhysical2716();yield return FreightRoutesCheck2716();yield return FreightSearchCheck2716();yield return FreightCargoLink2718();
     yield return CargoPicture2715("2718-freight-yard-overview.png",RaidMap.PortPoint(27.5f,155),0,23,true,1600,1600);
     yield return FreightLiveImage2718("2718-freight-outdoor-live",RaidMap.FreightPreviewSpawn2716,new Vector3(1,0,-.25f),45);
     yield return FreightRooms2718();
    }
    if(Array.IndexOf(Environment.GetCommandLineArgs(),"--verify-freight-2718")<0)yield return RunAnnexContainers2718();
    FreightPlace2716(RaidMap.FreightExitPoint2716,"2718 final extraction");G.ExtractTime=0;G.TickExtraction(5.05f);yield return null;
    Check(G.Phase==GamePhase.Base&&!G.TrainingActive&&JsonUtility.ToJson(G.Save)==inventory,"2718 preview exit restores exact inventory");
    for(int i=0;i<formal.Length;i++)Check(MainBytes273(bytes[i],File.Exists(Path.Combine(dir,formal[i]))?File.ReadAllBytes(Path.Combine(dir,formal[i])):null),"2718 formal save unchanged "+formal[i]);
   }finally{G.Controls=controls;UnityEngine.Random.state=random;G.Paused=true;File.WriteAllLines(Path.Combine(folder,"2718-freight-actual.csv"),freightRows2716);File.WriteAllLines(Path.Combine(folder,"2718-freight-rooms.csv"),freightRoomRows2717);}
  }
  IEnumerator FreightCargoLink2718(){
   var map=G.Map;var connection=map.HarborConnections2717.Find(r=>r.Name=="货运场至中央集装箱北侧");
   Check(connection!=null&&map.HarborRoadRecords2717.Count==12,"2718 actual freight to cargo link is registered");
   FreightPlace2716(connection.Points[0],"2718 freight-cargo route start");
   for(int i=1;i<connection.Points.Length;i++){
    var a=connection.Points[i-1];var b=connection.Points[i];Check(G.Navigation.ClearSegment(a,b),"2718 local connection has real unobstructed capsule sweep "+i);
    FreightRoomWalk2717(b,"2718 freight to central cargo segment "+i);int count=Mathf.CeilToInt(Vector3.Distance(a,b)*3);
    bool continuous=true;for(int j=0;j<=count;j++){var p=Vector3.Lerp(a,b,j/(float)Mathf.Max(1,count));continuous&=map.HarborSurfaceClear(p,.34f)&&ConnectionPixel2717(p).a>.98f;}
    Check(continuous,"2718 connection stays on continuous opaque legal land "+i);yield return null;
   }
   var road=map.HarborRoadRecords2717[2];bool aligned=true;for(int i=1;i<road.Points.Length;i++){var d=road.Points[i]-road.Points[i-1];aligned&=Mathf.Abs(d.x)<.01f||Mathf.Abs(d.z)<.01f;}Check(aligned,"2718 freight west bend uses aligned road segments");
   yield return CargoPicture2715("2718-freight-central-connection.png",RaidMap.PortPoint(55,137),0,42,true,1800,1200);
  }
  IEnumerator FreightLiveImage2718(string name,Vector3 p,Vector3 aim,float yaw){
   FreightRoomWalk2717(p,name+" player location");G.Rig.ResetView(yaw);G.Rig.Tick(0);G.Player.Aim=aim.normalized;G.Vision.Refresh(p,G.Player.Aim);G.Player.Draw(G.View,0);yield return null;
   float fog=Shader.GetGlobalFloat("_HarborFogEnabled");Check(fog>.5f,"2718 live image retains actual fog "+name);
   var image=HvRender277(G.View,name+".png",1600,900);try{int lit=0,white=0;float low=1,high=0;foreach(var pixel in image.GetPixels32()){Color c=pixel;if(c.grayscale>.018f){lit++;low=Mathf.Min(low,c.grayscale);high=Mathf.Max(high,c.grayscale);if(c.r>.98f&&c.g>.98f&&c.b>.98f)white++;}}Check(lit>1000&&high-low>.04f&&white/(float)Mathf.Max(1,lit)<.08f,"2718 live fog image has readable revealed geometry and no white clipping "+name+" lit="+lit);}finally{Destroy(image);}Check(Shader.GetGlobalFloat("_HarborFogEnabled")==fog,"2718 image preserves live fog "+name);
  }
  void FreightFloorCheck2718(){
   var map=G.Map;Check(map.StableGround137&&map.StableGround137.mipmapCount>1,"2718 floor uses actual filtered stable texture");
   foreach(var zone in map.FreightFloorZones2717){
    var colors=new HashSet<int>();float minimum=1,maximum=0;bool opaque=true;
    for(int z=0;z<23;z++)for(int x=0;x<23;x++){var p=new Vector3(Mathf.Lerp(zone.Area.xMin+.2f,zone.Area.xMax-.2f,(x+.38f)/23),0,Mathf.Lerp(zone.Area.yMin+.2f,zone.Area.yMax-.2f,(z+.47f)/23));Color c=CargoGroundPixel2715(p);opaque&=c.a>.98f;minimum=Mathf.Min(minimum,c.grayscale);maximum=Mathf.Max(maximum,c.grayscale);colors.Add((Mathf.RoundToInt(c.r*255)<<16)|(Mathf.RoundToInt(c.g*255)<<8)|Mathf.RoundToInt(c.b*255));}
    Check(opaque&&colors.Count>18&&maximum-minimum>.025f,"2718 actual floor material has grain and variation "+zone.Name+" colors="+colors.Count+" range="+(maximum-minimum));
    FreightRoomRow2717("2718 real floor texture",zone.Name,Vector3.zero,"material="+zone.Material+" colors="+colors.Count+" brightnessRange="+(maximum-minimum));
   }
   int walls=0;foreach(var renderer in map.Root.GetComponentsInChildren<Renderer>())foreach(var m in renderer.sharedMaterials)if(m&&m.shader&&m.shader.name=="GreyHarbor/FreightWall2718"){walls++;Check(!renderer.receiveShadows,"2718 wall skin avoids incoherent realtime shadows "+renderer.name);}
   Check(walls>8,"2718 real freight walls use interior-aware exploration shader count="+walls);
  }
  IEnumerator FreightRooms2718(){
   var outdoor=G.Map;var maps=new List<RaidMap>();var visions=new List<RaidVision>();var contents=new List<string[]>();var memories=new List<bool[]>();freightRoomRows2717.Clear();freightRoomRows2717.Add("kind,name,x,y,z,detail");
   foreach(string key in new[]{"freightduty","freightwarehouse"}){
    var stand=key=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;var portal=outdoor.Doors.Find(d=>d.Portal&&d.Destination==key);FreightRoomPlace2717(stand,key+" outside");FreightRoomHold2717(portal);yield return null;var indoor=G.Map;
    Check(indoor!=outdoor&&indoor.FreightInterior2716&&indoor.InteriorId==key&&G.Enemies.Count==0,"2718 enters correct actual furnished freight room "+key);FreightFloorCheck2718();FreightWallPixels2718(false);
    foreach(var room in indoor.FreightRooms2716)Check(!room.Door.Open&&!G.Vision.RoomAllowed(room.WalkPoint),"2718 unknown adjacent room remains concealed "+room.Name);
    yield return FreightLiveImage2718("2718-"+key+"-initial-closed",indoor.Spawn,new Vector3(0,0,1),45);
    yield return FreightRoomDoors2717();yield return FreightRoomSearch2717();FreightWallPixels2718(true);
    var p=key=="freightduty"?indoor.FreightRooms2716[0].WalkPoint:new Vector3(1.5f,0,3.2f);
    foreach(float yaw in new[]{45f,135f,225f,315f})yield return FreightLiveImage2718("2718-"+key+"-live-"+yaw,p,key=="freightduty"?new Vector3(-1,0,1):Vector3.right,yaw);
    yield return CargoPicture2715("2718-"+key+"-floor-materials.png",Vector3.zero,0,11,true,1800,1800);
    maps.Add(indoor);visions.Add(G.Vision);contents.Add(indoor.FreightLootRecords2717.ConvertAll(r=>JsonUtility.ToJson(new CargoItems2715{Items=r.Container.Items.ToArray()})).ToArray());memories.Add((bool[])G.Vision.Seen.Clone());FreightRoomWalk2717(indoor.Spawn,key+" exit");FreightRoomHold2717(indoor.FreightExit2716);yield return null;Check(G.Map==outdoor,"2718 exterior object restored "+key);
   }
   for(int m=0;m<maps.Count;m++){
    var indoor=maps[m];var stand=indoor.InteriorId=="freightduty"?RaidMap.FreightDutyArrival2716:RaidMap.FreightWarehouseArrival2716;FreightRoomPlace2717(stand,"2718 revisit");FreightRoomHold2717(outdoor.Doors.Find(d=>d.Destination==indoor.InteriorId));yield return null;
    Check(G.Map==indoor&&G.Vision==visions[m]&&G.Map.Loot.Count==6,"2718 two furnished rooms retain independent cached identity");bool preserved=true;for(int i=0;i<memories[m].Length;i++)if(memories[m][i]&&!G.Vision.Seen[i])preserved=false;Check(preserved,"2718 exploration memory survives room revisit");
    for(int i=0;i<indoor.FreightLootRecords2717.Count;i++){var box=indoor.FreightLootRecords2717[i].Container;Check(box.Opened&&contents[m][i]==JsonUtility.ToJson(new CargoItems2715{Items=box.Items.ToArray()}),"2718 searched contents survive "+box.Name);}
    FreightRoomWalk2717(indoor.Spawn,"2718 cached exit");FreightRoomHold2717(indoor.FreightExit2716);yield return null;Check(G.Map==outdoor,"2718 final room exit restores same exterior");
   }
  }
 }
}
