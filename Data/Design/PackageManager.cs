using Bistronger.Areas.Identity;
using Bistronger.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bistronger.Data.Design
{
    public class PackageManager : BaseManager, IPackageManager
    {
        public PackageManager(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
            : base(context, userManager, httpContextAccessor)
        {
        }

        public Package CreatePackage(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));
            _context.Packages.Add(package);
            _context.SaveChanges();
            return package;
        }

        public void DeletePackage(int id)
        {
            var package = GetPackage(id);
            if (package == null)
                throw new ArgumentNullException(nameof(id), "ID does not exists");
            _context.Entry(package).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public Package EditPackage(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));
            _context.Entry(package).State = EntityState.Modified;
            _context.SaveChanges();
            return package;
        }

        public Package GetPackage(int id)
        {
            var qry = from t in _context.Packages
                      where t.ID == id
                      select t;
            return qry.SingleOrDefault();
        }

        public DataSet<Package> GetPackages()
        {
            var dataSet = new DataSet<Package>();
            var qry = _context.Packages.Select(x => x);
            dataSet.Data = qry.ToList();
            return dataSet;
        }

        public DataSet<Package> GetPackages(PackageFilter filter)
        {
            if (filter == null)
                return GetPackages();
            else
            {
                var dataSet = new DataSet<Package>();
                var qry = _context.Packages.Select(x => x);
                if (filter.Type.HasValue)
                {
                    qry = from t in qry
                          where t.Type == filter.Type.Value
                          select t;
                }
                dataSet.Data = qry.ToList();
                return dataSet;
            }
        }

        public DataSet<Package> GetPackages(PackageType type)
        {
            var dataSet = new DataSet<Package>();
            var qry = _context.Packages.Select(x => x);
            qry = from t in qry
                  where t.Type == type
                  select t;
            dataSet.Data = qry.ToList();
            return dataSet;
        }
    }
}