using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public interface IModel
    {
        /// <summary>
        /// Gets or the value of the column _id
        /// </summary>
        int Id { get; }
    }
}