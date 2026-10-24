using UnityEngine;

namespace GreyHarbor
{
    public sealed class RaidCamera : MonoBehaviour
    {
        public const float GameplaySize=8.5f,ViewTilt310=50;
        public Transform Target;
        public float Yaw=45f;
        public bool Turning { get; private set; }
        float from,to,elapsed;
        int pending;
        public float Shake;
        readonly EdgePan309.State lookState310=new EdgePan309.State();
        public Vector2 LookOffset309=>lookState310.Offset;
        public void TickEdgePan309(Vector2 cursor,int width,int height,bool enabled,float dt){EdgePan309.Step(lookState310,cursor,width,height,enabled,dt);}
        public Vector3 LookWorldOffset309=>Right*LookOffset309.x+Forward*LookOffset309.y;
        public float DestinationYaw=>Turning?Mathf.Repeat(to,360):Yaw;
        public void ResetView(float yaw){Yaw=yaw;Turning=false;pending=0;Shake=0;lookState310.Reset();}
        public void Turn(int direction)
        {
            if(direction==0) return;
            if(Turning) { pending=direction; return; }
            from=Yaw; to=Yaw+Mathf.Sign(direction)*90; elapsed=0; Turning=true;
        }
        public void Tick(float dt)
        {
            if(Turning)
            {
                elapsed+=dt; float t=Mathf.Clamp01(elapsed/.3f);
                Yaw=Mathf.Lerp(from,to,t*t*(3-2*t));
                if(t>=1) { Yaw=Mathf.Repeat(to,360); Turning=false; int next=pending; pending=0; if(next!=0)Turn(next); }
            }
            transform.rotation=Quaternion.Euler(ViewTilt310,Yaw,0);
            Vector3 focus=Target?Target.position:Vector3.zero;
            transform.position=focus+LookWorldOffset309+Vector3.up*.4f-transform.forward*45;
            if(Shake>.001f)transform.position+=transform.right*Mathf.Sin(Time.unscaledTime*110)*Shake;
            Shake=Mathf.MoveTowards(Shake,0,dt*.9f);
        }
        public Vector3 Right { get { var v=transform.right; v.y=0; return v.normalized; } }
        public Vector3 Forward { get { var v=transform.forward; v.y=0; return v.normalized; } }
    }
}
