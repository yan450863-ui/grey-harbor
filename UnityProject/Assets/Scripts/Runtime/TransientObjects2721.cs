using UnityEngine;

namespace GreyHarbor {
 public static class TransientObjects2721 {
  // Only for objects created while assembling a map or its art. Keep the
  // existing delayed runtime lifetime, and release editor bake scratch objects.
  public static void Release(Object value){
   if(!value)return;
   if(Application.isPlaying)Object.Destroy(value);
   else Object.DestroyImmediate(value);
  }
 }
}
