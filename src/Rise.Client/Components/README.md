# Documentatie voor Rise.Client.Components

Documentatie / usage voor alle custom componenten in het Rise.Client project.

---

## 1. RiseButton Component
### Parameters
| Parameter | Type | Vereist | Default | Beschrijving                                  |
|-----------|------|---------|---------|-----------------------------------------------|
| `OnClick` | `EventCallback` | V       | - | Event handler die wordt aangeroepen bij klik  |
| `ChildContent` | `RenderFragment?` | V       | - | De inhoud van de button (tekst, iconen, etc.) |
| `Disabled` | `bool` | X       | `false` | Maakt button niet kikbaar wanneer `true`      |
| `Type` | `RiseButtonType` | X       | `Primary` | Bepaalt de visuele stijl (Filled of Outlined) |
| `Size` | `RiseButtonSize` | X       | `Medium` | Bepaalt de grootte van de button              |

### Enums
**RiseButtonType**
- `Primary` - Gevulde button met primaire kleur
- `Secondary` - Outlined button met border

**RiseButtonSize**
- `Small` - Compact formaat (`px-4 py-2`)
- `Medium` - Standaard formaat (`px-6 py-3`)
- `Large` - Groot formaat (`px-10 py-4`)

### Gebruik
```razor
<!-- Basis gebruik -->
<RiseButton OnClick="@HandleClick">
    Opslaan
</RiseButton>

<!-- Secondary button met large size -->
<RiseButton 
    OnClick="@HandleCancel"
    Type="RiseButton.RiseButtonType.Secondary"
    Size="RiseButton.RiseButtonSize.Large">
    Annuleren
</RiseButton>

<!-- Disabled button -->
<RiseButton 
    OnClick="@HandleSubmit"
    Disabled="@isLoading">
    @if (isLoading)
    {
        <RiseLoader />
    }
    Verzenden
</RiseButton>
```

---

## 2. RiseLoader Component
### Parameters
Geen

### Gedrag
- **Initiële positie**: Letters staan in een cirkel gerangschikt
- **Animatie cyclus**:
  1. Letters bewegen naar centrum en worden vervangen door cirkels (700ms)
  2. Rotatie offset wordt verhoogd
  3. Cirkels bewegen naar buiten en worden vervangen door letters (700ms)
  4. Reset voor volgende iteratie (700ms)

### Gebruik
```razor
<!-- Basis gebruik -->
<RiseLoader />
```
---

## 3. RiseForm Component
### Parameters
| Parameter | Type | Vereist | Default | Beschrijving |
|-----------|------|---------|---------|--------------|
| `ChildContent` | `RenderFragment?` | x       | `null` | De formulier velden |
| `Model` | `object` | V       | - | Het data model voor het formulier |
| `Validation` | `object?` | x       | `null` | FluentValidation validator |
| `OnValidSubmit` | `EventCallback` | V       | - | Event bij geldige submit |
| `OnInvalidSubmit` | `EventCallback` | x       | - | Event bij ongeldige submit |
| `OnCancel` | `EventCallback` | x       | - | Event bij annuleren |
| `SubmitButtonText` | `string` | x       | `"Opslaan"` | Tekst voor submit button |
| `IsSubmitting` | `bool` | x       | `false` | Toon loader tijdens submit |

### Publieke Methoden
**`ResetValidation()`** - Reset alle validatie errors in het formulier

