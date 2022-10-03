using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace HexagonalTileGrid.Utilities{
    public static class JSONDataReader
    {

        #region PublicMethods =================================================================================================================================================
        public static async Task<TileGridData> GetTileGridDataFromURL(string url)
        {
            string jSONRequestResult = await asyncGetJSONFromURL(url);
            TileGridData tileGridData = ProcessJSONData(jSONRequestResult);
            return tileGridData;
        }

        #endregion

        #region InternalLogic =========================================================================================================================================


        private static async Task<string> asyncGetJSONFromURL(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SendWebRequest();
            while (!www.isDone)
            {
                Debug.Log("waiting...");
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {

                string returnText = www.downloadHandler.text;
                Debug.Log("responce received");
                www.Dispose();
                return returnText;
            }
            else
            {
                Debug.LogError($"SAYS: Error occured while retreiving JSON data from URL");
                Debug.LogError(www.error);
                www.Dispose();
                return "";
            }
        }

        private static TileGridData ProcessJSONData(string textJSON)
        {
            TileGridData tileGridData = JsonUtility.FromJson<TileGridData>(textJSON);
            return tileGridData;
        }
        #endregion

    }
}
