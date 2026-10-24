using UnityEngine;

namespace GreyHarbor
{
    public sealed class RaidActor : MonoBehaviour
    {
        public LootContainer CorpseContainer325;
        internal float FootstepDistance326;
        public bool Enemy,Dead,Backward;
        public MonsterKind Monster;public bool IsMonster,LungeHit;public float AttackCooldown,Windup,Lunge,Memory;public Vector3 AttackDirection;public Vector3[] PatrolTargets;
        public bool MonsterMoving;public float AttackPose;float monsterClock;
        public void ConfigureMonster(MonsterKind kind){IsMonster=true;Monster=kind;NpcRules270.SetMonsterLevel(this,NpcLevel);Gun.enabled=Muzzle.enabled=false;Body.sprite=MonsterArt.Get(kind,0);groundShadow.transform.localScale=Vector3.one*(kind==MonsterKind.BallastHauler?1.4f:kind==MonsterKind.Pipewhisker?1.25f:kind==MonsterKind.PaperbackCreeper?1.1f:kind==MonsterKind.Shell?1.45f:kind==MonsterKind.Runner?1.2f:1);}
        public int NpcLevel=1;public ItemKind NpcWeapon=ItemKind.AK,NpcAmmo=ItemKind.GreenAmmo;public bool IsRebel;public RebelKind Rebel;public ItemStack RebelArmor;public int RebelBurst;public float RebelShotDelay;
        public ItemStack TestArmor;public float Health=100;
        public float NpcArmorPoints,NpcMaxArmor,NpcMaxHealth;
        public Vector3 Aim=Vector3.forward,Home;
        public float Alert,LastSeen,NextShot,PatrolClock;
        public Vector3 LastKnown;
        public int Facing=-1;
        public readonly System.Collections.Generic.List<Vector3> Route=new System.Collections.Generic.List<Vector3>();
        public int RouteIndex,PatrolIndex;
        public float RepathTime,StuckTime,WaitTime;
        public Vector3 RouteGoal;
        public bool VisibleToPlayer=true;
        SpriteRenderer groundShadow;
        public SpriteRenderer Body,Gun,Muzzle,Warning;
        public Transform Figure;
        public Collider HitCollider;
        // Visual hold point is independent from the shared combat/aim plane.
        public const float WeaponHoldHeight306=.45f;
        float walkTime,flashTime,hitTime,recoil;
        public ItemKind VisualWeapon279 {get{var kind=Enemy?NpcWeapon:RaidGame.Instance&&RaidGame.Instance.Equipped!=null?RaidGame.Instance.Equipped.Kind:ItemKind.AK;return WeaponRules279.IsWeapon(kind)?kind:ItemKind.AK;}}
        public void RefreshWeaponArt279(){if(!Gun||!Muzzle)return;var kind=VisualWeapon279;Gun.sprite=PixelArt.WeaponSprite279(kind);Gun.name=WeaponRules279.Get(kind).Name;UpdateMuzzleSocket307();}
        public Vector3 VisualMuzzleLocal307 {get {var socket=PixelArt.WeaponMuzzle279(VisualWeapon279);if(Gun&&Gun.flipY)socket.y=-socket.y;return socket;}}
        void UpdateMuzzleSocket307(){if(Muzzle)Muzzle.transform.localPosition=VisualMuzzleLocal307+new Vector3(0,0,-.01f);}
        public void Initialize(bool enemy)
        {
            Enemy=enemy; Health=enemy?72:PlayerLife321.Maximum;if(enemy)NpcMaxHealth=Health;Home=transform.position;gameObject.layer=9;
            var hit=gameObject.AddComponent<CapsuleCollider>();hit.center=new Vector3(0,.7f,0);hit.radius=.3f;hit.height=1.4f;HitCollider=hit;
            Figure=new GameObject("SideOnlyCharacter").transform;Figure.SetParent(transform,false);
            Body=Figure.gameObject.AddComponent<SpriteRenderer>();Body.sprite=(enemy?PixelArt.Guard:PixelArt.Hero)[0];
            Body.sharedMaterial=new Material(RenderShaders307.Load("HarborActor307"));
            Gun=SpriteChild(WeaponRules279.Get(VisualWeapon279).Name,Figure,PixelArt.WeaponSprite279(VisualWeapon279));
            Gun.sharedMaterial=new Material(RenderShaders307.Load("HarborWeapon307"));
            Gun.sharedMaterial.SetFloat("_DepthBias",.012f);
            Muzzle=SpriteChild("MuzzleFlash",Gun.transform,PixelArt.Flash);Muzzle.transform.localPosition=PixelArt.WeaponMuzzle279(VisualWeapon279)+new Vector3(0,0,-.01f);Muzzle.enabled=false;
            Muzzle.sharedMaterial=new Material(RenderShaders307.Load("HarborWeapon307"));Muzzle.sharedMaterial.SetFloat("_DepthBias",.04f);Muzzle.sharedMaterial.renderQueue=2462;
            // Only the player is the laser overlay target; enemy sprites retain ordinary occlusion.
            Body.sharedMaterial.SetFloat("_CharacterMask",enemy?0:1);Gun.sharedMaterial.SetFloat("_CharacterMask",enemy?0:2);Muzzle.sharedMaterial.SetFloat("_CharacterMask",enemy?0:2);
            var resources=gameObject.AddComponent<WorkshopArtResources>();resources.Materials.Add(Body.sharedMaterial);resources.Materials.Add(Gun.sharedMaterial);resources.Materials.Add(Muzzle.sharedMaterial);
            Warning=SpriteChild("Detection",Figure,PixelArt.Marker);Warning.transform.localPosition=new Vector3(0,1.7f,0);Warning.enabled=false;
            var shadow=SpriteChild("GroundShadow",transform,PixelArt.Shadow);shadow.transform.localPosition=new Vector3(0,.018f,0);shadow.transform.rotation=Quaternion.Euler(90,0,0);groundShadow=shadow;
        }
        static SpriteRenderer SpriteChild(string name,Transform parent,Sprite sprite) {var g=new GameObject(name);g.transform.SetParent(parent,false);var s=g.AddComponent<SpriteRenderer>();s.sprite=sprite;return s;}
        public static bool IsBackward(Vector3 movement,Vector3 aim)
        {
            movement.y=aim.y=0;
            if(movement.sqrMagnitude<.000001f||aim.sqrMagnitude<.000001f)return false;
            // Rear sector is 120 degrees wide: 60 degrees either side of straight back.
            return Vector3.Dot(movement.normalized,aim.normalized)<=-.5f+.000001f;
        }
        public void Move(Vector3 displacement)
        {
            if(Dead)return;
            Vector3 p=transform.position;
            Vector3 x=new Vector3(displacement.x,0,0),z=new Vector3(0,0,displacement.z);
            p=Sweep(p,x);p=Sweep(p,z);
            float traveled=Vector3.Distance(transform.position,p);transform.position=p;RaidGame.Instance?.ActorMoved326(this,traveled);if(IsMonster)MonsterMoving=traveled>.00001f;
            if(!Enemy){if(traveled>.00001f)walkTime+=traveled*(1f/(3.4f*.18f));else walkTime=0;}
            else if(traveled>.00001f)walkTime+=Time.deltaTime*(Backward?-9:9);
            else walkTime=0;
        }
        bool Blocked(Vector3 p)=>Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up*1.0f,.3f,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
        Vector3 Sweep(Vector3 p,Vector3 step)
        {
            float length=step.magnitude;if(length<.00001f)return p;
            if(Physics.CapsuleCast(p+Vector3.up*.35f,p+Vector3.up, .3f,step/length,out var hit,length,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore))step=step/length*Mathf.Max(0,hit.distance-.01f);
            var map=RaidGame.Instance?RaidGame.Instance.Map:null;if(map!=null&&map.HarborWhitebox&&!map.HarborSurfaceClear(p+step,.3f))return p;if(map!=null&&map.UndergroundWhitebox2752&&!map.UndergroundFloorContains2752(p+step,.3f))return p;return Blocked(p+step)?p:p+step;
        }
        public Vector3 MuzzleOnPlane(Camera camera){RefreshWeaponArt279();Vector3 screen=camera.WorldToScreenPoint(Gun.transform.TransformPoint(VisualMuzzleLocal307));var ray=camera.ScreenPointToRay(screen);var plane=new Plane(Vector3.up,transform.position+Vector3.up*RaidGame.CombatHeight);return plane.Raycast(ray,out float distance)?ray.GetPoint(distance):transform.position+Vector3.up*RaidGame.CombatHeight;}
        public void Shot() {flashTime=.05f;recoil=.12f;}
        public void Damage(float amount)
        {
            if(Dead||OptionalFeatures2761.BlocksDamage(this))return;if(amount>0&&RaidGame.Instance&&RaidGame.Instance.Player==this)RaidGame.Instance.MarkPlayerCombat321();Health=Mathf.Max(0,Health-amount);hitTime=.12f;
            if(Health<=0) {Dead=true;HitCollider.enabled=false;Gun.enabled=false;Warning.enabled=false;}
        }
        public void Draw(Camera camera,float dt)
        {
            RefreshWeaponArt279();
            Figure.gameObject.SetActive(VisibleToPlayer);groundShadow.enabled=VisibleToPlayer;
            Body.sharedMaterial.SetVector("_Anchor",transform.position+Vector3.up*.08f);
            Gun.sharedMaterial.SetVector("_Anchor",transform.position+Vector3.up*1.15f);
            Muzzle.sharedMaterial.SetVector("_Anchor",transform.position+Vector3.up*1.15f);
            Figure.rotation=camera.transform.rotation;
            if(Dead) {Figure.rotation*=Quaternion.Euler(0,0,85);Figure.localScale=Vector3.one*.75f;Body.color=new Color(.35f,.36f,.34f,.8f);Muzzle.enabled=false;return;}
            Vector3 dir=camera.WorldToScreenPoint(transform.position+Vector3.up*RaidGame.CombatHeight+Aim)-camera.WorldToScreenPoint(transform.position+Vector3.up*RaidGame.CombatHeight);
            if(Enemy && Mathf.Abs(dir.x)>2)Facing=dir.x>0?1:-1;
            Body.flipX=Facing>0;
            int frame=Mathf.Abs(Mathf.FloorToInt(walkTime))%4;
            Body.sprite=IsRebel?RebelArt145.Get(Rebel,frame):IsMonster?MonsterArt.Get(Monster,frame):(Enemy?PixelArt.Guard:PixelArt.Hero)[frame];
            Figure.localScale=Vector3.one;
            int order=-Mathf.RoundToInt(camera.WorldToScreenPoint(transform.position).z*100);
            Body.sortingOrder=order;Gun.sortingOrder=order+2;Muzzle.sortingOrder=order+3;Warning.sortingOrder=order+4;
            float angle=Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
            Gun.transform.localRotation=Quaternion.Euler(0,0,angle);Gun.flipY=Facing<0;UpdateMuzzleSocket307();
            recoil=Mathf.MoveTowards(recoil,0,dt*.9f);
            Gun.transform.localPosition=new Vector3(0,WeaponHoldHeight306-(!Enemy&&SoldierArt.Loaded&&frame%2==1?.03f:0),-.03f)-new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad),Mathf.Sin(angle*Mathf.Deg2Rad),0)*recoil;
            if(IsMonster){monsterClock+=dt;MonsterArt.Animate(this,monsterClock);}
            Gun.enabled=!IsMonster&&(Enemy||RaidGame.Instance.Equipped!=null);flashTime-=dt;hitTime-=dt;Muzzle.enabled=Gun.enabled&&flashTime>0;Body.color=hitTime>0?new Color(1,.45f,.3f):NpcLevel==2&&((IsMonster&&!MonsterTuning.IsNew(Monster))||Rebel==RebelKind.BlueHeavy)?new Color(.78f,.88f,1):Color.white;
            Warning.transform.localScale=Vector3.one*(Windup>0?1.5f:1);Warning.enabled=Enemy && Alert>.05f;Warning.color=Alert>=1?new Color(1,.3f,.23f):new Color(1,.76f,.3f,Mathf.Clamp01(Alert+.3f));
        }
    }
}