### Gebruik
```razor
<!-- Basis formulier -->
<RiseForm Model="@userModel" OnValidSubmit="@HandleSubmit">
    <RiseTextField Label="Naam" @bind-Value="@userModel.Name" />
    <RiseTextField Label="Email" @bind-Value="@userModel.Email" />
</RiseForm>

<!-- Met FluentValidation -->
<RiseForm 
    Model="@userModel"
    Validation="@(new UserValidator())"
    OnValidSubmit="@HandleSubmit"
    OnInvalidSubmit="@HandleInvalidSubmit">
    <RiseTextField Label="Naam" @bind-Value="@userModel.Name" />
    <RiseTextArea Label="Bio" @bind-Value="@userModel.Bio" />
</RiseForm>

<!-- Met loading state en cancel -->
<RiseForm 
    Model="@model"
    OnValidSubmit="@Submit"
    OnCancel="@Cancel"
    SubmitButtonText="Registreren"
    IsSubmitting="@isSubmitting">
    <!-- Form fields -->
</RiseForm>

@code {
    private UserModel userModel = new();
    private bool isSubmitting;

    private async Task HandleSubmit()
    {
        isSubmitting = true;
        await UserService.CreateUser(userModel);
        isSubmitting = false;
    }
}
```

### Validatie in Forms
Gebruik FluentValidation validators met `RiseForm` voor robuuste validatie:

```csharp
public class UserValidator : AbstractValidator<UserModel>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
    }
}
```
---

### 3.1. RiseTextField Component
#### Parameters
| Parameter | Type | Vereist | Default | Beschrijving                                   |
|-----------|------|---------|---------|------------------------------------------------|
| `Label` | `string` | V       | - | Het label boven het veld                       |
| `Value` | `string?` | x       | `null` | De waarde van het tekstveld                    |
| `ValueChanged` | `EventCallback?` | x       | `null` | Event voor waarde wijzigingen                  |
| `HelperText` | `string?` | x       | `null` | Hulptekst onder het veld                       |
| `Disabled` | `bool` | x       | `false` | Maakt textfield niet wijzigbaar wanneer `true` |
| `Error` | `bool` | x       | `false` | Toon error state (rode omlijning)              |
| `ErrorText` | `string?` | x       | `null` | Error boodschap                                |
| `InputType` | `string` | x       | `"text"` | HTML input type                                |

#### Gebruik
```razor
<!-- Basis tekstveld -->
<RiseTextField Label="Voornaam" @bind-Value="@model.FirstName" />

<!-- Met helper text -->
<RiseTextField 
    Label="Email"
    @bind-Value="@model.Email"
    HelperText="Kleine tekst onder textfield" />

<!-- Met error -->
<RiseTextField 
    Label="Gebruikersnaam"
    @bind-Value="@model.Username"
    Error="@hasError"
    ErrorText="Deze gebruikersnaam bestaat al" />

<!-- Disabled field -->
<RiseTextField 
    Label="ID"
    Value="@userId"
    Disabled="true" />
```
---

### 3.2 RiseTextArea Component
#### Parameters
| Parameter | Type | Vereist | Default | Beschrijving |
|-----------|------|---------|---------|--------------|
| `Label` | `string` | V       | - | Het label boven het veld |
| `Value` | `string?` | x       | `null` | De waarde van het tekstveld |
| `ValueChanged` | `EventCallback?` | x       | `null` | Event voor waarde wijzigingen |
| `HelperText` | `string?` | x       | `null` | Hulptekst onder het veld |
| `Disabled` | `bool` | x       | `false` | Maakt textfield niet wijzigbaar wanneer `true` |
| `Error` | `bool` | x       | `false` | Toon error state (rode omlijning) |
| `ErrorText` | `string?` | x       | `null` | Error boodschap |
| `Lines` | `int` | x       | `5` | Aantal zichtbare regels |

### Gebruik
```razor
<!-- Basis textarea -->
<RiseTextArea Label="Beschrijving" @bind-Value="@model.Description" />

<!-- Met custom aantal lijnen -->
<RiseTextArea 
    Label="Opmerkingen"
    @bind-Value="@model.Comments"
    Lines="10" />

<!-- Met validatie -->
<RiseTextArea 
    Label="Bio"
    @bind-Value="@model.Bio"
    Error="@isTooLong"
    ErrorText="Maximaal 500 karakters toegestaan"
    HelperText="Kleine tekst onder textfield" />
```

