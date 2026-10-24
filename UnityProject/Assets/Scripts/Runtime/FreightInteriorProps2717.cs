using UnityEngine;
namespace GreyHarbor {
 public sealed partial class RaidMap {
  GameObject FreightModelRoot2717(string name,Vector3 p){var view=new GameObject("FreightAsset2717_"+name);view.transform.SetParent(Root,false);view.transform.position=p;return view;}
  void FreightHit2717(GameObject root,string name,Vector3 p,Vector3 size,ObstacleKind kind,Quaternion? rotation=null){var c=new GameObject("Physical_"+name);c.transform.SetParent(root.transform,false);c.transform.localPosition=p;if(rotation.HasValue)c.transform.localRotation=rotation.Value;c.AddComponent<BoxCollider>().size=size;SceneObstacle.Apply(c,kind);}
  void FreightTubeHit2717(GameObject root,string name,Vector3 a,Vector3 b,float radius,ObstacleKind kind){FreightHit2717(root,name,(a+b)/2,new Vector3(radius*2,Vector3.Distance(a,b),radius*2),kind,Quaternion.FromToRotation(Vector3.up,b-a));}
  void FreightBakeModel2717(GameObject root,WorkshopWastelandArt a,float yaw){a.Bake(root,root.AddComponent<WorkshopArtResources>());root.transform.rotation=Quaternion.Euler(0,yaw,0);}
  GameObject FreightDesk2717(Vector3 p,float yaw,bool computer,bool radio=false){
   var view=FreightModelRoot2717(computer?"ComputerDesk":"WritingDesk",p);var a=new WorkshopWastelandArt(this);
   if(computer)DecorationDesk274(a,true);else{
    a.Cube("TimberDesktop2717","9A8865",new Vector3(0,.80f,0),new Vector3(2.2f,.08f,.85f));
    foreach(float x in new[]{-.98f,.98f})foreach(float z in new[]{-.32f,.32f})a.Cube("DeskLeg2717","4D685D",new Vector3(x,.38f,z),new Vector3(.075f,.76f,.075f));
    a.Cube("TableRearBrace2717","607665",new Vector3(0,.22f,.31f),new Vector3(2,.065f,.055f));
    for(int i=0;i<3;i++){a.Cube("ShipmentForms2717",i==1?"AAA685":"C2B89A",new Vector3(-.25f+i*.21f,.85f+i*.006f,-.04f),new Vector3(.42f,.015f,.48f));for(int j=0;j<4;j++)a.Cube("PrintedFormLines2717","788175",new Vector3(-.30f+i*.21f,.861f+i*.006f,-.17f+j*.07f),new Vector3(.24f,.004f,.008f));}
    a.Cube("Clipboard2717","665F49",new Vector3(.66f,.866f,-.09f),new Vector3(.35f,.045f,.45f));a.Cube("ClipboardPaper2717","C8C1A4",new Vector3(.66f,.891f,-.09f),new Vector3(.30f,.006f,.38f));
    a.Tube("DeskPen2717","425B55",new Vector3(.33f,.856f,-.20f),new Vector3(.43f,.856f,-.02f),.007f);a.Tube("CeramicMug2717","B1B39B",new Vector3(-.86f,.846f,.18f),new Vector3(-.86f,1.01f,.18f),.06f);
   }
   foreach(float x in new[]{-.98f,.98f})foreach(float z in new[]{-.32f,.32f})FreightHit2717(view,"DeskLeg",new Vector3(x,.38f,z),new Vector3(.075f,.76f,.075f),ObstacleKind.Low);
   FreightHit2717(view,"Desktop",new Vector3(0,.80f,0),new Vector3(2.2f,.08f,.85f),ObstacleKind.Low);
   a.Cube("DeskDrawerCase2717","5F7B72",new Vector3(-.65f,.38f,.02f),new Vector3(.52f,.55f,.62f));FreightHit2717(view,"DrawerCase",new Vector3(-.65f,.38f,.02f),new Vector3(.52f,.55f,.62f),ObstacleKind.Low);
   for(int i=0;i<3;i++){float y=.21f+i*.16f;a.Cube("DeskDrawerFace2717",i%2==0?"708B7D":"6A8174",new Vector3(-.65f,y,-.299f),new Vector3(.47f,.135f,.025f));a.Tube("DrawerPull2717","BBB79B",new Vector3(-.76f,y,-.319f),new Vector3(-.54f,y,-.319f),.012f);}
   if(computer){FreightHit2717(view,"ComputerTower",new Vector3(.74f,.30f,.02f),new Vector3(.28f,.56f,.42f),ObstacleKind.Low);FreightHit2717(view,"Monitor",new Vector3(.25f,1.21f,.16f),new Vector3(.66f,.43f,.105f),ObstacleKind.High);}
   if(radio){
    a.Cube("DutyRadio2717","456A64",new Vector3(.56f,1.02f,.17f),new Vector3(.60f,.34f,.32f));a.Cube("RadioSpeaker2717","2C4440",new Vector3(.72f,1.01f,-.003f),new Vector3(.21f,.23f,.016f));a.Cube("RadioFrequencyScale2717","BCB993",new Vector3(.40f,1.07f,-.008f),new Vector3(.24f,.055f,.018f));
    for(int i=0;i<3;i++)a.Tube("RadioKnob2717","7F9785",new Vector3(.32f+i*.07f,.955f,-.025f),new Vector3(.32f+i*.07f,.955f,-.040f),.019f);
    a.Tube("RadioAerial2717","51645B",new Vector3(.80f,1.17f,.23f),new Vector3(.90f,1.66f,.23f),.011f);
   }
   FreightBakeModel2717(view,a,yaw);return view;
  }
  GameObject FreightChair2717(Vector3 p,float yaw,bool office){
   var view=FreightModelRoot2717(office?"CasterChair":"VisitorChair",p);var a=new WorkshopWastelandArt(this);string cloth=office?"526E69":"9B896B";
   a.Cube("ChairCushion2717",cloth,new Vector3(0,.48f,0),new Vector3(.56f,.12f,.54f));FreightHit2717(view,"ChairSeat",new Vector3(0,.48f,0),new Vector3(.56f,.12f,.54f),ObstacleKind.Low);
   a.Cube("UpholsteredChairBack2717",cloth,new Vector3(0,.83f,.24f),new Vector3(.54f,.55f,.11f));FreightHit2717(view,"ChairBack",new Vector3(0,.83f,.24f),new Vector3(.54f,.55f,.11f),ObstacleKind.Low);
   a.Cube("ChairWornPatch2717","A49D7B",new Vector3(-.13f,.548f,-.08f),new Vector3(.16f,.011f,.20f));
   if(office){
    a.Tube("ChairGasLift2717","4B625A",new Vector3(0,.12f,0),new Vector3(0,.43f,0),.038f);FreightTubeHit2717(view,"GasLift",new Vector3(0,.12f,0),new Vector3(0,.43f,0),.038f,ObstacleKind.Low);
    for(int i=0;i<5;i++){float angle=i*Mathf.PI*2/5;Vector3 end=new Vector3(Mathf.Cos(angle)*.29f,.09f,Mathf.Sin(angle)*.29f);a.Tube("ChairStarBase2717","7E8C7E",new Vector3(0,.15f,0),end,.022f);FreightTubeHit2717(view,"StarBase",new Vector3(0,.15f,0),end,.022f,ObstacleKind.Low);a.Tube("ChairCaster2717","2B3E35",end-new Vector3(.045f,.032f,0),end+new Vector3(.045f,-.032f,0),.045f);FreightHit2717(view,"Caster",end-new Vector3(0,.032f,0),new Vector3(.10f,.09f,.10f),ObstacleKind.Low);}
    foreach(float x in new[]{-.29f,.29f}){a.Cube("ChairArmrest2717","445B53",new Vector3(x,.69f,.01f),new Vector3(.065f,.06f,.40f));a.Tube("ChairArmPost2717","6E8172",new Vector3(x,.50f,.16f),new Vector3(x,.67f,.16f),.020f);}
   }else foreach(float x in new[]{-.22f,.22f})foreach(float z in new[]{-.21f,.21f}){a.Cube("ChairSteelLeg2717","526B5A",new Vector3(x,.215f,z),new Vector3(.045f,.43f,.045f));FreightHit2717(view,"ChairLeg",new Vector3(x,.215f,z),new Vector3(.045f,.43f,.045f),ObstacleKind.Low);}
   FreightBakeModel2717(view,a,yaw);return view;
  }
  GameObject FreightCabinet2717(Vector3 p,float yaw,bool high){
   var view=FreightModelRoot2717("PaintedCabinet",p);var a=new WorkshopWastelandArt(this);float h=high?1.85f:.78f;string blue="637F89";
   a.Cube("BlueCabinetCarcass2717",blue,new Vector3(0,h/2,0),new Vector3(1.12f,h,.60f));FreightHit2717(view,"Cabinet",new Vector3(0,h/2,0),new Vector3(1.12f,h,.60f),high?ObstacleKind.High:ObstacleKind.Low);
   foreach(float x in new[]{-.27f,.27f}){a.Cube("CabinetDoor2717","819594",new Vector3(x,h/2,-.312f),new Vector3(.51f,h-.10f,.025f));a.Tube("CabinetPull2717","BEBA98",new Vector3(x+.15f,h*.47f,-.341f),new Vector3(x+.15f,h*.65f,-.341f),.014f);a.Cube("CabinetLabel2717","C1B894",new Vector3(x,h*.72f,-.329f),new Vector3(.22f,.085f,.012f));}
   a.Cube("ChippedBluePaint2717","9B795B",new Vector3(-.36f,.15f,-.331f),new Vector3(.18f,.07f,.014f));a.Cube("CabinetTopLip2717","869689",new Vector3(0,h+.024f,0),new Vector3(1.15f,.045f,.62f));FreightBakeModel2717(view,a,yaw);return view;
  }
  GameObject FreightArchiveShelf2717(Vector3 p,float yaw){
   var view=FreightModelRoot2717("ArchiveShelf",p);var a=new WorkshopWastelandArt(this);
   foreach(float x in new[]{-.90f,.90f})foreach(float z in new[]{-.27f,.27f}){a.Cube("ArchiveShelfUpright2717","5F7664",new Vector3(x,1, z),new Vector3(.06f,2,.06f));FreightHit2717(view,"ArchivePost",new Vector3(x,1,z),new Vector3(.06f,2,.06f),ObstacleKind.High);}
   foreach(float y in new[]{.12f,.68f,1.25f,1.82f})a.Cube("ArchiveShelfDeck2717","8E957D",new Vector3(0,y,0),new Vector3(1.85f,.065f,.65f));
   string[] palette={"A3956E","668070","9D835D","7A9190","B5A887"};
   for(int row=0;row<3;row++)for(int col=0;col<6;col++){float x=-.74f+col*.29f,y=.35f+row*.57f;float height=.35f+(col%3)*.04f;a.Cube("PaperArchiveBinders2717",palette[(row*3+col)%palette.Length],new Vector3(x,y,-.03f),new Vector3(.21f,height,.50f));a.Cube("BinderPaperLabel2717","C6BDA0",new Vector3(x,y-.02f,-.291f),new Vector3(.13f,.085f,.016f));FreightHit2717(view,"BinderStock",new Vector3(x,y,-.03f),new Vector3(.21f,height,.50f),row==0?ObstacleKind.Low:ObstacleKind.High);}
   FreightBakeModel2717(view,a,yaw);return view;
  }
  GameObject FreightPaperBoxes2717(Vector3 p,float yaw){
   var view=FreightModelRoot2717("ArchiveCartons",p);var a=new WorkshopWastelandArt(this);
   Vector3[] positions={new Vector3(-.32f,.25f,0),new Vector3(.36f,.20f,.10f),new Vector3(-.28f,.64f,.06f)};Vector3[] sizes={new Vector3(.54f,.50f,.52f),new Vector3(.55f,.40f,.50f),new Vector3(.46f,.29f,.47f)};
   for(int i=0;i<3;i++){a.Cube("BrownArchiveCarton2717",i==1?"AF9D77":"978362",positions[i],sizes[i]);a.Cube("CartonPaperLabel2717","CBC19D",positions[i]+new Vector3(0,.07f,-sizes[i].z/2-.009f),new Vector3(.21f,.11f,.014f));a.Cube("CartonTape2717","B8AC86",positions[i]+Vector3.up*(sizes[i].y/2+.006f),new Vector3(.05f,.015f,sizes[i].z));FreightHit2717(view,"Carton",positions[i],sizes[i],ObstacleKind.Low);}
   FreightBakeModel2717(view,a,yaw);return view;
  }
  GameObject FreightPrivacy2717(Vector3 p){
   var view=FreightModelRoot2717("PrivacyScreen",p);var a=new WorkshopWastelandArt(this);a.Cube("WashroomPrivacyPanel2717","7C9283",new Vector3(0,.98f,0),new Vector3(.10f,1.96f,1.40f));FreightHit2717(view,"PrivacyPanel",new Vector3(0,.98f,0),new Vector3(.10f,1.96f,1.40f),ObstacleKind.High);foreach(float z in new[]{-.68f,.68f})a.Cube("ScreenEdge2717","A9B49B",new Vector3(0,.98f,z),new Vector3(.13f,1.96f,.06f));FreightBakeModel2717(view,a,0);return view;
  }
  GameObject FreightPackingBench2717(Vector3 p,float yaw){
   var view=FreightModelRoot2717("PackingBench",p);var a=new WorkshopWastelandArt(this);a.Cube("PackingWorkTop2717","9B8C6B",new Vector3(0,.89f,0),new Vector3(2.6f,.12f,1.1f));FreightHit2717(view,"PackingTop",new Vector3(0,.89f,0),new Vector3(2.6f,.12f,1.1f),ObstacleKind.Low);
   foreach(float x in new[]{-1.12f,1.12f})foreach(float z in new[]{-.43f,.43f}){a.Cube("PackingSteelLeg2717","586F61",new Vector3(x,.415f,z),new Vector3(.075f,.83f,.075f));FreightHit2717(view,"PackingLeg",new Vector3(x,.415f,z),new Vector3(.075f,.83f,.075f),ObstacleKind.Low);}
   a.Cube("ParcelScales2717","607F77",new Vector3(.62f,1.04f,.10f),new Vector3(.62f,.18f,.57f));a.Cube("ScalesPlate2717","A3AE97",new Vector3(.62f,1.143f,.10f),new Vector3(.58f,.035f,.53f));a.Cube("ScalesDigitalWindow2717","314D45",new Vector3(.62f,1.025f,-.198f),new Vector3(.23f,.07f,.018f));
   a.Cube("HalfSealedParcel2717","A58D63",new Vector3(-.58f,1.10f,.10f),new Vector3(.58f,.30f,.62f));a.Cube("PackageTape2717","BDAC7E",new Vector3(-.58f,1.257f,.10f),new Vector3(.075f,.015f,.62f));a.Cube("ParcelAddress2717","D0C2A0",new Vector3(-.73f,1.264f,-.02f),new Vector3(.19f,.012f,.28f));
   a.Cube("LabelPrinter2717","4D6764",new Vector3(-.05f,1.04f,.23f),new Vector3(.30f,.18f,.30f));a.Cube("PrinterLabelRoll2717","C4BCA1",new Vector3(-.05f,1.146f,.23f),new Vector3(.23f,.055f,.12f));a.Cube("HandheldScanner2717","435E57",new Vector3(.79f,.98f,-.32f),new Vector3(.08f,.09f,.22f));
   for(int i=0;i<3;i++)a.Cube("LoosePackingSheets2717","C7BDA2",new Vector3(-.89f+i*.06f,.96f+i*.008f,-.32f),new Vector3(.35f,.013f,.25f));FreightBakeModel2717(view,a,yaw);return view;
  }
  void FreightDressWalls2717(){
   var a=new WorkshopWastelandArt(this);
   foreach(var r in Root.GetComponentsInChildren<MeshRenderer>())if(r.name=="FreightWhiteboxWall2716"){
    var b=r.bounds;bool horizontal=b.size.x>b.size.z;Vector3 along=horizontal?Vector3.right:Vector3.forward,normal=horizontal?Vector3.forward:Vector3.right;float width=horizontal?b.size.x:b.size.z,depth=horizontal?b.size.z:b.size.x;
    foreach(float sign in new[]{-1f,1f})a.WallSkin(new Vector3(b.center.x,0,b.center.z)+normal*sign*(depth/2+.012f),along,normal*sign,width,InteriorId=="freightduty"?"70847A":"647F73");
   }
   var skin=FreightModelRoot2717("WallSkins",Vector3.zero);FreightBakeModel2717(skin,a,0);FreightTrack2717("WallSkins","全室",skin,ObstacleKind.Decoration,true);
   if(InteriorId=="freightduty"){
    FreightWallPanel2717("OfficeNotice","值班办公室",new Vector3(-3.4f,1.6f,-.135f),0,"Notice");
    FreightWallPanel2717("RestNotice","员工休息区",new Vector3(4.6f,1.6f,-.135f),0,"Notice");
    FreightWallPanel2717("ArchiveNotice","货运资料室",new Vector3(-2.5f,1.6f,8.275f),0,"Notice");
    FreightWallPanel2717("WashMirror","卫生间",new Vector3(8.275f,1.55f,4.4f),90,"Mirror");
    FreightWallPanel2717("CleaningRack","卫生间",new Vector3(3,1.45f,.13f),180,"Cleaning");
    FreightWallPanel2717("OfficeWindow","值班办公室",new Vector3(-8.275f,1.8f,-5.2f),270,"Window");
    FreightWallPanel2717("RestWindow","员工休息区",new Vector3(8.275f,1.8f,-5.0f),90,"Window");
    FreightWallPanel2717("CorridorServicePanel","公共走廊",new Vector3(-1.18f,1.5f,6.7f),270,"Notice");
   }else{
    FreightWallPanel2717("WarehouseSafetyPanel","仓储通间",new Vector3(8.775f,1.6f,1.7f),90,"Notice");
    FreightWallPanel2717("ReceivingDispatchBoard","货物收发室",new Vector3(-5.4f,1.6f,8.275f),0,"Notice");
    FreightWallPanel2717("WarehouseWindow","仓储通间",new Vector3(-8.775f,2.25f,-2.2f),270,"Window");
   }
   foreach(var room in FreightRooms2716){var plaque=FreightModelRoot2717("RoomPlaque",room.Door.Position+new Vector3(room.Area.center.x<room.Door.Position.x? .10f:-.10f,1.60f,1.03f));var art=new WorkshopWastelandArt(this);art.Cube("RoomIdentityPlaque2717","AFA07D",Vector3.zero,new Vector3(.035f,.20f,.44f));art.Cube("PlaqueInset2717","48685C",new Vector3(.025f,0,0),new Vector3(.012f,.075f,.31f));FreightBakeModel2717(plaque,art,0);FreightTrack2717("RoomPlaque",room.Name,plaque,ObstacleKind.Decoration,true);}
  }
  void FreightWallPanel2717(string name,string room,Vector3 p,float yaw,string type){
   var view=FreightModelRoot2717(name,p);var a=new WorkshopWastelandArt(this);
   if(type=="Mirror"||type=="Window"){
    float w=type=="Window"?2.6f:1.6f,h=type=="Window"? .95f:.74f;a.Cube("WindowOrMirrorFrame2717","45675F",Vector3.zero,new Vector3(w,h,.07f));a.Cube(type=="Window"?"BlueOpaqueGlazing2717":"WashroomMirror2717",type=="Window"?"6D9291":"A6B6A7",new Vector3(0,0,-.041f),new Vector3(w-.14f,h-.14f,.025f));
    if(type=="Window")foreach(float x in new[]{-.78f,0,.78f})a.Cube("WindowMullion2717","8B9A83",new Vector3(x,0,-.068f),new Vector3(.045f,h-.05f,.035f));
   }else if(type=="Cleaning"){
    a.Cube("CleaningRail2717","879C87",Vector3.zero,new Vector3(1.10f,.07f,.07f));foreach(float x in new[]{-.34f,.04f,.36f}){a.Tube("MopHandle2717","9B8C66",new Vector3(x,-.04f,-.07f),new Vector3(x,-1.10f,-.07f),.014f);a.Cube("MopHead2717",x<0?"AAB19A":"6B8C7A",new Vector3(x,-1.15f,-.07f),new Vector3(.21f,.15f,.10f));}
   }else{
    a.Cube("CorkNoticeBoard2717","8F7759",Vector3.zero,new Vector3(1.35f,.75f,.045f));foreach(float x in new[]{-.42f,0,.42f}){a.Cube("PinnedDispatch2717","C9C1A4",new Vector3(x,.015f,-.030f),new Vector3(.31f,.52f,.018f));for(int line=0;line<4;line++)a.Cube("NoticePrint2717","6E7E6B",new Vector3(x,-.12f+line*.07f,-.042f),new Vector3(.22f,.012f,.006f));a.Cube("PaperPin2717","A18150",new Vector3(x,.23f,-.048f),new Vector3(.025f,.025f,.014f));}
   }
   FreightBakeModel2717(view,a,yaw);FreightTrack2717(type,room,view,ObstacleKind.Decoration,true);
  }
 }
}

