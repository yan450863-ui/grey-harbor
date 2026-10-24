using System.Collections;using UnityEngine;
namespace GreyHarbor {public sealed partial class RaidVerification {
 IEnumerator VerifyHomeArt316(){var ui=G.GetComponent<RaidUI>();G.OpenCampPage315(CampPage315.Home);var art=ui.HomeArt316();Check(art!=null&&art.width>=1600&&art.height>=900,"316 actual deployed menu illustration loads");results.Add("INFO hidden swapchain does not dispatch native OnGUI for pointer testing; no native click or UI screenshot pass claimed.");yield return null;}
}}
