using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RAuth.Application.Constants;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.Repository;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.AddressModel;
using RAuth.Core.Models.User;
using System.Security.Claims;

namespace RAuth.Application.RTC
{
    [Authorize]
    public class RtcHub(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private static Dictionary<string, RAuthTransRequest> _pendingRequests = [];
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            if (userId != null)
            {
                _pendingRequests.Remove(userId, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }
        
        public async Task RequestAccess(string username)
        {
            var requesterId = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            var existingUser = await _userManager.FindByNameAsync(username) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            RAuthTransRequest request = new();
            request.RequesterId = requesterId;
            request.ReceiverId = existingUser.Id;
            _pendingRequests[requesterId] = request;
            await Clients.User(existingUser.Id).SendAsync("ReceiveMessage", "New Authorization Request");
        }

        public async Task RespondToRequest(string userId, bool isApproved)
        {
            var request = _pendingRequests.Values.FirstOrDefault(r => r.ReceiverId == userId) ?? throw new NotFoundException("No Requests Found");
            _pendingRequests.Remove(request.RequesterId);
            if(isApproved)
            {
                var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
                GetRAuthUserDTO rAuthUserDTO = new();
                rAuthUserDTO.UserName = user.UserName!;
                GetRAuthUserResponseDTO ResponseData = await _unitOfWork.RAuthRepository.GetRAuthUserAsync(rAuthUserDTO);
                await Clients.User(request.RequesterId).SendAsync("ReceiveUserData", ResponseData);
            }
            else
            {
                await Clients.User(request.RequesterId).SendAsync("RequestDenied", "Access denied by user.");
            }
        }
    }
}
