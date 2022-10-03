using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Location;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class LocationService : ILocationService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;

        public LocationService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public Location GetById(int id)
        {
            string procName = "[dbo].[Locations_SelectById]";
            Location location = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    location = MapSingleLocation(reader, ref index);
                });

            return location;
        }

        public Paged<Location> GetAllPaginated(int pageIndex, int pageSize)
        {
            Paged<Location> pagedResult = null;
            List<Location> results = null;
            int totalCount = 0;
            string procName = "[dbo].[Locations_SelectAll]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Location aLocation = MapSingleLocation(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (results == null)
                    {
                        results = new List<Location>();
                    }

                    results.Add(aLocation);
                });

            if (results != null)
            {
                pagedResult = new Paged<Location>(results, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<Location> GetByCreatedByPaginated(int pageIndex, int pageSize, int userId)
        {
            Paged<Location> pagedResult = null;
            List<Location> results = null;
            int totalCount = 0;
            string procName = "[dbo].[Locations_SelectByCreatedBy]";

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@CreatedBy", userId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Location aLocation = MapSingleLocation(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (results == null)
                    {
                        results = new List<Location>();
                    }

                    results.Add(aLocation);
                });

            if (results != null)
            {
                pagedResult = new Paged<Location>(results, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public List<Location> GetByGeo(int radius, double latitude, double longitude)
        {
            string procName = "[dbo].[Locations_SelectByGeo]";
            List<Location> results = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Radius", radius);
                    col.AddWithValue("@Latitude", latitude);
                    col.AddWithValue("@Longitude", longitude);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;

                    Location location = MapSingleLocation(reader, ref index);

                    if (results == null)
                    {
                        results = new List<Location>();
                    }

                    results.Add(location);
                });

            return results;
        }

        public int Create(LocationAddRequest model, int userId)
        {
            string procName = "[dbo].[Locations_Insert]";
            int id = 0;

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(col, model, userId);
                    SqlParameter outputId = new SqlParameter("@Id", SqlDbType.Int);
                    outputId.Direction = ParameterDirection.Output;
                    col.Add(outputId);
                }, returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object idOut = returnCol["@Id"].Value;
                    int.TryParse(idOut.ToString(), out id);
                });

            return id;
        }

        public void Update(LocationUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Locations_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(col, model, userId);
                });
        }

        public void DeleteById(int id)
        {
            string procName = "[dbo].[Locations_DeleteById]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                });
        }

        public Location MapSingleLocation(IDataReader reader, ref int startIndex)
        {
            Location location = new Location();

            location.Id = reader.GetSafeInt32(startIndex++);
            location.LocationType = _lookUpService.MapSingleLookUp(reader, ref startIndex);
            location.LineOne = reader.GetSafeString(startIndex++);
            location.LineTwo = reader.GetSafeString(startIndex++);
            location.City = reader.GetSafeString(startIndex++);
            location.Zip = reader.GetSafeString(startIndex++);
            location.State = _lookUpService.MapSingleState(reader, ref startIndex);
            location.Latitude = reader.GetSafeDouble(startIndex++);
            location.Longitude = reader.GetSafeDouble(startIndex++);

            return location;
        }

        private static void AddCommonParams(SqlParameterCollection col, LocationAddRequest model, int userId)
        {
            col.AddWithValue("@LocationTypeId", model.LocationTypeId);
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@LineTwo", model.LineTwo);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@Zip", model.Zip);
            col.AddWithValue("@StateId", model.StateId);
            col.AddWithValue("@Latitude", model.Latitude);
            col.AddWithValue("@Longitude", model.Longitude);
            col.AddWithValue("@UserId", userId);
        }
    }
}