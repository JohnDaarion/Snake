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
        void DownloadNextPicture();
    }
}