---

## 4. RiseDropdown Components (Abstract Base - niet instantieerbaar)
`RiseDropdownBase<TItem>` is een abstracte basisklasse voor dropdown componenten.

### Parameters (Gedeeld)
| Parameter | Type | Vereist | Default | Beschrijving |
|-----------|------|---------|---------|--------------|
| `Items` | `IEnumerable<TItem>` | ✅ | - | De lijst van items in de dropdown |
| `ItemTextFunc` | `Func<TItem, string>?` | ❌ | `null` | Functie om item naar tekst te converteren |
| `Placeholder` | `string` | ❌ | `"Selecteer..."` | Placeholder tekst |
| `ActivatorContent` | `RenderFragment?` | ❌ | `null` | Custom activator button content |

### ActivatorContent
Het `ActivatorContent` parameter laat toe om de standaard dropdown button te vervangen door een volledig aangepast UI element. Dit is handig wanneer je:
- Een custom button design wilt met iconen, avatars of badges (UserProfile)

### Dropdown Type Veiligheid
Gebruik altijd het `TItem` type parameter expliciet:

### Protected Methods
**`GetItemText(TItem? item)`** - Converteert een item naar display tekst  
**`OnItemClick(TItem item)`** - Abstract - te implementeren door child classes

```razor
<RiseSelectionDropdown TItem="MyClass" ... />
```
---

### 4.1. RiseNavigationDropdown Component
#### Extra Parameters
| Parameter | Type | Vereist | Default | Beschrijving |
|-----------|------|---------|---------|--------------|
| `NavigationUrlFunc` | `Func<TItem, string>` | V       | - | Functie die URL bepaalt per item |
| `Label` | `string` | x       | `"Selecteer optie"` | Label voor de dropdown |

#### Gebruik
```razor
<!-- Navigatie menu -->
<RiseNavigationDropdown 
    TItem="PageInfo"
    Items="@pages"
    NavigationUrlFunc="@(p => p.Url)"
    ItemTextFunc="@(p => p.Title)"
    Label="Ga naar pagina" />

<!-- Met custom activator -->
<RiseNavigationDropdown 
    TItem="Section"
    Items="@sections"
    NavigationUrlFunc="@(s => $"/section/{s.Id}")
    ItemTextFunc="@(s => s.Name)">
    <ActivatorContent>
        <MudButton Variant="Variant.Text">
            <MudIcon Icon="@Icons.Material.Filled.Menu" />
            Secties
        </MudButton>
    </ActivatorContent>
</RiseNavigationDropdown>

@code {
    private record PageInfo(string Title, string Url);
    
    private List<PageInfo> pages = new()
    {
        new("Dashboard", "/dashboard"),
        new("Profiel", "/profile"),
        new("Instellingen", "/settings")
    };
}
```
---

### 4.2. RiseSelectionDropdown Component
#### Extra Parameters
| Parameter | Type | Vereist | Default | Beschrijving |
|-----------|------|---------|---------|--------------|
| `SelectedItem` | `TItem?` | x       | `null` | Het geselecteerde item |
| `SelectedItemChanged` | `EventCallback<TItem>` | V       | - | Event bij item selectie |

#### Gebruik
```razor
<!-- Basis selectie dropdown -->
<RiseSelectionDropdown 
    TItem="string"
    Items="@categories"
    @bind-SelectedItem="@selectedCategory"
    Placeholder="Kies categorie" />

<!-- Met objecten -->
<RiseSelectionDropdown 
    TItem="Product"
    Items="@products"
    @bind-SelectedItem="@selectedProduct"
    ItemTextFunc="@(p => $"{p.Name} - €{p.Price:F2}")"
    Placeholder="Selecteer product" />

<!-- Filter voorbeeld -->
<RiseSelectionDropdown 
    TItem="StatusFilter"
    Items="@statusFilters"
    SelectedItem="@currentFilter"
    SelectedItemChanged="@OnFilterChanged"
    ItemTextFunc="@(f => f.DisplayName)" />

@code {
    private string? selectedCategory;
    private Product? selectedProduct;
    private StatusFilter currentFilter;
    
    private async Task OnFilterChanged(StatusFilter filter)
    {
        currentFilter = filter;
        await LoadDataWithFilter(filter);
    }
}
```

