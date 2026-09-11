using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;

public static class FilesDownloader
{
    public static async Task<List<GitHubFile>> GetGithubFiles(string url)
    {
        UnityWebRequest request  = UnityWebRequest.Get(url);

        // GitHub API expects a User-Agent.
        request.SetRequestHeader("User-Agent", "Unity");

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"GitHub error: {request.error}");
            return null;
        }
        //Debug.Log(request.downloadHandler.text);
        return JsonConvert.DeserializeObject<List<GitHubFile>>(request.downloadHandler.text);
    }
    public static async Task<Sprite> DownloadSprite(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return BytesToSprite(request.downloadHandler.data);
    }
    public static Sprite BytesToSprite(byte[] imageData)
    {
        Texture2D texture = new Texture2D(2, 2);

        if (!texture.LoadImage(imageData))
        {
            Debug.LogError("Failed to load image.");
            return null;
        }

        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
    public static async Task<string> DownloadJson(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return request.downloadHandler.text;
    }
    static async Task<UnityWebRequest> GetRequest(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }
        return request;
    }
    public static async Task DownloadDirectoryRecursive(string githubDirectoryUrl, string localDirectory)
    {
        //string apiUrl = ConvertToApiUrl(githubDirectoryUrl);
        string apiUrl = githubDirectoryUrl;


        string json = await DownloadJson(apiUrl);

        if (string.IsNullOrEmpty(json))
            return;

        List<GitHubFile> files = JsonConvert.DeserializeObject<List<GitHubFile>>(json);

        Directory.CreateDirectory(localDirectory);

        foreach (GitHubFile file in files)
        {
            string localPath = Path.Combine(
                localDirectory,
                file.name
            );

            if (file.type == "dir")
            {
                // The API URL for this directory is contained in the
                // original API path, so construct it from the current URL.

                string childApiUrl = apiUrl.Substring(0, apiUrl.IndexOf("/contents/") + 10) + file.path;

                // Preserve branch parameter
                int queryIndex = apiUrl.IndexOf('?');

                if (queryIndex != -1)
                    childApiUrl += apiUrl.Substring(queryIndex);

                await DownloadDirectoryRecursive(childApiUrl, localPath);
            }
            else if (file.type == "file")
            {
                await DownloadFile(file.download_url, localPath);
            }
        }
    }
    private static string ConvertToApiUrl(string githubUrl)
    {
        Debug.Log(githubUrl);
        Uri uri = new Uri(githubUrl);

        string[] segments = uri.AbsolutePath
            .Trim('/')
            .Split('/');

        // Expected:
        // owner / repo / tree / branch / optional/path

        if (segments.Length < 4 || segments[2] != "tree")
            throw new ArgumentException(
                "Invalid GitHub directory URL."
            );

        string owner = segments[0];
        string repo = segments[1];
        string branch = segments[3];

        string path = "";

        if (segments.Length > 4)
        {
            path = string.Join(
                "/",
                segments,
                4,
                segments.Length - 4
            );
        }

        string apiUrl =
            $"https://api.github.com/repos/{owner}/{repo}/contents";

        if (!string.IsNullOrEmpty(path))
            apiUrl += "/" + path;

        apiUrl += $"?ref={Uri.EscapeDataString(branch)}";

        return apiUrl;
    }
    private static async Task DownloadFile(string url, string localPath)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader(
            "User-Agent",
            "Unity-GitHub-Downloader"
        );

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"Failed to download:\n{url}\n{request.error}"
            );

            return;
        }

        string directory = Path.GetDirectoryName(localPath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllBytes(
            localPath,
            request.downloadHandler.data
        );

        Debug.Log($"Downloaded: {localPath}");
    }
}