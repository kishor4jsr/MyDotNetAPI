using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Models
{
    public class FileMetadata
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public string FilePath { get; set; }
    }
}
