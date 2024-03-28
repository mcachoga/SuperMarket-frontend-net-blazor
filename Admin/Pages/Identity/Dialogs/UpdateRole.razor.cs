﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using SuperMarket.Common.Requests.Identity;

namespace SuperMarket.Client.Pages.Identity.Dialogs
{
    public partial class UpdateRole
    {
        [Parameter] 
        public UpdateRoleRequest UpdateRoleRequest { get; set; } = new();
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; } = default!;

        private async Task SaveRoleAsync()
        {
            var response = await _roleService.UpdateAsync(UpdateRoleRequest);
            if (response.IsSuccessful)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}