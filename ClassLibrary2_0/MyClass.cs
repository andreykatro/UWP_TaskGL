using System;
using System.Collections.Generic;

namespace ClassLibrary2_0
{
    [Serializable]
    public class MyClass
    {
        public string DirStructure { get; set; }
        public byte[] MyFile { get; set; }
    }

    [Serializable]
    public class MyList : List<MyClass> { }
}
