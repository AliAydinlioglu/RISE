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
    private FilterOption? _selectedCampus;
    private FilterOption? _selectedCategory;

    private readonly List<FilterOption> _campusOptions = new()
    {
        new FilterOption("", "Alle campussen"),
        new FilterOption("Schoonmeersen", "Schoonmeersen"),
        new FilterOption("Mercator", "Mercator"),
        new FilterOption("Gent Campus", "Gent Campus"),
        new FilterOption("Geen locatie", "Geen locatie (Online diensten)")
    };

    private readonly List<FilterOption> _categoryOptions = new()
    {
        new FilterOption("", "Alle categorieën"),
        new FilterOption("Administratief", "Administratief"),
        new FilterOption("Ondersteunend", "Ondersteunend"),
        new FilterOption("Veiligheid en welzijn", "Veiligheid en welzijn")
    };

    protected IEnumerable<ContactDto.Index>? contactFacilities => _contactFacilities;
    protected IEnumerable<ContactDto.Index> filteredFacilities => _filteredFacilities;
    protected int? expandedServiceId => _expandedServiceId;
    protected List<FilterOption> campusOptions => _campusOptions;
    protected List<FilterOption> categoryOptions => _categoryOptions;
    protected FilterOption? selectedCampus
    {
        get => _selectedCampus;
        set => _selectedCampus = value;
    }
    protected FilterOption? selectedCategory
    {
        get => _selectedCategory;
        set => _selectedCategory = value;
    }

    protected override async Task OnInitializedAsync()
    {
        _selectedCampus = _campusOptions[0];
        _selectedCategory = _categoryOptions[0];
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
        if (!string.IsNullOrEmpty(_selectedCampus?.Value))
        {
            if (_selectedCampus.Value == "Geen locatie")
            {
                _filteredFacilities = _filteredFacilities.Where(f => f.Location == null);
            }
            else
            {
                _filteredFacilities = _filteredFacilities.Where(f => 
                    f.Location != null && f.Location.LocationName == _selectedCampus.Value);
            }
        }

        // Filter by category
        if (!string.IsNullOrEmpty(_selectedCategory?.Value))
        {
            _filteredFacilities = _filteredFacilities.Where(f => 
                f.FacilityCategoryName == _selectedCategory.Value);
        }

        // Reset expanded service when filters change
        _expandedServiceId = null;
    }

    protected void OnCampusFilterChanged(FilterOption option)
    {
        _selectedCampus = option;
        ApplyFilters();
    }

    protected void OnCategoryFilterChanged(FilterOption option)
    {
        _selectedCategory = option;
        ApplyFilters();
    }

    protected void ClearFilters()
    {
        _selectedCampus = _campusOptions[0];
        _selectedCategory = _categoryOptions[0];
        ApplyFilters();
    }

    protected bool HasActiveFilters => 
        !string.IsNullOrEmpty(_selectedCampus?.Value) || 
        !string.IsNullOrEmpty(_selectedCategory?.Value);

    public record FilterOption(string Value, string Label)
    {
        public override string ToString() => Label;
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


