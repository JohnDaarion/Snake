using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Abstractions
{
    /// <summary>
    /// Picture downloader
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Download new picture
        /// </summary>
        /// <returns></returns>
        void DownloadNextPicture(BlockingCollection<string> downloaded, Action after, CancellationToken cancellationToken);
    }
}
