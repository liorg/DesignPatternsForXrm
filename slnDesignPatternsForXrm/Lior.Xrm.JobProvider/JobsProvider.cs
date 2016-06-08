using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.JobsProvider.DataModel;
using Lior.Xrm.JobsProvider.Errors;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Data.SqlClient;
using System.ServiceModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xrm.Sdk.Client;
using System.Diagnostics;


namespace Lior.Xrm.JobsProvider
{
    public class JobsProvider<T> : IJobProvider<T> where T : JobRecordBase
    {
        #region Field
        enum StatusRecord { Ready = 1, OnProgress = 2, Finish = 3, Failed = 4 }
        public const string JobProviderConnectionString = "connStrJobManager";
        ICommandJob<T> commandJobHandler;
        Action<T, string, EventLogEntryType> log;
        string connectionString;
        IOrganizationService service;
        RunningJob runningJob;
        List<ErrorMessage> errMangerList;
        IProccessErrorHandler<T> _errorHandler;
        #endregion

        #region Ctor

        public JobsProvider(ICommandJob<T> commandJob, IProccessErrorHandler<T> errorHandler, IOrganizationService crmService)
            : this(commandJob, errorHandler, null, crmService)
        {
        }

        public JobsProvider(ICommandJob<T> commandJob, IProccessErrorHandler<T> errorHandler, string conn, IOrganizationService crmService)
        {
            if (String.IsNullOrEmpty(conn))
                connectionString = ConfigurationManager.AppSettings[JobProviderConnectionString];
            else
                connectionString = conn;

            //if (crmService == null)
            //    LoadCrmService();
            //else
                service = crmService;

            commandJobHandler = commandJob;
            _errorHandler = errorHandler;
            commandJobHandler.JobProvider = this;
            runningJob = new RunningJob();
        }

       

        #endregion

        #region Run
        public void Run()
        {
            try
            {
                _errorHandler.StartWritingErrors(commandJobHandler);
                InsertToSql();
                MonitorRunningBegin();
                ExcuteRecords();
                PostExecute();
                _errorHandler.FinishWritingErrors(runningJob);
            }
            catch (Exception ex)
            {
                _errorHandler.WriteLog("JobProvider=>Run()", ex.StackTrace, EventLogEntryType.Error);
                Console.WriteLine("Run ex " + ex.Message);
            }
            finally
            {
                MonitorRunningEnd();
            }
        }
        #endregion

        #region Connection
        //private void LoadCrmService()
        //{
        //    //if (service == null)
        //    //{
        //    //  //  SDKService serviceFactory = new SDKService();
        //    //    service = serviceFactory.GetService(false, ConfigurationManager.AppSettings["Org"]);
        //    //    if (service is OrganizationServiceProxy)
        //    //        (service as OrganizationServiceProxy).EnableProxyTypes();
        //    //}
        //}

        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }
        #endregion

        #region Monitor

