using Foodies.ViewModels;

namespace Foodies.Interfaces.Services
{
    public interface IBranchManagerService
    {
        public Task<BranchManager> CreateBranchManager(AddbranchViewmodel viewModel, Admin admin, Branch branch);

    }
}