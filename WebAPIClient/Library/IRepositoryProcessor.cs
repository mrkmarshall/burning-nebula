using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Library
{
    public interface IRepositoryProcessor
    {
        public Task<List<Repository>> ProcessRepositories();
    }

}