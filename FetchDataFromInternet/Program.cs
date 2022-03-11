using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;

namespace FetchDataFromInternet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] arr = args[0].Split(".");
            SearchResult searchResult = SearchArtist(string.Join(" ", arr), "true");

            List<string> objType = new List<string>() {
                "Canvas",
                "Drawings",
                "Oil on canvas",
                "Prints",
                "Printing",
                "Paintings "};

            for (int i = 0; i < searchResult.total; i++)
            {
                Arts arts = SearchArt(searchResult.objectIDs[i]);
                if (arts != null)
                {
                    Thread.Sleep(Convert.ToInt32(args[1]));


                    List<string> temp = new List<string>();
                    temp.Add(arts.artistDisplayName);
                    temp.Add(arts.title);
                    temp.Add(string.IsNullOrEmpty(arts.objectDate) ? "" : "(" + arts.objectDate + ")");


                    string fileName = string.Join(" - ", temp.FindAll(x => !string.IsNullOrEmpty(x)));


                    SaveImage(arts.primaryImage, fileName);
                }
            }
        }


        private static void SaveImage(string url, string fileName)
        {
            if (url != "")
            {
                using (WebClient client = new WebClient())
                {
                    if (!Directory.Exists("Images"))
                    {
                        Directory.CreateDirectory("Images");
                    }
                    if (!File.Exists(".\\Images\\" + fileName + ".jpg"))
                    {
                        if (!client.IsBusy)
                        {
                            client.DownloadFileAsync(new Uri(url), ".\\Images\\" + fileName + ".jpg");
                        }
                    }
                }
            }
        }

        private static Arts SearchArt(int objectIds)
        {
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://collectionapi.metmuseum.org/public/collection/v1/objects/" + objectIds));

                WebReq.Method = "GET";

                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                string jsonString;
                using (Stream stream = WebResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }
                return JsonSerializer.Deserialize<Arts>(jsonString);

            }
            catch (Exception)
            {
                return null;
            }

        }

        private static SearchResult SearchArtist(string artistName, string isPublicDomain)
        {
            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://collectionapi.metmuseum.org/public/collection/v1/search?isPublicDomain=" + isPublicDomain + "&q=" + artistName));

                WebReq.Method = "GET";

                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                string jsonString;
                using (Stream stream = WebResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }

                return JsonSerializer.Deserialize<SearchResult>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
