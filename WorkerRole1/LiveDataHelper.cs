using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
//using CommonUtils;

namespace DataType.Live
{
    /// <summary>
    /// </summary>
    /// <devdoc>
    /// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
    /// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
    /// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
    /// PARTICULAR PURPOSE.
    /// </devdoc>
    /// <author name="George Huey" />
    /// 
    public class LiveDataDataHelper
    {
        /*public static List<LiveData> GetUnsentLiveData(string sqlConnectionString)
        {
            List<LiveData> dataList = new List<LiveData>();

            Retry.ExecuteRetryAction(() =>
            {
                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter ("@timestamp", SqlDbType.DateTime2),
                    };
                    parameters[0].Value = DateTime.UtcNow.ToString();

                    con.Open();
                    SqlDataReader sdr = SqlHelper.ExecuteReader(con, CommandType.StoredProcedure, "dbo.GetUnsentLiveData", parameters);
                    while (sdr.Read())
                    {
                       // LiveData rd = new LiveData();
                        rd.TagId = sdr.GetInt32(0);
                        rd.MachineSection = sdr.GetString(1);
                        rd.Description = sdr.GetString(2);
                        rd.TagName = sdr.GetString(3);
                        rd.TagValue = sdr.GetFloat(4);
                        rd.TagStatus = sdr.GetInt32(5);
                        rd.TimeStamp = sdr.GetDateTime(6);
                        rd.Machine = sdr.GetString(7);
                        dataList.Add(rd);
                    }
                    sdr.Close();
                }
            });
            return dataList;
        }

        public static void UploadLiveData(string sqlConnectionString, ref CBH header, List<LiveData> dataList)
        {
            if (dataList == null || dataList.Count < 1) return;

            DataTable dt = new DataTable();
            dt.Columns.Add("TagId", typeof(int));
            dt.Columns.Add("Message", typeof(string));
            

            // Add rows
            /*foreach (LiveData ld in dataList)
            {
                dt.Rows.Add(ld.TagId, ld.Message, ld.Description, ld.TagName, ld.TagValue, ld.TagStatus, ld.TimeStamp, ld.Machine);
            }

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    // Create a command and bind parameter
                    SqlCommand cmd = new SqlCommand("dbo.InsertLiveDataRecords", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CommonFunc.GetSqlParameter("@CustomerId", header.CustomerId, SqlDbType.Int));
                    cmd.Parameters.Add(CommonFunc.GetSqlParameter("@SiteId", header.SiteId, SqlDbType.Int));
                    cmd.Parameters.Add(CommonFunc.GetSqlParameter("@VSEId", header.VSEId, SqlDbType.Int));
                    SqlParameter param = cmd.Parameters.Add(CommonFunc.GetSqlParameter("@dataList", dt, SqlDbType.Structured));
                    param.TypeName = "LiveDataTableType";

                    // Execute command
                    Retry.ExecuteRetryAction(() =>
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }, () =>
                    {
                        if (con.State != ConnectionState.Closed)
                        {
                            con.Close();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw (ex);
            }
        }*/
    }
}


