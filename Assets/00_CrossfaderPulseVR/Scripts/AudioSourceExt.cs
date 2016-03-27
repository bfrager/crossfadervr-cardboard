 using System;
 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public static class AudioSourceExtensions
 {
     public static void FadeTo (this AudioSource audioSource, float targetVolume)
     {
         FadeTo (audioSource, targetVolume, null);
     }
 
     public static void FadeTo (this AudioSource audioSource, float targetVolume, Action onComplete)
     {
         FadeTo (audioSource, targetVolume, 0.3f, onComplete);
     }
 
     public static void FadeTo (this AudioSource audioSource, float targetVolume, float duration, Action onComplete)
     {
         if (coroutines.ContainsKey (audioSource)) {
             RoutineRunner.instance.StopCoroutine (coroutines [audioSource]);
         }
 
         var coroutine = FadeToCoroutine (audioSource, targetVolume, duration, onComplete);
         RoutineRunner.instance.StartCoroutine (coroutines [audioSource] = coroutine);
     }
     
     public static IEnumerator FadeToCoroutine (AudioSource audioSource, float targetVolume, float duration, Action onComplete)
     {
         float elapsed = 0;
         float startVolume = audioSource.volume;
         while ((elapsed += Time.deltaTime) < duration) {
             audioSource.volume = Mathf.Lerp (startVolume, targetVolume, elapsed / duration);
             yield return null;
         }
 
         audioSource.volume = targetVolume;
         coroutines.Remove (audioSource);
         if (onComplete != null)
             onComplete ();
     }
 
     static Dictionary<AudioSource, IEnumerator> coroutines = new Dictionary<AudioSource, IEnumerator> ();
 }