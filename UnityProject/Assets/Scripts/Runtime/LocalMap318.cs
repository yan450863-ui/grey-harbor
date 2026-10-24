using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidGame {
  internal RaidMap OverviewMap318=>Map!=null&&(Map.HarborWhitebox||Map.UndergroundWhitebox2752)?Map:outsideArea!=null&&outsideArea.Map.HarborWhitebox?outsideArea.Map:Map;
  internal Vector3 OverviewPosition318=>OverviewMap318!=Map?outsideReturn:Player.transform.position;
 }
 public sealed partial class RaidUI {
  internal const float NearbyMapMeters318=18f;
  Texture2D playerCircle318,playerArrow318;RaidMap mapContext318;
  internal bool FullMapOpen318=>whiteboxOverview;
  internal static Rect NearbyBounds318(Vector3 player)=>new Rect(player.x-NearbyMapMeters318/2,player.z-NearbyMapMeters318/2,NearbyMapMeters318,NearbyMapMeters318);
  internal static Vector2 MarkerDirection318(MiniMapProjection projection,Vector3 player,Vector3 aim){var d=projection.Point(player+aim)-projection.Point(player);return d.sqrMagnitude>.000001f?d.normalized:Vector2.down;}
  internal bool ToggleFullMap318(){if(G.Phase!=GamePhase.Raid||G.InventoryOpen||G.Paused||G.IsLoading2721 )return false;mapContext318=G.Map;whiteboxOverview=!whiteboxOverview;ClearMapQuest319();G.CancelQuestInteraction319();return true;}
  internal bool CloseFullMap318(){if(!whiteboxOverview)return false;whiteboxOverview=false;ClearMapQuest319();return true;}
  void Update(){if(G==null)return;if(G.Phase!=GamePhase.Raid||G.InventoryOpen||G.Paused||G.IsLoading2721){whiteboxOverview=false;ClearMapQuest319();mapContext318=G.Map;return;}if(mapContext318!=G.Map){whiteboxOverview=false;ClearMapQuest319();mapContext318=G.Map;}if(Input.GetKeyDown(KeyCode.M))ToggleFullMap318();}
  internal void EnsurePlayerMarker318(){
   if(playerCircle318)return;playerCircle318=new Texture2D(128,128,TextureFormat.RGBA32,true){name="PlayerCircle327",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp};playerArrow318=new Texture2D(96,96,TextureFormat.RGBA32,true){name="PlayerArrow327",filterMode=FilterMode.Trilinear,wrapMode=TextureWrapMode.Clamp};var cyan=new Color(.55f,.85f,.91f);var rim=new Color(.94f,.99f,1);
   for(int y=0;y<128;y++)for(int x=0;x<128;x++){float r=new Vector2((x+.5f)*.5f-32,(y+.5f)*.5f-32).magnitude;Color c=Color.Lerp(cyan,rim,Mathf.Clamp01((r-12.25f)/.5f));float body=Mathf.Clamp01((16.25f-r)/.5f),glow=.23f*Mathf.Clamp01((25-r)/4);c.a=Mathf.Max(body,glow);if(body<1){float a=c.a;c=Color.Lerp(new Color(.63f,.85f,.88f),c,body);c.a=a;}playerCircle318.SetPixel(x,y,c);}
   for(int y=0;y<96;y++)for(int x=0;x<96;x++){float yy=(y+.5f)/3,xx=(x+.5f)/3,width=(32-yy)*.48f;float alpha=Mathf.Clamp01((width-Mathf.Abs(xx-16))/.333f+.5f);playerArrow318.SetPixel(x,y,new Color(cyan.r,cyan.g,cyan.b,alpha));}playerCircle318.Apply(true,false);playerArrow318.Apply(true,false);
  }
  void PlayerMarker318(Vector2 center,Vector2 direction,float scale318=1){EnsurePlayerMarker318();GUI.DrawTexture(new Rect(center.x-17*scale318,center.y-17*scale318,34*scale318,34*scale318),playerCircle318);var prior=GUI.matrix;float angle=Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg+90;GUI.matrix=prior*Matrix4x4.TRS(new Vector3(center.x,center.y,0),Quaternion.Euler(0,0,angle),Vector3.one);GUI.DrawTexture(new Rect(-6*scale318,-23*scale318,12*scale318,9*scale318),playerArrow318);GUI.matrix=prior;}
  void OnDestroy(){ReleaseMapPicture332();ReleaseMapGpu330();ReleaseMapTerrain325();if(playerCircle318)Destroy(playerCircle318);if(playerArrow318)Destroy(playerArrow318);}
  void DrawMapTerrain318(RaidMap map,MiniMapProjection projection,bool fog318){
   // Draw an ordinary GUI rectangle: transforming a 1x1 rectangle inside BeginGroup
   // made Unity IMGUI clip the entire neighborhood after the M-map migration.
   if(IsNearbyTerrain327(projection.Viewport)){
    // GPU work is prepared in LateUpdate, outside IMGUI clipping/render state.
    if(PreparedTerrain331)GUI.DrawTexture(projection.Viewport,PreparedTerrain331,ScaleMode.StretchToFill,false);
   }else GUI.DrawTexture(projection.Viewport,TerrainImage325(map,projection),ScaleMode.StretchToFill,false);
  }
  Vector2 OverviewPoint318(Vector3 world,Rect frame,RaidMap source)=>new Vector2(frame.x+(world.x-source.Bounds.xMin)/source.Bounds.width*frame.width,frame.y+(source.Bounds.yMax-world.z)/source.Bounds.height*frame.height);
  void OverviewName318(Vector3 world,string title,Rect frame,RaidMap source,Color color){var p=OverviewPoint318(world,frame,source);float width=Mathf.Clamp(title.Length*13+12,72,130);float x=Mathf.Clamp(p.x-width/2,frame.x+2,frame.xMax-width-2),y=Mathf.Clamp(p.y-(source.TransitWhitebox346?10:50),frame.y+2,frame.yMax-24);Fill(new Rect(x,y,width,22),panel);Label(x,y,width,22,title,12,color,TextAnchor.MiddleCenter);}
  void DrawOverview318(){var source=G.OverviewMap318;if(source==null)return;Fill(new Rect(40,65,1520,775),new Color(.03f,.055f,.065f,.98f));Label(65,83,1020,34,source.HarborWhitebox?"港口全图 / 行动任务":source.UndergroundWhitebox2752?"地下管网 / 行动任务":G.Map.InteriorTitle+" / 地图全貌",27,white);if(PanelButton(new Rect(1390,82,140,36),"关闭 / M")){CloseFullMap318();return;}Label(65,122,1420,25,$"{source.Bounds.width:0} × {source.Bounds.height:0}米 · 北朝上"+(source!=G.Map?" · 当前位于室内，标记为建筑入口":""),15,muted);
   float s=Mathf.Min(890/source.Bounds.width,606/source.Bounds.height);var frame=new Rect(510-source.Bounds.width*s/2,165,source.Bounds.width*s,source.Bounds.height*s);Fill(frame,new Color(.12f,.21f,.25f));if(source.HarborWhitebox){GUI.DrawTexture(frame,source.HarborPlanTexture());TaskCircle319(source,frame);for(int i=0;i<Mathf.Min(source.RegionAnchors.Count,RaidMap.WhiteboxNames.Length);i++)OverviewName318(source.RegionAnchors[i],RaidMap.WhiteboxNames[i],frame,source,white);var freight=RaidMap.FreightExteriorBounds2716.center;OverviewName318(new Vector3(freight.x,0,freight.y),"西北货运场",frame,source,white);OverviewName318(RaidMap.PortPoint(190,103),"废弃客轮",frame,source,white);}else if(source.UndergroundWhitebox2752){GUI.DrawTexture(frame,source.UndergroundPlanTexture2752());foreach(var entry in source.UndergroundEntries2752){var p=OverviewPoint318(entry.ExitPosition,frame,source);Fill(new Rect(p.x-5,p.y-5,10,10),mint);OverviewName318(entry.ExitPosition,entry.Name,frame,source,mint);}}else{CacheMiniMap();var projection=new MiniMapProjection(source.Bounds,new Rect(frame.x-6,frame.y-6,frame.width+12,frame.height+12),Quaternion.Euler(90,0,0));DrawMapTerrain318(source,projection,false);for(int i=0;i<Mathf.Min(source.ExteriorAreaPoints.Count,RaidMap.ExteriorAreaNames.Length);i++)OverviewName318(source.ExteriorAreaPoints[i],RaidMap.ExteriorAreaNames[i],frame,source,white);}
   if(source.TransitWhitebox346)for(int i=0;i<source.TransitNames346.Count;i++)OverviewName318(source.TransitLabels346[i],source.TransitNames346[i],frame,source,white);
   for(int i=0;i<source.Exits.Count;i++){var p=OverviewPoint318(source.Exits[i],frame,source);Fill(new Rect(p.x-5,p.y-5,10,10),mint);OverviewName318(source.Exits[i],source.HarborWhitebox?(i==0?"① 西北货运门":"② 东南码头"):"撤离点 "+(i+1),frame,source,mint);}var player=OverviewPoint318(G.OverviewPosition318,frame,source);var aim=source==G.Map?new Vector2(G.Player.Aim.x,-G.Player.Aim.z).normalized:Vector2.down;PlayerMarker318(player,aim);if(source.TransitWhitebox346){Label(1010,172,515,32,"一楼白模 · 第二版",23,mint);WrappedLabel(new Rect(1010,236,460,220),"检查房间、门洞和障碍之间的绕行空间。楼梯A预留通向二楼，楼梯B预留通向地下层。本版均不切换楼层。无敌人、无掉落、无撤离点。Esc可返回营地。",20,white);}else TaskMapList319(new Rect(1010,164,515,620));Label(65,800,1455,23,source.TransitWhitebox346?"蓝白：当前位置与朝向 · M 关闭全图 · 本版暂不配置撤离点":"蓝白：你   绿：撤离点   金圈：任务搜索范围   关闭地图清除定位 · 地图打开时仍会受攻击",15,white,TextAnchor.MiddleCenter);
  }
 }
}
