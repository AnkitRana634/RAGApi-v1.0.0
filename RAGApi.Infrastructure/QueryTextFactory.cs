using RAGApi.Domain.Repository;
using RAGApi.Infrastructure.Repositories.MSSQL;
using RAGApi.Infrastructure.Repositories.PostgreDB;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGApi.Domain.Enum;

namespace RAGApi.Infrastructure
{
    public class QueryTextFactory
    {
        protected QueryTextFactory()
        { }
        private static Dictionary<int, Func<IQueryText>> QueryTextFactories = new Dictionary<int, Func<IQueryText>>
        {
            {(int)QueryTextFactoryImplementation.MSSQLDB, () => new MSSQLDBQueryText() },
                  {(int)QueryTextFactoryImplementation.PostgreDB, () => new PostgreDBQueryText() },
        };
        public static IQueryText CreateInstance()
        {
            return QueryTextFactories[(int)System.Enum.Parse(typeof(QueryTextFactoryImplementation), "MSSQLDB")]();
        }

        public static IQueryText CreateMSSQLInstance()
        {
            return QueryTextFactories[(int)System.Enum.Parse(typeof(QueryTextFactoryImplementation), "MSSQLDB")]();
        }
    }
}
