using Common.Models;
using DAL.Interfaces;
using DAL.Models;
using Newtonsoft.Json;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class DBSeederService : IDBSeederService
    {
        public DBSeederService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        IUnitOfWork _unitOfWork;
        public void JsonSeed(string jsonData)
        {
           if (!_unitOfWork.Stations.GetAll().Any())
           {                
              var stations = JsonConvert.DeserializeObject<List<StationDB>>(jsonData);
              _unitOfWork.Stations.AddRange(stations);
              _unitOfWork.Commit();
           }
        }
    }
}
