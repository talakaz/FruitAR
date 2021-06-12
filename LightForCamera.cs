using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using UnityEngine.UI;
//20201223
//using Facebook.Unity;
//20201223
using System.Collections.Generic;

public class LightForCamera : MonoBehaviour {

	enum CameraState{
		Hitting,
		notHitting,
		last,
	};
	CameraState castate;
	public static float y = 0;
	public GameObject m_cARCamera;											// 各場景的AR攝影機.
	public GameObject m_cNCamera;	
	public float V;
	//20201223
	VuforiaControl frame;
	//20201223
	string msg = string.Empty;
	public RawImage picture;
	public RawImage SmallPic;
	public GameObject gPicture;
	public GameObject[] changesiders= new GameObject[2];
	public GameObject ISides;
	public GameObject takePlane;
	public GameObject OKPlane;
	public GameObject LastP;
	public GameObject HintPanel;
	public GameObject FBPost;
	public GameObject TSendSucces;
	public GameObject LastHint;
	public GameObject SaveImageDone;

	public GameObject takebuttom;
	public GameObject Posebuttom;
	public GameObject Hintbuttom;
	public GameObject ReTakebuttom;
	public GameObject[] changesid = new GameObject[2];
	public GameObject Okbut;
	public GameObject[] Logo = new GameObject[3];
	private Texture2D FinalImage;

	public Sprite[] SidersL = new Sprite[5];
	public Sprite[] SidersP = new Sprite[5];

	int si = 1;

	GameObject GrandMa;
	Animator aGrandMa;
	bool bGrandmacomeout = false;
	int iGrandMaPos = 1;

	GameObject[] glights = new GameObject[2] ;
	Light[] lLights = new Light[2];

	void Start() {
		//20201223
		//FB.Init ();
		//20201223
		//20201223
		frame = transform.Find ("ImageTarget").GetComponent<VuforiaControl> ();
		//20201223
		GrandMa = transform.Find ("ImageTarget/FruitGrandma").gameObject;
		aGrandMa = GrandMa.transform.GetComponent<Animator> ();
		glights[0] = transform.Find ("ImageTarget/Area Light").gameObject;
//		glights[1] = transform.Find ("ImageTarget/Area Light2").gameObject;
		lLights[0] = glights[0].transform.GetComponent<Light> ();
//		lLights[1] = glights[1].transform.GetComponent<Light> ();
		gPicture.SetActive (false);
		HintPanel.SetActive (true);
		takePlane.SetActive (true);
		LastP.SetActive (false);
		FBPost.SetActive (false);
		LastHint.SetActive (false);
		// Register Vuforia life-cycle callbacks:
		//20201223

		VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
		VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
		VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        //20201223
        castate = CameraState.Hitting;
	}
	void Update(){
		if (castate != CameraState.last) {
			/*讓螢幕可以自動翻轉左右*/
			if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) && (Screen.orientation != ScreenOrientation.LandscapeLeft)) {
				takebuttom.transform.localRotation = Quaternion.Euler (0, 0, -90);
				Posebuttom.transform.localRotation = Quaternion.Euler (0, 0, -90);
				Hintbuttom.transform.localRotation = Quaternion.Euler (0, 0, -90);
				ReTakebuttom.transform.localRotation = Quaternion.Euler (0, 0, -90);
				changesid [0].SetActive (false);
				changesid [1].SetActive (true);
				changesid [1].transform.localRotation = Quaternion.Euler (0, 0, -90);
				Okbut.transform.localRotation = Quaternion.Euler (0, 0, -90);
				Logo [0].SetActive (false);
				Logo [1].SetActive (true);
				Logo [2].SetActive (false);
			}
			if ((Input.deviceOrientation == DeviceOrientation.LandscapeRight) && (Screen.orientation != ScreenOrientation.LandscapeRight)) {
				takebuttom.transform.localRotation = Quaternion.Euler (0, 0, 90);
				Posebuttom.transform.localRotation = Quaternion.Euler (0, 0, 90);
				Hintbuttom.transform.localRotation = Quaternion.Euler (0, 0, 90);
				ReTakebuttom.transform.localRotation = Quaternion.Euler (0, 0, 90);
				changesid [0].SetActive (false);
				changesid [1].SetActive (true);
				changesid [1].transform.localRotation = Quaternion.Euler (0, 0, 90);
				Okbut.transform.localRotation = Quaternion.Euler (0, 0, 90);
				Logo [0].SetActive (false);
				Logo [1].SetActive (false);
				Logo [2].SetActive (true);
			}
			if ((Input.deviceOrientation == DeviceOrientation.Portrait) && (Screen.orientation == ScreenOrientation.Portrait)) {
				takebuttom.transform.localRotation = Quaternion.Euler (0, 0, 0);
				Posebuttom.transform.localRotation = Quaternion.Euler (0, 0, 0);
				Hintbuttom.transform.localRotation = Quaternion.Euler (0, 0, 0);
				ReTakebuttom.transform.localRotation = Quaternion.Euler (0, 0, 0);
				changesid [0].SetActive (true);
				changesid [1].SetActive (false);
				Okbut.transform.localRotation = Quaternion.Euler (0, 0, 0);
				Logo [0].SetActive (true);
				Logo [1].SetActive (false);
				Logo [2].SetActive (false);
			}
		}
		V = y;
		lLights[0].intensity = V;
