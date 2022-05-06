using System;
using System.Collections.ObjectModel;

namespace UtilityLibraries
{

    public interface IRepositoryProcessor
    {
        public ReadOnlyCollection<Repository> ProcessRepositories();
    }

}