## 5. RiseAppHeader Component 
Component voor de app header (balk bovenaan de pagina). Deze wordt enkel gebruikt in MainLayout.razor

### Parameters 
| Parameter          | Type                   | Vereist | Default          | Beschrijving                                                                  |
|--------------------|------------------------|---------|------------------|-------------------------------------------------------------------------------|
| `IconMenu`         | `RenderFragment`       | ✅ | -                | Sectie waar allerlei icon buttons kunenn worden geplaatst zoals announcements |
| `ProfileSection`   | `RenderFragment` | ✅ | -                | Hier wordt de profiel sectie geplaatst (login/logout/etc)                     |

### TitleState
TitleState is een Singleton service die de pagina titel beheert. Deze neemt de titel in het RiseHeadTitle component uit de pagina en hergebruikt die voor de mobiele secondary header.
Zie PageTitleService in Client/Shared voor de singleton service.


```razor
<RiseAppHeader >
  <IconMenu>
    <!-- Hier komt dan de content van de IconMenu. Dit is zelf te bepalen -->
  </IconMenu>
  <ProfileSection>
    <!-- Hier komt dan de content van de ProfileSection. Dit is zelf te bepalen -->
   </ProfileSection>
</RiseAppHeader>
```

## 6. RiseHeadTitle Component
Algemene component voor de paginatitel. Deze is verplicht op pagina's als je een mobiele secondart header wilt.
### Parameters 
| Parameter      | Type             | Vereist | Default          | Beschrijving                                          |
|----------------|------------------|---------|------------------|-------------------------------------------------------|
| `ChildContent` | `RenderFragment` | ✅ | -                | Hier komt de hoofdtitel van de pagina.                |
| `SubTitle`     | `string`         | ❌ | -                | Een extra kleinere titel die onder de hoofdtitel komt |   
| `ImageUrl`     | `string`        | ❌ | -                | Optionele afbeelding als achtergrond van de header    |
| `OverlayColor `| `string`        | ❌ | `rgba(0, 0, 0, 0.5)` | Kleur overlay over de afbeelding (indien gebruikt)     |

### BackgroundStyle
Deze property geeft de CSS stijl voor de achtergrond van de header. Indien `ImageUrl` is opgegeven, wordt deze gebruikt als achtergrondafbeelding. Anders wordt de primarykleur gebruikt.

### TitleState / OnParamatersSet
Bij het instantiëren van de component wordt de `TitleState` singleton service bijgewerkt met de titel zodat die gebruikt kan worden voor de mobiele header

```razor
<!-- Voorbeeld van een image header -->
<RiseHeadTitle SubTitle="bla bla bla bla bla " ImageUrl="social.JPG">Home</RiseHeadTitle>

<!-- Voorbeeld van een standaard header -->
<RiseHeadTitle>Home</RiseHeadTitle>
```

## 7. RiseHomeBlock Component
Deze Component wordt gebruikt voor de blokken op de homepage. Basically een wrapper rond RiseItemLink Component 
### Parameters
| Parameter       | Type             | Vereist | Default             | Beschrijving                                                                                                                   |
|-----------------|------------------|---------|---------------------|--------------------------------------------------------------------------------------------------------------------------------|
| `Href`          | `string`         | ✅ | `null!`              | link naar waar de blok moet doorsturen.                                                                                        |
| `Icon`          | `string`         |  ✅ | -                   | Het icoon dat gebruikt wordt. Deze moet uit de Mudblazor icon gallery komen voorbeeld formaat: @Icons.Material.Filled.Favorite |   
| `ChildContent`  | `RenderFragment` | ✅ | -                   | Wordt gebruikt voor de label                                                                                                   |

