using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GreyHarbor
{
    public enum GamePhase { Base,Raid,Extracted,Dead }
    public sealed partial class RaidGame : MonoBehaviour
    {
        public static RaidGame Instance;
        public const float CombatHeight=.95f;
        public IRaidInput Controls=new DesktopInput();
        public Camera View;
        public RaidCamera Rig;
        [NonSerialized] public RaidMap Map;
        [NonSerialized] public RaidVision Vision;
        [NonSerialized] public RaidNavigation Navigation;
        public RaidActor Player;
        public readonly List<RaidActor> Enemies=new List<RaidActor>();
        [NonSerialized] public RaidInventory Bag=new RaidInventory();
        public BaseSave Save=new BaseSave();
        public GamePhase Phase=GamePhase.Base;
        public bool Paused,InventoryOpen,VerifyMode,HardMode,ActiveHardMode;
        public int Magazine=0,Kills,ResultValue;
        public float ReloadTime,HealTime,ExtractTime,RaidTime,HitMarker,HurtFlash;
        [NonSerialized] public LootContainer OpenLoot,Nearby;
        public string Notice="";
        public float NoticeTime;
        public string SavePath;
        float nextShot;
        AudioSource audioPlayer;
        AudioClip shotClip,enemyShotClip,emptyClip,lootClip;
        readonly List<Projectile> shots=new List<Projectile>();
        sealed class Projectile {public ShotgunVolley303 Shotgun303;public Vector3 Origin303;public Vector3 Position,Velocity; public float Life; public float RemainingRange=float.PositiveInfinity; public RaidActor Owner;public LineRenderer Line;public bool Spit;public float Damage,Penetration;public int AmmoGrade;public AkShotSample276 HandlingSample;}
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Boot() {if(!FindAnyObjectByType<RaidGame>())new GameObject("GreyHarbor").AddComponent<RaidGame>();}
        void Awake()
        {
            Instance=this;ResetOptionalFeatures2761();Application.targetFrameRate=60;QualitySettings.vSyncCount=0;
            VerifyMode=Array.IndexOf(Environment.GetCommandLineArgs(),"--verify")>=0;
            SavePath=Path.Combine(Application.persistentDataPath,VerifyMode?"verification-stash-3.0.47.json":(Array.IndexOf(Environment.GetCommandLineArgs(),"--harbor-preview")>=0||Array.IndexOf(Environment.GetCommandLineArgs(),"--soldier-preview-2745")>=0||Array.IndexOf(Environment.GetCommandLineArgs(),"--asset-showcase")>=0||Array.IndexOf(Environment.GetCommandLineArgs(),"--ui-check")>=0||File.Exists(Path.Combine(Application.dataPath,"../QA-ui-check.flag")))?"ui-check-stash-3.0.47.json":"stash-v43.json");
            LoadGraphicsPreferences2721();PixelArt.Initialize();
            var cam=new GameObject("RaidCamera");View=cam.AddComponent<Camera>();View.tag="MainCamera";View.orthographic=true;View.orthographicSize=RaidCamera.GameplaySize;View.nearClipPlane=.1f;View.farClipPlane=120;View.backgroundColor=PixelArt.Hex("242F34");View.clearFlags=CameraClearFlags.SolidColor;View.allowHDR=false;
            Rig=cam.AddComponent<RaidCamera>();cam.AddComponent<AudioListener>();
            RenderSettings.ambientLight=new Color(.66f,.73f,.76f);RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;RenderSettings.fog=false;
            var sun=new GameObject("LateAfternoon").AddComponent<Light>();sun.type=LightType.Directional;sun.transform.rotation=Quaternion.Euler(48,-35,0);sun.color=new Color(1,.91f,.74f);sun.intensity=1.15f;sun.shadows=LightShadows.Soft;sun.shadowStrength=.45f;QualitySettings.shadowDistance=70;
            audioPlayer=gameObject.AddComponent<AudioSource>();shotClip=Sound(110,.12f,.42f);enemyShotClip=Sound(75,.16f,.28f);emptyClip=Sound(600,.035f,.1f);lootClip=Sound(960,.13f,.12f);
            LoadSave();BuildCamp();CampBootReady2721();gameObject.AddComponent<RaidUI>();
            if(VerifyMode)gameObject.AddComponent<RaidVerification>();
            // Local UI inspection fixture; never present in distributed builds.
            string fixture=Path.Combine(Application.dataPath,"../QA-ui-check.flag");
            if(!VerifyMode&&File.Exists(fixture)){string focus=File.ReadAllText(fixture).Trim();if(focus=="monsters"||focus=="exterior"||focus=="containers"||focus=="interior"||focus=="repair"||focus=="mainhouse")gameObject.AddComponent<TrainingExteriorSession>();else if(focus=="search")gameObject.AddComponent<InventoryUIFixture>();else if(focus=="market"){Player.transform.position=RaidMap.Market;Save.Coins=1000;}else if(focus=="warehouse")Player.transform.position=RaidMap.Warehouse;else if(focus=="workbench")Player.transform.position=RaidMap.Workbench;else if(focus=="door")Player.transform.position=RaidMap.Door;Physics.SyncTransforms();Rig.Tick(0);}
            if(!VerifyMode&&Array.IndexOf(Environment.GetCommandLineArgs(),"--harbor-preview")>=0)gameObject.AddComponent<HarborWhiteboxSession>();
            if(!VerifyMode&&Array.IndexOf(Environment.GetCommandLineArgs(),"--soldier-preview-2745")>=0)gameObject.AddComponent<SoldierPreview2745>();
            if(!VerifyMode&&Array.IndexOf(Environment.GetCommandLineArgs(),"--asset-showcase")>=0)gameObject.AddComponent<AssetShowcaseSession2717>();
            if(!VerifyMode&&!File.Exists(fixture)&&Array.IndexOf(Environment.GetCommandLineArgs(),"--training-preview")>=0)gameObject.AddComponent<TrainingExteriorSession>();
        }
        AudioClip Sound(float frequency,float duration,float volume)
        {
            int count=Mathf.RoundToInt(22050*duration);var data=new float[count];var rng=new System.Random(7);
            for(int i=0;i<count;i++) {float t=(float)i/22050;float envelope=Mathf.Pow(1-(float)i/count,3);data[i]=((float)rng.NextDouble()*2-1)*envelope*volume*.65f+Mathf.Sin(t*frequency*6.283f)*envelope*volume*.35f;}
            var clip=AudioClip.Create("ProceduralFeedback",count,1,22050,false);clip.SetData(data,0);return clip;
        }
        void BuildRaid() {foreach(var step in RaidBuildSteps2721())step.Build();}
        RaidActor Actor(string name,Vector3 p,bool enemy) {var g=new GameObject(name);g.transform.SetParent(Map.Root,false);g.transform.position=p;var a=g.AddComponent<RaidActor>();a.Initialize(enemy);return a;}
        public bool StartRaid() { return VerifyMode?((NextMap==RaidDestination.Training||NextMap==RaidDestination.TransitCenter)?BeginTraining():Depart()):RequestRaidLoad2721(); }
        public void ReturnBase() {if(Phase==GamePhase.Raid)return;BuildCamp();}
        public void Notify(string text) {Notice=text;NoticeTime=4;}
        void Update()
        {
            TickDisplay311();TickNpcClock319();TickMapInput319();
            if(IsLoading2721)return;
            CheckAkScope276();
            bool sprintToggle2711=Controls.SprintToggle;
            bool sprintInputAllowed2711=Phase==GamePhase.Raid&&!Paused&&!InventoryOpen;
            bool fireRequest279=TickPlayerTrigger279(Controls.Fire,Phase==GamePhase.Raid&&!Paused&&!InventoryOpen&&!MapFireBlocked319&&Player&&!Player.Dead);
            float dt=Time.deltaTime;NoticeTime=Mathf.Max(0,NoticeTime-Time.unscaledDeltaTime);
            if(Phase==GamePhase.Base) {TickSound326(0);if(sprintToggle2711&&!Paused&&!InventoryOpen)Notify("营地安全区只支持步行；进入行动后 Shift 切换奔跑。");TickCamp(dt);return;}
            if(Controls.Pause) {if(GetComponent<RaidUI>()?.CloseFullMap318()==true){}else if(InventoryOpen){CloseInventory();}else if(Phase==GamePhase.Raid){ResetScope30();Paused=!Paused;ResetWorldInteraction(true);}}
            if(Phase!=GamePhase.Raid||Paused){ResetOfficeFall2748();Sprint2711.Stop();return;}
            TickSound326(dt);
            if(GetComponent<RaidUI>()?.FullMapOpen318==true){var tier320=LoadTier320;Sprint2711.Tick(dt,false,Vector3.zero,0,tier320.Drain,tier320.Recovery);Player.Move(Vector3.zero);RaidTime+=dt;HitMarker=Mathf.Max(0,HitMarker-dt);HurtFlash=Mathf.Max(0,HurtFlash-dt);TickOfficeFall2748(dt);TickActions(dt);TickPlayerRecovery321(dt);TickEnemies(dt);Physics.SyncTransforms();TickProjectiles(dt);TickExtraction(dt);if(Phase==GamePhase.Raid&&Player.Dead)Finish(false);return;}
            bool inventoryChanged329=Controls.Inventory;
            if(inventoryChanged329)ToggleRaidInventory301();
            else TickQuickInput329();
            if(sprintToggle2711&&sprintInputAllowed2711&&!InventoryOpen)Sprint2711.Toggle();
            TickScope30(Time.unscaledDeltaTime);
            TickCameraLook309(Time.unscaledDeltaTime);
            if(InventoryOpen){var tier320=LoadTier320;Sprint2711.Tick(dt,false,Vector3.zero,0,tier320.Drain,tier320.Recovery);}
            int inventoryTurn2758=Controls.Turn;if(!InventoryOpen)Rig.Turn(inventoryTurn2758);Rig.Tick(dt);
            RaidTime+=dt;HitMarker=Mathf.Max(0,HitMarker-dt);HurtFlash=Mathf.Max(0,HurtFlash-dt);
            if(!InventoryOpen)UpdateAim();
            if(!InventoryOpen)
            {
                Vector2 movement=Vector2.ClampMagnitude(Controls.Move,1);
                Vector3 walkVelocity=PlayerWalkVelocity2751(movement);
                walkVelocity*=ModRules30.MoveMultiplier(Equipped)*(ScopeReady30?.7f:1);
                MovePlayer2711(walkVelocity,dt);
                TickAkHandling276(dt,dt>.00001f?LastActualMovement277.magnitude/(3.4f*dt):0);
                // Finalize the follow camera before sampling the mouse and firing.
                Rig.Tick(0);UpdateAim();
                if(Controls.Reload)BeginReload();
                if(fireRequest279)FirePlayer();
            }
            TickOfficeFall2748(dt);TickActions(dt);TickPlayerRecovery321(dt);TickEnemies(dt);Physics.SyncTransforms();TickProjectiles(dt);TickInteraction(dt);
            if(Map.Root&&Map.Root.gameObject.activeInHierarchy)Navigation?.WarmAround2721(Player.transform.position,.35);
            if(Player.Dead)Finish(false);
        }
        public void UpdateAim()
        {
            Vector3 screen=View.WorldToScreenPoint(Player.transform.position+Vector3.up*CombatHeight);
            Vector2 cursor=Controls.AimScreen;
            float dx=cursor.x-screen.x;
            if(Mathf.Abs(dx)>12)Player.Facing=dx>0?1:-1;
            // Facing keeps its visual hysteresis, while true aim stays continuous across screen vertical.
            var plane=new Plane(Vector3.up,Player.transform.position+Vector3.up*CombatHeight);
            var ray=View.ScreenPointToRay(cursor);
            if(plane.Raycast(ray,out float enter)) {var aim=ray.GetPoint(enter)-Player.transform.position;aim.y=0;if(aim.sqrMagnitude>.015f)ApplyAkAim276(aim);}
        }
        public void BeginReload()
        {
            if(Phase!=GamePhase.Raid||Paused||ActiveWeapon279==null||ReloadTime>0||RepairTime>0||Magazine>=MagazineCapacity279)return;
            if(TrainingLoadoutAvailable301){reloadAmmo270=WeaponRules279.Compatible(Equipped.Kind,MagazineAmmo)?MagazineAmmo:WeaponRules279.GreenAmmoFor(Equipped.Kind);reloadArea=ItemArea.Rig;reloadId=null;ReloadTime=ActiveWeapon279.ReloadSeconds;return;}
            var chosen=AmmoRules270.Select(Chest,MagazineAmmo,Magazine,Equipped.Kind);
            if(chosen==null){Notify("胸挂没有当前弹种；另一弹种可右键使用换弹，背包弹药需移入胸挂。");return;}
            if(!PrepareAmmo270(chosen.Kind))return;reloadAmmo270=chosen.Kind;reloadArea=ItemArea.Rig;reloadId=null;ReloadTime=ActiveWeapon279.ReloadSeconds;
        }
        public void BeginHeal(){CancelHeal();}
        public void TickActions(float dt)
        {
            TickArmorRepair(dt);
            if(ReloadTime>0){ReloadTime-=dt;if(ReloadTime<=0)CompleteReload270();}


        }
        public void FirePlayer()
        {
            if(Phase!=GamePhase.Raid||Paused||InventoryOpen||MapFireBlocked319||ActiveWeapon279==null||Time.time<nextShot||ReloadTime>0||RepairTime>0||Player.Dead)return;
            if(!WeaponRules279.Compatible(Equipped.Kind,MagazineAmmo)){Notify("当前弹匣与武器口径不匹配，请先卸弹。");return;}
            if(Magazine<=0) {nextShot=Time.time+.3f;audioPlayer.PlayOneShot(emptyClip);Notify("弹匣已空，按 R 换弹");return;}
            nextShot=Time.time-nextShot>ActiveWeapon279.Interval?Time.time+ActiveWeapon279.Interval:nextShot+ActiveWeapon279.Interval;if(Magazine<=MagazineFound)MagazineFound=Mathf.Max(0,MagazineFound-1);Magazine--;MarkPlayerCombat321();EmitSound326(Player,SoundKind326.Gunshot);if(Equipped.Kind==ItemKind.SG12)FireShotgun30(PlayerShotDirection276());else SpawnShot(Player,PlayerShotDirection276());FinishAkShot276();StoreActiveMagazine314();
            Rig.Shake=ScopeReady30?0:.045f;audioPlayer.PlayOneShot(shotClip);
            foreach(var e in Enemies)if(!e.Dead&&Vector3.Distance(e.transform.position,Player.transform.position)<ModRules30.NoiseRange(Equipped)){e.LastKnown=Player.transform.position;e.LastSeen=Time.time;e.Memory=7;e.Alert=Mathf.Max(e.Alert,.5f);}
        }
        public void SpawnShot(RaidActor actor,Vector3 direction)
        {
            if(actor.Enemy)EmitSound326(actor,SoundKind326.Gunshot);actor.Shot();actor.Draw(View,0);var line=new GameObject("BulletTrace").AddComponent<LineRenderer>();line.transform.SetParent(Map.Root,false);line.material=new Material(Shader.Find("Sprites/Default"));line.startColor=actor.Enemy?new Color(1,.48f,.24f):new Color(1,.87f,.54f);line.endColor=new Color(1,.9f,.65f,.15f);line.startWidth=.045f;line.endWidth=.015f;line.positionCount=2;
            var chest=actor.transform.position+Vector3.up*CombatHeight;var origin=actor.MuzzleOnPlane(View);if(Physics.Linecast(chest,origin,out var muzzleBlock,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore))origin=muzzleBlock.point-(origin-chest).normalized*.015f;line.SetPosition(0,origin);line.SetPosition(1,origin);
            bool playerShot=actor==Player;var weapon=Equipped?.Kind??ItemKind.AK;
            var profile=ActiveWeapon279??WeaponRules279.AK;
            var projectile=new Projectile{Owner=actor,Position=origin,Velocity=direction.normalized*(actor.Enemy?28:profile.ProjectileSpeed),Life=1.5f,RemainingRange=playerShot?WeaponRange278.Maximum(weapon):(actor.IsRebel&&SoldierProfiles2745.IsNew(actor.Rebel)?SoldierProfiles2745.Range(actor.Rebel):float.PositiveInfinity),Line=line,Damage=playerShot?profile.Damage:NpcCombat275.IndependentDamage(actor),Penetration=playerShot?profile.Penetration:NpcCombat275.IndependentPenetration(actor),AmmoGrade=playerShot?RaidInventory.Grade(MagazineAmmo):NpcCombat275.AttackGrade345(actor),HandlingSample=playerShot?RecordAkProjectile276(origin,direction.normalized):null};shots.Add(projectile);
            if(VerifyMode&&actor.Enemy){LastEnemyShotDirection276=projectile.Velocity.normalized;LastEnemyShotDamage276=projectile.Damage;LastEnemyShotPen276=projectile.Penetration;}
        }
        public void TickProjectiles(float dt)
        {
            for(int i=shots.Count-1;i>=0;i--)
            {
                var b=shots[i];Vector3 step=Vector3.ClampMagnitude(b.Velocity*dt,Mathf.Max(0,b.RemainingRange));bool impact=false;
                var hits=Physics.RaycastAll(b.Position,step.normalized,step.magnitude,SceneObstacle.ShotMask,QueryTriggerInteraction.Ignore);Array.Sort(hits,(a,c)=>a.distance.CompareTo(c.distance));
                foreach(var hit in hits)
                {
                    var actor=hit.collider.GetComponent<RaidActor>();if(actor==b.Owner||(actor!=null&&actor.Enemy&&(b.Spit||b.Owner.IsRebel)))continue;
                    if(actor!=null&&!actor.Dead)
                    {
                        if(b.Shotgun303!=null)RecordShotgunHit303(b.Shotgun303,actor);
                        else ApplyCombatHit(actor,b.Damage,b.Penetration,0,b.AmmoGrade);
                        if(actor==Player)HurtFlash=.3f;
                        else if(!b.Owner.Enemy&&b.Shotgun303==null){HitMarker=.13f;actor.Alert=1;actor.LastKnown=Player.transform.position;actor.LastSeen=Time.time;if(actor.Dead&&!IsTrainingDummy315(actor)){Kills++;if(actor.IsRebel)RebelDeath145(actor);else MonsterDeath(actor);}}
                    }
                    if(b.HandlingSample!=null){b.HandlingSample.Impact=hit.point;b.HandlingSample.Hit=hit.collider.name;b.HandlingSample.Ended=true;}
                    b.Position=hit.point;impact=true;break;
                }
                if(!impact)b.Position+=step;
                b.RemainingRange=Mathf.Max(0,b.RemainingRange-step.magnitude);
                if(b.Shotgun303!=null){float length=Mathf.Min(1.1f,Vector3.Distance(b.Origin303,b.Position));b.Line.SetPosition(0,b.Position-b.Velocity.normalized*length);b.Line.SetPosition(1,b.Position);}
                else {b.Line.SetPosition(0,b.Position);b.Line.SetPosition(1,b.Position-b.Velocity.normalized*(b.Spit?.28f:.8f));}b.Life-=dt;
                bool rangeEnd=b.RemainingRange<=.00001f;
                if(!impact&&rangeEnd&&b.HandlingSample!=null){b.HandlingSample.Impact=b.Position;b.HandlingSample.Hit="RangeEnd";b.HandlingSample.Ended=true;}
                if(impact||rangeEnd||b.Life<=0) {if(b.Shotgun303!=null)CompleteShotgunPellet303(b.Shotgun303);Destroy(b.Line.material);Destroy(b.Line.gameObject);shots.RemoveAt(i);}
            }
        }
        public bool ClearSight(Vector3 a,Vector3 b)=>!Physics.Linecast(a+Vector3.up*CombatHeight,b+Vector3.up*CombatHeight,SceneObstacle.SightMask,QueryTriggerInteraction.Ignore);
        public void TickEnemies(float dt)
        {
            foreach(var e in Enemies)
            {
                if(e.Dead||!e.gameObject.activeInHierarchy)continue;if(e.IsRebel){TickRebel145(e,dt);continue;}if(e.IsMonster){TickMonster(e,dt);continue;}Vector3 delta=Player.transform.position-e.transform.position;float dist=delta.magnitude;
                bool visible=dist<10.5f&&ClearSight(e.transform.position,Player.transform.position);
                if(visible){e.Alert=Mathf.Min(1,e.Alert+dt/.85f);e.LastKnown=Player.transform.position;e.LastSeen=Time.time;}
                else e.Alert=Mathf.Max(0,e.Alert-dt*.12f);
                if(e.Alert>.1f)
                {
                    Vector3 toward=e.LastKnown-e.transform.position;toward.y=0;if(toward.sqrMagnitude>.01f)e.Aim=toward.normalized;
                    if(e.Alert>=1&&visible&&Time.time>=e.NextShot)
                    {e.NextShot=Time.time+.95f;SpawnShot(e,Quaternion.Euler(0,UnityEngine.Random.Range(-5,5),0)*e.Aim);audioPlayer.PlayOneShot(enemyShotClip,Mathf.Clamp01(1-dist/SoundRules326.HearingRange327));}
                    if((!visible||dist>6.5f)&&toward.magnitude>1&&Time.time-e.LastSeen<7)FollowPath(e,e.LastKnown,1.55f,dt);
                }
                else
                {
                    Patrol(e,dt);
                }
            }
        }
        public void Patrol(RaidActor e,float dt)
        {
            if(Map.IsHarbor){HarborPatrol(e,dt);return;}
            if(e.WaitTime>0){e.WaitTime-=dt;return;}
            Vector3[] points;
            if(e.name.Contains("Office"))points=new[]{new Vector3(-9,0,4.5f),new Vector3(-5,0,8),new Vector3(-10,0,8)};
            else if(e.name.Contains("Storage"))points=new[]{new Vector3(0,0,1),new Vector3(4.5f,0,7),new Vector3(-1,0,8)};
            else if(e.name.Contains("Yard"))points=new[]{new Vector3(8,0,1),new Vector3(14,0,-2),new Vector3(8,0,-10)};
            else points=new[]{new Vector3(13,0,7),new Vector3(15,0,12),new Vector3(8,0,10)};
            Vector3 goal=points[e.PatrolIndex%points.Length];
            if(Vector3.Distance(e.transform.position,goal)<.25f){e.PatrolIndex++;e.WaitTime=.8f;e.Route.Clear();return;}
            if(!FollowPath(e,goal,.95f,dt)){e.PatrolIndex++;e.WaitTime=.25f;}
        }
        public bool FollowPath(RaidActor e,Vector3 goal,float speed,float dt)
        {
            e.RepathTime-=dt;
            if(e.Route.Count==0&&e.RepathTime>0&&(goal-e.RouteGoal).sqrMagnitude<=1)return (goal-e.transform.position).sqrMagnitude<.0144f;
            if(e.Route.Count==0||e.RepathTime<=0||(goal-e.RouteGoal).sqrMagnitude>1)
            {
                e.Route.Clear();e.Route.AddRange(Navigation.FindPath(e.transform.position,goal));e.RouteIndex=0;e.RouteGoal=goal;e.RepathTime=1.5f;
                if(e.Route.Count==0)return false;
            }
            while(e.RouteIndex<e.Route.Count&&(e.Route[e.RouteIndex]-e.transform.position).magnitude<.12f)e.RouteIndex++;
            if(e.RouteIndex>=e.Route.Count){e.Route.Clear();e.RepathTime=0;return true;}
            Vector3 d=e.Route[e.RouteIndex]-e.transform.position;Vector3 before=e.transform.position;e.Aim=d.normalized;
            e.Move(d.normalized*Mathf.Min(d.magnitude,speed*dt));
            if((e.transform.position-before).magnitude<speed*dt*.15f)e.StuckTime+=dt;else e.StuckTime=0;
            if(e.StuckTime>.6f){e.Route.Clear();e.RepathTime=0;e.StuckTime=0;return false;}
            return true;
        }
        bool lootInteractHeld;LootContainer openingBox;
        internal void ResetIdentification(){if(OpenLoot!=null){OpenLoot.IdentifyProgress=0;OpenLoot.IdentifyingId=null;}}
        public static float IdentificationSeconds(ItemKind kind)=>(RaidInventory.Grade(kind)+1)*.5f;
        internal void TickIdentification(float dt){
            if(!InventoryOpen||OpenLoot==null||!OpenLoot.Opened||Paused)return;
            ItemStack next=null;
            if(OpenLoot.Corpse325?.Soldier==true){foreach(var item in OpenLoot.Corpse325.AllItems()){if(item!=null&&!item.Identified){next=item;break;}}}
            else next=EnsureLootGrid(OpenLoot).Slots.Find(p=>!p.Identified);
            if(next==null){ResetIdentification();return;}
            if(OpenLoot.IdentifyingId!=next.Id){OpenLoot.IdentifyingId=next.Id;OpenLoot.IdentifyProgress=0;}
            OpenLoot.IdentifyProgress+=Mathf.Max(0,dt);
            if(OpenLoot.IdentifyProgress+.00001f>=IdentificationSeconds(next.Kind)){next.Identified=true;OpenLoot.IdentifyingId=null;OpenLoot.IdentifyProgress=0;}
        }
        public void TickExtraction(float dt)
        {
            if(Phase!=GamePhase.Raid||Player.Dead||Map.IsInterior||Map.TransitWhitebox346){ExtractTime=0;return;}
            Vector3 exit=NearestExit();Vector3 delta=Player.transform.position-exit;
            if(Mathf.Abs(delta.x)<1.8f&&Mathf.Abs(delta.z)<1.8f){ExtractTime+=dt;if(ExtractTime>=5)Finish(true);}
            else ExtractTime=0;
        }
        public Vector3 NearestExit(){Vector3 best=Map.Extraction;foreach(var exit in Map.Exits)if((exit-Player.transform.position).sqrMagnitude<(best-Player.transform.position).sqrMagnitude)best=exit;return best;}
        public void TakeLoot(int index){MoveItem(ItemArea.Loot,index,ItemArea.Backpack,-1);}
        public void Drop(int index){if(index>=0&&index<Bag.Slots.Count)DropItem(ItemArea.Backpack,Bag.Slots[index].Cell);}
        void LateUpdate()
        {
            if(IsLoading2721||Player==null)return;float dt=Paused?0:Time.deltaTime;Rig.Tick(0);
            bool fog=Phase!=GamePhase.Base&&!Map.AssetShowcase2717;Shader.SetGlobalFloat("_HarborFogEnabled",fog?1:0);
            View.backgroundColor=fog&&!Map.ExteriorPreview&&!Map.HarborWhitebox?Color.black:PixelArt.Hex("35433F");
            SyncScopeLight335();
            if(fog)Vision.RefreshRuntime2717(Player.transform.position,Player.Aim,Map.LayoutRevision);
            DrawArmorDummy(dt);DrawTrainingDummies315(dt);Player.Draw(View,dt);foreach(var e in Enemies){e.VisibleToPlayer=!fog||Vision.CanSee(e.transform.position);e.Draw(View,dt);}
            foreach(var l in Map.Loot){if(Vision.CanSee(l.Position))l.Known=true;if(l.Prop==null)l.View.SetActive(!fog||l.Known);}
            foreach(var label in Map.Labels){bool current=Vision.CanSee(label.transform.position);label.GetComponent<Renderer>().enabled=!fog||Vision.Explored(label.transform.position);label.color=!fog||current?new Color(.75f,.85f,.78f):new Color(.25f,.28f,.26f);}
            foreach(var b in shots)b.Line.enabled=!fog||Vision.CanSee(b.Position);
            RenderScope30();

        }
    }
}









