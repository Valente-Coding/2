using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScroll : MonoBehaviour
{
    
    public Vector2 offsetSpeed = new Vector2(0.1f, 0.1f); // Adjust the speed of offset change

    private void LateUpdate()
    {
        float offsetX = Time.time * offsetSpeed.x;
        float offsetY = Time.time * offsetSpeed.y;
        RenderSettings.skybox.SetTextureOffset("_FrontTex", new Vector2(offsetX, offsetY));
        RenderSettings.skybox.SetTextureOffset("_LeftTex", new Vector2(offsetX, offsetY));
        RenderSettings.skybox.SetTextureOffset("_DownTex", new Vector2(offsetX, offsetY));
    }
}
