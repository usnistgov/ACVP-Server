using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class VectorSetService : IVectorSetService
    {
        private readonly IVectorSetProvider _vectorSetProvider;
        private readonly IUserProvider _userProvider;

        public VectorSetService(IVectorSetProvider vectorSetProvider, IUserProvider userProvider)
        {
            _vectorSetProvider = vectorSetProvider;
            _userProvider = userProvider;
        }

        public VectorSet GetPrompt(long vsID)
        {
            throw new System.NotImplementedException();
        }
    }
}