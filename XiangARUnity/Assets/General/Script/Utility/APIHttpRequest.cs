using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class APIHttpRequest
{

    public static IEnumerator NativeCurl(string url, string httpMethods, string rawJsonObject, System.Action<string> success_callback, System.Action fail_callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = 4;
            webRequest.method = httpMethods;

            if (rawJsonObject != null) {
                webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(rawJsonObject));
                webRequest.uploadHandler.contentType = "application/json";
            }

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError) {
                if (fail_callback != null) fail_callback();

                yield break;
            }

            try
            {
                string rawJSON = webRequest.downloadHandler.text;

                if (success_callback != null) success_callback(rawJSON);
            }
            catch {
                if (fail_callback != null) fail_callback();
            }
        }
    }

}
