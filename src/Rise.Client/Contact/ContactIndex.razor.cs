﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rise.Client.Attributes;
using Rise.Shared.Contact;
using Rise.Shared.Common;

namespace Rise.Client.Contact;

[HomeBlock(icon: @Icons.Material.Outlined.EventNote, label: "Contact", route: "/contact")]
public partial class ContactIndex
{
    private IEnumerable<ContactDto.Index>? _contactFacilities;
    private IEnumerable<ContactDto.Index> _filteredFacilities = [];
    
    [Inject] public required IContactService ContactService { get; set; }
    
    private int? _expandedServiceId;
    private string _selectedCampus = string.Empty;
    private string _selectedCategory = string.Empty;

    protected IEnumerable<ContactDto.Index>? contactFacilities => _contactFacilities;
    protected IEnumerable<ContactDto.Index> filteredFacilities => _filteredFacilities;
    protected int? expandedServiceId => _expandedServiceId;
    protected string selectedCampus
    {
        get => _selectedCampus;
        set => _selectedCampus = value;
    }
    protected string selectedCategory
    {
        get => _selectedCategory;
        set => _selectedCategory = value;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadContactFacilitiesAsync();
    }

    private async Task LoadContactFacilitiesAsync()
    {
        var request = new QueryRequest.SkipTake
        {
            Skip = 0,
            Take = 1000
        };

        var result = await ContactService.GetIndexAsync(request);
        if (result.IsSuccess)
        {
            _contactFacilities = result.Value.Facilities;
            ApplyFilters();
        }
    }

    private void ApplyFilters()
    {
        _filteredFacilities = _contactFacilities ?? [];

        // Filter by campus
        if (!string.IsNullOrEmpty(_selectedCampus))
        {
            if (_selectedCampus == "Geen locatie")
            {
                _filteredFacilities = _filteredFacilities.Where(f => f.Location == null);
            }
            else
            {
                _filteredFacilities = _filteredFacilities.Where(f => 
                    f.Location != null && f.Location.LocationName == _selectedCampus);
            }
        }

        // Filter by category
        if (!string.IsNullOrEmpty(_selectedCategory))
        {
            _filteredFacilities = _filteredFacilities.Where(f => 
                f.FacilityCategoryName == _selectedCategory);
        }

        // Reset expanded service when filters change
        _expandedServiceId = null;
    }

    protected void OnCampusFilterChanged()
    {
        ApplyFilters();
    }

    protected void OnCategoryFilterChanged()
    {
        ApplyFilters();
    }

    protected void ClearFilters()
    {
        _selectedCampus = string.Empty;
        _selectedCategory = string.Empty;
        ApplyFilters();
    }

    protected void ToggleService(int serviceId)
    {
        if (_expandedServiceId == serviceId)
        {
            _expandedServiceId = null; // Collapse if already expanded
        }
        else
        {
            _expandedServiceId = serviceId; // Expand clicked service
        }
    }
}


