using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace UtilityLibraries
{

    public interface IRepositoryProcessor
    {
        public ICollection<Repository> ProcessRepositories();
    }

}