        void MonitorRunningBegin()
        {
            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_InsertJobHistory", connection);
                command.Parameters.AddWithValue("@jobid", runningJob.JobId);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                runningJob.ID = (Guid)command.ExecuteScalar();
            }
            runningJob.BeginRun = DateTime.Now;
        }

        void MonitorRunningEnd()
        {
            if (runningJob.ID == Guid.Empty)
                return;

            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_UpdateJobHistory", connection);
                command.Parameters.AddWithValue("@id", runningJob.ID);
                command.Parameters.AddWithValue("@total", runningJob.Total);
                command.Parameters.AddWithValue("@noupdate", runningJob.NoUpdate);
                command.Parameters.AddWithValue("@update", runningJob.Update);
                command.Parameters.AddWithValue("@insert", runningJob.Insert);
                command.Parameters.AddWithValue("@success", runningJob.Success);
                command.Parameters.AddWithValue("@failed", runningJob.Failed);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
            runningJob.EndRun = DateTime.Now;
            Console.WriteLine("runningJob.Insert= " + runningJob.Insert);
        }

        #endregion

        #region Set And Get Bussiness Rows

        void InsertToSql()
        {
            Console.WriteLine("InsertToSql start ");
            var jobs = commandJobHandler.Get(_errorHandler.WriteLog);
            Console.WriteLine("records " + (jobs == null ? "null" : jobs.Count().ToString()));
            var configJob = commandJobHandler.CofigurationJob;
            if (jobs == null || !jobs.Any())
            {
                runningJob.JobId = GetJobIdByJobName();
                if (runningJob.JobId == null)
                    throw new ArgumentNullException("there is no any jobid for " + GetFullJobNameByCofigurationJob(configJob));
                //return;
            }
            else
            {
                //Insert jobs, and get job id.
                foreach (var job in jobs)
                {
                    Console.WriteLine("Insert record: " + (job.CurrentStep != null ? job.CurrentStep.ToString() : " "));
                    #region Code was refractored to InsertRecordJob(T job, CofigurationJob configJob)
                    //var xmlObject = SerializeToXml(job);
                    //using (var connection = GetSqlConnection())
                    //{

                    //    var command = new SqlCommand(@"dbo.GS_InsertRecordJob", connection);
                    //    command.Parameters.AddWithValue("@jobName", GetFullJobNameByCofigurationJob(configJob));
                    //    command.Parameters.AddWithValue("@ModelXml", xmlObject);
                    //    command.Parameters.AddWithValue("@ModelTypeXml", typeof(T).FullName);
                    //    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //    connection.Open();

                    //    if (runningJob.JobId == null)
                    //        runningJob.JobId = (Guid)command.ExecuteScalar();
                    //    else
                    //        command.ExecuteNonQuery();
                    //} 
                    #endregion
                    InsertRecordJob(job, configJob);
                }
            }
            Console.WriteLine("InsertToSql end ");
            if (commandJobHandler is IPostGetJob)
                ((IPostGetJob)commandJobHandler).PostGet();
            //if (jobs != null && jobs.Any() && commandJobHandler is IPostGetJob)
            //{
            //    ((IPostGetJob)commandJobHandler).PostGet();
            //}
            //if there are no jobs in the current proccess -> get job id by name
            //if (runningJob.JobId == null)
            //{
            //    using (var connection = GetSqlConnection())
            //    {
            //        var command = new SqlCommand(@"dbo.GS_GetJobIdByName", connection);
            //        command.Parameters.AddWithValue("@jobName", GetFullJobNameByCofigurationJob(configJob));
            //        if (commandJobHandler.CofigurationJob.MaxRetries.HasValue)
            //            command.Parameters.AddWithValue("@MaxRetries", commandJobHandler.CofigurationJob.MaxRetries);
            //        command.CommandType = System.Data.CommandType.StoredProcedure;
            //        connection.Open();

            //        if (runningJob.JobId == null)
            //            runningJob.JobId = (Guid)command.ExecuteScalar();
            //    }
            //}
        }

        public void SetRecord(T job)
        {
           

            try
            {
                _errorHandler.StartWritingErrors(commandJobHandler);
                var configJob = commandJobHandler.CofigurationJob;
                runningJob.JobId = GetJobIdByJobName();
                if (runningJob.JobId == null)
                    throw new ArgumentNullException("there is no any jobid for " + GetFullJobNameByCofigurationJob(configJob));

                InsertRecordJob(job, configJob);
                _errorHandler.FinishWritingErrors(runningJob);
            }
            catch (Exception ex)
            {
                _errorHandler.WriteLog("JobProvider=>Run()", ex.StackTrace, EventLogEntryType.Error);
                Console.WriteLine("Run ex " + ex.Message);
            }
        }

        private void InsertRecordJob(T job, CofigurationJob configJob)
        {
            var xmlObject = SerializeToXml(job);
            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_InsertRecordJob", connection);
                command.Parameters.AddWithValue("@jobName", GetFullJobNameByCofigurationJob(configJob));
                command.Parameters.AddWithValue("@ModelXml", xmlObject);
                command.Parameters.AddWithValue("@ModelTypeXml", typeof(T).FullName);
                command.Parameters.AddWithValue("@MaxRetries", configJob.MaxRetries);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();

                if (runningJob.JobId == null)
                    runningJob.JobId = (Guid)command.ExecuteScalar();
                else
                    command.ExecuteNonQuery();
            }
        }


        public IEnumerable<RecordJob<T>> GetRecordsById()
        {
            Console.WriteLine("GetRecordsById start");
            var list = new System.Collections.Generic.List<RecordJob<T>>();

            using (var connection = GetSqlConnection())
            {
                Console.WriteLine("GetRecordsById after get connection");
                var command = new SqlCommand(@"dbo.GS_getRecordsByJobId", connection);
                command.Parameters.AddWithValue("@JobId", runningJob.JobId);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                Console.WriteLine("GetRecordsById after open connection");
                var drOutput = command.ExecuteReader();
                Console.WriteLine("GetRecordsById after exec reader");
                while (drOutput.Read())
                {
                    var recordJob = new RecordJob<T>();
                    var xml = (drOutput["ModelXml"] != Convert.DBNull) ? drOutput["ModelXml"].ToString() : null;
                    recordJob.JobId = (drOutput["JobId"] != Convert.DBNull) ? Guid.Parse(drOutput["JobId"].ToString()) : Guid.Empty;
                    recordJob.RecordId = (drOutput["ID"] != Convert.DBNull) ? Guid.Parse(drOutput["ID"].ToString()) : Guid.Empty;
                    recordJob.Retry = (drOutput["Retry"] != Convert.DBNull) ? int.Parse(drOutput["Retry"].ToString()) : 0;
                    recordJob.JobRecord = DeserializeFromXml(xml);
                    list.Add(recordJob);
                }

            }
            return list;
        }

        public IEnumerable<RecordJob<T>> GetFailedRecords()
        {
            var list = new System.Collections.Generic.List<RecordJob<T>>();

            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_getFaildRecords", connection);
                command.Parameters.AddWithValue("@JobId", runningJob.JobId);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var drOutput = command.ExecuteReader();

                while (drOutput.Read())
                {
                    var recordJob = new RecordJob<T>();
                    var xml = (drOutput["ModelXml"] != Convert.DBNull) ? drOutput["ModelXml"].ToString() : null;
                    recordJob.JobId = (drOutput["JobId"] != Convert.DBNull) ? Guid.Parse(drOutput["JobId"].ToString()) : Guid.Empty;
                    recordJob.RecordId = (drOutput["ID"] != Convert.DBNull) ? Guid.Parse(drOutput["ID"].ToString()) : Guid.Empty;
                    recordJob.Retry = (drOutput["Retry"] != Convert.DBNull) ? int.Parse(drOutput["Retry"].ToString()) : 0;
                    recordJob.JobRecord = DeserializeFromXml(xml);
                    list.Add(recordJob);
                }
            }
            return list;
        }

        #endregion

        #region Get Job Details

        /// <summary>
        /// prevent getting some data again
        /// </summary>
        /// 
        public bool IsJobRunToday
        {
            get
            {
                DateTime? lastJobDate = GetLastJobDate();
                if (lastJobDate == null)
                    return false;
                return lastJobDate.Value.Date == DateTime.Today;
            }
        }

        public DateTime? GetLastJobDate()
        {
            DateTime lastDate = DateTime.Now;
            bool succeeded = false; var configJob = commandJobHandler.CofigurationJob;
            using (var connection = GetSqlConnection())
            {
                SqlCommand command = new SqlCommand(@"dbo.GS_GetLastJobDate", connection);
                command.Parameters.AddWithValue("@JobName", GetFullJobNameByCofigurationJob(configJob));
                // command.Parameters.AddWithValue("@MaxRetries", commandJobHandler.CofigurationJob.MaxRetries);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                var drOutput = command.ExecuteReader();

                while (drOutput.Read())
                {
                    if (DateTime.TryParse(drOutput["StartedAt"].ToString(), out lastDate))
                        succeeded = true;
                }
            }
            if (succeeded)
                return (DateTime?)lastDate;
            else
                return null;
        }

        protected string GetFullJobNameByCofigurationJob(CofigurationJob cofigurationJob)
        {
            return cofigurationJob.JobName + cofigurationJob.Version;
        }

        private Guid? GetJobIdByJobName()
        {
            Guid? jobid = null;
            CofigurationJob cofigurationJob = commandJobHandler.CofigurationJob;
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings[JobProviderConnectionString]))
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

        private int GetMaxRetries(Guid jobId)
        {
            int maxRetrys = -1;
            using (var connection = new SqlConnection(ConfigurationManager.AppSettings[JobProviderConnectionString]))
            {
                SqlCommand command = new SqlCommand(@"dbo.GS_GetJobMaxRetries", connection);
                command.Parameters.AddWithValue("@JobId", jobId);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                maxRetrys = Convert.ToInt32(command.ExecuteScalar());
            }
            return maxRetrys;
        }

        #endregion

        #region Excute Bussiness Row

        public void ExcuteXml(string xml)
        {

            var jobRecord = DeserializeFromXml(xml);
            commandJobHandler.Execute(jobRecord, _errorHandler.WriteLog);
        }

        private void ExcuteRecords()
        {
            Console.WriteLine("ExcuteRecords start ");
            if (this.RunningJob.JobId == null)
                throw new ArgumentException("Running job must contain JobId");

            var getRecordsById = GetRecordsById();
            //1	מוכן לטעינה
            //2	בתהליך
            //3	הסתיים
            //4	נכשל
            Console.WriteLine("ExcuteRecords before foreach ");
            foreach (var record in getRecordsById)
            {
                Console.WriteLine("ExcuteRecords foreach : " + runningJob.Total.ToString());
                int status = 0; string action = "";
                try
                {
                    runningJob.Total++;
                    UpdateOnProgressRow(record);
                    Console.WriteLine("ExcuteRecords foreach after UpdateOnProgressRow ");
                    ResultJob returnValues = commandJobHandler.Execute(record.JobRecord, _errorHandler.WriteLog);
                    action = returnValues.ToString();
                    Console.WriteLine("ExcuteRecords foreach after returnValues ");
                    if (returnValues.HasFlag(ResultJob.Insert))
                        runningJob.Insert++;
                    if (returnValues.HasFlag(ResultJob.Update))
                        runningJob.Update++;
                    if (returnValues.HasFlag(ResultJob.NoUpdate))
                        runningJob.NoUpdate++;
                    if (returnValues.HasFlag(ResultJob.Failed) || returnValues.HasFlag(ResultJob.FailedRetry))
                        runningJob.Failed++;
                    else
                        runningJob.Success++;

                    if (returnValues.HasFlag(ResultJob.FailedRetry))
                        status = (int)StatusRecord.Failed;
                    else
                        status = (int)StatusRecord.Finish;
                    Console.WriteLine("ExcuteRecords foreach finish");
                }
                catch (FaultException<OrganizationServiceFault> fex)
                {
                    if (_errorHandler != null)
                        _errorHandler.WriteLog(record.JobRecord, fex.StackTrace + "_" + fex.Message, EventLogEntryType.Error);
                    runningJob.Failed++;
                    HandleFualtException(fex, ref status);
                    Console.WriteLine("Error: " + fex.Message + Environment.NewLine + fex.InnerException);
                }
                catch (Exception e)
                {
                    if (_errorHandler != null)
                        _errorHandler.WriteLog(record.JobRecord, e.StackTrace + "_ex_" + e.ToString(), EventLogEntryType.Error);

                    runningJob.Failed++;
                    HandleUnHandeledExc(e, ref status);
                    Console.WriteLine("Error: " + e.Message + Environment.NewLine + e.InnerException);
                }
                finally
                {
                    UpdateRow(record, status, action);
                }
                Console.WriteLine("ResultJob.Insert " + ResultJob.Insert);
                Console.WriteLine("ResultJob.Update " + ResultJob.Update);

            }
            Console.WriteLine("ExcuteRecords end ");
        }

        private void PostExecute()
        {
            var failedRecords = GetFailedRecords();
            commandJobHandler.PostExecute(failedRecords, _errorHandler.WriteLog);
        }

        void UpdateOnProgressRow(RecordJob<T> record, string action = "")
        {
            int status = (int)StatusRecord.OnProgress;
            UpdateStatus(record, status);
        }

        void UpdateStatus(RecordJob<T> record, int status)
        {
            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_UpdateRecord", connection);
                command.Parameters.AddWithValue("@recordid", record.RecordId);
                command.Parameters.AddWithValue("@status", status);
                // command.Parameters.AddWithValue("@retry", record.Retry);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        void UpdateRow(RecordJob<T> record, int status, string action = "")
        {
            record.Retry += 1;
            var xmlObject = SerializeToXml(record.JobRecord);
            using (var connection = GetSqlConnection())
            {
                var command = new SqlCommand(@"dbo.GS_UpdateRecord", connection);
                command.Parameters.AddWithValue("@recordid", record.RecordId);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@retry", record.Retry);
                command.Parameters.AddWithValue("@ModelXml", xmlObject);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Handler Errors

        private void HandleFualtException(FaultException<OrganizationServiceFault> fex, ref int status)
        {

            //ErrorMessage msg = errMangerList.Where(n => n.Value == fex.Detail.Message).FirstOrDefault();
            //if (msg != null)
            //{
            //    HandleLogicExc(fex, ref status);
            //}
            //else
            //{
            //    HandleUnHandeledExc(fex, ref status);
            //}
            HandleUnHandeledExc(fex, ref status);
        }

        private void HandleLogicExc(Exception e, ref int status)
        {
            status = (int)StatusRecord.Finish;
        }

        private void HandleUnHandeledExc(Exception e, ref int status)
        {
            status = (int)StatusRecord.Failed;
        }

        #endregion

        #region Helper

        public static string SerializeToXml(T obj)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T DeserializeFromXml(string xml)
        {
            T result;
            var ser = new XmlSerializer(typeof(T));
            using (var tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }

        #endregion

        #region Implementation IJobProvider

        public IOrganizationService Service
        {
            get
            {
                //if (service == null)
                //{
                //    LoadCrmService();
                //}
                return service;
            }

        }

        public RunningJob RunningJob
        {
            get { return runningJob; }
        }
        #endregion
    }
}
