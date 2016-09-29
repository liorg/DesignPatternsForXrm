
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public static class JobUtilHelper
    {
        public const string JobProviderConnectionString = "connStrJobManager";

        static string _connectionString = "";

        static SqlConnection GetSqlConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public static void InitConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static Guid AddHistory(Guid? jobid, DateTime? startAt = null)
        {
            using (var connection = GetSqlConnection(GetConectionString()))
            {
                var command = new SqlCommand(@"dbo.GS_InsertJobHistory", connection);
                command.Parameters.AddWithValue("@jobid", jobid);
                command.Parameters.AddWithValue("@StartedAt", startAt);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var id = (Guid)command.ExecuteScalar();
                return id;
            }
        }

        static string GetConectionString()
        {
            return !String.IsNullOrEmpty(_connectionString) ? _connectionString : ConfigurationManager.AppSettings[JobProviderConnectionString].ToString();
        }

        public static void UpdateJobHistory(RunningJob runningJob, bool? isGetDataComplete = null)
        {
            using (var connection = GetSqlConnection(GetConectionString()))
            {
                var command = new SqlCommand(@"dbo.GS_UpdateJobHistory", connection);
                command.Parameters.AddWithValue("@id", runningJob.ID);
                command.Parameters.AddWithValue("@total", runningJob.Total);
                command.Parameters.AddWithValue("@noupdate", runningJob.NoUpdate);
                command.Parameters.AddWithValue("@update", runningJob.Update);
                command.Parameters.AddWithValue("@insert", runningJob.Insert);
                command.Parameters.AddWithValue("@success", runningJob.Success);
                command.Parameters.AddWithValue("@failed", runningJob.Failed);
                command.Parameters.AddWithValue("@IsGetDataComplete", isGetDataComplete);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        public static string GetFullJobNameByCofigurationJob(CofigurationJob cofigurationJob)
        {
            return cofigurationJob.JobName + cofigurationJob.Version;
        }

        public static DateTime? GetLastJob(CofigurationJob configJob)
        {
            DateTime? getlastDate = null;
            DateTime lastDate = DateTime.Now;
            int page, total = 0;
            Guid jobid = Guid.Empty;

            using (var connection = GetSqlConnection(GetConectionString()))
            {
                SqlCommand command = new SqlCommand(@"dbo.GS_GetLastJobDate", connection);
                command.Parameters.AddWithValue("@JobName", GetFullJobNameByCofigurationJob(configJob));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var drOutput = command.ExecuteReader();
                ;
                while (drOutput.Read())
                {
                    DateTime.TryParse(drOutput["d"].ToString(), out lastDate);
                    getlastDate = lastDate;
                }
            }
            return null;
        }

        //public static DateTime? GetLastJobDate(CofigurationJob configJob)
        //{
        //    DateTime lastDate = DateTime.Now;
        //    bool succeeded = false;

        //    using (var connection = GetSqlConnection(GetConectionString()))
        //    {
        //        SqlCommand command = new SqlCommand(@"dbo.GS_GetLastJobDate", connection);
        //        command.Parameters.AddWithValue("@JobName", GetFullJobNameByCofigurationJob(configJob));
        //        // command.Parameters.AddWithValue("@MaxRetries", commandJobHandler.CofigurationJob.MaxRetries);
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        connection.Open();
        //        var drOutput = command.ExecuteReader();

        //        while (drOutput.Read())
        //        {
        //            if (DateTime.TryParse(drOutput["StartedAt"].ToString(), out lastDate))
        //                succeeded = true;
        //        }
        //    }
        //    if (succeeded)
        //        return (DateTime?)lastDate;
        //    else
        //        return null;
        //}

        public static Guid? GetJobIdByJobName(CofigurationJob cofigurationJob)
        {
            Guid? jobid = null;

            using (var connection = new SqlConnection(GetConectionString()))
            {
                //SqlCommand command = new SqlCommand(@"dbo.GS_GetJobIdByJobName", connection);
                SqlCommand command = new SqlCommand(@"dbo.GS_GetJobIdByName", connection);

                command.Parameters.AddWithValue("@jobName", GetFullJobNameByCofigurationJob(cofigurationJob));
                command.Parameters.AddWithValue("@MaxRetries", cofigurationJob.MaxRetries);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var returnJobid = command.ExecuteScalar();
                if (returnJobid is Guid)
                {
                    jobid = (Guid)returnJobid;
                }
            }
            return jobid;
        }

        public static int GetMaxRetries(Guid jobId)
        {
            int maxRetrys = -1;
            using (var connection = new SqlConnection(GetConectionString()))
            {
                SqlCommand command = new SqlCommand(@"dbo.GS_GetJobMaxRetries", connection);
                command.Parameters.AddWithValue("@JobId", jobId);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                maxRetrys = Convert.ToInt32(command.ExecuteScalar());
            }
            return maxRetrys;
        }

        public static void UpdateStatus(Guid recordid, int status, CofigurationJob cofigurationJob)
        {
            using (var connection = GetSqlConnection(GetConectionString()))
            {
                var command = new SqlCommand(@"dbo.GS_UpdateRecord", connection);
                command.Parameters.AddWithValue("@recordid", recordid);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@isDelSuccess", cofigurationJob.IsDeleteSuccessRows);
                // command.Parameters.AddWithValue("@retry", record.Retry);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateRow(Guid recordid, string xmlObject, int retry, int status, CofigurationJob cofigurationJob,
            string action = "")
        {
            using (var connection = GetSqlConnection(GetConectionString()))
            {
                var command = new SqlCommand(@"dbo.GS_UpdateRecord", connection);
                command.Parameters.AddWithValue("@recordid", recordid);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@retry", retry);
                command.Parameters.AddWithValue("@ModelXml", xmlObject);
                command.Parameters.AddWithValue("@isDelSuccess", cofigurationJob.IsDeleteSuccessRows);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static Guid InsertRecordJob(string xmlObject, string fullname,
            CofigurationJob configJob, RunningJob runnerjob)
        {
            using (var connection = GetSqlConnection(GetConectionString()))
            {
                var command = new SqlCommand(@"dbo.GS_InsertRecordJob", connection);
                command.Parameters.AddWithValue("@jobName", JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));
                command.Parameters.AddWithValue("@MaxRetries", configJob.MaxRetries);
                command.Parameters.AddWithValue("@ModelXml", xmlObject);
                command.Parameters.AddWithValue("@ModelTypeXml", fullname);
                command.Parameters.AddWithValue("@historyId", runnerjob.ID);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var jobid = (Guid)command.ExecuteScalar();
                return jobid;
                //if (runningJob.JobId == null)
                //    runningJob.JobId = (Guid)command.ExecuteScalar();
                //else
                //    command.ExecuteNonQuery();
            }
        }
    }
}
