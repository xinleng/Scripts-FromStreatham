using System;
using System.Collections.Generic;
using UnityEngine;
using DaikonForge.Tween.Components;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SlideShowGallery:MonoBehaviour { 

	public float startDelay;
	public float fadeLength;

	TweenComponentGroup slideShowTween;

	public RawImage bottomImage;
	public RawImage topImage;
	
	public int CurrentImageIndex ;

	public String CurrentImageName ;
	public String NextImageName ;

	public string folderPath = "";
	public List<string> imageNameList ;

	public void Start ()
	{

		slideShowTween = GetComponent<TweenComponentGroup>();

		foreach (TweenColorProperty tweenColor in GetComponents<TweenColorProperty>())
		{
			tweenColor.StartDelay = startDelay;
			tweenColor.Duration = fadeLength;
		}

		slideShowTween.StartDelay = startDelay;

		slideShowTween.Tweens[0].TweenCompleted += HandleHideTweenCompleted;
		slideShowTween.Tweens[1].TweenCompleted += HandleShowTweenCompleted;

		UpdateImageNames();
		UpdateImageHolderImages();

	}

	public void OnEnable ()
	{

		slideShowTween = GetComponent<TweenComponentGroup>();

		if(!slideShowTween.IsPlaying) slideShowTween.Play();
		if(slideShowTween.IsPaused) slideShowTween.Resume();

	}

	public  void OnDisable ()
	{
		slideShowTween.Pause();
	}

	void HandleShowTweenCompleted (TweenPlayableComponent sender)
	{
		Forward();
		bottomImage.texture = Resources.Load (NextImageName) as Texture; 
	}

	void HandleHideTweenCompleted (TweenPlayableComponent sender)
	{
		Forward();
		topImage.texture = Resources.Load (NextImageName) as Texture;
	}
	

	void UpdateImageHolderImages ()
	{
		if(Resources.Load(CurrentImageName) == null) Debug.LogWarning ("Can not load image " + CurrentImageName);
		topImage.texture = Resources.Load (CurrentImageName) as Texture;

		if(Resources.Load(NextImageName) == null) Debug.LogWarning ("Can not load image " + NextImageName);
		bottomImage.texture = Resources.Load (NextImageName) as Texture;
	}


	public void Forward ()
	{
	
		if(CurrentImageIndex<imageNameList.Count-1) CurrentImageIndex ++;
		else CurrentImageIndex = 0;
		
		UpdateImageNames ();
	
	}
	

  	void UpdateImageNames ()
	{
	
		CurrentImageIndex = Mathf.Clamp(CurrentImageIndex,0,imageNameList.Count-1);

		CurrentImageName = imageNameList [CurrentImageIndex];

		if(CurrentImageIndex < imageNameList.Count-1) NextImageName = imageNameList [CurrentImageIndex + 1];
		else if (CurrentImageIndex == imageNameList.Count-1) NextImageName = imageNameList[0];

	}

	[Header ("Editor Tools")]
	public List<Texture> imageList;
	public bool order = false;
	public bool grabImageNameNow;

	void Update(){		

		if(grabImageNameNow) GrabImageNames();
	
	}
	
	void GrabImageNames(){
		
		imageNameList.Clear();
		if (folderPath == "") Debug.LogWarning("Enter Subfolder to the images");

		foreach(Texture texture in imageList) imageNameList.Add (folderPath + texture.name);	

		if (order) imageNameList.Sort();
		grabImageNameNow = false;
		imageList.Clear();
		
	}

}