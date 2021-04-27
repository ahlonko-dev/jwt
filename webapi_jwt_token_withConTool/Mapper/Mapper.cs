using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Mapper
{
    internal static class Mapper
    {
        internal static User ToApi(this System.Data.IDataRecord dataRecord)
        {

            return new User()
            {
                Id = (int)dataRecord["id"],
                FirstName = (string)dataRecord["FirstName"],
                LastName = (string)dataRecord["LastName"],
                //Password = (string)dataRecord["Password"]
            };
        }
 


    }
}