//		lLights[1].intensity = V;
		#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))
		#elif UNITY_ANDROID || UNITY_IPHONE
//		if (Input.anyKeyDown)
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		#endif
		{
			//20201223
			CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
			//20201223
		}
        //20201223
        if (frame.isbeingTracked)
        {
            if (bGrandmacomeout == false)
            {
                aGrandMa.Play("Open");
                bGrandmacomeout = true;
            }
        }
        else
        {
            bGrandmacomeout = false;
        }
        if (frame.isbeingTracked)
            DetectLightRGB();
        //20201223

        if (castate == CameraState.last){
			if (!LastP.activeSelf) {
				if (LastHint.activeSelf) {
					if (Input.anyKeyDown) {
						LastHint.SetActive (false);
					}
				}else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
					LastP.SetActive (true);
				}
			}
		}
	}

	public void vGrandMaPose(){//奶奶換動作
		switch (iGrandMaPos) {
		case 0:
			aGrandMa.Play ("Idle");
			iGrandMaPos++;
			break;
		case 1:
			aGrandMa.Play ("Pose02");
			iGrandMaPos++;
			break;
		case 2:
			aGrandMa.Play ("Pose03");
			iGrandMaPos++;
			break;
		case 3:
			aGrandMa.Play ("Pose04");
			iGrandMaPos = 0;
			break;
		}
	}

	public void changeSides(){//換外框
		if (si < SidersL.Length) {
			ISides.transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersP [si];
			si++;
			if (si < SidersL.Length) {
				changesiders[0].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [si];
				changesiders[1].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [si];
			} else {
				changesiders[0].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [0];
				changesiders[1].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [0];
			}
		} else {
			si = 0;
			ISides.transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersP [si];
			si ++;
			changesiders[0].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [si];
			changesiders[1].transform.GetComponent<UnityEngine.UI.Image>().sprite = SidersL [si];
		}
	}
	public void Screenshot(){//拍照
		castate = CameraState.notHitting;
		StartCoroutine (TakeScreenshot ());
	}

	public void CloseSaveDoneInfo() {
		SaveImageDone.SetActive(false);
	}

	public void CheckOK(){//確認照片OK
		changesiders[0].SetActive (false);
		OKPlane.SetActive (false);
		changesiders[1].SetActive (false);
		StartCoroutine (TakeScreenshot ());
	}

	//public void vPosttoFB(){//貼到FB上
 //                           //20201223
 //       if (!FB.IsLoggedIn)
 //       {
 //           FB.LogInWithPublishPermissions(new List<string> { "publish_actions" }, LoginCallBack);
 //       }
 //       else
 //       {
 //           StartCoroutine(PosttoFB());
 //       }
 //       //20201223
 //   }

	public void ReTake(){//重拍
		castate = CameraState.Hitting;
		takePlane.SetActive (true);
		LastP.SetActive (false);
		OKPlane.SetActive (true);
		changesiders[0].SetActive (true);
		changesiders[1].SetActive (true);
		gPicture.SetActive (false);
	}

	public void Hintdissapear (){//說明消失
		HintPanel .SetActive (false);

	}
	public void Hintapear (){//說明出現
		HintPanel.SetActive (true);
	}

	public void FruitFB(){//開啟水果奶奶FB連結
		Application.OpenURL ("https://www.facebook.com/pts.fruitypie.tw/?fref=ts");
	}
	public void FruitTV(){//開啟公視節目表
		Application.OpenURL ("http://web.pts.org.tw/php/programX/program_time_detail.php?PAENO=0000060&ID=PTS");
	}
	public void HoPLAY(){//連到Hoplay官網
		Application.OpenURL ("https://www.hoplay.cc/");
	}
	public void SaveToGallery(){//開啟FB貼文欄
#if UNITY_ANDROID
		NativeGallery.SaveImageToGallery(FinalImage, "水果奶奶AR相機", "MyPhoto.png");
#endif
#if UNITY_IOS
		NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(FinalImage, "水果奶奶AR相機", "MyPhoto.png");
#endif
		SaveImageDone.SetActive(true);
	}
	public void CloseFBPost(){
		FBPost.SetActive (false);
	}
	public void Msg(string s){//存入輸入的FB訊息
		msg = s;
	}
	public void ShowPic(){
		LastP.SetActive (false);
		LastHint.SetActive (true);
	}

    /*			Call Back			*/
    //20201223
    //void InitComplete()
    //{
    //    Debug.Log("FB has been initiased");
    //    if (FB.IsLoggedIn)
    //    {
    //        FB.LogOut();
    //    }
    //}
    //void HandleResult(IGraphResult result)
    //{
    //    if (result.Error != null)
    //    {
    //        Debug.LogError(result.Error);
    //    }
    //    else
    //    {
    //        Debug.Log("picture taken!");
    //    }
    //}
    //void LoginCallBack(ILoginResult result)
    //{
    //    if (result.Error == null)
    //    {
    //        Debug.Log("FB Has login.");
    //        if (FB.IsLoggedIn)
    //            StartCoroutine(PosttoFB());
    //    }
    //    else
    //    {
    //        Debug.Log("Error doing login" + result.Error);
    //    }
    //}

    //void PictuerCallBack(IGraphResult result)
    //{
    //    Texture2D image = result.Texture;
    //    //		Canvas.transform.Find ("Panel/MakeID/FBImage").GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create (image, new Rect (0, 0, 100, 100), new Vector2 (0.5f, 0.5f));
    //    //		Canvas.transform.Find ("Panel/TakePicture/pictures/FBImage").GetComponent<UnityEngine.UI.Image> ().sprite = Sprite.Create (image, new Rect (0, 0, 100, 100), new Vector2 (0.5f, 0.5f));
    //}
    //void NameCallBack(IGraphResult result)
    //{
    //    IDictionary<string, object> profile = result.ResultDictionary;
    //    //		Canvas.transform.Find ("Panel/MakeID/ID/Name").GetComponent<Text>().text = ""+ profile ["first_name"];
    //}

    //void FeedCallback(IResult result)
    //{
    //    if (result.Error == null)
    //    {
    //        Debug.Log("shared");
    //    }
    //    else
    //    {
    //        Debug.Log("Error" + result.Error);
    //    }
    //}
    //20201223
    /*				End Callback				*/

    private IEnumerator TakeScreenshot(){
		takePlane.SetActive (false);

		yield return new WaitForEndOfFrame();
		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D (width, height, TextureFormat.RGB24, false);				//設定要截圖的範圍大小  和 RECT的 WH相同
		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);							//REC(X 最左的起始位置 , Y 最下的起始位置 , w 總寬 , h 總高)  若 XY為00 則是螢幕最左下方
		tex.Apply ();
		FinalImage = tex;
		picture.texture = tex;
		//			string path = System.IO.Path.Combine(Application.persistentDataPath, "image.png");
		//			screenshot = System.IO.File.ReadAllBytes (path);
