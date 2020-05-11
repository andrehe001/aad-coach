using System;
using AzureGameDay.Web.Models;

namespace AzureGameDay.Web.Services
{
    public interface IOverlordStrategy
    {
        Move NextMove(Guid challengerId);
    }
}