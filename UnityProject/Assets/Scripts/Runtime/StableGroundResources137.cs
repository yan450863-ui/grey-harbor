using UnityEngine;
namespace GreyHarbor {public sealed class StableGroundResources137:MonoBehaviour {
 public Texture2D Texture;
 public bool OwnsTexture=true;
 void OnDestroy(){if(OwnsTexture&&Texture){if(Application.isPlaying)Destroy(Texture);else DestroyImmediate(Texture);}}
}}
