using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DaikonForge.Tween.Components;
using UnityEngine.EventSystems;

// Applied on the same OBJ that has Tween.
public class TweenToggle_Animation : MonoBehaviour {

	// Works with:
	TweenVector3Property tweenV3;
	TweenObjectScale tweenOBJscale;
	TweenObjectPosition tweenOBJpos;
	TweenObjectRotation tweetOBJrotation;
	
	public bool tweenPlayed = false;

	void Start () {
	
		tweenOBJpos = GetComponent<TweenObjectPosition>();
		tweenOBJscale = GetComponent<TweenObjectScale>();
		tweenV3 = GetComponent<TweenVector3Property>();
		tweetOBJrotation = GetComponent<TweenObjectRotation>();
		
	}

	public void PlayTween(){
	
		if (!tweenPlayed){
			if (tweenOBJpos != null) tweenOBJpos.PlayDirection = DaikonForge.Tween.TweenDirection.Forward;
			if (tweenOBJscale != null) tweenOBJscale.PlayDirection = DaikonForge.Tween.TweenDirection.Forward;
			if (tweenV3 != null) tweenV3.PlayDirection = DaikonForge.Tween.TweenDirection.Forward;
			if (tweetOBJrotation != null) tweetOBJrotation.PlayDirection = DaikonForge.Tween.TweenDirection.Forward;
			
			Play ();
			
		}else{
			if (tweenOBJpos != null) tweenOBJpos.PlayDirection = DaikonForge.Tween.TweenDirection.Reverse;
			if (tweenOBJscale != null) tweenOBJscale.PlayDirection = DaikonForge.Tween.TweenDirection.Reverse;
			if (tweenV3 != null) tweenV3.PlayDirection = DaikonForge.Tween.TweenDirection.Reverse;
			if (tweetOBJrotation != null) tweetOBJrotation.PlayDirection = DaikonForge.Tween.TweenDirection.Reverse;
			
			Play ();
			
		}
		
		if (!tweenPlayed) tweenPlayed = true;
		else tweenPlayed = false;
		
	}
	
	void Play(){
		
		if (tweenOBJpos != null) tweenOBJpos.Play();
		if (tweenOBJscale != null) tweenOBJscale.Play();
		if (tweenV3 != null) tweenV3.Play();
		if (tweetOBJrotation != null) tweetOBJrotation.Play();
		
	}
}