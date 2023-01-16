using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Domain
{
    public static class AppConstant
    {
        public const string NAME = "MWorkforce";
        public static class AssignmentStatus
        {
            public const string AGENT_ARRIVED = "AGENT_ARRIVED";
            public const string AGENT_STARTED = "AGENT_STARTED";
            public const string TASK_COMPLETED = "TASK_COMPLETED";
            public const string TASK_FAILED = "TASK_FAILED";
            public const string TASK_RECEIVED = "TASK_RECEIVED";

            public const string COMPLETEUNVERIFIED = "Complete/Non Verif";
            public const string COMPLETEVERIFIED = "Complete/Verif";
            public const string UNCOMPLETEUNVERIFIED = "Uncomplete/Verif";
            public const string UNCOMPLETEVERIFIED = "Uncomplete/Non Verif";
        }

        public static class Role
        {
            public const string DRIVER = "DRIVER";
            public const string SALES = "SALES";
            public const string SUPERVISOR = "SPV";
            public const string ADMIN = "SA";
            public const string OPERATOR = "OPR";
        }
    }
}
