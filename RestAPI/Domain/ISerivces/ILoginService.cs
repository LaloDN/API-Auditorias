using RestAPI.Domain.Models;
using RestAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.ISerivces
{
    public interface ILoginService
    {
        Task<Usuario> ValidateUser(LoginDTO usuario);

    }
}
