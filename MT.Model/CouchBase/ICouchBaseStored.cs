using MT.LQQ.Models.Enum;
using Newtonsoft.Json;

namespace MT.LQQ.Models.CouchBase
{
    /// <summary>
    /// CouchBase存储接口
    /// </summary>
    public interface ICouchBaseStored
    {
        /// <summary>
        /// KEY
        /// </summary>
        [JsonIgnore]
        string Key { get; }

        /// <summary>
        /// Bucket Name
        /// </summary>
        [JsonIgnore]
        DataBucketEnum Bucket { get; }
    }
}