#if UNITY_ANDROID
		
		//string myFilename = System.DateTime.Now.ToString("yyyy-MM-dd-HHmm") + ".png";
		//string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
		//string myFolderLocation = "/storage/emulated/0/DCIM/MyFolder/";
		//string myScreenshotLocation = myFolderLocation + myFilename;
#endif
		//20210113
#if UNITY_ANDROID
		//if (!System.IO.Directory.Exists(myFolderLocation))
		//{
		//    System.IO.Directory.CreateDirectory(myFolderLocation);
		//}
		//ScreenCapture.CaptureScreenshot(myFilename);
		//System.IO.File.Move(myDefaultLocation, myScreenshotLocation);
#endif

#if UNITY_IOS
			//NativeGallery.SaveImageToGallery(tex, "水果奶奶AR", "MyPhoto.png");
		//System.IO.File.WriteAllBytes (Application.persistentDataPath+"image.png", screenshot);
		//ScreenCapture.CaptureScreenshot("screenshot.png");
#endif
		//20210113
		if (!gPicture.activeSelf)
			gPicture.SetActive (true);
		else {
//				gPicture .SetActive (false);
			LastP.SetActive (true);
			castate = CameraState.last;
		}
	}
//	IEnumerator PosttoFB(){
//		yield return new WaitForEndOfFrame();

//		//			string path = System.IO.Path.Combine(Application.persistentDataPath, "image.png");
//		//			screenshot = System.IO.File.ReadAllBytes (path);
//#if UNITY_ANDROID
//		string myFilename = System.DateTime.Now.ToString("yyyy-MM-dd-HHmm") + ".png";
//		string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
//		string myFolderLocation = "/storage/emulated/0/DCIM/MyFolder/";
//		string myScreenshotLocation = myFolderLocation + myFilename;
//#endif

