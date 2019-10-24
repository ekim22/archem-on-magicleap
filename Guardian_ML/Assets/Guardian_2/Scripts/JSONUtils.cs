using System;
using System.Collections.Generic;
using UnityEngine;

public static class JSONUtils
{

    public static KeyValuePair<string, double> maxPrediction(String jsonString)
    {

        string working = jsonString.Substring(1, jsonString.Length - 2);  // strip [ and ]
        working = working.Replace(",null,null", "");                      // will need to fix if we start seeing other vals from automl
        working = working.Replace(",\"metadata\":{}", "");                // "    "
        working = working.Replace("payload", "Items");                    // JsonHelper seems to want this

        string bestName = "";
        double bestScore = 0.0D;

        foreach (Payload p in JsonHelper.FromJson<Payload>(working))
        {
            double score = p.classification.score;
            if (score > bestScore)
            {
                bestName = p.displayName;
                bestScore = score;
            }
        }
        return new KeyValuePair<string, double>(bestName, bestScore);
    }

}

[Serializable]
public class Predictions
{
    public Payload[] payloads;
    public Metadata metadata;
}

[Serializable]
public class Metadata
{
}

[Serializable]
public class Payload
{
    public string annotationSpecId;
    public string displayName;
    public Classification classification;
    public string detail;
}

[Serializable]
public class Classification
{
    public double score;
}

// from:
// https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
//
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

