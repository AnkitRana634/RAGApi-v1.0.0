using System;
using System.Collections.Generic;
using System.Text;

namespace RAGApi.Domain
{
    public class Enum
    {
        public enum QueryTextFactoryImplementation
        {
            MSSQLDB,
            PostgreDB
        }
        public enum CommandTextFactoryImplementation
        {
            MSSQLDB,
            PostgreDB
        }

    }
}