//#if UNITY_ANDROID
//		//			if (!System.IO.Directory.Exists (myFolderLocation)) {
//		//			System.IO.Directory.CreateDirectory (myFolderLocation);
//		//			}
//		//			Application.CaptureScreenshot(myFilename);
//		//			System.IO.File.Move(myDefaultLocation, myScreenshotLocation);
//#endif

//#if UNITY_IOS

//		//			System.IO.File.WriteAllBytes (Application.persistentDataPath+"image.png", screenshot);
//		Application.CaptureScreenshot("screenshot.png");
//#endif
//		var picturetext= picture.texture as Texture2D;
//		var screenshot = picturetext. EncodeToPNG ();
//		var wwwForm = new WWWForm ();
//		wwwForm.AddBinaryData ("image", screenshot, "InteractiveConsole.png");
//		wwwForm.AddField ("message", msg);
//		//20201223
//		//FB.API("me/photos", HttpMethod.POST, HandleResult, wwwForm);
//		//20201223
//		yield return new WaitForSeconds (0.2f);
//		TSendSucces .SetActive (true);
//		yield return new WaitForSeconds (1f);
//		TSendSucces .SetActive (false);
//		FBPost .SetActive (false);
//	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////
	byte r;
	byte g;
	byte b;

	void DetectLightRGB(){
        //20201223
        if (mPixelFormat == Vuforia.PIXEL_FORMAT.RGB888)
        {
            for (int i = 0; i < pixels.Length; i += 600)
            {
                float h;
                float v;
                float s;
                Color color;
                r = pixels[i + 0];
                g = pixels[i + 1];
                b = pixels[i + 2];
                color = new Color32(r, g, b, 255);
                Color.RGBToHSV(color, out h, out s, out v);
                y += v;
            }
        }
        else if (mPixelFormat == Vuforia.PIXEL_FORMAT.RGBA8888)
        {
            for (int i = 0; i < pixels.Length; i += 800)
            {
                float h;
                float v;
                float s;
                Color color;
                r = pixels[i + 0];
                g = pixels[i + 1];
                b = pixels[i + 2];
                color = new Color32(r, g, b, 255);
                Color.RGBToHSV(color, out h, out s, out v);
                y += v;
            }
        }
        y = y - 200;
        y = (y / (pixels.Length / 200)) * 4.5f;
        //20201223
    }

	private bool mAccessCameraImage = true;

	// The desired camera image pixel format
	//20201223
	private Vuforia.PIXEL_FORMAT mPixelFormat = Vuforia.PIXEL_FORMAT.RGBA8888; // or RGBA8888, RGB888, RGB565, YUV
	//20201223
	// Boolean flag telling whether the pixel format has been registered
	private bool mFormatRegistered = false;

	private void OnVuforiaStarted()
	{
        // Try register camera image format
        //20201223
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            mPixelFormat = Vuforia.PIXEL_FORMAT.RGB888;
            OnVuforiaStarted();
            return;
            //Debug.LogError("Failed to register pixel format " + mPixelFormat.ToString() +
            //    "\n the format may be unsupported by your device;" +
            //    "\n consider using a different pixel format.");
            //mFormatRegistered = false;
        }
        //20201223
    }

	private void OnPause(bool paused)
	{
		if (paused)
		{
			Debug.Log("App was paused");
//			UnregisterFormat();
		}
		else
		{
			Debug.Log("App was resumed");
//			RegisterFormat();
		}
	}
	//20201223
	Vuforia.Image image ;
	//20201223
	byte[] pixels;
	private void OnTrackablesUpdated()
	{
        //20201223
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
                    image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                if (image != null)
                {
                    string imageInfo = mPixelFormat + " image: \n";
                    imageInfo += " size: " + image.Width + " x " + image.Height + "\n";
                    imageInfo += " bufferSize: " + image.BufferWidth + " x " + image.BufferHeight + "\n";
                    imageInfo += " stride: " + image.Stride;
                    //					Debug.Log(imageInfo);
                    pixels = image.Pixels;
                    if (pixels != null && pixels.Length > 0)
                    {
                        //						Debug.Log (pixels.Length);
                        //						t.text = pixels.Length.ToString();
                        //						Debug.Log("Image pixels: " + pixels[0] + "," + pixels[1] + "," + pixels[2] + ",...");
                    }


                }
            }
        }
        //20201223
    }
	/// <summary>
	/// Unregister the camera pixel format (e.g. call this when app is paused)
	/// </summary>
	private void UnregisterFormat()
	{
		//		Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
		//20201223
		CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
		//20201223
		mFormatRegistered = false;
	}
	/// <summary>
	/// Register the camera pixel format
	/// </summary>
	private void RegisterFormat()
	{
        //20201223
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            //			Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
            //			mFormatRegistered = true;
        }
        else
        {
            //			Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
            //			mFormatRegistered = false;
        }
        //20201223
    }
}
