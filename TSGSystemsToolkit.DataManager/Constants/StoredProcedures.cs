namespace TsgSystemsToolkit.DataManager.Constants
{
    public static class StoredProcedures
    {
        public static class User
        {
            public const string GetById = "dbo.spUser_GetById";
            public const string Insert = "dbo.spUser_Insert";
        }

        public static class Stations
        {
            public const string Insert = "dbo.spStations_Insert";
            public const string GetById = "dbo.spStations_GetById";
            public const string GetAll = "dbo.spStations_GetAll";
            public const string GetByKimoceId = "dbo.spStations_GetByKimoceId";
        }
        
        public static class SerialDevices
        {
            public const string Insert = "dbo.spSerialDevices_Insert";
            public const string GetByPosId = "dbo.spSerialDevices_GetByPosId";
        }
        
        public static class Pos
        {
            public const string Insert = "dbo.spPos_Insert";
            public const string GetIdByNumber = "dbo.spPos_GetIdByNumber";
            public const string GetByStationId = "dbo.spPos_GetByStationId";
        }
        
        public static class DebugProcesses
        {
            public const string GetByName = "dbo.spDebugProcess_GetByName";
            public const string GetAll = "dbo.spDebugProcesses_GetAll";
            public const string Insert = "dbo.spDebugProcesses_Insert";
            public const string GetAllWithParams = "spDebugProcesses_GetAllWithParams";
        }

        public static class DebugParameters
        {
            public const string GetByProcessId = "dbo.spDebugParameters_GetByProcessId";
        }
        
    }
}
