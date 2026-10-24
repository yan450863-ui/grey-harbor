using UnityEngine;

namespace GreyHarbor
{
    // Gameplay depends on this contract; a touch adapter can replace DesktopInput.
    public interface IRaidInput
    {
        Vector2 Move { get; }
        Vector2 AimScreen { get; }
        bool Fire { get; }
        bool Reload { get; }
        bool Interact { get; }
        bool Heal { get; }
        bool Inventory { get; }
        bool Pause { get; }
        bool SprintToggle { get; }
        int Turn { get; }
    }

    public interface IWeaponAim30 { bool Aiming30 {get;} }
    public sealed class DesktopInput : IRaidInput,IWeaponAim30
    {
        public bool Aiming30=>Input.GetMouseButton(1);
        public Vector2 Move => new Vector2((Input.GetKey(KeyCode.D)?1:0)-(Input.GetKey(KeyCode.A)?1:0), (Input.GetKey(KeyCode.W)?1:0)-(Input.GetKey(KeyCode.S)?1:0));
        public Vector2 AimScreen => Input.mousePosition;
        public bool Fire => Input.GetMouseButton(0);
        public bool Reload => Input.GetKeyDown(KeyCode.R);
        public bool Interact => Input.GetKey(KeyCode.F);
        public bool Heal => Input.GetKeyDown(KeyCode.H);
        public bool Inventory => Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.B);
        public bool Pause => Input.GetKeyDown(KeyCode.Escape);
        public bool SprintToggle => Input.GetKeyDown(KeyCode.LeftShift)||Input.GetKeyDown(KeyCode.RightShift);
        public static int TurnDirection(bool q,bool e)=>q?1:e?-1:0;
        public int Turn => TurnDirection(Input.GetKeyDown(KeyCode.Q),Input.GetKeyDown(KeyCode.E));
    }

    // UI integrations write these fields; one-shot actions are consumed once.
    public sealed class TouchInput : IRaidInput,IWeaponAim30
    {
        public bool Aiming30 {get;set;}
        public Vector2 Movement, AimPosition;
        public bool Shooting, Searching, ReloadPressed, HealPressed, InventoryPressed, PausePressed, SprintPressed;
        public int TurnPressed;
        public Vector2 Move => Movement;
        public Vector2 AimScreen => AimPosition;
        public bool Fire => Shooting;
        public bool Interact => Searching;
        public bool Reload => Take(ref ReloadPressed);
        public bool Heal => Take(ref HealPressed);
        public bool Inventory => Take(ref InventoryPressed);
        public bool Pause => Take(ref PausePressed);
        public bool SprintToggle => Take(ref SprintPressed);
        public int Turn { get { int t=TurnPressed; TurnPressed=0; return t; } }
        static bool Take(ref bool field) { bool result=field; field=false; return result; }
    }
}
