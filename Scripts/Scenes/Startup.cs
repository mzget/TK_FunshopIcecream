using UnityEngine;
using System.Collections;

public class Startup : Mz_BaseScene {

	// Use this for initialization
	void Start () {
		this.Initialization();
		//<!-- get name quality.
		//		qualities_list = QualitySettings.names;
		this.AutomaticSetup_QualitySetting();

		Handheld.PlayFullScreenMovie("Movies/LogoVista.mp4", Color.white, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
		
		if(Application.isLoadingLevel == false) {
			Application.LoadLevelAsync(Mz_BaseScene.SceneNames.WaitForStart.ToString());
		}
	}
	
	private new void Initialization ()
	{		
		Mz_OnGUIManager.CalculateViewportScreen();

		Mz_StorageManage.Language_id = PlayerPrefs.GetInt(Mz_StorageManage.KEY_SYSTEM_LANGUAGE, 0);
		Main.Mz_AppLanguage.appLanguage = (Main.Mz_AppLanguage.SupportLanguage)Mz_StorageManage.Language_id;
	}
	
	private void AutomaticSetup_QualitySetting() {
		#if UNITY_IOS && !UNITY_EDITOR

		if(iPhone.generation == iPhoneGeneration.iPad1Gen || iPhone.generation == iPhoneGeneration.iPodTouch4Gen ||
		   iPhone.generation == iPhoneGeneration.iPhone3G || iPhone.generation == iPhoneGeneration.iPhone3GS) {
			QualitySettings.SetQualityLevel(0);
			Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 30;
		}

		#elif UNITY_ANDROID && !UNITY_EDITOR

		if(Screen.height <= 480) {
			QualitySettings.SetQualityLevel(0);
			Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 30;
		}

		#else 

		QualitySettings.SetQualityLevel(3);
		Application.targetFrameRate = 60;

		#endif
	}
}
