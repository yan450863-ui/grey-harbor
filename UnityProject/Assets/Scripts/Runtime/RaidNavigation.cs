using System.Collections.Generic;
using UnityEngine;

namespace GreyHarbor
{
    public sealed class RaidNavigation
    {
        const float Radius=.34f;
        readonly float Cell,MinX,MinZ;readonly int W,H;
        // Cell states: 0 unknown, 1 blocked, 2 walkable. Unknown cells are always sampled before use.
        readonly byte[] walk,edges,knownEdges;readonly System.Func<Vector3,bool> surface;
        readonly int[] parent;
        readonly Queue<int> frontier=new Queue<int>();
        public int PathRequests2717 {get;private set;}
        public int CheckedCells2721 {get;private set;}
        public int TotalCells2721=>walk.Length;
        int warmX=int.MinValue,warmZ,warmRadius,warmOffset;
        static readonly int[] DX={1,-1,0,0},DZ={0,0,1,-1};
        public RaidNavigation():this(new Rect(-18,-16,36,32),.5f){}
        public RaidNavigation(Rect bounds,float cell,System.Func<Vector3,bool> surface=null,bool eager2721=false)
        {
            this.surface=surface;Cell=cell;MinX=bounds.xMin;MinZ=bounds.yMin;W=Mathf.CeilToInt(bounds.width/Cell);H=Mathf.CeilToInt(bounds.height/Cell);walk=new byte[W*H];edges=new byte[W*H];knownEdges=new byte[W*H];parent=new int[W*H];
            Physics.SyncTransforms();
            // The original full sampling order remains available only for verification comparisons.
            if(eager2721){
                for(int i=0;i<walk.Length;i++){Vector3 p=Position(i);walk[i]=(byte)((surface==null||surface(p))&&!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,Radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)?2:1);}
                CheckedCells2721=walk.Length;
                for(int i=0;i<walk.Length;i++)if(walk[i]==2)for(int k=0;k<4;k+=2){int x=i%W+DX[k],z=i/W+DZ[k];if(x>=W||z>=H)continue;int j=z*W+x;if(walk[j]==2&&ClearSegment(Position(i),Position(j))){edges[i]|=(byte)(1<<k);edges[j]|=(byte)(1<<(k+1));}}
                for(int i=0;i<knownEdges.Length;i++)knownEdges[i]=15;
            }
        }
        bool Walkable2721(int i){
            if(walk[i]==0){Vector3 p=Position(i);walk[i]=(byte)((surface==null||surface(p))&&!Physics.CheckCapsule(p+Vector3.up*.35f,p+Vector3.up,Radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)?2:1);CheckedCells2721++;}
            return walk[i]==2;
        }
        bool Edge2721(int i,int j,int k){
            byte bit=(byte)(1<<k);if((knownEdges[i]&bit)==0){
                int reverse=k^1;knownEdges[i]|=bit;knownEdges[j]|=(byte)(1<<reverse);
                // Match the old eager sweep direction: positive x/z, regardless of which endpoint was visited first.
                int a=(k&1)==0?i:j,b=(k&1)==0?j:i;
                if(Walkable2721(a)&&Walkable2721(b)&&ClearSegment(Position(a),Position(b))){edges[i]|=bit;edges[j]|=(byte)(1<<reverse);}
            }
            return (edges[i]&bit)!=0;
        }
        public void WarmAround2721(Vector3 point,double maxMilliseconds=.35){
            var g=RaidGame.Instance;
            // Cached maps are disabled during interior switches; sampling them would omit their colliders.
            if(g&&(g.Navigation!=this||g.Map==null||!g.Map.Root||!g.Map.Root.gameObject.activeInHierarchy))return;
            if(maxMilliseconds<=0||W==0||H==0)return;
            int cx=Mathf.Clamp(Mathf.FloorToInt((point.x-MinX)/Cell),0,W-1),cz=Mathf.Clamp(Mathf.FloorToInt((point.z-MinZ)/Cell),0,H-1);
            if(warmX==int.MinValue||Mathf.Abs(cx-warmX)>3||Mathf.Abs(cz-warmZ)>3){warmX=cx;warmZ=cz;warmRadius=warmOffset=0;}
            long start=System.Diagnostics.Stopwatch.GetTimestamp();double ticks=maxMilliseconds*System.Diagnostics.Stopwatch.Frequency/1000.0;
            while(warmRadius<=Mathf.Max(W,H)){
                int r=warmRadius,n=warmOffset++,x=warmX,z=warmZ;
                if(r>0){int side=2*r;if(n<side){x+=-r+n;z-=r;}else if(n<side*2){x+=r;z+=-r+n-side;}else if(n<side*3){x+=r-(n-side*2);z+=r;}else{x-=r;z+=r-(n-side*3);}}
                if(warmOffset>=(r==0?1:8*r)){warmOffset=0;warmRadius++;}
                if(x>=0&&z>=0&&x<W&&z<H){int i=z*W+x;if(Walkable2721(i))for(int k=0;k<4;k++){int nx=x+DX[k],nz=z+DZ[k];if(nx>=0&&nz>=0&&nx<W&&nz<H)Edge2721(i,nz*W+nx,k);}}
                // The budget is soft: one cell and its exact adjacent sweeps finish atomically.
                if(System.Diagnostics.Stopwatch.GetTimestamp()-start>=ticks)return;
            }
        }
        Vector3 Position(int i)=>new Vector3(MinX+(i%W+.5f)*Cell,0,MinZ+(i/W+.5f)*Cell);
        public bool ClearSegment(Vector3 a,Vector3 b)
        {
            Vector3 d=b-a;float length=d.magnitude;if(surface!=null){int n=Mathf.Max(1,Mathf.CeilToInt(length/(Cell*.5f)));for(int i=0;i<=n;i++)if(!surface(Vector3.Lerp(a,b,i/(float)n)))return false;}if(length<.001f)return true;
            return !Physics.CapsuleCast(a+Vector3.up*.35f,a+Vector3.up,Radius,d/length,length,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore)
                &&!Physics.CheckCapsule(b+Vector3.up*.35f,b+Vector3.up,Radius,SceneObstacle.MovementMask,QueryTriggerInteraction.Ignore);
        }
        int Closest(Vector3 p)
        {
            int best=-1;float distance=4;int cx=Mathf.FloorToInt((p.x-MinX)/Cell),cz=Mathf.FloorToInt((p.z-MinZ)/Cell),r=Mathf.CeilToInt(2/Cell);
            for(int z=Mathf.Max(0,cz-r);z<=Mathf.Min(H-1,cz+r);z++)for(int x=Mathf.Max(0,cx-r);x<=Mathf.Min(W-1,cx+r);x++){int i=z*W+x;if(Walkable2721(i)){float d=(Position(i)-p).sqrMagnitude;if(d<distance&&ClearSegment(p,Position(i))){distance=d;best=i;}}}
            return best;
        }
        public List<Vector3> FindPath(Vector3 start,Vector3 goal)
        {
            PathRequests2717++;
            var path=new List<Vector3>();int a=Closest(start),b=Closest(goal);if(a<0||b<0)return path;
            if(ClearSegment(start,goal)){path.Add(goal);return path;}
            for(int i=0;i<parent.Length;i++)parent[i]=-2;parent[a]=-1;frontier.Clear();frontier.Enqueue(a);
            while(frontier.Count>0&&parent[b]==-2)
            {
                int i=frontier.Dequeue(),x=i%W,z=i/W;
                for(int k=0;k<4;k++){int nx=x+DX[k],nz=z+DZ[k];if(nx<0||nz<0||nx>=W||nz>=H)continue;int j=nz*W+nx;
                    if(parent[j]==-2&&Edge2721(i,j,k)){parent[j]=i;frontier.Enqueue(j);}}
            }
            if(parent[b]==-2)return path;
            for(int i=b;i>=0;i=parent[i])path.Add(Position(i));path.Reverse();
            // Keep only waypoints with a collision-free swept route, including clearance at corners.
            var smooth=new List<Vector3>();Vector3 anchor=start;int cursor=0;
            while(cursor<path.Count){int last=cursor;while(last+1<path.Count&&ClearSegment(anchor,path[last+1]))last++;smooth.Add(path[last]);anchor=path[last];cursor=last+1;}
            if(ClearSegment(anchor,goal))smooth.Add(goal);return smooth;
        }
    }
}
