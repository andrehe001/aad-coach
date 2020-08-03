using System;
using System.Collections.Generic;

namespace GameDayRunner
{
    public class Properties
    {
        public Int32 NumberOfRequestExecutorsPerTeam { get; set; }

        public Int32 RequestExecutorLatencyMillis { get; set; }

        public Status Status { get; set; }

        public IList<String> GameEngineURis { get; set; }
        
        public string Name { get; internal set; }
    }
}