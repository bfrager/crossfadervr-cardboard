 using UnityEngine;
 public class RoutineRunner : MonoBehaviour
 {
     public static RoutineRunner instance;
 
     void Awake ()
     {
         instance = this;
     }
 }    