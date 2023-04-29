using System;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Shared.ConstantObjects;

public enum DatabaseEvent
{
    INSERT, UPDATE, DELETE, NONE
}

public static class DatabaseEventExtensions
{
    public static string GetConfigurationInfoMessage(this DatabaseEvent databaseEvent, string value = "")
    {
        switch (databaseEvent)
        {
            case DatabaseEvent.INSERT:
                return string.Format(Texts.ApiResponse_InsertedConfiguration_Configuration_With_Value_Has_Been_Added, value);
            case DatabaseEvent.UPDATE:
                return string.Format(Texts.ApiReponse_ConfigurationUpdated_Configuration_Has_Been_Updated_With_Value, value);
            case DatabaseEvent.NONE:
                return Texts.ApiResponse_ConfigurationNotChanged_Record_Already_Exists;
            default:
                throw new ArgumentException("DatabaseEvent doesnt have configuration info message");
        }
    }
}

