

using TodoLibrary.Model;

namespace TodoLibrary.DataAccess
{
    public class TodoData : ITodoData
    {
        private readonly ISqlDataAccess _sql;

        public TodoData(ISqlDataAccess sql)
        {
            this._sql = sql;
        }

        public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
        {

            return _sql.LoadData<TodoModel, dynamic>("dbo.spTodo_GetAllAssigned",
                 new { AssignedTo = assignedTo },
                 "Default");

        }

        public async Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId)
        {

            var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodo_GetAllAssigned",
                 new { AssignedTo = assignedTo, TodoId = todoId },
                 "Default");

            return results.FirstOrDefault();

        }

        public async Task<TodoModel?> Create(int assignedTo, string task)
        {

            var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodo_Create",
                 new { AssignedTo = assignedTo, Task = task },
                 "Default");

            return results.FirstOrDefault();

        }

        public Task UpdateTask(int assignedTo, int todoId, string task)
        {

            return _sql.SaveData<dynamic>("dbo.spTodo_Update",
                 new { AssignedTo = assignedTo, TdoId = todoId, Task = task },
                 "Default");

        }

        public Task CompleteTodo(int assignedTo, int todoId)
        {

            return _sql.SaveData<dynamic>("dbo.spTodo_CompletTodo",
                 new { AssignedTo = assignedTo, TdoId = todoId },
                 "Default");

        }

        public Task Delete(int assignedTo, int todoId)
        {

            return _sql.SaveData<dynamic>("dbo.spTodo_Delete",
                 new { AssignedTo = assignedTo, TdoId = todoId },
                 "Default");

        }


    }
}
