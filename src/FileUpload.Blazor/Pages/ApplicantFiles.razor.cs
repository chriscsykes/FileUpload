using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using FileUpload.ApplicantFiles;
using FileUpload.Permissions;
using FileUpload.Shared;

namespace FileUpload.Blazor.Pages
{
    public partial class ApplicantFiles
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<ApplicantFileDto> ApplicantFileList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }
        private bool CanCreateApplicantFile { get; set; }
        private bool CanEditApplicantFile { get; set; }
        private bool CanDeleteApplicantFile { get; set; }
        private ApplicantFileCreateDto NewApplicantFile { get; set; }
        private Validations NewApplicantFileValidations { get; set; }
        private ApplicantFileUpdateDto EditingApplicantFile { get; set; }
        private Validations EditingApplicantFileValidations { get; set; }
        private Guid EditingApplicantFileId { get; set; }
        private Modal CreateApplicantFileModal { get; set; }
        private Modal EditApplicantFileModal { get; set; }
        private GetApplicantFilesInput Filter { get; set; }
        private DataGridEntityActionsColumn<ApplicantFileDto> EntityActionsColumn { get; set; }
        protected string SelectedCreateTab = "applicantFile-create-tab";
        protected string SelectedEditTab = "applicantFile-edit-tab";
        
        public ApplicantFiles()
        {
            NewApplicantFile = new ApplicantFileCreateDto();
            EditingApplicantFile = new ApplicantFileUpdateDto();
            Filter = new GetApplicantFilesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:ApplicantFiles"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            
            
            Toolbar.AddButton(L["NewApplicantFile"], async () =>
            {
                await OpenCreateApplicantFileModalAsync();
            }, IconName.Add, requiredPolicyName: FileUploadPermissions.ApplicantFiles.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateApplicantFile = await AuthorizationService
                .IsGrantedAsync(FileUploadPermissions.ApplicantFiles.Create);
            CanEditApplicantFile = await AuthorizationService
                            .IsGrantedAsync(FileUploadPermissions.ApplicantFiles.Edit);
            CanDeleteApplicantFile = await AuthorizationService
                            .IsGrantedAsync(FileUploadPermissions.ApplicantFiles.Delete);
        }

        private async Task GetApplicantFilesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ApplicantFilesAppService.GetListAsync(Filter);
            ApplicantFileList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetApplicantFilesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ApplicantFileDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetApplicantFilesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateApplicantFileModalAsync()
        {
            NewApplicantFile = new ApplicantFileCreateDto{
                
                
            };
            await NewApplicantFileValidations.ClearAll();
            await CreateApplicantFileModal.Show();
            Console.WriteLine("---------------- HELLLLLLLLLLLOOOOOOOOOOOOOOO --------------------");
        }

        private async Task CloseCreateApplicantFileModalAsync()
        {
            NewApplicantFile = new ApplicantFileCreateDto{
                
                
            };
            await CreateApplicantFileModal.Hide();
        }

        private async Task OpenEditApplicantFileModalAsync(ApplicantFileDto input)
        {
            var applicantFile = await ApplicantFilesAppService.GetAsync(input.Id);
            
            EditingApplicantFileId = applicantFile.Id;
            EditingApplicantFile = ObjectMapper.Map<ApplicantFileDto, ApplicantFileUpdateDto>(applicantFile);
            await EditingApplicantFileValidations.ClearAll();
            await EditApplicantFileModal.Show();
        }

        private async Task DeleteApplicantFileAsync(ApplicantFileDto input)
        {
            await ApplicantFilesAppService.DeleteAsync(input.Id);
            await GetApplicantFilesAsync();
        }

        private async Task CreateApplicantFileAsync()
        {
            try
            {
                if (await NewApplicantFileValidations.ValidateAll() == false)
                {
                    return;
                }

                await ApplicantFilesAppService.CreateAsync(NewApplicantFile);
                await GetApplicantFilesAsync();
                await CloseCreateApplicantFileModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditApplicantFileModalAsync()
        {
            await EditApplicantFileModal.Hide();
        }

        private async Task UpdateApplicantFileAsync()
        {
            try
            {
                if (await EditingApplicantFileValidations.ValidateAll() == false)
                {
                    return;
                }

                await ApplicantFilesAppService.UpdateAsync(EditingApplicantFileId, EditingApplicantFile);
                await GetApplicantFilesAsync();
                await EditApplicantFileModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        
        /*private async Task OnChanged(FileChangedEventArgs e)
        {

        }*/

    }
}
