using Bistronger.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Joren
/// </summary>
namespace Bistronger.Data.Design
{
    public interface IPackageManager
    {
        DataSet<Package> GetPackages();

        DataSet<Package> GetPackages(PackageFilter filter);

        DataSet<Package> GetPackages(PackageType type);

        Package GetPackage(int id);

        Package CreatePackage(Package package);

        Package EditPackage(Package package);

        void DeletePackage(int id);
    }
}