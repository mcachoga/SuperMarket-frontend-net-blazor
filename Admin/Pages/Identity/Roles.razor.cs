﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SuperMarket.Client.Extensions;
using SuperMarket.Client.Pages.Identity.Dialogs;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;

namespace SuperMarket.Client.Pages.Identity
{
    public partial class Roles
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        private List<RoleResponse> _roleList = new();
        private RoleResponse _role = new();

        private bool _canCreateRoles;
        private bool _canUpdateRoles;
        private bool _canDeleteRoles;
        private bool _canViewRoleClaims;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthState).User;
            _canCreateRoles = await AuthService.HasPermissionAsync(user, AppFeature.Roles, AppAction.Create);
            _canUpdateRoles = await AuthService.HasPermissionAsync(user, AppFeature.Roles, AppAction.Update);
            _canDeleteRoles = await AuthService.HasPermissionAsync(user, AppFeature.Roles, AppAction.Delete);
            _canViewRoleClaims = await AuthService.HasPermissionAsync(user, AppFeature.RoleClaims, AppAction.Read);

            await GetRolesAsync();
            _loaded = true;
        }

        private async Task GetRolesAsync()
        {
            var response = await _roleService.GetRolesAsync();
            if (response.IsSuccessful)
            {
                _roleList = response.ResponseData.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void ManagePermissions(string roleId)
        {
            _navigationManager.NavigateTo($"/pages/identity/role-permissions/{roleId}");
        }

        private async Task CreateRoleDialog()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<CreateRole>("Create New Role", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await GetRolesAsync();
            }
        }

        private async Task UpdateRoleDialog(string roleId)
        {
            var roleToUpdate = _roleList.First(role => role.Id == roleId);

            var parameters = new DialogParameters()
            {
                { nameof(UpdateRole.UpdateRoleRequest), new UpdateRoleRequest
                    {
                        RoleId = roleId,
                        RoleName = roleToUpdate.Name,
                        RoleDescription = roleToUpdate.Description,
                    } 
                }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<UpdateRole>("Update Role", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await GetRolesAsync();
            }
        }

        private async Task Delete(string roleId)
        {
            string deleteContent = "Are you sure you want to delete Role?";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Components.Dialogs.DeleteConfirmation.ConfirmationMessage), string.Format(deleteContent, roleId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Components.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await _roleService.DeleteAsync(roleId);
                if (response.IsSuccessful)
                {
                    await GetRolesAsync();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/");
        }
    }
}