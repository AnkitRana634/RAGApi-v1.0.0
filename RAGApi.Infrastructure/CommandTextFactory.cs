using RAGApi.Domain.Repository;
using RAGApi.Infrastructure.Repositories.MSSQL;
using RAGApi.Infrastructure.Repositories.PostgreDB;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGApi.Domain.Enum;

namespace RAGApi.Infrastructure
{
    internal class CommandTextFactory
    {
        protected CommandTextFactory()
        { }
        private static Dictionary<int, Func<ICommandText>> CommandTextFactories = new Dictionary<int, Func<ICommandText>>
        {
            {(int)CommandTextFactoryImplementation.MSSQLDB, () => new MSSQLDBCommandText() },
                  {(int)CommandTextFactoryImplementation.PostgreDB, () => new PostgreDBCommandText() },
        };
        public static ICommandText CreateInstance()
        {
            return CommandTextFactories[(int)System.Enum.Parse(typeof(CommandTextFactoryImplementation), "MSSQLDB")]();
        }

        public static ICommandText CreateMSSQLInstance()
        {
            return CommandTextFactories[(int)System.Enum.Parse(typeof(CommandTextFactoryImplementation), "MSSQLDB")]();
        }
    }
}
