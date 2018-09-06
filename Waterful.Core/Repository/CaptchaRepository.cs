using System;
using System.Collections.Generic;
using System.Text;
using Waterful.Core.Models;

namespace Waterful.Core.Repository
{
    public interface ICaptchaRepository : IRepository<Captcha>
    {

    }

    public class CaptchaRepository : RepositoryBase<Captcha>, ICaptchaRepository
    {

        public CaptchaRepository(PomeloMySqlDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}

