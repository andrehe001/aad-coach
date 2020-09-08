using System;
using System.Net.Http;
using System.Threading;

namespace AdventureDayRunner
{
    class BackgroundTaskRunner
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Run()
        {
            Properties properties = Utils.ReadPropertiesFromDb();
            do 
            {
                foreach (string uri in properties.GameEngineURis) 
                {
                    for (int i = 0; i < properties.NumberOfRequestExecutorsPerTeam; i++) {
                        ThreadPool.QueueUserWorkItem(BackgroundTask, uri);
                    }
                }
                properties = Utils.ReadPropertiesFromDb();
                Thread.Sleep(properties.RequestExecutorLatencyMillis);
            } while (properties.Status == Status.ACTIVE);
        }

        static async void BackgroundTask(Object uri)  
        {
            try 
            {             
                await new RandomPlayer((string)uri).Play();
            } catch (HttpRequestException e) {
                Logger.Error(e.Message);
            }    
        }
    }
}