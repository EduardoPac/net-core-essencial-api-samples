using System.Data;

namespace ApiTasksV2.Data
{
    public class TaskContext
    {
        public delegate Task<IDbConnection> GetConnection();
    }
}
