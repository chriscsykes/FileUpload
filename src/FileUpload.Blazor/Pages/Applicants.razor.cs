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
using FileUpload.Applicants;
using FileUpload.Permissions;
using FileUpload.Shared;

namespace FileUpload.Blazor.Pages
{
    public partial class Applicants
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<ApplicantDto> ApplicantList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }
        private bool CanCreateApplicant { get; set; }
        private bool CanEditApplicant { get; set; }
        private bool CanDeleteApplicant { get; set; }
        private ApplicantCreateDto NewApplicant { get; set; }
        private Validations NewApplicantValidations { get; set; }
        private ApplicantUpdateDto EditingApplicant { get; set; }
        private Validations EditingApplicantValidations { get; set; }
        private Guid EditingApplicantId { get; set; }
        private Modal CreateApplicantModal { get; set; }
        private Modal EditApplicantModal { get; set; }
        private GetApplicantsInput Filter { get; set; }
        private DataGridEntityActionsColumn<ApplicantDto> EntityActionsColumn { get; set; }
        protected string SelectedCreateTab = "applicant-create-tab";
        protected string SelectedEditTab = "applicant-edit-tab";
        
        public Applicants()
        {
            NewApplicant = new ApplicantCreateDto();
            EditingApplicant = new ApplicantUpdateDto();
            Filter = new GetApplicantsInput
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Applicants"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewApplicant"], async () =>
            {
                await OpenCreateApplicantModalAsync();
            }, IconName.Add, requiredPolicyName: FileUploadPermissions.Applicants.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateApplicant = await AuthorizationService
                .IsGrantedAsync(FileUploadPermissions.Applicants.Create);
            CanEditApplicant = await AuthorizationService
                            .IsGrantedAsync(FileUploadPermissions.Applicants.Edit);
            CanDeleteApplicant = await AuthorizationService
                            .IsGrantedAsync(FileUploadPermissions.Applicants.Delete);
        }

        private async Task GetApplicantsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ApplicantsAppService.GetListAsync(Filter);
            ApplicantList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetApplicantsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await ApplicantsAppService.GetDownloadTokenAsync()).Token;
            NavigationManager.NavigateTo($"/api/app/applicants/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ApplicantDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetApplicantsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateApplicantModalAsync()
        {
            NewApplicant = new ApplicantCreateDto{
                
                
            };
            await NewApplicantValidations.ClearAll();
            await CreateApplicantModal.Show();
        }

        private async Task CloseCreateApplicantModalAsync()
        {
            NewApplicant = new ApplicantCreateDto{
                
                
            };
            await CreateApplicantModal.Hide();
        }

        private async Task OpenEditApplicantModalAsync(ApplicantDto input)
        {
            var applicant = await ApplicantsAppService.GetAsync(input.Id);
            
            EditingApplicantId = applicant.Id;
            EditingApplicant = ObjectMapper.Map<ApplicantDto, ApplicantUpdateDto>(applicant);
            await EditingApplicantValidations.ClearAll();
            await EditApplicantModal.Show();
        }

        private async Task DeleteApplicantAsync(ApplicantDto input)
        {
            await ApplicantsAppService.DeleteAsync(input.Id);
            await GetApplicantsAsync();
        }

        private async Task CreateApplicantAsync()
        {
            try
            {
                if (await NewApplicantValidations.ValidateAll() == false)
                {
                    return;
                }

                await ApplicantsAppService.CreateAsync(NewApplicant);
                await GetApplicantsAsync();
                await CloseCreateApplicantModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditApplicantModalAsync()
        {
            await EditApplicantModal.Hide();
        }

        private async Task UpdateApplicantAsync()
        {
            try
            {
                if (await EditingApplicantValidations.ValidateAll() == false)
                {
                    return;
                }

                await ApplicantsAppService.UpdateAsync(EditingApplicantId, EditingApplicant);
                await GetApplicantsAsync();
                await EditApplicantModal.Hide();                
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
        

    }
}
