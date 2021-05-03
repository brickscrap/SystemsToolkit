namespace TsgSystemsToolkit.DataManager.Constants
{
    public static class StoredProcedures
    {
        // User
        public const string User_GetById = "dbo.spUser_GetById";
        public const string User_Insert = "dbo.spUser_Insert";

        // Station
        public const string Stations_Insert = "dbo.spStations_Insert";
        public const string Stations_GetById = "dbo.spStations_GetById";
        public const string Stations_GetAll = "dbo.spStations_GetAll";

        // SerialDevices
        public const string SerialDevices_Insert = "dbo.spSerialDevices_Insert";
        public const string SerialDevices_GetByPosId = "dbo.spSerialDevices_GetByPosId";

        // POS
        public const string Pos_Insert = "dbo.spPos_Insert";
        public const string Pos_GetIdByNumber = "dbo.spPos_GetIdByNumber";
        public const string Pos_GetByStationId = "dbo.spPos_GetByStationId";

        // DebugProccesses
        public const string DebugProcess_GetByName = "dbo.spDebugProcess_GetByName";
        public const string DebugProcesses_GetAll = "dbo.spDebugProcesses_GetAll";
        public const string DebugProcesses_Insert = "dbo.spDebugProcesses_Insert";

        // DebugParameters
    }
}
