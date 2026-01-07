using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.ENTITY
{
    public enum Status
    {
        InActive = 0,
        Active = 1,
    }

    public enum FolderDirectoryMasterTypeEnum
    {
        DatafilePullingDirectory = 1,
        PulledDatafileSavingDirectory = 2,
    }

    public enum IndicatorEnum
    {
        Insert = 0,
        Edit = 1,
        Delete = 2,
    }
    public enum IsActive
    {
        False = 0,
        True = 1,
    }
    public enum IsStaged
    {
        False = 0,
        True = 1,
    }
}
