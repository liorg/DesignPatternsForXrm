using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using System.Configuration;

using System.Data.SqlClient;
using System.ServiceModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xrm.Sdk.Client;
using System.Diagnostics;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;

using Lior.Xrm.JobsProvider.Errors;

namespace Lior.Xrm.JobsProvider.DataModel
{
    /// <summary>
    /// version 2.0.0.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JobsProvider<T> : IJobProvider<T> where T : JobRecordBase
    {
        #region Field
        enum StatusRecord { Ready = 1, OnProgress = 2, Finish = 3, Failed = 4 }
        // public const string JobProviderConnectionString = "connStrJobManager";
        ICommandJobBase<T> commandJobHandler;
        Action<T, string, EventLogEntryType> log;
        string connectionString;
        IOrganizationService service;
        RunningJob runningJob;
        List<ErrorMessage> errMangerList;
        IProccessErrorHandler<T> _errorHandler;
        #endregion

        #region Ctor

        public JobsProvider(ICommandJobBase<T> commandJob, IProccessErrorHandler<T> errorHandler, IOrganizationService crmService)
            : this(commandJob, errorHandler, null, crmService)
        {
        }

        public JobsProvider(ICommandJobBase<T> commandJob, IProccessErrorHandler<T> errorHandler, string conn)
            : this(commandJob, errorHandler, conn, null)
        {

        }

        public JobsProvider(ICommandJobBase<T> commandJob, IProccessErrorHandler<T> errorHandler, string conn, IOrganizationService crmService)
        {
            if (String.IsNullOrEmpty(conn))
                connectionString = ConfigurationManager.AppSettings[JobUtilHelper.JobProviderConnectionString];
            else
                connectionString = conn;

            JobUtilHelper.InitConnectionString(connectionString);

            if (crmService == null)
                LoadCrmService();
            else
                service = crmService;

            commandJobHandler = commandJob;
            _errorHandler = errorHandler;
            commandJobHandler.JobProvider = this;
            runningJob = new RunningJob();
        }

        public JobsProvider(ICommandJobBase<T> commandJob, IProccessErrorHandler<T> errorHandler)
            : this(commandJob, errorHandler, System.Configuration.ConfigurationManager.AppSettings[JobUtilHelper.JobProviderConnectionString])
        {
        }

        #endregion

        #region Run
        public void Run()
        {
            try
            {
                _errorHandler.StartWritingErrors(commandJobHandler);
                MonitorRunningBegin();
                InsertToSql();

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
        private void LoadCrmService()
        {
            if (service == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyCRMServer"].ConnectionString;
                CrmConnection connection = CrmConnection.Parse(connectionString);
                service = new OrganizationService(connection);
            }
        }

        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }

        #endregion

        #region Monitor

        void MonitorRunningBegin()
        {
            var configJob = commandJobHandler.CofigurationJob;
            runningJob.JobId = GetJobIdByJobName();
            if (runningJob.JobId == null)
                throw new ArgumentNullException("there is no any jobid for " + JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));

            runningJob.BeginRun = DateTime.Now;
            var id = JobUtilHelper.AddHistory(runningJob.JobId, runningJob.BeginRun);
            runningJob.ID = id;
        }

        void MonitorCompleteGetData()
        {
            if (runningJob.ID == Guid.Empty)
                return;
            JobUtilHelper.UpdateJobHistory(runningJob, true);
            runningJob.EndRun = DateTime.Now;
            Console.WriteLine("runningJob.MonitorCompleteGetData= " + runningJob.Insert);
        }

        void MonitorRunningEnd()
        {
            if (runningJob.ID == Guid.Empty)
                return;
            JobUtilHelper.UpdateJobHistory(runningJob);
            runningJob.EndRun = DateTime.Now;
            Console.WriteLine("runningJob.end= " + runningJob.Insert);
        }

        #endregion

        #region Set And Get Bussiness Rows

        #region insert to sql

        void InsertToSql()
        {
            if(commandJobHandler is IFetchFilterXmlObjects)
            {
                IFetchFilterXmlObjects fetchFilter = (IFetchFilterXmlObjects)commandJobHandler;
                var queue = fetchFilter.FetchFilterXmlObjects;
                var fetchFilterObjectXml = "";
                do
                {
                    fetchFilterObjectXml = queue.Dequeue();
                    InsertToSqlChunkData(fetchFilterObjectXml);
                }
                while (queue.Count > 0);

            }
            else if (commandJobHandler is IChunkData)
                InsertToSqlChunkData();
            else
                InsertToSqlAll();

            if (commandJobHandler is IPostGetJob)
                ((IPostGetJob)commandJobHandler).PostGet();

            MonitorCompleteGetData();
        }

        void InsertToSqlAll()
        {
            Console.WriteLine("InsertToSql start ");
            ICommandJob<T> JobHandler = (ICommandJob<T>)commandJobHandler;//.Get(_errorHandler.WriteLog);
            var jobs = JobHandler.Get(_errorHandler.WriteLog);
            Console.WriteLine("records " + (jobs == null ? "null" : jobs.Count().ToString()));
            var configJob = commandJobHandler.CofigurationJob;
            if (jobs == null || !jobs.Any())
            {
                runningJob.JobId = GetJobIdByJobName();
                if (runningJob.JobId == null)
                    throw new ArgumentNullException("there is no any jobid for " + JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));
            }
            else
            {
                //Insert jobs, and get job id.
                foreach (var job in jobs)
                {
                    Console.WriteLine("Insert record: " +job.CurrentStep.ToString() );
                    InsertRecordJob(job, configJob, runningJob);
                }
            }
            Console.WriteLine("InsertToSql end ");
        }