### Werking RiseHomeBlock op homepage
De Home blokken worden doormiddel van een ['custom attribute'](https://learn.microsoft.com/en-us/dotnet/standard/attributes/writing-custom-attributes) automatisch gegenereerd. 
Via `[HomeBlock(icon:@Icons.Material.Outlined.EventNote, label:"label", route:"/home")]` toe te voegen op de pagina. Deze worden door de HomeBlockService bij builden eenmalig opgehaald en kunnen opgevraagd worden via de singleton. Hiermee kan de lijst nog verder gefilterd worden en gesorteerd naar wens.
`

```razor
<!-- Voorbeeld van een homeblock -->
<RiseHomeBlock Href="/home" Icon="@Icons.Material.Outlined.House">Homel</RiseHomeBlock>

```

## 8. RiseItemLink Component
Deze component laat toe om een eenvoudige klikbare navigatie te maken met een icoon en label.
### Parameters
| Parameter      | Type             | Vereist | Default             | Beschrijving                                                                                                                |
|----------------|------------------|---------|---------------------|-----------------------------------------------------------------------------------------------------------------------------|
| `Href`         | `string`         | ✅ | `null!`              | link naar waar  moet doorsturen.                                                                                        |
| `Icon`         | `string`         |  ✅ | -                   | Het icoon dat gebruikt wordt. Deze moet uit de Mudblazor icon gallery komen voorbeeld formaat: @Icons.Material.Filled.Favorite |   
| `ChildContent` | `RenderFragment` | ✅ | -                   | Wordt gebruikt voor de label                                                                                                |
| `Class`        | `string`         |  ❌ | -                   | Extra CSS classes om toe te voegen aan de root element                                                                      |
| `Style`        | `string`         |  ❌ | -                   | Extra CSS styles om toe te voegen aan de root element                                                                       |
| `Size` | `Size`|         |  ❌ | `Size.Medium`       | Bepaalt de grootte van het icoon (Small, Medium, Large)                                                                     |

```razor
<!-- Voorbeeld van een homeblock -->
    <RiseItemLink Href="/link" Icon="@Icons.Material.Outlined.House" Size="Size.Large"  Class="custom-class" Style="margin-top:10px;">
        label
    </RiseItemLink>
```

## 9. Notification Component
Deze component laat toe om een eenvoudige klikbare navigatie te maken met een icoon en label.
### Parameters
| Parameter  | Type             | Vereist | Default         | Beschrijving                                              |
|------------|------------------|---------|-----------------|-----------------------------------------------------------|
| `Title`    | `string`         | ✅ | `string.Empty` | HoofdTitel van de notificatie.                            |
| `Subtitle` | `string`         |  ❌ | `string.Empty`              | Secondaire titel, iets kleiner                            |   
| `Severity` | `NotificationSeverity` | ✅ | -               | Hiermee wordt het type notificatie bepaalt                |
| `ChildContent`    | `RenderFragment`         |  ❌ | -               | Hiermee kan de inhoud van de notificatie worden ingesteld |

### Opties voor de notificatie
- `NotificationSeverity.Info` - Informatieve notificatie (zwart)
- `NotificationSeverity.Success` - Succes notificatie (groen)
- `NotificationSeverity.Warning` - Waarschuwing notificatie (oranje)
- `NotificationSeverity.Error` - Fout notificatie (rood)
Deze enum wordt static ingeladen over heel de site dus kan je zonder NotificationSeverity te prefixen gebruiken.
```razor
<!-- Voorbeeld van een homeblock -->
    <RiseNotification Title="Hoofdtitel" Subtitle="bla bla bla" Severity="Info">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.
    </RiseItemLink>
```