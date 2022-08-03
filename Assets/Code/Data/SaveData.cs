using System;
using System.Collections.Generic;

namespace Code.Data
{
    [Serializable]
    public class SaveData
    {
        public List<EventData> Data = new List<EventData>();
    }
}