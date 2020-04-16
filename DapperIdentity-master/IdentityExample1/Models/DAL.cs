using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample1.Models
{
    public class DAL
    {

        private SqlConnection conn;

        public DAL(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public int CreateTask(Tasks t, int loginId)
        {
            t.UserId = loginId;
            t.Complete = 0;
            //t.DueDate = DateTime.Now;

            string addQuery = "INSERT INTO Tasks (UserId, TaskTitle, TaskDescription, DueDate, Complete) ";
            addQuery += "VALUES (@UserId, @TaskTitle, @TaskDescription, @DueDate, @Complete)";

            return conn.Execute(addQuery, t);
        }

        public IEnumerable<Tasks> GetTasksAll()
        {
            string queryString = "SELECT * FROM Tasks";
            IEnumerable<Tasks> t = conn.Query<Tasks>(queryString);

            return t;
        }

        public IEnumerable<Tasks> GetTasksByUserId(int id)
        {
            string queryString = "SELECT * FROM Tasks WHERE UserId = @id";
            return conn.Query<Tasks>(queryString, new { id = id });
        }

        public Tasks GetTasksById(int id)
        {
            string queryString = "SELECT * FROM Tasks WHERE Id = @val";
            Tasks t = conn.QueryFirstOrDefault<Tasks>(queryString, new { val = id });

            return t;
        }

        public int DeleteTasksById(int id)
        {
            string deleteString = "DELETE FROM Tasks WHERE Id = @val";
            return conn.Execute(deleteString, new { val = id });
        }

        public int UpdateTasksById(Tasks t)
        {
            string editString = "UPDATE Tasks SET TaskTitle = @TaskTitle, TaskDescription = @TaskDescription, ";
            editString += "DueDate = @DueDate, Complete = @Complete WHERE Id = @Id AND UserID = @UserID";
            return conn.Execute(editString, t);
        }
    }
}
