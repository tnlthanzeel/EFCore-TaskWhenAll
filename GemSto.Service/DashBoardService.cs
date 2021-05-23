using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class DashBoardService : IDashBoardService
    {
        private readonly GemStoContext gemStoContext;

        public DashBoardService(GemStoContext gemStoContext)
        {
            this.gemStoContext = gemStoContext;
        }

        public async Task<DashBoardData> GetDashBoardDataAsync()
        {
            var dashBoardData = new DashBoardData();
            var connecionString = gemStoContext.Database.GetDbConnection().ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connecionString))
                {
                    SqlCommand command = new SqlCommand("[dbo].[spGetDashBoardData]", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add("@inStock", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@certification", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@exported", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@approval", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@thirdPartyCertification", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@thirdPartyExported", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@totalStockValue", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@totalUnpaidAndRemainingPartialPayment", SqlDbType.Decimal).Direction = ParameterDirection.Output;


                    SqlParameter totalStockValue = command.Parameters["@totalStockValue"];
                    totalStockValue.Precision = 18;
                    totalStockValue.Scale = 2;

                    SqlParameter totalUnpaidAndRemainingPartialPayment = command.Parameters["@totalUnpaidAndRemainingPartialPayment"];
                    totalUnpaidAndRemainingPartialPayment.Precision = 18;
                    totalUnpaidAndRemainingPartialPayment.Scale = 2;

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    dashBoardData.InStock = Convert.ToInt32(command.Parameters["@inStock"].Value);
                    dashBoardData.Certification = Convert.ToInt32(command.Parameters["@certification"].Value);
                    dashBoardData.Exported = Convert.ToInt32(command.Parameters["@exported"].Value);
                    dashBoardData.Approval = Convert.ToInt32(command.Parameters["@approval"].Value);
                    dashBoardData.ThirdPartyCertification = Convert.ToInt32(command.Parameters["@thirdPartyCertification"].Value);
                    dashBoardData.ThirdPartyExported = Convert.ToInt32(command.Parameters["@thirdPartyExported"].Value);
                    dashBoardData.TotalStockValue = Convert.ToDecimal(command.Parameters["@totalStockValue"].Value);
                    dashBoardData.TotalUnpaidAndRemainingPartialPayment = Convert.ToDecimal(command.Parameters["@totalUnpaidAndRemainingPartialPayment"].Value);

                    connection.Close();
                }
                return dashBoardData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