        void InsertToSqlChunkData(string fetchFilterXmlObject="")
        {
            var jobHandler = ((IFetchFilterXmlObjects<T>)commandJobHandler);
            Console.WriteLine("InsertToSqlChunkData start ");
            ChunkData chunkdata = new ChunkData();
            chunkdata.Limit = jobHandler.Limit;
            chunkdata.Page = jobHandler.Page;
            chunkdata.FromDate = jobHandler.FromDate;
            var maxdate = jobHandler.MaxDate ?? DateTime.Now;
            chunkdata.FetchFilterXmlObject = fetchFilterXmlObject;
            chunkdata.ToDate = jobHandler.Next(chunkdata.FromDate);

            while (chunkdata.FromDate < maxdate)
            {
                bool hasrows = true;
                do
                {
                    var jobs = jobHandler.Get(_errorHandler.WriteLog, chunkdata);
                    Console.WriteLine("records " + (jobs == null ? "null" : jobs.Count().ToString()));
                    var configJob = commandJobHandler.CofigurationJob;
                    if (jobs == null || !jobs.Any())
                    {
                        runningJob.JobId = GetJobIdByJobName();
                        if (runningJob.JobId == null)
                            throw new ArgumentNullException("there is no any jobid for " + JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));
                        //return;
                        hasrows = false;
                        chunkdata.JobId = runningJob.JobId.Value;
                    }
                    else
                    {
                        runningJob.JobId = runningJob.JobId.HasValue ? runningJob.JobId.Value : GetJobIdByJobName();
                        if (runningJob.JobId == null)
                            throw new ArgumentNullException("there is no any jobid!!! for " + JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));

                        chunkdata.JobId = runningJob.JobId.Value;
                        //Insert jobs, and get job id.
                        hasrows = jobs.Any();
                        foreach (var job in jobs)
                        {
                            Console.WriteLine("Insert record: " + job.CurrentStep.ToString());
                            InsertRecordJob(job, configJob, runningJob);
                        }
                    }
                    if (!hasrows)
                        chunkdata.Page = 1;
                    else
                        chunkdata.Page += 1;

                } while (hasrows);

                Console.WriteLine("InsertToSqlChunkData end ");
                chunkdata.FromDate = chunkdata.ToDate;
                chunkdata.ToDate = jobHandler.Next(chunkdata.FromDate);

            }
        }
        #endregion

        public void SetRecord(T job)
        {
            try
            {
                _errorHandler.StartWritingErrors(commandJobHandler);
                var configJob = commandJobHandler.CofigurationJob;
                runningJob.JobId = GetJobIdByJobName();

                if (runningJob.JobId == null)
                    throw new ArgumentNullException("there is no any jobid for " + JobUtilHelper.GetFullJobNameByCofigurationJob(configJob));

                InsertRecordJob(job, configJob, runningJob);
                runningJob.Insert = 1;
                runningJob.Success = 1;
                runningJob.Total = 1;
                runningJob.Desc = SerializeToXml(job);

                _errorHandler.FinishWritingErrors(runningJob);
            }
            catch (Exception ex)
            {
                _errorHandler.WriteLog("JobProvider=>SetRecord()", ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void InsertRecordJob(T job, CofigurationJob configJob, RunningJob runnerjob)
        {
            var xmlObject = SerializeToXml(job);
            var fullname = typeof(T).FullName;
            var jobid = JobUtilHelper.InsertRecordJob(xmlObject, fullname, commandJobHandler.CofigurationJob, runnerjob);
            if (runningJob.JobId == null)
                runningJob.JobId = jobid;
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
            return JobUtilHelper.GetLastJobDate(commandJobHandler.CofigurationJob);
        }

        private Guid? GetJobIdByJobName()
        {
            Guid? jobid = JobUtilHelper.GetJobIdByJobName(commandJobHandler.CofigurationJob);
            return jobid;
        }

        private int GetMaxRetries(Guid jobId)
        {
            int maxRetrys = JobUtilHelper.GetMaxRetries(jobId);
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
            JobUtilHelper.UpdateStatus(record.RecordId, status, commandJobHandler.CofigurationJob);
        }

        void UpdateRow(RecordJob<T> record, int status, string action = "")
        {
            record.Retry += 1;
            var xmlObject = SerializeToXml(record.JobRecord);
            JobUtilHelper.UpdateRow(record.RecordId, xmlObject, record.Retry, status, commandJobHandler.CofigurationJob, action);
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
                if (service == null)
                {
                    LoadCrmService();
                }
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
