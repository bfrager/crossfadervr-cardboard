// using UnityEngine;
// using System.Collections;

// public class ApiCall2 : MonoBehaviour {

// 	// Use this for initialization
// 	void Start () {
		
// 	   // Parses the json Object
// JSONParser parser = new JSONParser();

// AssetData assetData = parser.ParseString(mJsonAssetInfo.text);
// mAssetData = assetData;

// // Updates state variables
// mIsLoadingAssetData = false;

// for(var i = 0; i < assetData.Assets.Count; i++){
//     GameObject newAsset = null;

//     if(((Assets)assetData.Assets[i]).AssetType == "Text"){

//         newAsset = (GameObject)Instantiate(asset, new Vector3(15*i, 0, 0), Quaternion.identity);
//         newAsset.GetComponent<TextMesh>().text = ((Assets)assetData.Assets[i]).AssetContent;
//         newAsset.transform.rotation = Quaternion.Euler(90, -180, 0);

//         string[] rgba = Regex.Split(((Assets)assetData.Assets[i]).AssetBgcolor, ", ");
//         float red = float.Parse(rgba[0]);
//         float green = float.Parse(rgba[1]);
//         float blue = float.Parse(rgba[2]);
//         float alpha = float.Parse(rgba[3]);

//         var child =  newAsset.renderer.transform.transform.Find("background");
//         child.renderer.material.color = new Color(red/255, green/255, blue/255, alpha);

//         float posXtag;
//         posXtag = (((((Assets)assetData.Assets[i]).AssetLeft * 100f / 1024f)  + (((Assets)bookData.Assets[i]).AssetWidth * 100f / 1024f) / 2f))-50f;

//         float posYtag;
//         posYtag = -1*((Assets)assetData.Assets[i]).AssetTop * 50f / 512f - ((Assets)assetData.Assets[i]).AssetHeight * 50f / 512f / 2f +25f;

//         newAsset.transform.localPosition = new Vector3(posXtag,0,posYtag);
//         child.renderer.transform.localScale = new Vector3(((Assets)assetData.Assets[i]).AssetWidth/102.4f,0.2f,((Assets)assetData.Assets[i]).AssetHeight/102.4f);

//     } else if (((Assets)assetData.Assets[i]).AssetType == "Image"){

//         newAsset = (GameObject)Instantiate(iasset, new Vector3(15*i, 0, 0), Quaternion.identity);
//         newAsset.transform.parent = AugmentationObject.transform;
//         Color color = newAsset.renderer.material.color;
//         color.a = 0f;
//         newAsset.renderer.material.color = color;
//         string url = ((Assets)bookData.Assets[i]).AssetContent;
//         StartCoroutine(DownloadImage(url, newAsset, ((Assets)assetData.Assets[i]).AssetFilename, "IMAGE"));

//         newBrick.transform.rotation = Quaternion.Euler(0, 0, 0);

//         float posXtag;
//         posXtag = (((((Assets)assetData.Assets[i]).AssetLeft * 100f / 1024f)  + (((Assets)assetData.Assets[i]).AssetWidth * 100f / 1024f) / 2f))-50f;

//         float posYtag;
//         posYtag = -1*((Assets)assetData.Assets[i]).AssetTop * 50f / 512f - ((Assets)assetData.Assets[i]).AssetHeight * 50f / 512f / 2f +25f;

//         newAsset.transform.localPosition = new Vector3(posXtag,0,posYtag);
//         newAsset.transform.localScale = new Vector3(((Assets)assetData.Assets[i]).AssetWidth/102.4f,0.2f,((Assets)assetData.Assets[i]).AssetHeight/102.4f);


//     } else if (((Assets)assetData.Assets[i]).AssetType == "Video"){

//         newAsset = (GameObject)Instantiate(video, new Vector3(15*i, 0, 0), Quaternion.identity);
//         newAsset.GetComponent<Playback>().m_path = ((Assets)assetData.Assets[i]).AssetContent;

//         string url = ((Assets)newAssetData.Assets[i]).AssetThumbnail;
//         StartCoroutine(DownloadImage(url, newAsset, ((Assets)assetData.Assets[i]).AssetFilename, "VIDEO"));


//         newAsset.transform.rotation = Quaternion.Euler(0, -180, 0);

//         float posXtag;
//         posXtag = (((((Assets)assetData.Assets[i]).AssetLeft * 100f / 1024f)  + (((Assets)assetData.Assets[i]).AssetWidth * 100f / 1024f) / 2f))-50f;

//         float posYtag;
//         posYtag = -1*((Assets)assetData.Assets[i]).AssetTop * 50f / 512f - ((Assets)assetData.Assets[i]).AssetHeight * 50f / 512f / 2f +25f;

//         newAsset.transform.localPosition = new Vector3(posXtag,0,posYtag);
//         newAsset.transform.localScale = new Vector3(((Assets)assetData.Assets[i]).AssetWidth/102.4f,0.2f,((Assets)assetData.Assets[i]).AssetHeight/102.4f);

//     }

//     newAsset.transform.tag = "Asset";


//     // IF ASSET IS SET TO FADEIN DO THAT HERE:
//     if(((Assets)assetData.Assets[i]).AssetFadein == "true"){
//         iTween.FadeTo(newBrick, 1f, 1);
//     } else {
//         Color color = newAsset.renderer.material.color;
//         color.a = 1f;
//         newAsset.renderer.material.color = color;
//     }
//     // EOF ASSET FADEIN

//     if(((Assets)assetData.Assets[i]).AssetAction != ""){
//         newAsset.AddComponent("TouchListener");
//         newAsset.GetComponent<TouchListener>().actionUrl = ((Assets)assetData.Assets[i]).AssetAction;
//     }

// }
	
// 	}
	
// 	// Update is called once per frame
// 	void Update () {
	
// 	}
// }
