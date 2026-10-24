using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor
{
    public sealed class LootContainer
    {
        public CorpseLoot325 Corpse325;
        public ContainerDropProfile230 DropProfile230;
        public Vector3 Position;
        public string Name;
        public bool Opened;public bool PlayerDrop314;
        public bool Known;public string IdentifyingId;public float IdentifyProgress;
        public float Search; public float SearchDuration=1.1f; public SupplyType Type; public ContainerTier Tier=ContainerTier.Common; public HarborDistrict District;public int GridRows=5;
        public readonly List<ItemStack> Items=new List<ItemStack>();
        public GameObject View;public ContainerProp Prop;
    }
    public sealed partial class RaidMap
    {
        public Transform Root;
        public readonly List<LootContainer> Loot=new List<LootContainer>();
        public readonly List<Renderer> Occluders=new List<Renderer>();
        public readonly List<TextMesh> Labels=new List<TextMesh>();
        readonly Dictionary<string,Material> materials=new Dictionary<string,Material>();
        public Vector3 Spawn=new Vector3(-11,0,-11),Extraction=new Vector3(13,0,11);
        public Material Mat(string hex,bool surface=false)
        {
            string key=hex+(surface?"-surface":"-ground");if(materials.TryGetValue(key,out var found))return found;
            var m=new Material(GreyHarbor.FlashlightShaders2763.Load("HarborWorld"));m.color=PixelArt.Hex(hex);m.SetFloat("_SolidFog",surface?1:0);materials.Add(key,m);var owner=Root.GetComponent<WorkshopArtResources>()??Root.gameObject.AddComponent<WorkshopArtResources>();owner.Materials.Add(m);return m;
        }
        public GameObject Box(string name,Vector3 pos,Vector3 scale,string hex,bool solid=false,bool fade=false)
        {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(Root,false);g.transform.position=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=Mat(hex,pos.y+scale.y/2>.2f);
            if(solid)SceneObstacle.Apply(g,ObstacleKind.High);else {if(Application.isPlaying)Object.Destroy(g.GetComponent<Collider>());else Object.DestroyImmediate(g.GetComponent<Collider>());}
            if(fade)
            {
                var r=g.GetComponent<Renderer>();var m=r.material;Root.GetComponent<WorkshopArtResources>().Materials.Add(m);Occluders.Add(r);
            }
            return g;
        }
        public void Build()
        {
            Exits.Add(Extraction);Root=new GameObject("TrainingGround").transform;
            Box("Ground",new Vector3(0,-.2f,0),new Vector3(38,.4f,34),"424D50");
            Box("WarehouseFloor",new Vector3(-3,.008f,3),new Vector3(18,.016f,14),"777B70");
            Box("EastServiceRoad",new Vector3(13,.008f,0),new Vector3(7,.016f,31),"343F43");
            for(int x=-18;x<=18;x+=2)Box("PavingJoint",new Vector3(x,.02f,0),new Vector3(.025f,.015f,32),"3A4649");
            for(int z=-16;z<=16;z+=2)Box("PavingJoint",new Vector3(0,.021f,z),new Vector3(36,.015f,.025f),"3A4649");
            for(int z=-14;z<15;z+=4)Box("RoadMark",new Vector3(13,.04f,z),new Vector3(.1f,.015f,1.7f),"A69F7C");
            Wall(-18,0,.5f,32);Wall(18,0,.5f,32);Wall(0,-16,36,.5f);Wall(0,16,36,.5f);
            // Warehouse perimeter with a broad south entrance and east service door.
            Wall(-12,3,.4f,14);Wall(-3,10,18,.4f);
            Wall(-8.5f,-4,7,.4f);Wall(2,-4,8,.4f);
            Wall(6,-2,.4f,4);Wall(6,7,.4f,6);
            // Internal office and storage bays, each with an unobstructed doorway.
            Wall(-3,7,.3f,6);Wall(-3,-1.5f,.3f,5);
            Wall(-9,3,6,.3f);Wall(-3.8f,3,1.4f,.3f);
            Box("EntryThreshold",new Vector3(-3,.04f,-4),new Vector3(3,.05f,.6f),"C2A465");
            for(int x=-4;x<-1;x++)Box("HazardStripe",new Vector3(x,.071f,-4),new Vector3(.3f,.015f,.6f),"444B45");
            Crate(-8,6);Crate(-6.8f,6);Crate(1,6);Crate(1,7.3f);Crate(3,1);
            Crate(-7,-8);Crate(-5.5f,-8);Crate(9,6);Crate(10.2f,6);
            Container(-14,-9,2.6f,5,"4E7775");Container(2,-10,6,2.5f,"836549");Container(10,-6,2.5f,5,"566E78");
            Box("WorkBench",new Vector3(-10,.5f,0),new Vector3(1.1f,1,3),"555D58",true);
            Box("WorkbenchTop",new Vector3(-10,1.02f,0),new Vector3(1.25f,.1f,3.1f),"A99C7A");
            Box("Generator",new Vector3(1,.7f,12.6f),new Vector3(3,1.4f,1.8f),"67735F",true,true);
            for(int i=0;i<6;i++)Box("Vent",new Vector3(-.1f+i*.4f,.95f,11.66f),new Vector3(.12f,.5f,.02f),"283A3B");
            Zone(Spawn,new Vector2(3,2.8f),"759D97");Zone(Extraction,new Vector2(3.6f,3.6f),"71B99D");
            
            
            
            
            AddLoot(new Vector3(-10,0,-7),"补给箱",ItemKind.Ammo,30,ItemKind.GreenArmorKit,1);
            AddLoot(new Vector3(-9,0,7),"仓储箱",ItemKind.Scrap,4,ItemKind.Circuit,2);
            AddLoot(new Vector3(-8.7f,0,.4f),"工具箱",ItemKind.Scrap,3,ItemKind.Ammo,20);
            AddLoot(new Vector3(3,0,8),"军用箱",ItemKind.Ammo,40,ItemKind.GreenArmorKit,1);
            AddLoot(new Vector3(3,0,-1),"物资箱",ItemKind.Circuit,2,ItemKind.Scrap,3);
            AddLoot(new Vector3(10,0,12.5f),"运输箱",ItemKind.Circuit,3,ItemKind.GreenArmorKit,1);
            PopulateResources();
            for(int i=0;i<25;i++) {float x=Random.Range(-16f,16f),z=Random.Range(-14f,14f);if(Physics.CheckSphere(new Vector3(x,.5f,z),.5f,1<<8))continue;Box("Debris",new Vector3(x,.025f,z),new Vector3(Random.Range(.15f,.5f),.04f,Random.Range(.15f,.4f)),i%2==0?"7B7D6C":"55544C");}
        }
        void PopulateResources()
        {
            Loot[0].Items.Add(new ItemStack(ItemKind.Cola,2,ItemOrigin.Certified));
            Loot[1].Items.Add(new ItemStack(ItemKind.GunParts,3,ItemOrigin.Certified));
            Loot[1].Items.Add(new ItemStack(ItemKind.Tools,1,ItemOrigin.Certified));
            Loot[2].Items.Add(new ItemStack(ItemKind.Cloth,3,ItemOrigin.Certified));
            Loot[2].Items.Add(new ItemStack(ItemKind.Medical,2,ItemOrigin.Certified));
            Loot[3].Items.Add(new ItemStack(ItemKind.Powder,3,ItemOrigin.Certified));
            Loot[3].Items.Add(new ItemStack(ItemKind.GunParts,2,ItemOrigin.Certified));
            Loot[4].Items.Add(new ItemStack(ItemKind.Powder,2,ItemOrigin.Certified));
            Loot[5].Items.Add(new ItemStack(ItemKind.Tools,1,ItemOrigin.Certified));
            Loot[5].Items.Add(new ItemStack(ItemKind.Intel,1,ItemOrigin.Certified));
            AddLoot(new Vector3(8,0,-10),"路边补给",ItemKind.Scrap,3,ItemKind.Cola,2);
        }
        void Wall(float x,float z,float width,float depth)
        {
            // A wall is one continuous solid. Old cutout segments prematurely
            // ended the first-hit fog volume, creating dark stripes at each seam.
            Box("ConcreteWall",new Vector3(x,1.125f,z),new Vector3(width,2.25f,depth),"A3A799",true,true);
        }
        void Crate(float x,float z)
        {
            Box("CoverCrate",new Vector3(x,.55f,z),new Vector3(1.05f,1.1f,1.05f),"927D55",true,true);
            Box("CrateBand",new Vector3(x,.565f,z),new Vector3(1.075f,.16f,1.075f),"464F48");
        }
        void Container(float x,float z,float w,float d,string c)
        {
            Box("CargoContainer",new Vector3(x,1.1f,z),new Vector3(w,2.2f,d),c,true,true);
            for(int i=0;i<5;i++)Box("ContainerRib",new Vector3(x-w*.4f+i*w*.2f,2.22f,z),new Vector3(.06f,.07f,d),"9DA699");
        }
        void Zone(Vector3 p,Vector2 size,string color)
        {
            Box("ZoneEdge",p+new Vector3(-size.x/2,.06f,0),new Vector3(.07f,.02f,size.y),color);
            Box("ZoneEdge",p+new Vector3(size.x/2,.06f,0),new Vector3(.07f,.02f,size.y),color);
            Box("ZoneEdge",p+new Vector3(0,.06f,-size.y/2),new Vector3(size.x,.02f,.07f),color);
            Box("ZoneEdge",p+new Vector3(0,.06f,size.y/2),new Vector3(size.x,.02f,.07f),color);
        }
public LootContainer AddLoot(Vector3 p,string name,ItemKind a,int ac,ItemKind b,int bc,bool random=true)
        {
            var l=new LootContainer {Position=p,Name=name};l.Items.Add(new ItemStack(a,ac+(random?Random.Range(0,3):0),ItemOrigin.Certified));l.Items.Add(new ItemStack(b,bc,ItemOrigin.Certified));
            l.View=Box(name,p+Vector3.up*.33f,new Vector3(.75f,.66f,.55f),"637965");
            var band=Box("LootLatch",p+new Vector3(0,.67f,0),new Vector3(.12f,.025f,.57f),"D5B967");band.transform.SetParent(l.View.transform,true);
            Loot.Add(l);return l;
        }
    }
}
