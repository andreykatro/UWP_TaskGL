using System;
using System.Collections.Generic;

namespace UWP_TaskGL
{
    [Serializable]
    public class ModelMemoryFile
    {
        public string DirStructure { get; set; }
        public string FileName { get; set; }
        public byte[] ByteFile { get; set; }
    }

    [Serializable]
    public class ListMemoryFile : List<ModelMemoryFile> { }
}
