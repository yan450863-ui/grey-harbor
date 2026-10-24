using UnityEngine;
namespace GreyHarbor {
 public enum SafeSize240 { Legacy,Small,Large }
 public static class MainSafeRules240 {
  // New sizes have fixed independent tables; legacy safes keep their original rules.
  public static double[] Quality(SafeSize240 size)=>size==SafeSize240.Large?new double[]{0,5,45,50,0}:new double[]{5,30,40,23,0};
  public static double RedChance(SafeSize240 size)=>size==SafeSize240.Large?.05:.01;
  public static void Dress(ContainerProp prop,SafeSize240 size){
   bool large=size==SafeSize240.Large;prop.transform.localScale=large?new Vector3(1.38f,1.55f,1.28f):new Vector3(.82f,.82f,.82f);
   foreach(var r in prop.GetComponentsInChildren<Renderer>())if(r.name=="Body"||r.name=="SteelDoor")r.material.color=large?new Color(.31f,.39f,.41f):new Color(.36f,.42f,.36f);
   // Steel bands distinguish the tall strongbox without changing its door hinge.
   if(large)foreach(float y in new[]{.15f,.94f}){var bar=GameObject.CreatePrimitive(PrimitiveType.Cube);bar.name="LargeSafeReinforcement240";bar.transform.SetParent(prop.transform,false);bar.transform.localPosition=new Vector3(0,y,.33f);bar.transform.localScale=new Vector3(.96f,.055f,.055f);Object.Destroy(bar.GetComponent<Collider>());bar.GetComponent<Renderer>().material.color=new Color(.55f,.57f,.50f);}
  }
 }
}
