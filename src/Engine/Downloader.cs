using Engine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Engine
{
    ///<inheritdoc/>
    public class Downloader : IDownloader
    {
        private readonly Random _random = new Random();
        private readonly List<string> Links = new List<string> { 
            "https://cdn.pixabay.com/photo/2016/08/31/18/19/snake-1634293_960_720.jpg",
            "https://cdn.pixabay.com/photo/2014/12/25/14/54/snake-579682_960_720.jpg",
            "https://cdn.pixabay.com/photo/2014/10/25/07/52/king-snake-502263_960_720.jpg",
            "https://cdn.pixabay.com/photo/2014/11/04/12/34/snake-516507_960_720.jpg",
            "https://cdn.pixabay.com/photo/2014/08/15/21/40/snake-419043_960_720.jpg"
        };

        ///<inheritdoc/>
        public void DownloadNextPicture()
        {
            using (WebClient client = new WebClient())
            {
                var current = _random.Next(0, 4);
                var uri = new Uri(Links[current]);
                client.DownloadFileAsync(new Uri(Links[current]), DateTime.UtcNow.Ticks + uri.Segments.Last());
            }
        }
    }
}
