using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Core.Entities;

namespace Travel.Core.Interfaces.IRepositories
{
    public interface ITourDestinationRepository
    {
        Task Create(List<TourDestination> toursDestination);
    }
}
