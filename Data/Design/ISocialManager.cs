using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// Stijn
/// </summary>
namespace Bistronger.Data.Design
{
    public interface ISocialManager
    {
        DataSet<Social> GetSocials(int businessID);
        Social GetSocial(int id);
        Social CreateSocial(Social social);
        Social GetSocialForBusiness(int businessID);
        Social EditSocial(Social social);
        void DeleteSocial(int id);
    }
}
