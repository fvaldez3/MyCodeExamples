using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Requests.Licenses;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class LicensesService : ILicensesService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService;

        public LicensesService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService; 
        }

        public LicenseExtended Get(int id)
        {
            string procName = "[dbo].[Licenses_Select_ByIdV2]";
            LicenseExtended licenses = null;
            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    licenses = MapSingleLicenseV2(reader, ref startingIndex);
                });

            return licenses;
        }

        public Paged<LicenseExtended> GetAll(int pageIndex, int pageSize)
        {
            Paged<LicenseExtended> pagedList = null;
            List<LicenseExtended> licensesList = null;
            int totalCount = 0;
            string procName = "[dbo].[Licenses_SelectAllV2]";

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);

                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    LicenseExtended licenses2 = MapSingleLicenseV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex);

                    if (licensesList == null)
                    {
                        licensesList = new List<LicenseExtended>();
                    }
                    licensesList.Add(licenses2);
                });
            if (licensesList != null)
            {
                pagedList = new Paged<LicenseExtended>(licensesList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<LicenseExtended> GetByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<LicenseExtended> pagedList = null;
            List<LicenseExtended> licensesList = null;
            
            int totalCount = 0;
            string procName = "[dbo].[Licenses_Select_ByCreatedByV2]";

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@CreatedBy", userId);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    LicenseExtended licenses2 = MapSingleLicenseV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex);

                    if (licensesList == null)
                    {
                        licensesList = new List<LicenseExtended>();
                    }
                    licensesList.Add(licenses2);
                });
            if (licensesList != null)
            {
                pagedList = new Paged<LicenseExtended>(licensesList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<LicenseExtended> GetByLicenseTypeId(int pageIndex, int pageSize, int licenseTypeId)
        {
            Paged<LicenseExtended> pagedList = null;
            List<LicenseExtended> licensesList = null;
            int totalCount = 0;
            string procName = "[dbo].[Licenses_Select_ByLicenseTypeIdV2]";

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@LicenseTypeId", licenseTypeId);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    LicenseExtended licenses2 = MapSingleLicenseV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex);

                    if (licensesList == null)
                    {
                        licensesList = new List<LicenseExtended>();
                    }
                    licensesList.Add(licenses2);
                });
            if (licensesList != null)
            {
                pagedList = new Paged<LicenseExtended>(licensesList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public int Add(LicenseAddRequest model, int userId)
        {
            int id = 0;
            
            string procName = "[dbo].[Licenses_InsertV2]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);


                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            }, delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }

        public void Update(LicenseUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Licenses_UpdateV2]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                AddCommonParams(model, col);
                col.AddWithValue("@ValidationTypeId", model.ValidationTypeId);
                col.AddWithValue("@ValidatedBy", userId);
                col.AddWithValue("@RejectMessage", model.RejectMessage);
            }, returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Licenses_Delete_ById]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        public Paged<LicenseExtended> Search(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Licenses_Search]";
            List<LicenseExtended> list = null;
            Paged<LicenseExtended> result = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);

            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
            LicenseExtended licenseExtended = MapSingleLicenseV2(reader, ref index);

            if (totalCount == 0)
            {
                totalCount = reader.GetSafeInt32(index++);
            }
            if (list == null)
            {
                list = new List<LicenseExtended>();
            }
            list.Add(licenseExtended);
            });
            if (list != null)
            {
                result = new Paged<LicenseExtended>(list, pageIndex, pageSize, totalCount);
            }
            return result;
            }

        public Paged<LicenseExtended> GetByLicenseState(int pageIndex, int pageSize, string state)
        {
            Paged<LicenseExtended> pagedList = null;
            List<LicenseExtended> licensesList = null;
            int totalCount = 0;
            string procName = "[dbo].[Licenses_Select_ByStates]";

            _data.ExecuteCmd(procName,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@State", state);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    LicenseExtended licenses2 = MapSingleLicenseV2(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex);

                    if (licensesList == null)
                    {
                        licensesList = new List<LicenseExtended>();
                    }
                    licensesList.Add(licenses2);
                });

            if (licensesList != null)
            {
                pagedList = new Paged<LicenseExtended>(licensesList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public License MapSingleLicense(IDataReader reader, ref int startingIndex)
        {
            License license = new License();
            license.Id = reader.GetSafeInt32(startingIndex++);
            license.StateType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            license.LicenseType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            license.LicenseNumber = reader.GetSafeString(startingIndex++);
            license.DateExpires = reader.GetSafeDateTime(startingIndex++);
            license.CreatedBy = reader.GetSafeInt32(startingIndex++);
            license.DateCreated = reader.GetSafeDateTime(startingIndex++);

            return license;
        }

        public LicenseExtended MapSingleLicenseV2(IDataReader reader, ref int startingIndex)
        {
            LicenseExtended license = new LicenseExtended();
            license.Id = reader.GetSafeInt32(startingIndex++);
            license.StateType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            license.LicenseType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            license.LicenseNumber = reader.GetSafeString(startingIndex++);
            license.ValidationType = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            license.ValidatedBy = reader.GetSafeInt32(startingIndex++);
            license.RejectMessage = reader.GetSafeString(startingIndex++);
            license.DateExpires = reader.GetSafeDateTime(startingIndex++);
            license.CreatedBy = reader.GetSafeInt32(startingIndex++);
            license.FirstName = reader.GetSafeString(startingIndex++);
            license.LastName = reader.GetSafeString(startingIndex++);
            license.DateCreated = reader.GetSafeDateTime(startingIndex++);
            license.FileUrl = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            return license;
        }

        private static void AddCommonParams(LicenseAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@LicenseStateId", model.LicenseStateId);
            col.AddWithValue("@LicenseTypeId", model.LicenseTypeId);
            col.AddWithValue("@LicenseNumber", model.LicenseNumber);
            col.AddWithValue("@DateExpires", model.DateExpires);
            col.AddWithValue("@FileId", model.FileId);
        }
    }
}
