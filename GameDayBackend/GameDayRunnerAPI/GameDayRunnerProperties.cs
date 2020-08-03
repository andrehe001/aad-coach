using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameDayRunner.Models
{
    public class GameDayRunnerProperties
    {
        public Int32 NumberOfRequestExecutorsPerTeam { get; set; }

        public Int32 RequestExecutorLatencyMillis { get; set; }

        public Status Status { get; set; }

        public IList<String> GameEngineURis { get; set; }    
    }
}
